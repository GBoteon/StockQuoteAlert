# Stock Quote Alert

Stock Quote Alert is a console application that monitors the stock quote of a specified asset and sends email notifications when the price exceeds certain thresholds. It utilizes an external API to retrieve the stock quote and sends emails via a configured SMTP server.

## Getting Started

These instructions will guide you on how to set up and run the Stock Quote Alert program on your local machine.

### Prerequisites

- .NET 5.0 SDK or higher installed on your machine.
- An API key for the stock quote API (e.g., Alpha Vantage).

### Installation

1. Clone the repository or download the source code.
2. Open the solution in your preferred IDE or editor (e.g., Visual Studio, JetBrains Rider).
3. Restore the NuGet packages for the solution.

### Configuration

1. Open the config.txt file in the project directory.
2. Update the following settings according to your requirements:
- EmailDestino: The email address where the alerts will be sent.
- SMTPHost: The SMTP server host.
- SMTPPort: The SMTP server port number.
- SMTPUsername: The username for authenticating with the SMTP server.
- SMTPPassword: The password for authenticating with the SMTP server.
- APIKey: Your API key for the stock quote API.

### Usage

Open a terminal or command prompt and navigate to the project directory. Run the following command:
```shell
$ dotnet run --project StockQuoteAlert -- <symbol> <entryPrice> <sellPrice>
```

Alternatively, you can run the program by navigating to the appropriate directory and executing the compiled executable file directly from the command line. Here's how you can run the program using the command:
```shell
$ .\StockQuoteAlert.exe <symbol> <entryPrice> <sellPrice>
```

