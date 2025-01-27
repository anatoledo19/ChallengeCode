using System.Globalization;
using System.Xml.Linq;

namespace GeneratorDataProcessor;
public class GenerationData
{
    // Private fields
    private List<WindGenerator> _windGenerators;
    private List<FossilsGenerator> _gasGenerators;
    private List<CoalGenerator> _coalGenerators;

    // Public properties
    public IReadOnlyList<WindGenerator> WindGenerators => _windGenerators.AsReadOnly();
    public IReadOnlyList<FossilsGenerator> GasGenerators => _gasGenerators.AsReadOnly();
    public IReadOnlyList<CoalGenerator> CoalGenerators => _coalGenerators.AsReadOnly();

    // Constructor
    public GenerationData()
    {
        _windGenerators = new List<WindGenerator>();
        _gasGenerators = new List<FossilsGenerator>();
        _coalGenerators = new List<CoalGenerator>();
    }

    // Methods to add generators
    public void AddWindGenerator(WindGenerator windGenerator)
    {
        if (windGenerator == null)
        {
            throw new ArgumentNullException(nameof(windGenerator), "Wind generator cannot be null.");
        }
        _windGenerators.Add(windGenerator);
    }

    public void AddGasGenerator(FossilsGenerator gasGenerator)
    {
        if (gasGenerator == null)
        {
            throw new ArgumentNullException(nameof(gasGenerator), "Gas generator cannot be null.");
        }
        _gasGenerators.Add(gasGenerator);
    }

    public void AddCoalGenerator(CoalGenerator coalGenerator)
    {
        if (coalGenerator == null)
        {
            throw new ArgumentNullException(nameof(coalGenerator), "Coal generator cannot be null.");
        }
        _coalGenerators.Add(coalGenerator);
    }

    // Static method to load generation report from XML
    public static GenerationData LoadGenerationReport(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var generationData = new GenerationData
        {
            _windGenerators = LoadWindGenerators(doc),
            _gasGenerators = LoadGasGenerators(doc),
            _coalGenerators = LoadCoalGenerators(doc)
        };

        return generationData;
    }

    // Load wind generators from XML
    private static List<WindGenerator> LoadWindGenerators(XDocument doc)
    {
        var windGenerators = new List<WindGenerator>();
        foreach (var wind in doc.Descendants("WindGenerator"))
        {
            try
            {
                var name = GetElementValue(wind, "Name");
                var location = GetElementValue(wind, "Location");
                var generationDays = LoadGenerationDays(wind);

                // Create a new WindGenerator instance
                var windGenerator = new WindGenerator(name, location);

                // Add each GenerationDay to the WindGenerator
                foreach (var generationDay in generationDays)
                {
                    windGenerator.AddGenerationDay(generationDay);
                }

                // Add the WindGenerator to the list
                windGenerators.Add(windGenerator);
            }
            catch (Exception ex)
            {
                // Handle or log the error as needed
                Console.WriteLine($"Error loading wind generator: {ex.Message}");
            }
        }
        return windGenerators;
    }

    // Load gas generators from XML
    private static List<FossilsGenerator> LoadGasGenerators(XDocument doc)
    {
        var gasGenerators = new List<FossilsGenerator>();

        foreach (var gas in doc.Descendants("GasGenerator"))
        {
            try
            {
                var name = GetElementValue(gas, "Name");
                var emissionsRating = GetElementValueAsDouble(gas, "EmissionsRating");
                var generationDays = LoadGenerationDays(gas);

                // Create a new FossilsGenerator instance
                var fossilsGenerator = new FossilsGenerator(name, emissionsRating);

                // Add each GenerationDay to the FossilsGenerator
                foreach (var generationDay in generationDays)
                {
                    fossilsGenerator.AddGenerationDay(generationDay);
                }

                // Add the FossilsGenerator to the list
                gasGenerators.Add(fossilsGenerator);
            }
            catch (Exception ex)
            {
                // Handle or log the error as needed
                Console.WriteLine($"Error loading gas generator: {ex.Message}");
            }
        }
        return gasGenerators;
    }

    // Load coal generators from XML
    private static List<CoalGenerator> LoadCoalGenerators(XDocument doc)
    {
        var coalGenerators = new List<CoalGenerator>();

        foreach (var coal in doc.Descendants("CoalGenerator"))
        {
            try
            {
                var name = GetElementValue(coal, "Name");
                var totalHeatInput = GetElementValueAsDouble(coal, "TotalHeatInput");
                var actualNetGeneration = GetElementValueAsDouble(coal, "ActualNetGeneration");
                var emissionsRating = GetElementValueAsDouble(coal, "EmissionsRating");
                var generationDays = LoadGenerationDays(coal);

                // Create a new CoalGenerator instance
                var coalGenerator = new CoalGenerator(name, emissionsRating, totalHeatInput, actualNetGeneration);

                // Add each GenerationDay to the CoalGenerator
                foreach (var generationDay in generationDays)
                {
                    coalGenerator.AddGenerationDay(generationDay);
                }

                // Add the CoalGenerator to the list
                coalGenerators.Add(coalGenerator);
            }
            catch (Exception ex)
            {
                // Handle or log the error as needed
                Console.WriteLine($"Error loading coal generator: {ex.Message}");
            }
        }

        return coalGenerators;
    }

    // Load generation days from XML
    private static List<GenerationDay> LoadGenerationDays(XElement generatorElement)
    {
        return generatorElement.Descendants("Day")
            .Select(day => new GenerationDay(
                DateTime.Parse(GetElementValue(day, "Date")),
                GetElementValueAsDouble(day, "Energy"),
                GetElementValueAsDouble(day, "Price")
            )).ToList();
    }

    // Get element value from XML
    private static string GetElementValue(XElement element, string name)
    {
        var value = element.Element(name)?.Value;
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Missing or empty element: {name}");
        }
        return value;
    }

    // Get element value as double from XML
    private static double GetElementValueAsDouble(XElement element, string name)
    {
        var value = GetElementValue(element, name);
        if (!double.TryParse(value, CultureInfo.InvariantCulture, out double result))
        {
            throw new InvalidOperationException($"Invalid double value for element: {name}");
        }
        return result;
    }
}