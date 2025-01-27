using System.Xml.Linq;

namespace GeneratorDataProcessor;
public class ActualHeatRates
{
    // Private field
    private List<HeatRate> _coalGenerators;

    // Public property
    public IReadOnlyList<HeatRate> CoalGenerators => _coalGenerators.AsReadOnly();

    // Constructor
    public ActualHeatRates()
    {
        _coalGenerators = new List<HeatRate>();
    }

    // Static method to calculate actual heat rates
    public static ActualHeatRates CalculateActualHeatRates(GenerationData generationData)
    {
        if (generationData == null)
        {
            throw new Exception("Generation data cannot be null.");
        }
        else{
        var actualHeatRates = new ActualHeatRates();

        // Calculate actual heat rates for Coal Generators
        foreach (var coal in generationData.CoalGenerators)
        {
            if (coal.ActualNetGeneration > 0) // Avoid division by zero
            {
                double heatRate = coal.TotalHeatInput / coal.ActualNetGeneration;
                actualHeatRates.AddCoalHeatRate(coal.Name, heatRate);
            }
        }

        return actualHeatRates;
        }
    }

    // Method to add coal heat rate
    public void AddCoalHeatRate(string name, double heatRate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        _coalGenerators.Add(new HeatRate (name,heatRate));
    }

    // Method to create XML output for actual heat rates
    public XElement CreateOutputXmlActualHeat()
    {
        var element = new XElement("ActualHeatRates",
            CoalGenerators.Select(coal => 
                new XElement("CoalGenerator",
                    new XElement("Name", coal.Name),
                    new XElement("HeatRate", coal.value)
                )
            )
        );
        return element;
    }
}

public class HeatRate
{
    // Private fields
    private string _name;
    private double _value;

    // Public properties
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or empty.", nameof(value));
            }
            _name = value;
        }
    }

    public double value
    {
        get => _value;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Heat rate cannot be negative.", nameof(value));
            }
            _value = value;
        }
    }

    // Constructor
    public HeatRate(string name, double heatRate)
    {
        Name = name; // This will invoke the property setter with validation
        value = heatRate; // This will invoke the property setter with validation
    }
}