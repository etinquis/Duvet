using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class SourceClass : ISourceClass
    {
        private CoverageDS _coverage;
        private CoverageDSPriv.ClassRow _row;
        private IEnumerable<CoverageDSPriv.MethodRow> _methodRows; 
        public SourceClass(CoverageDS coverage, CoverageDSPriv.ClassRow classRow)
        {
            CoverageStats = new CoverageStats(classRow);
            Name = classRow.ClassName;
            _methodRows = classRow.GetMethodRows();
        }

        public ICoverageStats CoverageStats { get; private set; }
        public CoverageLevel Coverage { get { return CoverageStatCoverter.ParseStats(CoverageStats); } }
        public string Name { get; private set; }

        public IEnumerable<ISourceMethod> Methods
        {
            get
            {
                foreach (var methodRow in _methodRows)
                {
                    yield return new SourceMethod(methodRow);
                }
            }
        }
    }
}
