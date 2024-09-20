# Worldline Acquiring .NET SDK

## Introduction

The .NET SDK helps you to communicate with the Worldline Acquiring API. Its primary features are:

* convenient C# wrapper around the API calls and responses
    * marshalls C# request objects to HTTP requests
    * unmarshalls HTTP responses to C# response objects or C# exceptions
* handling of all the details concerning authentication
* handling of required metadata

See the [Worldline Acquiring Documentation](https://docs.acquiring.worldline-solutions.com/Developer-Tools/sdk/dotnet) for more information on how to use the SDK.

## Structure of this repository

This repository consists out of four main components:

1. The source code of the SDK itself: `/Worldline.Acquiring.Sdk`
2. The source code of the SDK unit tests: `/Worldline.Acquiring.Sdk.Tests`
3. The source code of the integration tests: `/Worldline.Acquiring.Sdk.IntegrationTests`

## Requirements

The .NET SDK supports [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) 2.0 and up.
The following packages are required:

* [Json.NET](https://www.nuget.org/packages/Newtonsoft.Json/) 13.0.3 or higher
* [NLog](https://www.nuget.org/packages/NLog/) 5.3.2 or higher
* [System.Collections.Immutable](https://www.nuget.org/packages/System.Collections.Immutable/) 1.7.1 or higher
* [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) 4.7.0 or higher

## Installation

### Release

#### Package Manager

To install the latest .NET SDK release, run the following command in the Package Manager Console (`Tools -> NuGet Package Manager -> Package Manager Console`) in Visual Studio:

    PM> Install-Package Worldline.Acquiring.Sdk

#### .NET CLI

To install the latest .NET SDK release, run the following command:

    dotnet add package Worldline.Acquiring.Sdk

### Release (Strong-Named)

To install the latest .NET SDK release as a [Strong-Named assembly](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/strong-named-assemblies), follow the instructions above but use `Worldline.Acquiring.Sdk.StrongName` instead of `Worldline.Acquiring.Sdk`.

## Building the repository

This repository uses [Visual Studio](https://www.visualstudio.com/) 2022 to build. Open `Worldline.Acquiring.Sdk.sln` in Visual Studio, and click build.
