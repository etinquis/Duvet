using System.Collections.Generic;
using System.IO;
using Duvet.Output.HTML.Pages;
using Duvet.Output.HTML.TeamCity;

namespace Duvet.Output.HTML
{
    public class TeamCityHtmlReport
    {
        private IEnumerable<ISourceAssembly> _assemblies;

        public TeamCityHtmlReport(IEnumerable<ISourceAssembly> assemblies)
        {
            _assemblies = assemblies;
        }

        public void WriteTo(DirectoryInfo directory)
        {
            DirectoryInfo css = new DirectoryInfo(Path.Combine(directory.FullName, ".css"));
            css.Create();
            DirectoryInfo js = new DirectoryInfo(Path.Combine(directory.FullName, ".js"));
            js.Create();

            string coverageCss = Path.Combine(css.FullName, "coverage.css");
            using(var coverage = new StreamWriter(coverageCss))
            {
                coverage.Write(Resources.coverage);
            }
            string ideaCss = Path.Combine(css.FullName, "idea.css");
            using (var idea = new StreamWriter(ideaCss))
            {
                idea.Write(Resources.idea);
            }

            string highlightJs = Path.Combine(js.FullName, "highlight.pack.js");
            using (var highlight = new StreamWriter(highlightJs))
            {
                highlight.Write(Resources.highlight_pack);
            }
            

            TeamCityHtmlReportPathResolver resolver = new TeamCityHtmlReportPathResolver(directory);

            IndexTeamCityHtmlReportPageContent indexContent = new IndexTeamCityHtmlReportPageContent(resolver, _assemblies);
            TeamCityHtmlReportPage indexPage = new TeamCityHtmlReportPage(indexContent);

            using (
                var writer =
                    new StreamWriter(Path.Combine(directory.FullName,
                                                  resolver.GetRelativePathFromRootForIndex())))
            {
                indexPage.WriteTo(writer);
            }

            foreach (var sourceAssembly in _assemblies)
            {
                SourceAssemblyTeamCityHtmlReportPageContent assemblyContent = new SourceAssemblyTeamCityHtmlReportPageContent(resolver, sourceAssembly);
                TeamCityHtmlReportPage assemblyPage = new TeamCityHtmlReportPage(assemblyContent);

                using (
                    var writer =
                        new StreamWriter(Path.Combine(directory.FullName,
                                                      resolver.GetRelativePathFromRootForAssembly(sourceAssembly))))
                {
                    assemblyPage.WriteTo(writer);
                }

                foreach (var sourceNamespace in sourceAssembly.Namespaces)
                {
                    SourceNamespaceTeamCityHtmlReportPageContent namespaceContent = new SourceNamespaceTeamCityHtmlReportPageContent(resolver, sourceAssembly, sourceNamespace);
                    TeamCityHtmlReportPage namespacePage = new TeamCityHtmlReportPage(namespaceContent);

                    using (
                        var writer =
                            new StreamWriter(Path.Combine(directory.FullName,
                                                          resolver.GetRelativePathFromRootForNamespace(sourceNamespace))))
                    {
                        namespacePage.WriteTo(writer);
                    }

                    foreach (var sourceFile in sourceNamespace.Files)
                    {
                        SourceFileTeamCityHtmlReportPageContent fileContent = new SourceFileTeamCityHtmlReportPageContent(resolver, sourceAssembly, sourceNamespace, sourceFile);
                        TeamCityHtmlReportPage filePage = new TeamCityHtmlReportPage(fileContent);

                        using (
                            var writer =
                                new StreamWriter(Path.Combine(directory.FullName,
                                                              resolver.GetRelativePathFromRootForFile(sourceFile))))
                        {
                            filePage.WriteTo(writer);
                        }
                    }
                }
            }
        }
    }
}
