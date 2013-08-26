using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duvet.Output.HTML.TeamCity
{
    class TeamCitySourceNamespaceHtmlReport
    {
        private ISourceAssembly _assembly;
        private ISourceNamespace _namespace;
        public TeamCitySourceNamespaceHtmlReport(ISourceAssembly assembly, ISourceNamespace nspace)
        {
            _assembly = assembly;
            _namespace = nspace;
        }
    }
}
