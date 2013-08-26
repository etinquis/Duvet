using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class SourceMethod : ISourceMethod
    {
        public SourceMethod(CoverageDSPriv.MethodRow row)
        {
            CoverageStats = new CoverageStats(row);
            Name = row.MethodName;
        }

        public ICoverageStats CoverageStats { get; private set; }

        public CoverageLevel Coverage
        {
            get { return CoverageStatCoverter.ParseStats(CoverageStats); }
        }

        public string Name { get; private set; }
    }
}
