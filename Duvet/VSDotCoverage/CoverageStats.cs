using System.Linq;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class CoverageStats : ICoverageStats
    {
        public CoverageStats()
        {
            TotalMethods = 0;
            TotalClasses = 0;
            TotalCoverableLines = 0;

            MethodsCovered = 0;
            LinesCovered = 0;
            ClassesCovered = 0;
        }

        public CoverageStats(CoverageDSPriv.LinesRow lineRow)
            : this()
        {
            TotalMethods = 0;
            MethodsCovered = 0;

            TotalClasses = 0;
            ClassesCovered = 0;

            TotalCoverableLines = 1;
            CoverageLevel coverage = LineCoverageConverter.Convert(lineRow.Coverage);
            LinesCovered = (uint) (coverage == CoverageLevel.FullyCovered || coverage == CoverageLevel.PartiallyCovered ? 1 : 0);
        }

        public CoverageStats(CoverageDSPriv.ClassRow classRow)
            : this()
        {
            TotalMethods = (uint) classRow.GetMethodRows().Count();
            MethodsCovered = (uint) classRow.GetMethodRows().Sum(method=>method.LinesCovered + method.LinesPartiallyCovered > 0 ? 1 : 0);

            TotalCoverableLines = classRow.LinesCovered + classRow.LinesNotCovered + classRow.LinesPartiallyCovered;
            LinesCovered = classRow.LinesCovered + classRow.LinesPartiallyCovered;

            TotalClasses = 1;
            if (MethodsCovered == 0 && LinesCovered == 0)
            {
                ClassesCovered = 0;
            }
            else
            {
                ClassesCovered = 1;
            }
        }

        public CoverageStats(CoverageDSPriv.MethodRow methodRow)
            : this()
        {
            TotalCoverableLines = methodRow.LinesCovered + methodRow.LinesNotCovered + methodRow.LinesPartiallyCovered;
            LinesCovered = methodRow.LinesCovered + methodRow.LinesPartiallyCovered;

            TotalMethods = 1;
            MethodsCovered = (uint) (LinesCovered == TotalCoverableLines ? 1 : 0);
        }

        public CoverageStats(CoverageDSPriv.ModuleRow module)
            : this()
        {
            TotalCoverableLines = module.LinesCovered + module.LinesNotCovered + module.LinesPartiallyCovered;
            LinesCovered = module.LinesCovered + module.LinesPartiallyCovered;

            foreach (var namespaceTableRow in module.GetNamespaceTableRows())
            {
                TotalClasses += (uint) (namespaceTableRow.GetClassRows().Count());
                foreach (var classRow in namespaceTableRow.GetClassRows())
                {
                    TotalMethods += (uint) (classRow.GetMethodRows().Count());
                    MethodsCovered += (uint) (classRow.GetMethodRows().Count(method => method.LinesNotCovered == 0));
                    ClassesCovered += (uint)(classRow.LinesCovered == 0 ? 1 : 0);
                }
            }
        }

        public CoverageStats(CoverageDSPriv.NamespaceTableRow namespaceTableRow)
        {
            TotalCoverableLines = namespaceTableRow.LinesCovered + namespaceTableRow.LinesNotCovered +
                                  namespaceTableRow.LinesPartiallyCovered;
            LinesCovered = namespaceTableRow.LinesCovered + namespaceTableRow.LinesPartiallyCovered;

            TotalClasses = (uint)(namespaceTableRow.GetClassRows().Count());
            foreach (var classRow in namespaceTableRow.GetClassRows())
            {
                TotalMethods += (uint)(classRow.GetMethodRows().Count());
                MethodsCovered += (uint)(classRow.GetMethodRows().Count(method => method.LinesNotCovered == 0));
                ClassesCovered += (uint)(classRow.LinesCovered == 0 ? 1 : 0);
            }
        }

        public uint TotalMethods { get; set; }
        public uint MethodsCovered { get; set; }
        public uint TotalCoverableLines { get; set; }
        public uint LinesCovered { get; set; }
        public uint TotalClasses { get; set; }
        public uint ClassesCovered { get; set; }
    }
}
