namespace Duvet.Generic
{
    class SourceLine : ISourceLine
    {
        public SourceLine(int lineNum, string line, CoverageLevel coverage, ICoverageStats stats)
        {
            LineNumber = lineNum;
            LineContents = line;
            CoverageStats = stats;
        }

        public int LineNumber { get; private set; }
        public string LineContents { get; private set; }
        public ICoverageStats CoverageStats { get; private set; }
        public CoverageLevel Coverage { get { return CoverageStatCoverter.ParseStats(CoverageStats); } }
    }
}
