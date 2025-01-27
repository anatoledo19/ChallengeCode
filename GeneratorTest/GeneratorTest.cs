namespace GeneratorDataProcessor.Tests;
public class GeneratorTests
{
    [Fact]
    public void Constructor_ValidName_SetsProperties()
    {
        // Arrange
        string name = "WindGen1";

        // Act
        var generator = new Generator(name);

        // Assert
        Assert.Equal(name, generator.Name);
        Assert.Empty(generator.GenerationDays); // Should be empty initially
    }

    [Fact]
    public void Constructor_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        string name = ""; // Invalid name

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Generator(name));
        Assert.Equal("Name cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void Constructor_NullName_ThrowsArgumentException()
    {
        // Arrange
        string name = null; // Invalid name

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Generator(name));
        Assert.Equal("Name cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void AddGenerationDay_ValidDay_AddsToList()
    {
        // Arrange
        var generator = new Generator("WindGen1");
        var generationDay = new GenerationDay(DateTime.Now, 100, 50);

        // Act
        generator.AddGenerationDay(generationDay);

        // Assert
        Assert.Single(generator.GenerationDays);
        Assert.Equal(generationDay, generator.GenerationDays[0]);
    }

    [Fact]
    public void AddGenerationDay_NullDay_ThrowsArgumentNullException()
    {
        // Arrange
        var generator = new Generator("WindGen1");

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => generator.AddGenerationDay(null));
        Assert.Equal("Generation day cannot be null. (Parameter 'generationDay')", exception.Message);
    }
}

public class GenerationDayTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        DateTime date = DateTime.Now;
        double energy = 100;
        double price = 50;

        // Act
        var generationDay = new GenerationDay(date, energy, price);

        // Assert
        Assert.Equal(date, generationDay.Date);
        Assert.Equal(energy, generationDay.Energy);
        Assert.Equal(price, generationDay.Price);
    }

    [Fact]
    public void Constructor_InvalidDate_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = default; // Invalid date
        double energy = 100;
        double price = 50;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new GenerationDay(date, energy, price));
        Assert.Equal("Date must be a valid DateTime.", exception.Message);
    }

    [Fact]
    public void Constructor_NegativeEnergy_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = DateTime.Now;
        double energy = -100; // Invalid energy
        double price = 50;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new GenerationDay(date, energy, price));
        Assert.Equal("Energy cannot be negative.", exception.Message);
    }

    [Fact]
    public void Constructor_NegativePrice_ThrowsArgumentException()
    {
        // Arrange
        DateTime date = DateTime.Now;
        double energy = 100;
        double price = -50; // Invalid price

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new GenerationDay(date, energy, price));
        Assert.Equal("Price cannot be negative.", exception.Message);
    }
}