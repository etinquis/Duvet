using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Duvet.Output.HTML.Pages
{
    class IndexTeamCityHtmlReportPageContent : ITeamCityHtmlReportPageContent
    {
        private IEnumerable<ISourceAssembly> _assemblies;
        private TeamCityHtmlReportPathResolver _pathResolver;

        public IndexTeamCityHtmlReportPageContent(TeamCityHtmlReportPathResolver pathResolver, IEnumerable<ISourceAssembly> assemblies)
        {
            _assemblies = assemblies;
            _pathResolver = pathResolver;
        }

        public string BuildBreadCrumbs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"breadCrumbs\">Current scope:");

            builder.Append("all assemblies");

            builder.Append("</div>");

            return builder.ToString();
        }

        public string BuildCoverageStatsTable()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("<table class=\"coverageStats\">");
            builder.Append("<tr><th class=\"name\">Assembly</th><th class=\"coverageStat\">Class, %</th><th class=\"coverageStat\">Method, %</th><th class=\"coverageStat\">Lines, %</th></tr>");

            string coverageFmt = "{0}% ({1}/{2})";
            string classCoverage;
            string methodCoverage;
            string lineCoverage;

            //classCoverage = string.Format(coverageFmt,
            //                              _namespace.CoverageStats.TotalClasses == 0
            //                                  ? "N/A"
            //                                  : (100 * _namespace.CoverageStats.ClassesCovered / (float)_namespace.CoverageStats.TotalClasses).ToString(
            //                                      CultureInfo.InvariantCulture), _namespace.CoverageStats.ClassesCovered,
            //                              _namespace.CoverageStats.TotalClasses);
            //methodCoverage = string.Format(coverageFmt,
            //                               _namespace.CoverageStats.TotalMethods == 0
            //                                   ? "N/A"
            //                                   : (100 * _namespace.CoverageStats.MethodsCovered / (float)_namespace.CoverageStats.TotalMethods).ToString(
            //                                       CultureInfo.InvariantCulture), _namespace.CoverageStats.MethodsCovered, _namespace.CoverageStats.TotalMethods);
            //lineCoverage = string.Format(coverageFmt,
            //                             _namespace.CoverageStats.TotalCoverableLines == 0
            //                                 ? "N/A"
            //                                 : (100 * _namespace.CoverageStats.LinesCovered / (float)_namespace.CoverageStats.TotalCoverableLines).ToString(
            //                                     CultureInfo.InvariantCulture), _namespace.CoverageStats.LinesCovered,
            //                             _namespace.CoverageStats.TotalCoverableLines);

            //builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", _namespace.Name, classCoverage, methodCoverage, lineCoverage);

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

            foreach (var sourceAssembly in _assemblies)
            {
                classCoverage = string.Format(coverageFmt,
                                              sourceAssembly.CoverageStats.TotalClasses == 0
                                                  ? "N/A"
                                                  : (100 * sourceAssembly.CoverageStats.ClassesCovered /
                                                     (float)sourceAssembly.CoverageStats.TotalClasses).ToString(
                                                         CultureInfo.InvariantCulture),
                                              sourceAssembly.CoverageStats.ClassesCovered,
                                              sourceAssembly.CoverageStats.TotalClasses);
                methodCoverage = string.Format(coverageFmt,
                                               sourceAssembly.CoverageStats.TotalMethods == 0
                                                   ? "N/A"
                                                   : (100 * sourceAssembly.CoverageStats.MethodsCovered /
                                                      (float)sourceAssembly.CoverageStats.TotalMethods).ToString(
                                                          CultureInfo.InvariantCulture),
                                               sourceAssembly.CoverageStats.MethodsCovered,
                                               sourceAssembly.CoverageStats.TotalMethods);
                lineCoverage = string.Format(coverageFmt,
                                             sourceAssembly.CoverageStats.TotalCoverableLines == 0
                                                 ? "N/A"
                                                 : (100 * sourceAssembly.CoverageStats.LinesCovered /
                                                    (float)sourceAssembly.CoverageStats.TotalCoverableLines).ToString(
                                                        CultureInfo.InvariantCulture),
                                             sourceAssembly.CoverageStats.LinesCovered,
                                             sourceAssembly.CoverageStats.TotalCoverableLines);

                builder.AppendFormat("<tr><td class=\"name\"><a href=\"{4}\">{0}</a></td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", sourceAssembly.Name, classCoverage, methodCoverage, lineCoverage, _pathResolver.RelativePathFromAssemblyToRoot + _pathResolver.GetRelativePathFromRootForAssembly(sourceAssembly));
            }

            builder.Append("</table>");

            return builder.ToString();
        }
    }
}