using System.Globalization;
using System.Xml.Linq;

namespace GeneratorDataProcessor;
public class Factor
{
    // Private fields
    private double _valueFactorHigh;
    private double _valueFactorMedium;
    private double _valueFactorLow;
    private double _emissionsFactorHigh;
    private double _emissionsFactorMedium;
    private double _emissionsFactorLow;

    // Public properties
    public double ValueFactorHigh
    {
        get => _valueFactorHigh;
        private set => _valueFactorHigh = value;
    }

    public double ValueFactorMedium
    {
        get => _valueFactorMedium;
        private set => _valueFactorMedium = value;
    }

    public double ValueFactorLow
    {
        get => _valueFactorLow;
        private set => _valueFactorLow = value;
    }

    public double EmissionsFactorHigh
    {
        get => _emissionsFactorHigh;
        private set => _emissionsFactorHigh = value;
    }

    public double EmissionsFactorMedium
    {
        get => _emissionsFactorMedium;
        private set => _emissionsFactorMedium = value;
    }

    public double EmissionsFactorLow
    {
        get => _emissionsFactorLow;
        private set => _emissionsFactorLow = value;
    }

    // Constructor
    public Factor(double valueFactorLow, double valueFactorMedium, double valueFactorHigh,
                            double emissionsFactorLow, double emissionsFactorMedium, double emissionsFactorHigh)
    {
        ValueFactorLow = valueFactorLow;
        ValueFactorMedium = valueFactorMedium;
        ValueFactorHigh = valueFactorHigh;
        EmissionsFactorLow = emissionsFactorLow;
        EmissionsFactorMedium = emissionsFactorMedium;
        EmissionsFactorHigh = emissionsFactorHigh;
    }

    // Factory method to load ReferenceData from XML
    public static Factor LoadReferenceData(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var factors = doc.Descendants("Factors").FirstOrDefault();

        if (factors == null)
        {
            throw new Exception("Factors element is missing in the XML.");
        }
        return new Factor(
            GetElementValueAsDouble(factors, "ValueFactor", "Low"),
            GetElementValueAsDouble(factors, "ValueFactor", "Medium"),
            GetElementValueAsDouble(factors, "ValueFactor", "High"),
            GetElementValueAsDouble(factors, "EmissionsFactor", "Low"),
            GetElementValueAsDouble(factors, "EmissionsFactor", "Medium"),
            GetElementValueAsDouble(factors, "EmissionsFactor", "High")
        );
    }

    // Private method to get element value as double
    private static double GetElementValueAsDouble(XElement parent, string elementName, string subElementName)
    {
        var element = parent.Element(elementName)?.Element(subElementName);
        if (element == null)
        {
            throw new InvalidOperationException($"Missing element: {elementName}/{subElementName}");
        }
        //string valueString = element.Value.Replace('.', ',');
        if (!double.TryParse(element.Value, CultureInfo.InvariantCulture ,out double result))
        {
            throw new InvalidOperationException($"Invalid double value for element: {elementName}/{subElementName}");
        }

        return result;
    }
}