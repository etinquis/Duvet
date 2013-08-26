using System;
using System.Globalization;
using System.Text;
using System.Web;
using Duvet.Output.HTML.Pages;
using Duvet.Output.HTML.TeamCity;

namespace Duvet.Output.HTML
{
    public class SourceFileTeamCityHtmlReportPageContent : ITeamCityHtmlReportPageContent
    {
        private ISourceAssembly _assembly;
        private ISourceNamespace _namespace;
        private ISourceFile _file;

        private TeamCityHtmlReportPathResolver _pathResolver;

        public SourceFileTeamCityHtmlReportPageContent(TeamCityHtmlReportPathResolver reportPathResolver, ISourceAssembly assembly, ISourceNamespace nspace, ISourceFile file)
        {
            _assembly = assembly;
            _namespace = nspace;
            _file = file;

            _pathResolver = reportPathResolver;
        }

        public string BuildBreadCrumbs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"breadCrumbs\">Current scope:");

            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromFileToRoot + _pathResolver.GetRelativePathFromRootForIndex(), "all assemblies");
            builder.Append("<span class=\"seperator\"></span>");
            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromFileToRoot + _pathResolver.GetRelativePathFromRootForAssembly(_assembly), _assembly.Name);
            builder.Append("<span class=\"seperator\"></span>");
            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromFileToRoot + _pathResolver.GetRelativePathFromRootForNamespace(_namespace), _namespace.Name);
            builder.Append("<span class=\"seperator\"></span>");
            builder.Append(_file.Name);

            builder.Append("</div>");

            return builder.ToString();
        }

        public string BuildCoverageStatsTable()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table class=\"coverageStats\">");
            builder.Append("<tr><th class=\"name\">Type</th><th class=\"coverageStat\">Class, %</th><th class=\"coverageStat\">Method, %</th><th class=\"coverageStat\">Lines, %</th></tr>");
            
            string coverageFmt = "{0}% ({1}/{2})";
            string classCoverage;
            string methodCoverage;
            string lineCoverage;

            foreach (var sourceClass in _file.Classes)
            {
                classCoverage = string.Format(coverageFmt,
                                              sourceClass.CoverageStats.TotalClasses == 0
                                                  ? "N/A"
                                                  : (100 * sourceClass.CoverageStats.ClassesCovered /
                                                     (float) sourceClass.CoverageStats.TotalClasses).ToString(
                                                         CultureInfo.InvariantCulture),
                                              sourceClass.CoverageStats.ClassesCovered,
                                              sourceClass.CoverageStats.TotalClasses);
                methodCoverage = string.Format(coverageFmt,
                                               sourceClass.CoverageStats.TotalMethods == 0
                                                   ? "N/A"
                                                   : (100 * sourceClass.CoverageStats.MethodsCovered /
                                                      (float) sourceClass.CoverageStats.TotalMethods).ToString(
                                                          CultureInfo.InvariantCulture),
                                               sourceClass.CoverageStats.MethodsCovered,
                                               sourceClass.CoverageStats.TotalMethods);
                lineCoverage = string.Format(coverageFmt,
                                             sourceClass.CoverageStats.TotalCoverableLines == 0
                                                 ? "N/A"
                                                 : (100 * sourceClass.CoverageStats.LinesCovered /
                                                    (float) sourceClass.CoverageStats.TotalCoverableLines).ToString(
                                                        CultureInfo.InvariantCulture),
                                             sourceClass.CoverageStats.LinesCovered,
                                             sourceClass.CoverageStats.TotalCoverableLines);

                builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", sourceClass.Name, classCoverage, methodCoverage, lineCoverage);
            }

            classCoverage = string.Format(coverageFmt,
                                          _file.CoverageStats.TotalClasses == 0
                                              ? "N/A"
                                              : (100 * _file.CoverageStats.ClassesCovered / (float)_file.CoverageStats.TotalClasses).ToString(
                                                  CultureInfo.InvariantCulture), _file.CoverageStats.ClassesCovered,
                                          _file.CoverageStats.TotalClasses);
            methodCoverage = string.Format(coverageFmt,
                                           _file.CoverageStats.TotalMethods == 0
                                               ? "N/A"
                                               : (100 * _file.CoverageStats.MethodsCovered / (float)_file.CoverageStats.TotalMethods).ToString(
                                                   CultureInfo.InvariantCulture), _file.CoverageStats.MethodsCovered, _file.CoverageStats.TotalMethods);
            lineCoverage = string.Format(coverageFmt,
                                         _file.CoverageStats.TotalCoverableLines == 0
                                             ? "N/A"
                                             : (100 * _file.CoverageStats.LinesCovered / (float)_file.CoverageStats.TotalCoverableLines).ToString(
                                                 CultureInfo.InvariantCulture), _file.CoverageStats.LinesCovered,
                                         _file.CoverageStats.TotalCoverableLines);

            builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", _file.Name, classCoverage, methodCoverage, lineCoverage);

            builder.Append("</table>");

            return builder.ToString();
        }

        public string BuildContent()
        {
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

            builder.AppendFormat("<h2>{0}</h2>", _file.Name);
            builder.AppendFormat("<pre><div class=\"sourceCode {0}\" id=\"sourceCode\">", sourceType);

            foreach (var line in _file.Lines)
            {
                string format = "{0}" + Environment.NewLine;
                if (line.Coverage != CoverageLevel.Empty)
                {
                    string coverageClass;

                    if (line.Coverage == CoverageLevel.FullyCovered)
                    {
                        coverageClass = "fc";
                    }
                    else if (line.Coverage == CoverageLevel.NotCovered)
                    {
                        coverageClass = "nc";
                    }
                    else
                    {
                        coverageClass = "pc";
                    }

                    format = string.Format(format, "<b class=\"" + coverageClass + "\">{0}</b>");
                }

                string lineNumber = string.Format("<i class=\"no-highlight\">{0}</i>", line.LineNumber);
                string lineContent = string.Format("{0}&nbsp;{1}", lineNumber, HttpUtility.HtmlEncode(line.LineContents));
                builder.AppendFormat(format, lineContent);
            }

            builder.Append("</div></pre>");

            return builder.ToString();
        }
    }
}