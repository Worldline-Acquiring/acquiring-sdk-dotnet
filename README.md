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

* [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/) 13.0.4 or higher
* [NLog](https://www.nuget.org/packages/NLog/) 5.3.2 or higher
* [System.Collections.Immutable](https://www.nuget.org/packages/System.Collections.Immutable/) 7.0.0 or higher
* [System.Configuration.ConfigurationManager](https://www.nuget.org/packages/System.Configuration.ConfigurationManager/) 8.0.0 or higher

The unit tests and the integration tests of this repository target `net10.0`, so building and testing the repository requires the .NET 10 SDK or higher.

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

This repository uses [Visual Studio](https://www.visualstudio.com/) to build, in a version that supports .NET 10. Open `Worldline.Acquiring.Sdk.sln` in Visual Studio, and click build.
Without Visual Studio, install the .NET 10 SDK, run `dotnet build Worldline.Acquiring.Sdk.sln` in the root of the repository to build it and `dotnet test Worldline.Acquiring.Sdk.Tests` to run the unit tests.

## Cancelling calls

Every API call has a method with the suffix `Async` that accepts an optional `CancellationToken`, in line with the .NET conventions.
The methods without the suffix are unchanged: they do not take a token, and forward to the `Async` methods.

    using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10)))
    {
        await client.V1.Ping.PingAsync(cancellationToken: cts.Token);
    }

Cancelling the token stops the call, including a pending request for an OAuth2 access token, and throws an `OperationCanceledException`.
A token that is cancelled after the response has arrived does not discard that response: the call returns its result, or throws the exception for an error response.
A call that times out still throws a `CommunicationException`, as it always did.

The operation that downloads a binary body also has an `Async` method that takes an asynchronous body handler of type `Func<Stream, IEnumerable<IResponseHeader>, Task>`.
The response stays open until the handler has completed, so the handler can read the stream asynchronously.

Custom implementations of `IConnection` and `IAuthenticator` keep working, as these interfaces have not changed.
To support cancellation, implement `ICancellableConnection` and `ICancellableAuthenticator`, which extend them with an overload that accepts a token.
For an implementation that does not, the token is only honoured before the request is sent.
