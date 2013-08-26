using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class SourceNamespace : ISourceNamespace
    {
        private readonly CoverageDS _coverage;
        private readonly CoverageDSPriv.NamespaceTableRow _namespace;
        public SourceNamespace(CoverageDS coverage, CoverageDSPriv.NamespaceTableRow namespaceTableRow)
        {
            _coverage = coverage;
            _namespace = namespaceTableRow;
            CoverageStats = new CoverageStats(namespaceTableRow);
            Name = namespaceTableRow.NamespaceName;

            if (string.IsNullOrEmpty(Name)) Name = "::";
        }

        public ICoverageStats CoverageStats { get; private set; }

        public CoverageLevel Coverage
        {
            get { return CoverageStatCoverter.ParseStats(CoverageStats); }
        }

        public string Name { get; private set; }

        public IEnumerable<ISourceClass> Classes
        {
            get
            {
                foreach (var classRow in _namespace.GetClassRows())
                {
                    yield return new SourceClass(_coverage, classRow);
                }
            }
        }

        public IEnumerable<ISourceFile> Files
        {
            get
            {
                HashSet<uint> files = new HashSet<uint>();
                foreach (var classRow in _namespace.GetClassRows())
                {
                    foreach (var methodRow in classRow.GetMethodRows())
                    {
                        foreach (var file in methodRow.GetLinesRows().Select(line=>line.SourceFileID).Distinct())
                        {
                            files.Add(file);
                        }
                    }
                }

                foreach (var file in files)
                {
                    yield return new SourceFile(_coverage, _coverage.SourceFileNames.First(f=>f.SourceFileID == file));
                }
            }
        }
    }
}