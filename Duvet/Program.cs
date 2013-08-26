using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.Coverage.Analysis;
using System.Numerics;
using Duvet.Output.HTML;
using Duvet.Output.HTML.TeamCity;
using Duvet.VSDotCoverage;

namespace Duvet
{
    internal class Program
    {
        private static readonly BigInteger TwoDecimalPlaces = new BigInteger(10000);
        private const float MethodCoverageThreshold = .75f;

        private static void Main(string[] args)
        {
            string coverageFile = args[0];
            // Create a coverage info object from the file

            CoverageInfo ci = CoverageInfo.CreateFromFile(coverageFile);

            CoverageDS summary = ci.BuildDataSet(null);

            BigInteger coveredBlocks = 0;
            BigInteger coveredLines = 0;

            BigInteger totalBlocks = 0;
            BigInteger totalLines = 0;

            DirectoryInfo htmlRoot = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "CoverageReport"));// Directory.CreateDirectory("html");
            htmlRoot.Create();

            CoverageDsParser parser = new CoverageDsParser(coverageFile);

            TeamCityHtmlReport report = new TeamCityHtmlReport(parser.Assemblies);
            report.WriteTo(htmlRoot);
            

            //foreach (CoverageDSPriv.SourceFileNamesRow fileRow in summary.SourceFileNames)
            //{
            //    int currentLineNumber = 1;

            //    FileInfo info = new FileInfo(fileRow.SourceFileName);

            //    StreamReader fileReader = new StreamReader(info.FullName);
            //    StringBuilder builder = new StringBuilder();
            //    builder.Append("<html id=\"htmlId\">");
            //    builder.Append("<head>");
            //    builder.AppendFormat("<title>{0}</title>", "Coverage Report > " + fileRow.SourceFileName);
            //    builder.AppendFormat("<style type=\"text/css\">{0}</style>",
            //                         "@import \"../../../.css/coverage.css\"; @import \"../../../.css/idea.css\";");
            //    builder.AppendFormat("<script type=\"text/javascript\" src=\"../../../.js/highlight.pack.js\"></script>");
            //    builder.Append("</head>");
            //    builder.Append("<body>");

            //    string sourceType = "cpp";

            //    if (info.Extension == ".cpp" || info.Extension == ".h")
            //    {
                    
            //    } else if (info.Extension == ".cs")
            //    {
            //        sourceType = "cs";
            //    }

            //    builder.AppendFormat("<pre><div class=\"sourceCode {0}\" id=\"sourceCode\"", sourceType);

            //    CoverageDSPriv.SourceFileNamesRow row = fileRow;
            //    foreach (CoverageDSPriv.LinesRow lineRow in summary.Lines.Where(line=>line.SourceFileID == row.SourceFileID).OrderBy(line=>line.LnStart))
            //    {
            //        while (currentLineNumber < lineRow.LnStart)
            //        {
            //            builder.AppendFormat("<i class=\"no-highlight\">{0}</i>&nbsp;{1}" + Environment.NewLine, currentLineNumber++, fileReader.ReadLine());
            //        }

            //        string cssClass = "nc";

            //        switch (lineRow.Coverage)
            //        {
            //            case 1:
            //                cssClass = "pc";
            //                break;
            //            case 2:
            //                cssClass = "nc";
            //                break;
            //            default:
            //                cssClass = "fc";
            //                break;
            //        }

            //        builder.AppendFormat(
            //            "<b class=\"{2}\"><i class=\"no-highlight\">{0}</i>&nbsp;{1}</b>" + Environment.NewLine,
            //            currentLineNumber++, fileReader.ReadLine(),
            //            cssClass);
            //    }

            //    while (fileReader.Peek() != -1)
            //    {
            //        builder.AppendFormat("<i class=\"no-highlight\">{0}</i>&nbsp;{1}" + Environment.NewLine, currentLineNumber++, fileReader.ReadLine());
            //    }
            //    builder.Append("</pre></div>");
            //    builder.AppendFormat("<script type=\"text/javascript\">{0}</script>", "(function() {var codeBlock = document.getElementById('sourceCode');if (codeBlock) {hljs.highlightBlock(codeBlock);}})();");
            //    builder.Append("</body>");
            //    builder.Append("</html>");

            //    string html = builder.ToString();
            //    StreamWriter writer = new StreamWriter(Path.Combine(htmlRoot.FullName, new FileInfo(fileRow.SourceFileName).Name + ".html"));
            //    writer.Write(html);
            //    writer.Close();
            //}

