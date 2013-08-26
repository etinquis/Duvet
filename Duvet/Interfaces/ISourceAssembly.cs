using System.Collections.Generic;

namespace Duvet
{
    public interface ISourceAssembly : ICoverable
    {
        string Name { get; }
        IEnumerable<ISourceNamespace> Namespaces { get; }
        IEnumerable<ISourceFile> Files { get; }
    }
}
