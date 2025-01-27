using System.Xml.Linq;

namespace GeneratorDataProcessor.Tests;
public class FactorTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsProperties()
    {
        // Arrange
        double valueFactorLow = 0.1;
        double valueFactorMedium = 0.2;
        double valueFactorHigh = 0.3;
        double emissionsFactorLow = 0.4;
        double emissionsFactorMedium = 0.5;
        double emissionsFactorHigh = 0.6;

        // Act
        var factor = new Factor(valueFactorLow, valueFactorMedium, valueFactorHigh,
                                emissionsFactorLow, emissionsFactorMedium, emissionsFactorHigh);

        // Assert
        Assert.Equal(valueFactorLow, factor.ValueFactorLow);
        Assert.Equal(valueFactorMedium, factor.ValueFactorMedium);
        Assert.Equal(valueFactorHigh, factor.ValueFactorHigh);
        Assert.Equal(emissionsFactorLow, factor.EmissionsFactorLow);
        Assert.Equal(emissionsFactorMedium, factor.EmissionsFactorMedium);
        Assert.Equal(emissionsFactorHigh, factor.EmissionsFactorHigh);
    }

    [Fact]
    public void LoadReferenceData_ValidXml_ReturnsFactor()
    {
        // Arrange
        string xmlFilePath = "TestReferenceData.xml"; // Path to the test XML file
        CreateTestXmlFile(xmlFilePath); // Create a test XML file

        // Act
        var factor = Factor.LoadReferenceData(xmlFilePath);

        // Assert
        Assert.NotNull(factor);
        Assert.Equal(0.1, factor.ValueFactorLow);
        Assert.Equal(0.2, factor.ValueFactorMedium);
        Assert.Equal(0.3, factor.ValueFactorHigh);
        Assert.Equal(0.4, factor.EmissionsFactorLow);
        Assert.Equal(0.5, factor.EmissionsFactorMedium);
        Assert.Equal(0.6, factor.EmissionsFactorHigh);

        // Clean up
        File.Delete(xmlFilePath);
    }

    [Fact]
    public void LoadReferenceData_MissingFactorsElement_ThrowsInvalidOperationException()
    {
        // Arrange
        string xmlFilePath = "InvalidReferenceData.xml"; // Path to the invalid XML file
        File.WriteAllText(xmlFilePath, "<Root></Root>"); // Create an invalid XML file

        // Act & Assert
        var exception = Assert.Throws<Exception>(() => Factor.LoadReferenceData(xmlFilePath));
        Assert.Equal("Factors element is missing in the XML.", exception.Message);

        // Clean up
        File.Delete(xmlFilePath);
    }

    [Fact]
    public void LoadReferenceData_MissingElementValue_ThrowsInvalidOperationException()
    {
        // Arrange
        string xmlFilePath = "InvalidValueReferenceData.xml"; // Path to the invalid XML file
        File.WriteAllText(xmlFilePath, "<ReferenceData><Factors><ValueFactor><High>0.946</High><Medium>0.696</Medium><Low></Low></ValueFactor><EmissionsFactor><High>0.812</High><Medium>0.562</Medium><Low>0.312</Low></EmissionsFactor></Factors></ReferenceData>"); // Create an invalid XML file

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => Factor.LoadReferenceData(xmlFilePath));
        Assert.Contains("Invalid double value for element: ValueFactor/Low", exception.Message);

        // Clean up
        File.Delete(xmlFilePath);
    }

    private void CreateTestXmlFile(string filePath)
    {
        var xmlContent = new XElement("Factors",
            new XElement("ValueFactor",
                new XElement("Low", 0.1),
                new XElement("Medium", 0.2),
                new XElement("High", 0.3)
            ),
            new XElement("EmissionsFactor",
                new XElement("Low", 0.4),
                new XElement("Medium", 0.5),
                new XElement("High", 0.6)
            )
        );

        xmlContent.Save(filePath);
    }
}
