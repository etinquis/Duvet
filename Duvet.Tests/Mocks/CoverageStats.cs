using Duvet;

namespace Duvet.Tests.Mocks
{
    internal class CoverageStats : ICoverageStats
    {
        public uint TotalMethods { get; private set; }
        public uint MethodsCovered { get; private set; }
        public uint TotalCoverableLines { get; private set; }
        public uint LinesCovered { get; private set; }
        public uint TotalClasses { get; private set; }
        public uint ClassesCovered { get; private set; }
    }
}