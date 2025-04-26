# C# Application

This repository contains a C# application built using a 3-layer architecture pattern (Controller-Service-Repository).

## Table of Contents
- [Architecture Overview](#architecture-overview)
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Project Structure](#project-structure)
- [Development](#development)
- [In-Memory Database](#in-memory-database)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License](#license)

## Architecture Overview

This application follows a 3-layer architecture:

1. **Controller Layer**: Handles HTTP requests and responses, manages the API endpoints.
2. **Service Layer**: Contains business logic and orchestrates workflows.
3. **Repository Layer**: Manages data access and persistence.

---

## Getting Started

### Prerequisites

To run this application, you need:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) (optional but recommended)

### Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/tuanhuu3264/ZimoziAssessment.git 

2. **Navigate to the project directory**

   ```bash
   cd ZimoziAssessment

2. **Restore dependencies**

   ```bash
   dotnet restore

2. **Build the project**

   ```bash
   dotnet build

2. **Run the application**

   ```bash
   dotnet run


The application will start by default at:

https://localhost:5001

http://localhost:5000

### Project Structure

    ```bash
    ProjectName/
    ├── Controllers/         # API controllers, request handling
    ├── Services/            # Business logic implementation
    ├── Repositories/        # Data access code
    ├── Models/              # Data models and DTOs
    ├── Configs/      # Application configuration
    ├── Program.cs           # Application entry point
    └── appsettings.json     # Application settings

Layers Overview
Controller Layer: Handles HTTP requests and routes them to the appropriate service.

Service Layer: Implements business logic and coordinates between controllers and repositories.

Repository Layer: Manages data access and persistence using an in-memory database.
In-Memory Database
This project uses an in-memory database for data storage:

No external database installation is required.

Data persists only for the lifetime of the application.

Ideal for development and testing purposes.
Development
Running the Application in Development Mode
To enable hot reload:

bash
Sao chép
Chỉnh sửa
dotnet watch run
In-Memory Database
This project uses an in-memory database for data storage:

No external database installation is required.

Data persists only for the lifetime of the application.

Ideal for development and testing purposes.

Troubleshooting
Common Issues
"Port already in use" error
Check if another application is using the same port.

Change the port in Properties/launchSettings.json.

Package restore failures
Try restoring packages without cache:

bash
Sao chép
Chỉnh sửa
dotnet restore --no-cache
Contributing
Fork the repository.

Create a feature branch:

bash
Sao chép
Chỉnh sửa
git checkout -b feature/amazing-feature
Commit your changes:

bash
Sao chép
Chỉnh sửa
git commit -m 'Add amazing feature'
Push to the branch:

bash
Sao chép
Chỉnh sửa
git push origin feature/amazing-feature
Open a Pull Request.

License
This project is licensed under the MIT License - see the LICENSE file for details.

less
Sao chép
Chỉnh sửa

---

✅ Mình đã format lại cho dễ đọc, chia thành các khối rõ ràng bằng `---` và thêm định dạng lệnh `bash` cho phần code command line.  
✅ Nếu bạn muốn, mình có thể hỗ trợ thêm cả phần badge ví dụ như:

```markdown
![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)