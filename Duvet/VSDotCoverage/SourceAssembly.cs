using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class SourceAssembly : ISourceAssembly
    {
        private CoverageDS _coverage;
        private CoverageDSPriv.ModuleRow _row;
        public SourceAssembly(CoverageDS coverage, CoverageDSPriv.ModuleRow module)
        {
            _coverage = coverage;
            _row = module;
            CoverageStats = new CoverageStats(module);

            Name = module.ModuleName;
        }

        public ICoverageStats CoverageStats { get; private set; }

        public CoverageLevel Coverage
        {
            get { return CoverageStatCoverter.ParseStats(CoverageStats); }
        }

        public string Name { get; private set; }

        public IEnumerable<ISourceNamespace> Namespaces
        {
            get
            {
                foreach (var namespaceTableRow in _row.GetNamespaceTableRows())
                {
                    yield return new SourceNamespace(_coverage, namespaceTableRow);
                }
            }
        }

        public IEnumerable<ISourceFile> Files
        {
            get
            {
                foreach (var namespaceRow in _row.GetNamespaceTableRows())
                {
                    foreach (var classRow in namespaceRow.GetClassRows())
                    {
                        foreach (var methodRow in classRow.GetMethodRows())
                        {
                            foreach (var file in methodRow.GetLinesRows().Select(line=>line.SourceFileID).Distinct())
                            {
                                yield return new SourceFile(_coverage, _coverage.SourceFileNames.First(f=>f.SourceFileID == file));
                            }
                        }
                    }
                }
            }
        }
    }
}