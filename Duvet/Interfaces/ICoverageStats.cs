namespace Duvet
{
    public interface ICoverageStats
    {
        uint TotalMethods { get; }
        uint MethodsCovered { get; }

        uint TotalCoverableLines { get; }
        uint LinesCovered { get; }

        uint TotalClasses { get; }
        uint ClassesCovered { get; }
    }
}
