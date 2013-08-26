namespace Duvet.Output.HTML.Pages
{
    public interface ITeamCityHtmlReportPageContent
    {
        string BuildBreadCrumbs();

        string BuildCoverageStatsTable();

        string BuildContent();
    }
}