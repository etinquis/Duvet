using System.Collections.Generic;
using System.Linq;
using Duvet;

namespace Duvet.Generic
{
    class SourceFile : ISourceFile
    {
        public SourceFile(string fileName, IEnumerable<ISourceLine> lines, IEnumerable<ISourceClass> classes, SourceLanguage language, ICoverageStats stats)
        {
            Name = fileName;
            var sourceLines = lines as ISourceLine[] ?? lines.ToArray();
            Lines = sourceLines;
            Classes = classes;
            Language = language;
            CoverageStats = stats;
        }

        public string Name { get; private set; }
        public IEnumerable<ISourceLine> Lines { get; private set; }
        public IEnumerable<ISourceClass> Classes { get; private set; }
        public SourceLanguage Language { get; private set; }
        public ICoverageStats CoverageStats { get; private set; }
        public CoverageLevel Coverage { get { return CoverageStatCoverter.ParseStats(CoverageStats); } }
    }
}
