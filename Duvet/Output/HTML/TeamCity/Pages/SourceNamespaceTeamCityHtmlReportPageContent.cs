using System.Globalization;
using System.Text;

namespace Duvet.Output.HTML.Pages
{
    class SourceNamespaceTeamCityHtmlReportPageContent : ITeamCityHtmlReportPageContent
    {
        private ISourceAssembly _assembly;
        private ISourceNamespace _namespace;
        private TeamCityHtmlReportPathResolver _pathResolver;

        public SourceNamespaceTeamCityHtmlReportPageContent(TeamCityHtmlReportPathResolver resolver,
                                                            ISourceAssembly assembly, ISourceNamespace nspace)
        {
            _assembly = assembly;
            _namespace = nspace;
            _pathResolver = resolver;
        }

        public string BuildBreadCrumbs()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"breadCrumbs\">Current scope:");

            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromNamespaceToRoot + _pathResolver.GetRelativePathFromRootForIndex(), "all assemblies");
            builder.Append("<span class=\"seperator\"></span>");
            builder.AppendFormat("<a href=\"{0}\">{1}</a>", _pathResolver.RelativePathFromNamespaceToRoot + _pathResolver.GetRelativePathFromRootForAssembly(_assembly), _assembly.Name);
            builder.Append("<span class=\"seperator\"></span>");
            builder.Append(_namespace.Name);

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
                                          _namespace.CoverageStats.TotalClasses == 0
                                              ? "N/A"
                                              : (100 * _namespace.CoverageStats.ClassesCovered / (float)_namespace.CoverageStats.TotalClasses).ToString(
                                                  CultureInfo.InvariantCulture), _namespace.CoverageStats.ClassesCovered,
                                          _namespace.CoverageStats.TotalClasses);
            methodCoverage = string.Format(coverageFmt,
                                           _namespace.CoverageStats.TotalMethods == 0
                                               ? "N/A"
                                               : (100 * _namespace.CoverageStats.MethodsCovered / (float)_namespace.CoverageStats.TotalMethods).ToString(
                                                   CultureInfo.InvariantCulture), _namespace.CoverageStats.MethodsCovered, _namespace.CoverageStats.TotalMethods);
            lineCoverage = string.Format(coverageFmt,
                                         _namespace.CoverageStats.TotalCoverableLines == 0
                                             ? "N/A"
                                             : (100 * _namespace.CoverageStats.LinesCovered / (float)_namespace.CoverageStats.TotalCoverableLines).ToString(
                                                 CultureInfo.InvariantCulture), _namespace.CoverageStats.LinesCovered,
                                         _namespace.CoverageStats.TotalCoverableLines);

            builder.AppendFormat("<tr><td class=\"name\">{0}</td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", _namespace.Name, classCoverage, methodCoverage, lineCoverage);

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

            foreach (var sourceClass in _namespace.Files)
            {
                classCoverage = string.Format(coverageFmt,
                                              sourceClass.CoverageStats.TotalClasses == 0
                                                  ? "N/A"
                                                  : (100 * sourceClass.CoverageStats.ClassesCovered /
                                                     (float)sourceClass.CoverageStats.TotalClasses).ToString(
                                                         CultureInfo.InvariantCulture),
                                              sourceClass.CoverageStats.ClassesCovered,
                                              sourceClass.CoverageStats.TotalClasses);
                methodCoverage = string.Format(coverageFmt,
                                               sourceClass.CoverageStats.TotalMethods == 0
                                                   ? "N/A"
                                                   : (100 * sourceClass.CoverageStats.MethodsCovered /
                                                      (float)sourceClass.CoverageStats.TotalMethods).ToString(
                                                          CultureInfo.InvariantCulture),
                                               sourceClass.CoverageStats.MethodsCovered,
                                               sourceClass.CoverageStats.TotalMethods);
                lineCoverage = string.Format(coverageFmt,
                                             sourceClass.CoverageStats.TotalCoverableLines == 0
                                                 ? "N/A"
                                                 : (100 * sourceClass.CoverageStats.LinesCovered /
                                                    (float)sourceClass.CoverageStats.TotalCoverableLines).ToString(
                                                        CultureInfo.InvariantCulture),
                                             sourceClass.CoverageStats.LinesCovered,
                                             sourceClass.CoverageStats.TotalCoverableLines);

                builder.AppendFormat("<tr><td class=\"name\"><a href=\"{4}\">{0}</a></td><td class=\"coverageStat\">{1}</td><td class=\"coverageStat\">{2}</td><td class=\"coverageStat\">{3}</td></tr>", sourceClass.Name, classCoverage, methodCoverage, lineCoverage, _pathResolver.RelativePathFromAssemblyToRoot + _pathResolver.GetRelativePathFromRootForFile(sourceClass));
            }
            
            builder.Append("</table>");

            return builder.ToString();

        }
    }
}