using System.Xml.Linq;

namespace GeneratorDataProcessor;
public class TotalGeneration
{
    // Private fields
    private Dictionary<string, double> _windGenerators;
    private Dictionary<string, double> _gasGenerators;
    private Dictionary<string, double> _coalGenerators;

    // Public properties
    public IReadOnlyDictionary<string, double> WindGenerators => _windGenerators;
    public IReadOnlyDictionary<string, double> GasGenerators => _gasGenerators;
    public IReadOnlyDictionary<string, double> CoalGenerators => _coalGenerators;

    // Constructor
    public TotalGeneration()
    {
        _windGenerators = new Dictionary<string, double>();
        _gasGenerators = new Dictionary<string, double>();
        _coalGenerators = new Dictionary<string, double>();
    }

    // Static method to calculate total generation
    public static TotalGeneration CalculateTotalGeneration(GenerationData generationData, Factor referenceData)
    {
        var totalGeneration = new TotalGeneration();

        // Calculate total generation for Wind Generators
        foreach (var wind in generationData.WindGenerators)
        {
            totalGeneration.AddWindGeneratorTotal(wind.Name, wind.CalculateTotalWindGeneration(referenceData));
        }

        // Calculate total generation for Gas Generators
        foreach (var fossils in generationData.GasGenerators)
        {
            totalGeneration.AddGasGeneratorTotal(fossils.Name, fossils.CalculateTotalFossilGenerator(referenceData));
        }

        // Calculate total generation for Coal Generators
        foreach (var coal in generationData.CoalGenerators)
        {
            totalGeneration.AddCoalGeneratorTotal(coal.Name, coal.CalculateTotalFossilGenerator(referenceData));
        }

        return totalGeneration;
    }

    // Methods to add generator totals
    public void AddWindGeneratorTotal(string name, double total)
    {
        _windGenerators[name] = total; // This will add or update the total for the generator
    }

    public void AddGasGeneratorTotal(string name, double total)
    {
        _gasGenerators[name] = total; // This will add or update the total for the generator
    }

    public void AddCoalGeneratorTotal(string name, double total)
    {
        _coalGenerators[name] = total; // This will add or update the total for the generator
    }

    // Method to create XML output
    public XElement CreateOutputXmlTotalGeneration()
    {
        var element = new XElement("Totals",
            new XElement("Wind", WindGenerators.Select(wind => 
                new XElement("Generator",
                    new XElement("Name", wind.Key),
                    new XElement("Total", wind.Value)))),
            new XElement("Gas", GasGenerators.Select(gas => 
                new XElement("Generator",
                    new XElement("Name", gas.Key),
                    new XElement("Total", gas.Value)))),
            new XElement("Coal", CoalGenerators.Select(coal => 
                new XElement("Generator",
                    new XElement("Name", coal.Key),
                    new XElement("Total", coal.Value))))
        );
        return element;
    }
}