            foreach (CoverageDSPriv.ModuleRow moduleRow in summary.Module.Rows)
            {
                BigInteger blocksCovered = moduleRow.BlocksCovered;
                BigInteger blocksNotCovered = moduleRow.BlocksNotCovered;

                coveredBlocks += blocksCovered;
                totalBlocks += blocksCovered + blocksNotCovered;

                BigInteger linesCovered = moduleRow.LinesCovered;
                BigInteger linesPartiallyCovered = moduleRow.LinesPartiallyCovered;
                BigInteger linesNotCovered = moduleRow.LinesNotCovered;

                coveredLines += linesCovered + linesPartiallyCovered;
                totalLines += linesCovered + linesPartiallyCovered + linesNotCovered;
            }

            BigInteger coveredMethods = 0;
            BigInteger totalMethods = summary.Method.Rows.Count;

            foreach (CoverageDSPriv.MethodRow methodRow in summary.Method.Rows)
            {
                float coverage = (methodRow.LinesCovered + methodRow.LinesPartiallyCovered)/
                                 (methodRow.LinesCovered + methodRow.LinesPartiallyCovered + (float)methodRow.LinesNotCovered); 
                coveredMethods += (coverage > MethodCoverageThreshold) ? 1 : 0;
            }

            float blockCoverage = ((uint) ((coveredBlocks*TwoDecimalPlaces)/totalBlocks))/10000f;
            float lineCoverage = ((uint)((coveredLines * TwoDecimalPlaces) / totalLines)) / 10000f;
            float methodCoverage = ((uint)((coveredMethods * TwoDecimalPlaces) / totalMethods)) / 10000f;

            XmlDocument doc = new XmlDocument();
            XmlElement rootElement = doc.CreateElement("build");

            XmlElement blockCoverageNode = CreateBlockCoverageElement(doc, blockCoverage);
            XmlElement lineCoverageNode = CreateLineCoverageElement(doc, lineCoverage);
            XmlElement methodCoverageNode = CreateMethodCoverageElement(doc, methodCoverage);

            XmlElement totalCoveredLinesNode = CreateCoveredLinesElement(doc, (uint) coveredLines);
            XmlElement totalLinesNode = CreateTotalLinesElement(doc, (uint) totalLines);

            XmlElement totalCoveredMethodsNode = CreateCoveredMethodsElement(doc, (uint)coveredMethods);
            XmlElement totalMethodsNode = CreateTotalMethodsElement(doc, (uint)totalMethods);

            rootElement.AppendChild(blockCoverageNode);
            rootElement.AppendChild(lineCoverageNode);
            rootElement.AppendChild(methodCoverageNode);

            rootElement.AppendChild(totalCoveredLinesNode);
            rootElement.AppendChild(totalLinesNode);

            rootElement.AppendChild(totalCoveredMethodsNode);
            rootElement.AppendChild(totalMethodsNode);

            doc.AppendChild(rootElement);
            using (var writer = new StreamWriter("teamcity-info.xml"))
            {
                writer.Write(doc.OuterXml);
            }
        }

        private static XmlElement CreateStatisticValueNode(XmlDocument doc, string key, string value)
        {
            XmlElement element = doc.CreateElement("statisticValue");

            XmlAttribute keyAttr = doc.CreateAttribute("key");
            keyAttr.Value = key;

            XmlAttribute valueAttr = doc.CreateAttribute("value");
            valueAttr.Value = value;

            element.Attributes.Append(keyAttr);
            element.Attributes.Append(valueAttr);

            return element;
        }

        private static XmlElement CreateBlockCoverageElement(XmlDocument doc, float value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageB", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateLineCoverageElement(XmlDocument doc, float value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageL", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateMethodCoverageElement(XmlDocument doc, float value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageM", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateCoveredLinesElement(XmlDocument doc, uint value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageAbsLCovered", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateTotalLinesElement(XmlDocument doc, uint value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageAbsLTotal", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateTotalMethodsElement(XmlDocument doc, uint value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageAbsMTotal", value.ToString(CultureInfo.InvariantCulture));
        }

        private static XmlElement CreateCoveredMethodsElement(XmlDocument doc, uint value)
        {
            return CreateStatisticValueNode(doc, "CodeCoverageAbsMCovered", value.ToString(CultureInfo.InvariantCulture));
        }
}
}
