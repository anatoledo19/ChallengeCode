using GeneratorDataProcessor;

namespace GeneratorDataProcessor.Tests;
public class MaxEmissionsTests
{
    [Fact]
    public void Constructor_InitializesList()
    {
        // Act
        var maxEmissions = new MaxEmissions();

        // Assert
        Assert.NotNull(maxEmissions.DailyMaxEmissions);
        Assert.Empty(maxEmissions.DailyMaxEmissions); // Should be empty initially
    }

    [Fact]
    public void AddDailyEmission_ValidParameters_AddsEmission()
    {
        // Arrange
        var maxEmissions = new MaxEmissions();
        DateTime date = DateTime.Now;
        string generatorName = "GasGen1";
        double emissions = 100;

        // Act
        maxEmissions.AddDailyEmission(date, generatorName, emissions);

        // Assert
        Assert.Single(maxEmissions.DailyMaxEmissions);
        Assert.Equal(emissions, maxEmissions.DailyMaxEmissions[0].Emission);
    }

    [Fact]
    public void AddDailyEmission_ExistingDate_UpdatesEmission()
    {
        // Arrange
        var maxEmissions = new MaxEmissions();
        DateTime date = DateTime.Now;
        string generatorName = "GasGen1";
        double emissions1 = 100;
        double emissions2 = 150; // Higher emission for the same date

        // Act
        maxEmissions.AddDailyEmission(date, generatorName, emissions1);
        maxEmissions.AddDailyEmission(date, generatorName, emissions2); // This should update the existing entry

        // Assert
        Assert.Single(maxEmissions.DailyMaxEmissions);
        Assert.Equal(emissions2, maxEmissions.DailyMaxEmissions[0].Emission); // Should be updated to the higher value
    }

    [Fact]
    public void AddDailyEmission_NullGeneratorName_ThrowsArgumentException()
    {
        // Arrange
        var maxEmissions = new MaxEmissions();
        DateTime date = DateTime.Now;
        double emissions = 100;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => maxEmissions.AddDailyEmission(date, null, emissions));
        Assert.Equal("Generator name cannot be null or empty. (Parameter 'generatorName')", exception.Message);
    }
}

public class DailyMaxEmissionTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        DateTime date = DateTime.Now;
        string generatorName = "GasGen1";
        double emission = 100;

        // Act
        var dailyMaxEmission = new DailyMaxEmission(date, generatorName, emission);

        // Assert
        Assert.Equal(date, dailyMaxEmission.Date);
        Assert.Equal(generatorName, dailyMaxEmission.GeneratorName);
        Assert.Equal(emission, dailyMaxEmission.Emission);
    }

    [Fact]
    public void Constructor_InvalidDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = default; // Invalid date
        string generatorName = "GasGen1";
        double emission = 100;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => new DailyMaxEmission(date, generatorName, emission));
        Assert.Equal("Date must be a valid DateTime.", exception.Message);
    }

    [Fact]
    public void Constructor_EmptyGeneratorName_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = DateTime.Now;
        string generatorName = ""; // Invalid generator name
        double emission = 100;

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => new DailyMaxEmission(date, generatorName, emission));
        Assert.Equal("Generator name cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Constructor_NegativeEmission_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = DateTime.Now;
        string generatorName = "GasGen1";
        double emission = -100; // Invalid emission

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => new DailyMaxEmission(date, generatorName, emission));
        Assert.Equal("Emission cannot be negative.", exception.Message);
    }
}