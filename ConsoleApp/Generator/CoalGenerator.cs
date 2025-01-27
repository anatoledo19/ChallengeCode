namespace GeneratorDataProcessor;
public class CoalGenerator : FossilsGenerator
{
    // Private fields
    private double _totalHeatInput;
    private double _actualNetGeneration;

    // Public properties
    public double TotalHeatInput
    {
        get => _totalHeatInput;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Total heat input cannot be negative.", nameof(value));
            }
            _totalHeatInput = value;
        }
    }

    public double ActualNetGeneration
    {
        get => _actualNetGeneration;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Actual net generation cannot be negative.", nameof(value));
            }
            _actualNetGeneration = value;
        }
    }

    // Constructor
    public CoalGenerator(string name, double emissionsRating, double totalHeatInput, double actualNetGeneration)
        : base(name, emissionsRating)
    {
        TotalHeatInput = totalHeatInput;
        ActualNetGeneration = actualNetGeneration;
    }
}