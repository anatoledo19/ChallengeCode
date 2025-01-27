using System.Xml.Linq;
namespace GeneratorDataProcessor;

public class MaxEmissions
{
    // Private field
    private List<DailyMaxEmission> _dailyMaxEmissions;

    // Public property
    public IReadOnlyList<DailyMaxEmission> DailyMaxEmissions => _dailyMaxEmissions.AsReadOnly();

    // Constructor
    public MaxEmissions()
    {
        _dailyMaxEmissions = new List<DailyMaxEmission>();
    }

    // Method to add daily emission
    public void AddDailyEmission(DateTime date, string generatorName, double emissions)
    {
        if (string.IsNullOrWhiteSpace(generatorName))
        {
            throw new ArgumentException("Generator name cannot be null or empty.", nameof(generatorName));
        }

        var existingEntry = _dailyMaxEmissions.FirstOrDefault(e => e.Date.Date == date.Date);
        if (existingEntry == null)
        {
            _dailyMaxEmissions.Add(new DailyMaxEmission(date, generatorName, emissions));
        }
        else
        {
            if (emissions > existingEntry.Emission)
            {
                existingEntry.Emission = emissions;
            }
        }
    }

    // Static method to calculate max emissions
    public static MaxEmissions CalculateMaxEmissions(GenerationData generationData, Factor referenceData)
    {
        if (generationData == null)
        {
            throw new ArgumentNullException(nameof(generationData), "Generation data cannot be null.");
        }

        if (referenceData == null)
        {
            throw new ArgumentNullException(nameof(referenceData), "Reference data cannot be null.");
        }

        var maxEmissions = new MaxEmissions();

        // Calculate max emissions for Gas Generators
        foreach (var gas in generationData.GasGenerators)
        {
            foreach (var day in gas.GenerationDays)
            {
                double emissionsFactor = referenceData.EmissionsFactorMedium;
                double dailyEmissions = day.Energy * gas.EmissionsRating * emissionsFactor;

                maxEmissions.AddDailyEmission(day.Date, gas.Name, dailyEmissions);
            }
        }

        // Calculate max emissions for Coal Generators
        foreach (var coal in generationData.CoalGenerators)
        {
            foreach (var day in coal.GenerationDays)
            {
                double emissionsFactor = referenceData.EmissionsFactorHigh;
                double dailyEmissions = day.Energy * coal.EmissionsRating * emissionsFactor;

                maxEmissions.AddDailyEmission(day.Date, coal.Name, dailyEmissions);
            }
        }

        return maxEmissions;
    }

    // Method to create XML output for max emissions
    public XElement CreateOutputXmlMaxEmissions()
    {
        var element = new XElement("MaxEmissionGenerators",
            DailyMaxEmissions.Select(emission => 
                new XElement("Day",
                    new XElement("Name", emission.GeneratorName),
                    new XElement("Date", emission.Date.ToString("o")), // ISO 8601 format
                    new XElement("Emission", emission.Emission)
                )
            )
        );
        return element;
    }
}
public class DailyMaxEmission
{
    // Private fields
    private DateTime _date;
    private string _generatorName;
    private double _emission;

    // Public properties
    public DateTime Date
    {
        get => _date;
        private set
        {
            if (value == default)
            {
                throw new Exception("Date must be a valid DateTime.");
            }
            _date = value;
        }
    }

    public string GeneratorName
    {
        get => _generatorName;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new Exception("Generator name cannot be null or empty.");
            }
            _generatorName = value;
        }
    }

    public double Emission
    {
        get => _emission;
        set
        {
            if (value < 0)
            {
                throw new Exception("Emission cannot be negative.");
            }
            _emission = value;
        }
    }

    // Constructor
    public DailyMaxEmission(DateTime date, string generatorName, double emission)
    {
        Date = date;
        GeneratorName = generatorName;
        Emission = emission;
    }
}