namespace Duvet
{
    public interface ICoverable
    {
        ICoverageStats CoverageStats { get; }
        CoverageLevel Coverage { get; }
    }
}
