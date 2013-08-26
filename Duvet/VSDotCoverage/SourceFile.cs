using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duvet.Generic;
using Microsoft.VisualStudio.Coverage.Analysis;

namespace Duvet.VSDotCoverage
{
    class SourceFile : ISourceFile
    {
        private CoverageDS _coverage;
        private CoverageDSPriv.SourceFileNamesRow _row;
        private string _filename;
        private IEnumerable<CoverageDSPriv.LinesRow> _linesRows;
        private IEnumerable<CoverageDSPriv.ClassRow> _classRows; 

        private CoverageStats _coverageStats;

        public SourceFile(CoverageDS coverage, CoverageDSPriv.SourceFileNamesRow nameRow)
        {
            _coverage = coverage;
            _filename = nameRow.SourceFileName;
            _row = nameRow;
            _linesRows = coverage.Lines.Where(line => line.SourceFileID == _row.SourceFileID).OrderBy(line=>line.LnStart);
            _classRows =
                coverage.Class.Where(
                    cls =>
                    cls.GetMethodRows()
                       .Any(method => method.GetLinesRows().Any(line => line.SourceFileID == nameRow.SourceFileID)));

            FileInfo info = new FileInfo(_filename);
            Name = info.Name;

            Language = SourceLanguageInferrer.InferFrom(_filename);

            _coverageStats = new CoverageStats();
        }

        public string Name { get; private set; }

        public IEnumerable<ISourceLine> Lines
        {
            get
            {
                int currentLineNumber = 1;
                using (StreamReader reader = new StreamReader(_filename))
                {
                    foreach (var linesRow in _linesRows)
                    {
                        string lineContent;
                        SourceLine line;
                        while (currentLineNumber < linesRow.LnStart)
                        {
                            lineContent = reader.ReadLine();
                            line = new SourceLine(currentLineNumber++, lineContent, CoverageLevel.Empty, new CoverageStats());

                            yield return line;
                        }

                        if (currentLineNumber == linesRow.LnStart)
                        {
                            CoverageLevel coverage = CoverageLevel.Empty;
                            switch (linesRow.Coverage)
                            {
                                case 0:
                                    coverage = CoverageLevel.FullyCovered;
                                    _coverageStats.LinesCovered++;
                                    break;
                                case 1:
                                    coverage = CoverageLevel.PartiallyCovered;
                                    _coverageStats.LinesCovered++;
                                    break;
                                case 2:
                                    coverage = CoverageLevel.NotCovered;
                                    break;
                            }

                            lineContent = reader.ReadLine();
                            line = new SourceLine(currentLineNumber++, lineContent, coverage, new CoverageStats(linesRow));

                            _coverageStats.TotalCoverableLines++;
                            yield return line;
                        }
                    }

                    while (reader.Peek() != -1)
                    {
                        string lineContent = reader.ReadLine();
                        SourceLine line = new SourceLine(currentLineNumber++, lineContent, CoverageLevel.Empty, new CoverageStats());

                        yield return line;

                    }
                }
            }
        }

        public SourceLanguage Language { get; private set; }
        public ICoverageStats CoverageStats { get { return _coverageStats; } }
        public CoverageLevel Coverage { get { return CoverageStatCoverter.ParseStats(CoverageStats); } }

        public IEnumerable<ISourceClass> Classes
        {
            get
            {
                foreach (var classRow in _classRows)
                {
                    yield return new SourceClass(_coverage, classRow);
                }
            }
        }
    }
}
