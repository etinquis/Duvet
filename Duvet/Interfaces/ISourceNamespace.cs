using System.Collections.Generic;

namespace Duvet
{
    public interface ISourceNamespace : ICoverable
    {
        string Name { get; }
        IEnumerable<ISourceClass> Classes { get; }
        IEnumerable<ISourceFile> Files { get; } 
    }
}
