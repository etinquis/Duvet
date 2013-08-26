using System.Collections.Generic;

namespace Duvet
{
    public interface ISourceClass : ICoverable
    {
        string Name { get; }
        IEnumerable<ISourceMethod> Methods { get; }
    }
}
