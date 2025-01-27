namespace GeneratorDataProcessor.Tests;
public class CoalGeneratorTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        string name = "CoalGen1";
        double emissionsRating = 0.5;
        double totalHeatInput = 300;
        double actualNetGeneration = 150;

        // Act
        var coalGenerator = new CoalGenerator(name, emissionsRating, totalHeatInput, actualNetGeneration);

        // Assert
        Assert.Equal(name, coalGenerator.Name);
        Assert.Equal(emissionsRating, coalGenerator.EmissionsRating);
        Assert.Equal(totalHeatInput, coalGenerator.TotalHeatInput);
        Assert.Equal(actualNetGeneration, coalGenerator.ActualNetGeneration);
    }

    [Fact]
    public void Constructor_NegativeTotalHeatInput_ThrowsArgumentException()
    {
        // Arrange
        string name = "CoalGen1";
        double emissionsRating = 0.5;
        double totalHeatInput = -300; // Invalid total heat input
        double actualNetGeneration = 150;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new CoalGenerator(name, emissionsRating, totalHeatInput, actualNetGeneration));
        Assert.Equal("Total heat input cannot be negative. (Parameter 'value')", exception.Message);
    }

    [Fact]
    public void Constructor_NegativeActualNetGeneration_ThrowsArgumentException()
    {
        // Arrange
        string name = "CoalGen1";
        double emissionsRating = 0.5;
        double totalHeatInput = 300;
        double actualNetGeneration = -150; // Invalid actual net generation

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new CoalGenerator(name, emissionsRating, totalHeatInput, actualNetGeneration));
        Assert.Equal("Actual net generation cannot be negative. (Parameter 'value')", exception.Message);
    }
}