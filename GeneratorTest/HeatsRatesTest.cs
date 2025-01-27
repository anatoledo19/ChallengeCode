namespace GeneratorDataProcessor.Tests;
public class ActualHeatRatesTests
    {
        [Fact]
        public void Constructor_InitializesList()
        {
            // Act
            var actualHeatRates = new ActualHeatRates();

            // Assert
            Assert.NotNull(actualHeatRates.CoalGenerators);
            Assert.Empty(actualHeatRates.CoalGenerators); // Should be empty initially
        }

        [Fact]
        public void CalculateActualHeatRates_ValidData_ReturnsCorrectHeatRates()
        {
            // Arrange
            var generationData = new GenerationData();
            var referenceData = new Factor(0.5, 1.0, 1.5, 0.1, 0.2, 0.3); // Example reference data

            // Create mock coal generator
            var coalGen = new CoalGenerator("CoalGen1", 0.5, 300, 150); // 300 total heat input, 150 actual net generation
            generationData.AddCoalGenerator(coalGen);

            // Act
            var actualHeatRates = ActualHeatRates.CalculateActualHeatRates(generationData);

            // Assert
            Assert.Single(actualHeatRates.CoalGenerators);
            Assert.Equal("CoalGen1", actualHeatRates.CoalGenerators[0].Name);
            Assert.Equal(2.0, actualHeatRates.CoalGenerators[0].value); // 300 / 150 = 2.0
        }

        [Fact]
        public void CalculateActualHeatRates_NullGenerationData_ThrowsArgumentNullException()
        {
            // Act & Assert
            var exception = Assert.Throws<Exception>(() => ActualHeatRates.CalculateActualHeatRates(null));
            Assert.Equal("Generation data cannot be null.", exception.Message);
        }
    }

    public class CoalHeatRateTests
    {
        [Fact]
        public void Constructor_ValidParameters_SetsProperties()
        {
            // Arrange
            string name = "CoalGen1";
            double heatRate = 2.0;

            // Act
            var coalHeatRate = new HeatRate(name, heatRate);

            // Assert
            Assert.Equal(name, coalHeatRate.Name);
            Assert.Equal(heatRate, coalHeatRate.value);
        }

        [Fact]
        public void Constructor_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            string name = ""; // Invalid name
            double heatRate = 2.0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new HeatRate(name, heatRate));
            Assert.Equal("Name cannot be null or empty. (Parameter 'value')", exception.Message);
        }

        [Fact]
        public void Constructor_NegativeHeatRate_ThrowsArgumentException()
        {
            // Arrange
            string name = "CoalGen1";
            double heatRate = -2.0; // Invalid heat rate

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new HeatRate(name, heatRate));
            Assert.Equal("Heat rate cannot be negative. (Parameter 'value')", exception.Message);
        }
    }