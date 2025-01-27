namespace GeneratorDataProcessor;
public class WindGenerator : Generator
{
    // Private field
    private string _location;

    // Public property
    public string Location
    {
        get => _location;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Location cannot be null or empty.");
            }
            _location = value;
        }
    }

    // Constructor
    public WindGenerator(string name, string location) : base(name)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            throw new ArgumentException("Location cannot be null or empty.", nameof(location));
        }
        Location = location;
    }

    // Method to calculate total wind generation
    public double CalculateTotalWindGeneration(Factor referenceData)
    {
        if (referenceData == null)
        {
            throw new ArgumentNullException(nameof(referenceData), "Reference data cannot be null.");
        }

        double valueFactor = Location == "Offshore" ? referenceData.ValueFactorLow : referenceData.ValueFactorHigh;
        double totalValue = GenerationDays.Sum(day => day.Energy * day.Price * valueFactor);
        return totalValue;
    }
}