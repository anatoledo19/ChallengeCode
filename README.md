# Generator Data Processor

## Overview

The **Generator Data Processor** is a production-ready console application developed as part of the Code Challenge for the Brady recruitment process. This application is designed to automatically process generator data from XML files, calculate key metrics, and output the results in a specified XML format.

## Purpose

The primary objectives of this application are to:
1. Calculate and output the Total Generation Value for each generator.
2. Identify and output the generator with the highest Daily Emissions for each day, along with the emission value.
3. Calculate and output the Actual Heat Rate for each coal generator.

## Features

- **Automatic File Processing**: Monitors a designated input folder for new XML files and processes them as soon as they are added.
- **Data Calculations**: Computes total generation values, daily emissions, and actual heat rates based on the provided generator data.
- **Output Generation**: Produces a single XML output file in the specified format, containing the calculated results.
- **Unit Testing**: Includes a test project using xUnit for unit testing various components of the application.

## Prerequisites

- .NET 8.0 or later
- Visual Studio or any compatible IDE
- XML files for reference data and generation reports (`ReferenceData.xml` and `GenerationReport.xml`)

## Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/GeneratorDataProcessor.git
