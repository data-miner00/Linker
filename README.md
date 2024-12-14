<a name="readme-top"></a>

<!-- PROJECT SHIELDS -->

[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]

<!-- PROJECT LOGO -->
<br />
<div align="center">
  <h3 align="center">Linker</h3>

  <p align="center">
    An application that stores and manages URL links.
    <br />
    <a href="https://github.com/data-miner00/Linker"><strong>View Demo »</strong></a>
    <br />
    <br />
    <a href="https://github.com/data-miner00/Linker">Explore the docs</a>
    ·
    <a href="https://github.com/data-miner00/Linker/issues">Report Bug</a>
    ·
    <a href="https://github.com/data-miner00/Linker/issues">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about">About The Project</a>
      <ul>
        <li><a href="#directories">Directories</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->

## About

A suite of applications to store, manage and share links. I found that keeping track of useful websites/URL can be arduous and that was the raison d'être for this project. However, as the time goes by, I started to add more functionalities such as authorization, real-time chat, GraphQL etc to expand my knowledge and challenge myself.

This project is still in active development and the entities and database design can be temporary found in the [drawio document](/docs/Linker.drawio) in the `docs` folder.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Directories

**Application**

| | Directory | Description |
| --- | --- | --- |
|  1. | Linker.Common | Library that includes commonly shared codes across other projects |
|  2. | Linker.ConsoleUI | The very first version of the application. Uses CSV as the backing store and has limited features |
|  3. | Linker.Core | Old library that contains the models and interfaces used across the codebase |
|  4. | Linker.Core.V2 | Latest library that contains the models and interfaces with the updated design decision |
|  5. | Linker.Data | The integration layer between the application and data sources such as database |
|  6. | Linker.Database | The project that stores all the TSQL definitions such as tables and stored procedures for the database |
|  7. | Linker.Debugger | A console for debugging codes |
|  8. | Linker.GraphQL | The client with GraphQL. Working in progress. |
|  9. | Linker.GrpcServer | An experimental client with gRPC. Working in progress. |
| 10. | Linker.Mvc | The main client for the application. Working in progress. |
| 11. | Linker.Preprocess | A small tool to collect minimal links |
| 12. | Linker.WebApi | A HTTP client for the app |
| 13. | Linker.WebJob | A cron-powered background service that housekeep the records |
| 14. | Linker.Wpf | A GUI client for the app. Working in progress. |

**Tests**
| | Directory | Description |
| --- | --- | --- |
|  1. | Linker.Common.UnitTests | Unit tests for `Linker.Common` |
|  2. | Linker.ConsoleUI.UnitTests | Unit tests for `Linker.ConsoleUI` |
|  3. | Linker.Mvc.UnitTests | Unit tests for `Linker.Mvc`. Uses SpecFlow for BDD styled tests. |
|  4. | Linker.TestCore | Library that contains utilities for unit test projects |
|  5. | Linker.WebApi.IntegrationTests | Integration tests for `Linker.WebApi` _(Have not set up yet)_. |
|  6. | Linker.WebApi.UnitTests | Unit tests for `Linker.WebApi` |

### Built With

The technologies and tools used within this project.

- .NET Core
- .NET MVC
- Pnpm
- TailwindCSS
- JavaScript
- jQuery
- TSQL/SQL Server
- SignalR
- Powershell
- xUnit
- SpecFlow

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->

## Getting Started

### Prerequisites

The list of tools that is used when development.

- [Visual Studio](https://visualstudio.microsoft.com/)
- npm
  ```sh
  npm install npm@latest -g
  ```
- Pnpm
  ```sh
  npm i -g pnpm
  ```
- [Git](https://git-scm.com/downloads)
- [.NET CLI](https://learn.microsoft.com/en-us/dotnet/core/tools/)

### Installation

To run this template project in your local for personal use or contribution, simply perform the following.

1. Clone the repo
   ```sh
   git clone https://github.com/data-miner00/Linker.git
   ```
2. Install Node dependencies
   ```sh
   pnpm i
   ```
3. Restore Nuget
   ```sh
   dotnet restore
   ```
4. Build projects
   ```
   dotnet build
   ```
5. Restore database (steps coming soon..)
6. Run Mvc
   ```
   dotnet run --project src/Linker.Mvc/Linker.Mvc.csproj
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->

## Roadmap

- [ ] Add OAuth/OIDC/Okta authentication
- [ ] Add QRCode
- [ ] Use RabbitMQ
- [ ] Use Grafana for telemetry tracking
- [ ] Configure code coverage with report generator
- [ ] Create Mobile App
- [ ] Sending Email
- [ ] Working SpecFlow unit tests
- [ ] Working integration tests
- [ ] Add component tests
- [ ] Change GitHub Workflows to use Azure Pipelines

See the [open issues](https://github.com/data-miner00/Linker/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTRIBUTING -->

## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->

## License

Distributed under the WTFPL License. See `LICENSE` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->

## Acknowledgments

List of resources that are helpful and would like to give credit to.

- [.NET](https://dotnet.microsoft.com/en-us/)
- [Dapper](https://www.learndapper.com/)
- [Custom Authorize Attribute](https://code-maze.com/custom-authorize-attribute-aspnetcore/)
- [Hot Chocolate AspNetCore](https://chillicream.com/docs/hotchocolate/v13/get-started-with-graphql-in-net-core)
- [Stored Procedure with optional "WHERE" parameters](https://stackoverflow.com/questions/697671/stored-procedure-with-optional-where-parameters)
- [Subscriptions - GRAPHQL API IN .NET w/ HOT CHOCOLATE #4](https://www.youtube.com/watch?v=JD5tfdzPIaI&list=PLA8ZIAm2I03g9z705U3KWJjTv0Nccw9pj&index=4)
- [Snack.xyz](https://snack.xyz)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->

[contributors-shield]: https://img.shields.io/github/contributors/data-miner00/Linker.svg?style=for-the-badge
[contributors-url]: https://github.com/data-miner00/Linker/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/data-miner00/Linker.svg?style=for-the-badge
[forks-url]: https://github.com/data-miner00/Linker/network/members
[stars-shield]: https://img.shields.io/github/stars/data-miner00/Linker.svg?style=for-the-badge
[stars-url]: https://github.com/data-miner00/Linker/stargazers
[issues-shield]: https://img.shields.io/github/issues/data-miner00/Linker.svg?style=for-the-badge
[issues-url]: https://github.com/data-miner00/Linker/issues
[license-shield]: https://img.shields.io/github/license/data-miner00/Linker.svg?style=for-the-badge
[license-url]: https://github.com/data-miner00/Linker/blob/master/LICENSE
