using System.Xml.Linq;

namespace GeneratorDataProcessor.Tests;
public class GenerationDataTests
{
    [Fact]
    public void Constructor_InitializesLists()
    {
        // Act
        var generationData = new GenerationData();

        // Assert
        Assert.NotNull(generationData.WindGenerators);
        Assert.NotNull(generationData.GasGenerators);
        Assert.NotNull(generationData.CoalGenerators);
        Assert.Empty(generationData.WindGenerators);
        Assert.Empty(generationData.GasGenerators);
        Assert.Empty(generationData.CoalGenerators);
    }

    [Fact]
    public void LoadGenerationReport_ValidXml_ReturnsGenerationData()
    {
        // Arrange
        string xmlFilePath = "TestGenerationData.xml"; // Path to the test XML file
        CreateTestXmlFile(xmlFilePath); // Create a test XML file

        // Act
        var generationData = GenerationData.LoadGenerationReport(xmlFilePath);

        // Assert
        Assert.NotNull(generationData);
        Assert.Single(generationData.WindGenerators);
        Assert.Single(generationData.GasGenerators);
        Assert.Single(generationData.CoalGenerators);

        // Clean up
        File.Delete(xmlFilePath);
    }

    [Fact]
    public void LoadGenerationReport_InvalidXml_EmptyGenerators()
    {
        // Arrange
        string xmlFilePath = "InvalidGenerationData.xml"; // Path to the invalid XML file
        File.WriteAllText(xmlFilePath, "<Root></Root>"); // Create an invalid XML file

        // Act
        var generationData = GenerationData.LoadGenerationReport(xmlFilePath);

        // Assert
        Assert.NotNull(generationData);
        Assert.Empty(generationData.WindGenerators);
        Assert.Empty(generationData.GasGenerators);
        Assert.Empty(generationData.CoalGenerators);

        // Clean up
        File.Delete(xmlFilePath);
    }

    private void CreateTestXmlFile(string filePath)
    {
        var xmlContent = new XElement("GenerationData",
            new XElement("WindGenerator",
                new XElement("Name", "WindGen1"),
                new XElement("Location", "Offshore"),
                new XElement("Day",
                    new XElement("Date", DateTime.Now.ToString("o")),
                    new XElement("Energy", 100),
                    new XElement("Price", 50)
                )
            ),
            new XElement("GasGenerator",
                new XElement("Name", "GasGen1"),
                new XElement("EmissionsRating", 0.5),
                new XElement("Day",
                    new XElement("Date", DateTime.Now.ToString("o")),
                    new XElement("Energy", 200),
                    new XElement("Price", 60)
                )
            ),
            new XElement("CoalGenerator",
                new XElement("Name", "CoalGen1"),
                new XElement("TotalHeatInput", 300),
                new XElement("ActualNetGeneration", 150),
                new XElement("EmissionsRating", 0.5),
                new XElement("Day",
                    new XElement("Date", DateTime.Now.ToString("o")),
                    new XElement("Energy", 300),
                    new XElement("Price", 70)
                )
            )
        );

        xmlContent.Save(filePath);
    }
}
