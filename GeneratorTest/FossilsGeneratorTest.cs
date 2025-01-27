namespace GeneratorDataProcessor.Tests;
public class FossilsGeneratorTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        string name = "GasGen1";
        double emissionsRating = 0.5;

        // Act
        var fossilsGenerator = new FossilsGenerator(name, emissionsRating);

        // Assert
        Assert.Equal(name, fossilsGenerator.Name);
        Assert.Equal(emissionsRating, fossilsGenerator.EmissionsRating);
    }

    [Fact]
    public void Constructor_NegativeEmissionsRating_ThrowsArgumentException()
    {
        // Arrange
        string name = "GasGen1";
        double emissionsRating = -0.5; // Invalid emissions rating

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new FossilsGenerator(name, emissionsRating));
        Assert.Equal("Emissions rating cannot be negative.", exception.Message);
    }

    [Fact]
    public void CalculateTotalFossilGenerator_ValidReferenceData_ReturnsCorrectValue()
    {
        // Arrange
        var fossilsGenerator = new FossilsGenerator("GasGen1", 0.5);
        var referenceData = new Factor(0.5, 1.0, 1.5, 0.1, 0.2, 0.3); // Example reference data

        // Add some generation days
        fossilsGenerator.AddGenerationDay(new GenerationDay(DateTime.Now, 100, 50)); // 100 energy, 50 price
        fossilsGenerator.AddGenerationDay(new GenerationDay(DateTime.Now.AddDays(1), 200, 60)); // 200 energy, 60 price

        // Act
        double totalGeneration = fossilsGenerator.CalculateTotalFossilGenerator(referenceData);

        // Assert
        Assert.Equal(17000, totalGeneration, 0.001); // Adjust the expected value based on your calculation logic
    }

    [Fact]
    public void CalculateTotalFossilGenerator_NullReferenceData_ThrowsArgumentNullException()
    {
        // Arrange
        var fossilsGenerator = new FossilsGenerator("GasGen1", 0.5);

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => fossilsGenerator.CalculateTotalFossilGenerator(null));
        Assert.Equal("Reference data cannot be null.", exception.Message);
    }
}

