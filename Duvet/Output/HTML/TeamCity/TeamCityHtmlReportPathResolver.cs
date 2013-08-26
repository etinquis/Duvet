using System.IO;

namespace Duvet.Output.HTML
{
    public class TeamCityHtmlReportPathResolver
    {
        private DirectoryInfo _root;
        public TeamCityHtmlReportPathResolver(DirectoryInfo reportRoot)
        {
            _root = reportRoot;
        }

        public string RelativePathFromFileToRoot
        {
            get { return ""; }
        }

        public string RelativePathFromNamespaceToRoot
        {
            get { return ""; }
        }

        public string RelativePathFromAssemblyToRoot
        {
            get { return ""; }
        }

        public string GetRelativePathFromRootForFile(ISourceFile file)
        {
            return file.Name + ".html";
        }

        public string GetRelativePathFromRootForNamespace(ISourceNamespace nspace)
        {
            return nspace.Name.Replace(':', '_') + ".namespace.html";
        }

        public string GetRelativePathFromRootForAssembly(ISourceAssembly assembly)
        {
            return assembly.Name + ".assembly.html";
        }

        public string GetRelativePathForRoot()
        {
            return "";
        }

        public string GetRelativePathFromRootForIndex()
        {
            return "index.html";
        }

    }
}
