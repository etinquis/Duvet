using System;
using System.Collections.Generic;
using System.IO;
using Duvet.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Duvet;
using Duvet.Output.HTML;
using Duvet.Output.HTML.Pages;
using Duvet.Output.HTML.TeamCity;

namespace Duvet.Tests.Integration
{
    [TestFixture]
    class TeamCityHtmlReportPageTests
    {
        private IEnumerable<ISourceLine> MockLines
        {
            get
            {
                Mock<ISourceLine> mockLine = new Mock<ISourceLine>();
                Mock<ICoverageStats> mockStats = new Mock<ICoverageStats>();
                mockStats.Setup(stats => stats.TotalCoverableLines).Returns(1);
                mockStats.Setup(stats => stats.LinesCovered).Returns(1);
                int lineNumber = 1;

                mockLine.Setup(line => line.LineNumber).Returns(lineNumber++);
                mockLine.Setup(line => line.LineContents).Returns("#include <iostream>");
                mockLine.Setup(line => line.CoverageStats).Returns(mockStats.Object);
                mockLine.Setup(line => line.Coverage).Returns(CoverageLevel.Empty);
                yield return mockLine.Object;

                mockLine = new Mock<ISourceLine>();
                mockLine.Setup(line => line.LineNumber).Returns(lineNumber++);
                mockLine.Setup(line => line.LineContents).Returns("");
                mockLine.Setup(line => line.CoverageStats).Returns(mockStats.Object);
                mockLine.Setup(line => line.Coverage).Returns(CoverageLevel.Empty);
                yield return mockLine.Object;

                mockLine = new Mock<ISourceLine>();
                mockLine.Setup(line => line.LineNumber).Returns(lineNumber++);
                mockLine.Setup(line => line.LineContents).Returns("int main(int argc, char **argv) {");
                mockLine.Setup(line => line.CoverageStats).Returns(mockStats.Object);
                mockLine.Setup(line => line.Coverage).Returns(CoverageLevel.FullyCovered);
                yield return mockLine.Object;

                mockLine = new Mock<ISourceLine>();
                mockLine.Setup(line => line.LineNumber).Returns(lineNumber++);
                mockLine.Setup(line => line.LineContents).Returns("    std::cout << \"hello world\" << std::endl;");
                mockLine.Setup(line => line.CoverageStats).Returns(mockStats.Object);
                mockLine.Setup(line => line.Coverage).Returns(CoverageLevel.PartiallyCovered);
                yield return mockLine.Object;

                mockLine = new Mock<ISourceLine>();
                mockLine.Setup(line => line.LineNumber).Returns(lineNumber);
                mockLine.Setup(line => line.LineContents).Returns("}");
                mockLine.Setup(line => line.CoverageStats).Returns(mockStats.Object);
                mockLine.Setup(line => line.Coverage).Returns(CoverageLevel.NotCovered);
                yield return mockLine.Object;
            }
        }

        [Test, Category("Integration")]
        public void Report()
        {
            Mock<ICoverageStats> mockStats = new Mock<ICoverageStats>();
            mockStats.Setup(stats => stats.TotalCoverableLines).Returns(3);
            mockStats.Setup(stats => stats.LinesCovered).Returns(2);

            Mock<ISourceAssembly> mockAssembly = new Mock<ISourceAssembly>();
            Mock<ISourceNamespace> mockNamespace = new Mock<ISourceNamespace>();
            Mock<ISourceFile> mockFile = new Mock<ISourceFile>();
            mockFile.Setup(file => file.Name).Returns("TestFile.cpp");
            mockFile.Setup(file => file.Language).Returns(SourceLanguage.Cpp);
            mockFile.Setup(file => file.CoverageStats).Returns(mockStats.Object);
            mockFile.Setup(file => file.Coverage).Returns(CoverageLevel.PartiallyCovered);
            mockFile.Setup(file => file.Lines).Returns(MockLines);
                
            TeamCityHtmlReportPathResolver resolver =
                new TeamCityHtmlReportPathResolver(new DirectoryInfo(Environment.CurrentDirectory));
            ITeamCityHtmlReportPageContent content = new SourceFileTeamCityHtmlReportPageContent(resolver,
                                                                                                 mockAssembly.Object,
                                                                                                 mockNamespace.Object,
                                                                                                 mockFile.Object);
            TeamCityHtmlReportPage page = new TeamCityHtmlReportPage(content);

            StreamWriter writer = new StreamWriter("test.html");
            page.WriteTo(writer);
            writer.Close();
        }
    }
}
