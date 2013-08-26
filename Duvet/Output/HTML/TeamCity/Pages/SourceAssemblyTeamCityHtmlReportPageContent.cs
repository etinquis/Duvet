using System.Globalization;
using System.Text;

namespace Duvet.Output.HTML.Pages
{
    class SourceAssemblyTeamCityHtmlReportPageContent : ITeamCityHtmlReportPageContent
    {
        private ISourceAssembly _assembly;
        private TeamCityHtmlReportPathResolver _pathResolver;

        public SourceAssemblyTeamCityHtmlReportPageContent(TeamCityHtmlReportPathResolver resolver,
                                                            ISourceAssembly assembly)
        {
            _assembly = assembly;
            _pathResolver = resolver;
        }

        public string BuildBreadCrumbs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"breadCrumbs\">Current scope:");

            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromAssemblyToRoot + _pathResolver.GetRelativePathFromRootForIndex(), "all assemblies");
            builder.Append("<span class=\"seperator\"></span>");
            builder.Append(_assembly.Name);

            builder.Append("</div>");

            return builder.ToString();
        }

        public string BuildCoverageStatsTable()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table class=\"coverageStats\">");
            builder.Append("<tr><th class=\"name\">Namespace</th><th class=\"coverageStat\">Class, %</th><th class=\"coverageStat\">Method, %</th><th class=\"coverageStat\">Lines, %</th></tr>");

            string coverageFmt = "{0}% ({1}/{2})";
            string classCoverage;
            string methodCoverage;
            string lineCoverage;

            classCoverage = string.Format(coverageFmt,
                                          _assembly.CoverageStats.TotalClasses == 0
                                              ? "N/A"
                                              : (100 * _assembly.CoverageStats.ClassesCovered / (float)_assembly.CoverageStats.TotalClasses).ToString(
                                                  CultureInfo.InvariantCulture), _assembly.CoverageStats.ClassesCovered,
                                          _assembly.CoverageStats.TotalClasses);
            methodCoverage = string.Format(coverageFmt,
                                           _assembly.CoverageStats.TotalMethods == 0
                                               ? "N/A"
                                               : (100 * _assembly.CoverageStats.MethodsCovered / (float)_assembly.CoverageStats.TotalMethods).ToString(
                                                   CultureInfo.InvariantCulture), _assembly.CoverageStats.MethodsCovered, _assembly.CoverageStats.TotalMethods);
            lineCoverage = string.Format(coverageFmt,
                                         _assembly.CoverageStats.TotalCoverableLines == 0
                                             ? "N/A"
                                             : (100 * _assembly.CoverageStats.LinesCovered / (float)_assembly.CoverageStats.TotalCoverableLines).ToString(
                                                 CultureInfo.InvariantCulture), _assembly.CoverageStats.LinesCovered,
                                         _assembly.CoverageStats.TotalCoverableLines);

            builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", "all types", classCoverage, methodCoverage, lineCoverage);

            builder.Append("</table>");

            return builder.ToString();
        }

        public string BuildContent()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table class=\"coverageStats\">");
            builder.Append("<tr><th class=\"name\">Type</th><th class=\"coverageStat\">Class, %</th><th class=\"coverageStat\">Method, %</th><th class=\"coverageStat\">Lines, %</th></tr>");

            string coverageFmt = "{0}% ({1}/{2})";
            string classCoverage;
            string methodCoverage;
            string lineCoverage;

            foreach (var sourceNamespace in _assembly.Namespaces)
            {
                classCoverage = string.Format(coverageFmt,
                                              sourceNamespace.CoverageStats.TotalClasses == 0
                                                  ? "N/A"
                                                  : (100 * sourceNamespace.CoverageStats.ClassesCovered /
                                                     (float)sourceNamespace.CoverageStats.TotalClasses).ToString(
                                                         CultureInfo.InvariantCulture),
                                              sourceNamespace.CoverageStats.ClassesCovered,
                                              sourceNamespace.CoverageStats.TotalClasses);
                methodCoverage = string.Format(coverageFmt,
                                               sourceNamespace.CoverageStats.TotalMethods == 0
                                                   ? "N/A"
                                                   : (100 * sourceNamespace.CoverageStats.MethodsCovered /
                                                      (float)sourceNamespace.CoverageStats.TotalMethods).ToString(
                                                          CultureInfo.InvariantCulture),
                                               sourceNamespace.CoverageStats.MethodsCovered,
                                               sourceNamespace.CoverageStats.TotalMethods);
                lineCoverage = string.Format(coverageFmt,
                                             sourceNamespace.CoverageStats.TotalCoverableLines == 0
                                                 ? "N/A"
                                                 : (100 * sourceNamespace.CoverageStats.LinesCovered /
                                                    (float)sourceNamespace.CoverageStats.TotalCoverableLines).ToString(
                                                        CultureInfo.InvariantCulture),
                                             sourceNamespace.CoverageStats.LinesCovered,
                                             sourceNamespace.CoverageStats.TotalCoverableLines);

                builder.AppendFormat("<tr><td class=\"name\"><a href=\"{4}\">{0}</a></td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", sourceNamespace.Name, classCoverage, methodCoverage, lineCoverage, _pathResolver.RelativePathFromAssemblyToRoot + _pathResolver.GetRelativePathFromRootForNamespace(sourceNamespace));
            }

            builder.Append("</table>");

            return builder.ToString();
        }
    }
}