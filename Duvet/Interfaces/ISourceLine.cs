namespace Duvet
{
    public interface ISourceLine : ICoverable
    {
        int LineNumber { get; }
        string LineContents { get; }
    }
}
