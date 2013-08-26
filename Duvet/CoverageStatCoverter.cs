using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duvet
{
    class CoverageStatCoverter
    {
        public static CoverageLevel ParseStats(ICoverageStats coverageStats)
        {

            if (coverageStats.MethodsCovered == 0
                && coverageStats.ClassesCovered == 0
                && coverageStats.LinesCovered == 0)
            {
                if (coverageStats.TotalMethods == 0 && coverageStats.TotalClasses == 0 &&
                    coverageStats.TotalCoverableLines == 0)
                {
                    return CoverageLevel.Empty;
                }
                else
                {
                    return CoverageLevel.NotCovered;
                }
            }
            else if (coverageStats.MethodsCovered == coverageStats.TotalMethods
                     && coverageStats.ClassesCovered == coverageStats.TotalClasses
                     && coverageStats.LinesCovered == coverageStats.TotalCoverableLines)
            {
                return CoverageLevel.FullyCovered;
            }
            else
            {
                return CoverageLevel.PartiallyCovered;
            }
        }
    }
}
