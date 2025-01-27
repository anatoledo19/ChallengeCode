
using System;
using Xunit;

namespace GeneratorDataProcessor.Tests;
public class WindGeneratorTests
{
    [Fact]
    public void Constructor_ValidLocation_SetsLocation()
    {
        // Arrange
        string name = "WindGen1";
        string location = "Onshore";

        // Act
        var windGenerator = new WindGenerator(name, location);

        // Assert
        Assert.Equal(location, windGenerator.Location);
    }

    [Fact]
    public void Constructor_NullLocation_ThrowsArgumentException()
    {
        // Arrange
        string name = "WindGen1";
        string location = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WindGenerator(name, location));
        Assert.Equal("Location cannot be null or empty. (Parameter 'location')", exception.Message);
    }

    [Fact]
    public void Constructor_EmptyLocation_ThrowsArgumentException()
    {
        // Arrange
        string name = "WindGen1";
        string location = "";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WindGenerator(name, location));
        Assert.Equal("Location cannot be null or empty. (Parameter 'location')", exception.Message);
    }

    [Fact]
    public void CalculateTotalWindGeneration_NullReferenceData_ThrowsArgumentNullException()
    {
        // Arrange
        var windGenerator = new WindGenerator("WindGen1", "Onshore");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => windGenerator.CalculateTotalWindGeneration(null));
        Assert.Equal("Reference data cannot be null. (Parameter 'referenceData')", exception.Message);
    }

    [Fact]
    public void CalculateTotalWindGeneration_ValidData_CalculatesCorrectly()
    {
        // Arrange
        var windGenerator = new WindGenerator("WindGen1", "Offshore");
        var referenceData = new Factor(0.5, 1.0, 1.5, 0.1, 0.2, 0.3);

        // Add some mock generation days
        windGenerator.AddGenerationDay(new GenerationDay(DateTime.Now, 100, 10));
        windGenerator.AddGenerationDay(new GenerationDay(DateTime.Now.AddDays(1), 200, 20));

        // Act
        double totalGeneration = windGenerator.CalculateTotalWindGeneration(referenceData);

        // Assert
        Assert.Equal(5000, totalGeneration); // (100 * 10 * 0.5) + (200 * 20 * 0.5) = 5000
    }

    [Fact]
    public void AddGenerationDay_NullGenerationDay_ThrowsArgumentNullException()
    {
        // Arrange
        var windGenerator = new WindGenerator("WindGen1", "Onshore");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => windGenerator.AddGenerationDay(null));
        Assert.Equal("Generation day cannot be null. (Parameter 'generationDay')", exception.Message);
    }

    [Fact]
    public void AddGenerationDay_ValidGenerationDay_AddsSuccessfully()
    {
        // Arrange
        var windGenerator = new WindGenerator("WindGen1", "Onshore");
        var generationDay = new GenerationDay(DateTime.Now, 100, 10);

        // Act
        windGenerator.AddGenerationDay(generationDay);

        // Assert
        Assert.Single(windGenerator.GenerationDays);
        Assert.Equal(generationDay, windGenerator.GenerationDays[0]);
    }
}