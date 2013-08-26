using System.Collections.Generic;

namespace Duvet
{
    public interface ISourceFile : ICoverable
    {
        /// <summary>
        /// File name with extension
        /// </summary>
        string Name { get; }
        IEnumerable<ISourceLine> Lines { get; }
        IEnumerable<ISourceClass> Classes { get; }
        SourceLanguage Language { get; }
    }
}
