namespace GeneratorDataProcessor;
public class Generator
{
    // Private fields
    private string _name;
    private List<GenerationDay> _generationDays;

    // Public properties
    public string Name
    {
        get => _name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or empty.");
            }
            _name = value;
        }
    }

    public IReadOnlyList<GenerationDay> GenerationDays => _generationDays.AsReadOnly();

    // Constructor
    public Generator(string name)
    {
        Name = name;
        _generationDays = new List<GenerationDay>();
    }

    // Method to add a generation day
    public void AddGenerationDay(GenerationDay generationDay)
    {
        if (generationDay == null)
        {
            throw new ArgumentNullException(nameof(generationDay), "Generation day cannot be null.");
        }
        _generationDays.Add(generationDay);
    }
}

public class GenerationDay
{
    // Private fields
    private DateTime _date;
    private double _energy;
    private double _price;

    // Public properties
    public DateTime Date
    {
        get => _date;
        private set
        {
            if (value == default)
            {
                throw new ArgumentException("Date must be a valid DateTime.");
            }
            _date = value;
        }
    }

    public double Energy
    {
        get => _energy;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Energy cannot be negative.");
            }
            _energy = value;
        }
    }

    public double Price
    {
        get => _price;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException("Price cannot be negative.");
            }
            _price = value;
        }
    }

    // Constructor
    public GenerationDay(DateTime date, double energy, double price)
    {
        Date = date;
        Energy = energy;
        Price = price;
    }
}
