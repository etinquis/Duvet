using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duvet.VSDotCoverage
{
    class LineCoverageConverter
    {
        public static CoverageLevel Convert(uint lineRowCoverage)
        {
            switch (lineRowCoverage)
            {
                case 0:
                    return CoverageLevel.FullyCovered;
                case 1:
                    return CoverageLevel.PartiallyCovered;
                default:
                    return CoverageLevel.NotCovered;
            }
        }
    }
}
