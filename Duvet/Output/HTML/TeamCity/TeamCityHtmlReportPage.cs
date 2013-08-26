using System;
using System.IO;
using System.Text;
using Duvet.Output.HTML.Pages;

namespace Duvet.Output.HTML
{
    public class TeamCityHtmlReportPage
    {
        private ITeamCityHtmlReportPageContent _content;
        public TeamCityHtmlReportPage(ITeamCityHtmlReportPageContent content)
        {
            _content = content;
        }

        public void WriteTo(TextWriter writer)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<html id=\"htmlId\">");
            builder.Append("<head>");
            builder.AppendFormat("<title>{0}</title>", "Coverage Report");
            builder.AppendFormat("<style type=\"text/css\">{0}</style>",
                                 "@import \".css/coverage.css\"; @import \".css/idea.css\";");
            builder.AppendFormat("<script type=\"text/javascript\" src=\".js/highlight.pack.js\"></script>");
            builder.Append("</head>");
            builder.Append("<body>");

            builder.Append("<div class=\"content\">");

            builder.Append(_content.BuildBreadCrumbs());
            builder.AppendFormat("<h1>Coverage Summary</h1>");

            builder.Append(_content.BuildCoverageStatsTable());

            builder.Append(_content.BuildContent());

            builder.Append("</div>");
            builder.AppendFormat("<script type=\"text/javascript\">{0}</script>", "(function() {var codeBlock = document.getElementById('sourceCode');if (codeBlock) {hljs.highlightBlock(codeBlock);}})();");
            builder.Append("</body>");
            builder.Append("</html>");

            writer.Write(builder.ToString());
        }
    }
}
