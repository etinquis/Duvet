using System.Collections.Generic;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class CoverageDsParser
    {
        private string _coverageFile;
        private CoverageDS _summary;

        public IEnumerable<ISourceAssembly> Assemblies
        {
            get
            {
                foreach (CoverageDSPriv.ModuleRow moduleRow in _summary.Module)
                {
                    yield return new SourceAssembly(_summary, moduleRow);
                } 
            }
        }

        public CoverageDsParser(string dotCoveragePath)
        {
            _coverageFile = dotCoveragePath;
            CoverageInfo ci = CoverageInfo.CreateFromFile(_coverageFile);

            _summary = ci.BuildDataSet(null);
        }
    }
}
