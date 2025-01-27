using System.Xml.Linq;

namespace GeneratorDataProcessor.Tests;
public class TotalGenerationTests
{
    [Fact]
    public void Constructor_InitializesDictionaries()
    {
        // Act
        var totalGeneration = new TotalGeneration();

        // Assert
        Assert.NotNull(totalGeneration.WindGenerators);
        Assert.NotNull(totalGeneration.GasGenerators);
        Assert.NotNull(totalGeneration.CoalGenerators);
        Assert.Empty(totalGeneration.WindGenerators);
        Assert.Empty(totalGeneration.GasGenerators);
        Assert.Empty(totalGeneration.CoalGenerators);
    }

    [Fact]
    public void CalculateTotalGeneration_ValidData_ReturnsCorrectTotals()
    {
        // Arrange
        var generationData = new GenerationData();
        var referenceData = new Factor(0.5, 1.0, 1.5, 0.1, 0.2, 0.3); // Example reference data

        // Create mock wind generator
        var windGen = new WindGenerator("WindGen1", "Offshore");
        windGen.AddGenerationDay(new GenerationDay(DateTime.Now, 100, 50)); // 100 energy, 50 price
        generationData.AddWindGenerator(windGen);

        // Create mock gas generator
        var gasGen = new FossilsGenerator("GasGen1", 0.5);
        gasGen.AddGenerationDay(new GenerationDay(DateTime.Now, 200, 60)); // 200 energy, 60 price
        generationData.AddGasGenerator(gasGen);

        // Create mock coal generator
        var coalGen = new CoalGenerator("CoalGen1", 0.5, 300, 150); // 300 total heat input, 150 actual net generation
        coalGen.AddGenerationDay(new GenerationDay(DateTime.Now, 100, 60)); // 200 energy, 60 price
        generationData.AddCoalGenerator(coalGen);

        // Act
        var totalGeneration = TotalGeneration.CalculateTotalGeneration(generationData, referenceData);

        // Assert
        Assert.Equal(2500, totalGeneration.WindGenerators["WindGen1"]); // 100 * 50 * value factor for wind
        Assert.Equal(12000, totalGeneration.GasGenerators["GasGen1"]); // 200 * 60 * value factor for gas
        Assert.Equal(6000, totalGeneration.CoalGenerators["CoalGen1"]); // Assuming a direct total for coal
    }

    [Fact]
    public void AddWindGeneratorTotal_AddsGeneratorTotal()
    {
        // Arrange
        var totalGeneration = new TotalGeneration();
        string name = "WindGen1";
        double total = 1000;

        // Act
        totalGeneration.AddWindGeneratorTotal(name, total);

        // Assert
        Assert.Equal(total, totalGeneration.WindGenerators[name]);
    }

    [Fact]
    public void AddGasGeneratorTotal_AddsGeneratorTotal()
    {
        // Arrange
        var totalGeneration = new TotalGeneration();
        string name = "GasGen1";
        double total = 2000;

        // Act
        totalGeneration.AddGasGeneratorTotal(name, total);

        // Assert
        Assert.Equal(total, totalGeneration.GasGenerators[name]);
    }

    [Fact]
    public void AddCoalGeneratorTotal_AddsGeneratorTotal()
    {
        // Arrange
        var totalGeneration = new TotalGeneration();
        string name = "CoalGen1";
        double total = 3000;

        // Act
        totalGeneration.AddCoalGeneratorTotal(name, total);

        // Assert
        Assert.Equal(total, totalGeneration.CoalGenerators[name]);
    }

    [Fact]
    public void CreateOutputXmlTotalGeneration_ReturnsCorrectXml()
    {
        // Arrange
        var totalGeneration = new TotalGeneration();
        totalGeneration.AddWindGeneratorTotal("WindGen1", 1000);
        totalGeneration.AddGasGeneratorTotal("GasGen1", 2000);
        totalGeneration.AddCoalGeneratorTotal("CoalGen1", 3000);

        // Act
        var xmlOutput = totalGeneration.CreateOutputXmlTotalGeneration();

        // Assert
        Assert.NotNull(xmlOutput);
        Assert.Equal("Totals", xmlOutput.Name);

        // Check each section
        AssertSection(xmlOutput, "Wind", "WindGen1", "1000");
        AssertSection(xmlOutput, "Coal", "CoalGen1", "3000");
        AssertSection(xmlOutput, "Gas", "GasGen1", "2000");
    }

    private void AssertSection(XElement xmlOutput, string sectionName, string expectedName, string expectedTotal)
    {
        var sectionElement = xmlOutput.Element(sectionName);
        Assert.NotNull(sectionElement);
        Assert.Single(sectionElement.Elements("Generator"));

        var generatorElement = sectionElement.Element("Generator");
        Assert.Equal(expectedName, generatorElement.Element("Name").Value);
        Assert.Equal(expectedTotal, generatorElement.Element("Total").Value); // Ensure the total is a string
    }
}