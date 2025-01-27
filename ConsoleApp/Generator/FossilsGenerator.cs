namespace GeneratorDataProcessor;
public class FossilsGenerator : Generator
{
    // Private field
    private double _emissionsRating;

    // Public property
    public double EmissionsRating
    {
        get => _emissionsRating;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Emissions rating cannot be negative.");
            }
            _emissionsRating = value;
        }
    }

    // Constructor
    public FossilsGenerator(string name, double emissionsRating) : base(name)
    {
        EmissionsRating = emissionsRating;
    }

    // Method to calculate total fossil generation
    public double CalculateTotalFossilGenerator(Factor referenceData)
    {
        if (referenceData == null)
        {
            throw new Exception("Reference data cannot be null.");
        }

        double valueFactor = referenceData.ValueFactorMedium;
        double totalValue = this.GenerationDays.Sum(day => day.Energy * day.Price * valueFactor);
        return totalValue;
    }

    // Static method to calculate max emissions for all fossil generators
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
            CalculateDailyEmissions(gas, referenceData.EmissionsFactorMedium, maxEmissions);
        }

        // Calculate max emissions for Coal Generators
        foreach (var coal in generationData.CoalGenerators)
        {
            CalculateDailyEmissions(coal, referenceData.EmissionsFactorHigh, maxEmissions);
        }

        return maxEmissions;
    }

    // Private method to calculate daily emissions for a given generator
    private static void CalculateDailyEmissions(FossilsGenerator fossils, double emissionsFactor, MaxEmissions maxEmissions)
    {
        foreach (var day in fossils.GenerationDays)
        {
            double dailyEmissions = day.Energy * fossils.EmissionsRating * emissionsFactor;
            maxEmissions.AddDailyEmission(day.Date, fossils.Name, dailyEmissions);
        }
    }
}
