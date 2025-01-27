using Microsoft.Extensions.Configuration;
using System.Xml.Linq;
using System;

namespace GeneratorDataProcessor;

class Program
{
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1); // Allow only one thread to enter
    static void Main(string[] args)
    {
        var config = LoadConfiguration();
        // Read the folder paths from the configuration
        string inputFolder = config["InputFolder"];
        string outputFolder = config["OutputFolder"];

        // Process existing files in the input folder
        ProcessExistingFiles(inputFolder, outputFolder);

        // Monitor the input folder for new XML files
        FileSystemWatcher watcher = new FileSystemWatcher(inputFolder, "*.xml");
        watcher.Created += (sender, e) => ProcessFile(e.FullPath, outputFolder);
        watcher.EnableRaisingEvents = true;

        Console.WriteLine("Monitoring input folder for XML files...");
        Console.WriteLine("Press 'q' to quit the application.");
        bool _keepRunning = true;

        // Run a loop to keep the application running until 'q' is pressed
        while (_keepRunning)
        {
            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
            {
                _keepRunning = false;
            }
            Thread.Sleep(100); // Sleep to prevent high CPU usage
        }

        Console.WriteLine("Exiting application...");
    }

    private static IConfiguration LoadConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    private static void ProcessExistingFiles(string inputFolder, string outputFolder)
    {
        var files = Directory.GetFiles(inputFolder, "*.xml");
        foreach (var file in files)
        {
            ProcessFile(file, outputFolder);
        }
    }

    private static void ProcessFile(string filePath, string outputFolder)
    {
        // Wait to enter the semaphore
        semaphore.Wait();
        try
        {
            // Introduce a delay to allow the file to be fully created
            Thread.Sleep(500); // Adjust the delay as necessary

            // Load Reference Data
            var referenceData = Factor.LoadReferenceData(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReferenceData.xml"));
        

            // Load Generation Report
            var generationData = GenerationData.LoadGenerationReport(filePath);

            // Perform calculations
            var totalGeneration = TotalGeneration.CalculateTotalGeneration(generationData, referenceData);
            var maxEmissions = MaxEmissions.CalculateMaxEmissions(generationData, referenceData);
            var actualHeatRates = ActualHeatRates.CalculateActualHeatRates(generationData);

            // Create output XML 
            var outputXml = CreateOutputXml(totalGeneration, maxEmissions, actualHeatRates);
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            // Set the name of the file which the date
            string outputFilePath = Path.Combine(outputFolder, $"GenerationOutput_{timestamp}.xml");
            outputXml.Save(outputFilePath);

            Console.WriteLine($"Processed file: {filePath}");
            Console.WriteLine($"Saved the output to {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {filePath}: {ex.Message}");
        }
        finally
        {
            // Release the semaphore
            semaphore.Release();
        }
    }
    public static XElement CreateOutputXml(TotalGeneration totalGeneration, MaxEmissions maxEmissions, ActualHeatRates actualHeatRates)
    {
        var output = new XElement("GenerationOutput", totalGeneration.CreateOutputXmlTotalGeneration());
        output.Add(maxEmissions.CreateOutputXmlMaxEmissions()); 
        output.Add(actualHeatRates.CreateOutputXmlActualHeat());
        return output;
    }
}
