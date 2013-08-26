using System;

namespace Duvet
{
    public class CoverageLevel
    {
        private CoverageLevel()
        {
            
        }

        public static CoverageLevel Empty = new CoverageLevel();
        public static CoverageLevel NotCovered = new CoverageLevel();
        public static CoverageLevel PartiallyCovered = new CoverageLevel();
        public static CoverageLevel FullyCovered = new CoverageLevel();

        public static CoverageLevel operator|(CoverageLevel one, CoverageLevel two)
        {
            if (one == Empty)
            {
                return two;
            }

            if (one == NotCovered)
            {
                if (two == Empty) return one;
                if (two == NotCovered) return one;

                return PartiallyCovered;
            }

            if (one == PartiallyCovered)
            {
                return PartiallyCovered;
            }

            if (one == FullyCovered)
            {
                if (two == Empty) return one;
                if (two == FullyCovered) return FullyCovered;
                return PartiallyCovered;
            }

            return Empty;
        }

        public override string ToString()
        {
            if (this == PartiallyCovered) return "Partially Covered";
            if (this == FullyCovered) return "Fully Covered";
            if (this == NotCovered) return "Not Covered";
            if (this == Empty) return "No Coverage Information";
            return base.ToString();
        }
    }
}
