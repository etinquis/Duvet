using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Duvet.Output.HTML
{
    class TeamCitySourceFileHtmlReport
    {
        private ISourceFile _file;
        private ISourceAssembly _assembly;
        private ISourceNamespace _namespace;
        public TeamCitySourceFileHtmlReport(ISourceAssembly assembly, ISourceNamespace nspace, ISourceFile file)
        {
            _file = file;
            _namespace = nspace;
            _assembly = assembly;
        }

        public void WriteTo(System.IO.StreamWriter writer)
        {
            ISourceLine[] lines = _file.Lines.ToArray();

            string sourceType;
            switch (_file.Language)
            {
                case SourceLanguage.Cpp:
                    sourceType = "cpp";
                    break;
                case SourceLanguage.CSharp:
                    sourceType = "cs";
                    break;
                default:
                    sourceType = "";
                    break;
            }

            StringBuilder builder = new StringBuilder();
            builder.Append("<html id=\"htmlId\">");
            builder.Append("<head>");
            builder.AppendFormat("<title>{0}</title>", "Coverage Report > " + _file.Name);
            builder.AppendFormat("<style type=\"text/css\">{0}</style>",
                                 "@import \"../../../.css/coverage.css\"; @import \"../../../.css/idea.css\";");
            builder.AppendFormat("<script type=\"text/javascript\" src=\"../../../.js/highlight.pack.js\"></script>");
            builder.Append("</head>");
            builder.Append("<body>");

            builder.Append("<div class=\"content\">");

            builder.Append("<div class=\"breadCrumbs\"></div>");
            builder.AppendFormat("<h1>Coverage Summary</h1>");

            builder.Append(BuildCoverageStatsTable(_file.Name, _file.Classes, _file.CoverageStats));

            builder.AppendFormat("<h2>{0}</h2>", _file.Name);
            builder.AppendFormat("<pre><div class=\"sourceCode {0}\" id=\"sourceCode\"", sourceType);

            foreach (var line in lines)
            {
                string format = "{0}" + Environment.NewLine;
                if (line.Coverage != CoverageLevel.Empty)
                {
                    string coverageClass;

                    if (line.Coverage == CoverageLevel.FullyCovered)
                    {
                        coverageClass = "fc";
                    } else if (line.Coverage == CoverageLevel.NotCovered)
                    {
                        coverageClass = "nc";
                    } else
                    {
                        coverageClass = "pc";
                    }
                    
                    format = string.Format(format, "<b class=\"" + coverageClass + "\">{0}</b>");
                }

                string lineNumber = string.Format("<i class=\"no-highlight\">{0}</i>", line.LineNumber);
                string lineContent = string.Format("{0}&nbsp;{1}", lineNumber, line.LineContents);

                builder.AppendFormat(format, lineContent);
            }

            builder.Append("</div></pre>");
            builder.Append("</div>");
            builder.AppendFormat("<script type=\"text/javascript\">{0}</script>", "(function() {var codeBlock = document.getElementById('sourceCode');if (codeBlock) {hljs.highlightBlock(codeBlock);}})();");
            builder.Append("</body>");
            builder.Append("</html>");

            writer.Write(builder.ToString());
        }

        private string BuildCoverageStatsTable(string fileName, IEnumerable<ISourceClass> classes, ICoverageStats stats)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table class=\"coverageStats\">");
            builder.Append("<tbody>");

            builder.AppendFormat(
                "<tr><th class=\"name\">Type</th><th class=\"coverageStat\">Class, %</th><th class=\"coverageStat\">Methods, %</th><th class=\"coverageStat\">Lines, %</th></tr>");

            string coverageFmt = "{0}% ({1}/{2})";
            string classCoverage;
            string methodCoverage;
            string lineCoverage;

            foreach (var sourceClass in classes)
            {
                classCoverage = string.Format(coverageFmt,
                                              sourceClass.CoverageStats.TotalClasses == 0
                                                  ? "N/A"
                                                  : (sourceClass.CoverageStats.ClassesCovered/
                                                     (float) sourceClass.CoverageStats.TotalClasses).ToString(
                                                         CultureInfo.InvariantCulture),
                                              sourceClass.CoverageStats.ClassesCovered,
                                              sourceClass.CoverageStats.TotalClasses);
                methodCoverage = string.Format(coverageFmt,
                                               sourceClass.CoverageStats.TotalMethods == 0
                                                   ? "N/A"
                                                   : (sourceClass.CoverageStats.MethodsCovered/
                                                      (float) sourceClass.CoverageStats.TotalMethods).ToString(
                                                          CultureInfo.InvariantCulture),
                                               sourceClass.CoverageStats.MethodsCovered,
                                               sourceClass.CoverageStats.TotalMethods);
                lineCoverage = string.Format(coverageFmt,
                                             sourceClass.CoverageStats.TotalCoverableLines == 0
                                                 ? "N/A"
                                                 : (sourceClass.CoverageStats.LinesCovered/
                                                    (float) sourceClass.CoverageStats.TotalCoverableLines).ToString(
                                                        CultureInfo.InvariantCulture),
                                             sourceClass.CoverageStats.LinesCovered,
                                             sourceClass.CoverageStats.TotalCoverableLines);

                builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", sourceClass.Name, classCoverage, methodCoverage, lineCoverage);
            }

            classCoverage = string.Format(coverageFmt,
                                          stats.TotalClasses == 0
                                              ? "N/A"
                                              : (stats.ClassesCovered/(float) stats.TotalClasses).ToString(
                                                  CultureInfo.InvariantCulture), stats.ClassesCovered,
                                          stats.TotalClasses);
            methodCoverage = string.Format(coverageFmt,
                                           stats.TotalMethods == 0
                                               ? "N/A"
                                               : (stats.MethodsCovered/(float) stats.TotalMethods).ToString(
                                                   CultureInfo.InvariantCulture), stats.MethodsCovered, stats.TotalMethods);
            lineCoverage = string.Format(coverageFmt,
                                         stats.TotalCoverableLines == 0
                                             ? "N/A"
                                             : (stats.LinesCovered/(float) stats.TotalCoverableLines).ToString(
                                                 CultureInfo.InvariantCulture), stats.LinesCovered,
                                         stats.TotalCoverableLines);

            builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", fileName, classCoverage, methodCoverage, lineCoverage);

            builder.Append("</tbody>");
            builder.Append("</table>");

            return builder.ToString();
        }
    }
}
