# ZimoziAssessment

![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)
![Build Status](https://github.com/tuanhuu3264/ZimoziAssessment/workflows/CI/badge.svg)
![Tests](https://img.shields.io/badge/tests-passing-brightgreen)

## Table of Contents
* Architecture Overview
* Getting Started
* Prerequisites
* Installation
* Project Structure
* Development
* In-Memory Database
* Testing
* CI/CD
* Troubleshooting
* Contributing
* License

## Architecture Overview

This application follows a 3-layer architecture:
1. **Controller Layer**: Handles HTTP requests and responses, manages the API endpoints.
2. **Service Layer**: Contains business logic and orchestrates workflows.
3. **Repository Layer**: Manages data access and persistence.

## Getting Started

### Prerequisites

To run this application, you need:
* .NET SDK 8.0
* Visual Studio or Visual Studio Code (optional but recommended)

### Installation

1. **Clone the repository**
```bash
git clone https://github.com/tuanhuu3264/ZimoziAssessment.git
```

2. **Navigate to the project directory**
```bash
cd ZimoziAssessment
```

3. **Restore dependencies**
```bash
dotnet restore
```

4. **Build the project**
```bash
dotnet build
```

5. **Run the application**
```bash
dotnet run
```

The application will start by default at:
- https://localhost:5001
- http://localhost:5000

## Project Structure

```bash
ProjectName/
├── Controllers/         # API controllers, request handling
├── Services/            # Business logic implementation
├── Repositories/        # Data access code
├── Models/              # Data models and DTOs
├── Configs/             # Application configuration
├── Program.cs           # Application entry point
└── appsettings.json     # Application settings
```

### Layers Overview
- **Controller Layer**: Handles HTTP requests and routes them to the appropriate service.
- **Service Layer**: Implements business logic and coordinates between controllers and repositories.
- **Repository Layer**: Manages data access and persistence using an in-memory database.

## In-Memory Database

This project uses an in-memory database for data storage:
- No external database installation is required
- Data persists only for the lifetime of the application
- Ideal for development and testing purposes

## Development

### Running the Application in Development Mode

To enable hot reload:
```bash
dotnet watch run
```

## Testing

### Running Tests

We use xUnit for unit testing. To run the tests:

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test -v n

# Generate code coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov /p:CoverletOutput=./lcov.info
```

### Test Project Structure

```bash
UnitTestAssigments/
├── Controllers/         # Controller unit tests
├── Services/            # Service layer tests
└── Repositories/        # Repository layer tests
```

### Testing Strategies

- Unit Tests: Verify individual components in isolation
- Integration Tests: Test interactions between layers
- Mock Dependencies: Use Moq for mocking dependencies

## Continuous Integration and Deployment (CI/CD)

### GitHub Actions Workflow

Our CI/CD pipeline is configured using GitHub Actions:

```yaml
name: .NET CI/CD Pipeline

on:
  push:
    branches: [ "main" ]
  pull_request:
    branches: [ "main" ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: 8.0.x
      
      - name: Restore dependencies
        run: dotnet restore
      
      - name: Build
        run: dotnet build --no-restore
      
      - name: Test
        run: dotnet test --no-build --verbosity normal
```

### Deployment

- Automated deployment to Render
- Triggered on successful build of main branch
- Includes Discord notifications for build and deployment status

## Troubleshooting

### Common Issues

1. **"Port already in use" error**
   - Check if another application is using the same port
   - Change the port in `Properties/launchSettings.json`

2. **Package restore failures**
```bash
dotnet restore --no-cache
```

## Contributing

1. Fork the repository
2. Create a feature branch
```bash
git checkout -b feature/amazing-feature
```

3. Commit your changes
```bash
git commit -m 'Add amazing feature'
```

4. Push to the branch
```bash
git push origin feature/amazing-feature
```

5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
