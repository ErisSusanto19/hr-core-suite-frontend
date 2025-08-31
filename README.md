# HR-Core Suite - Frontend

**HR-Core Suite - Frontend** is the user interface for the HR-Core Suite platform. This initial phase is developed to meet the specific requirements of the recruitment test, focusing on core functionalities for data display and file uploading.

The project is built using **ASP.NET Core with Razor Pages**, leveraging HTML and CSS for the presentation layer. This approach aligns perfectly with the test's requirement of using ".NET Core dengan HTML + CSS" and ensures seamless integration with the existing backend.

---

## Table of Contents

1.  [About This Phase](#about-this-phase)
2.  [Technology Stack](#technology-stack)
3.  [Core Features (Phase 1)](#core-features-phase-1)
4.  [Project Structure](#project-structure)
5.  [Getting Started](#getting-started)
    *   [Prerequisites](#prerequisites)
    *   [Setup](#setup)
6.  [How It Works](#how-it-works)

---

## About This Phase

The primary goal of this phase is to deliver a functional user interface that fulfills all frontend requirements specified in the evaluation test. The focus is on simplicity, functionality, and direct communication with the **HR-Core Suite Backend API**.

This application will provide HR staff with a simple web portal to:
*   View a list of all employees, branches, and positions.
*   Upload employee data in bulk using an Excel file.
*   View the status of the file upload process.

---

## Technology Stack

*   **Framework:** ASP.NET Core 8 (Razor Pages)
*   **Language:** C#, HTML, CSS
*   **HTTP Client:** `IHttpClientFactory` for communicating with the backend API.
*   **Deployment Target:** IIS on Windows

---

## Core Features (Phase 1)

This phase implements the following features as required by the test:

*   [ ] **Data Display Page:**
    *   A single page that displays three distinct tables:
        *   Table for Employee Data.
        *   Table for Branch Data.
        *   Table for Position Data.
    *   Data is fetched directly from the backend API.

*   [ ] **File Upload Page:**
    *   A dedicated form for uploading Excel files.
    *   The form will accept a file and submit it to the backend's `/api/employee/upload` endpoint.

*   [ ] **Upload Result Display:**
    *   A section or a separate page to display a list of files that have been successfully uploaded during the user's session.

---

## Project Structure

The project will follow the standard ASP.NET Core Razor Pages structure to maintain clarity and separation of concerns.

/hr-core-suite-frontend
|-- src/
| |-- HRCoreSuite.Frontend/
| | |-- Pages/
| | | |-- Index.cshtml (Main page for data display)
| | | |-- Upload.cshtml (Page with the upload form)
| | | |-- Shared/
| | |-- wwwroot/
| | | |-- css/
| | | | |-- site.css (Custom styles)
| | |-- Services/
| | | |-- ApiClient.cs (Service to handle all backend API calls)
| | |-- ViewModels/
| | | |-- EmployeeViewModel.cs
| | | |-- BranchViewModel.cs
| | | |-- PositionViewModel.cs
| | |-- appsettings.json
| | |-- Program.cs
|-- .gitignore
|-- README.md


---

## Getting Started

### Prerequisites

*   [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
*   The **HR-Core Suite Backend** application must be running.

### One-Time Environment Setup (Important!)

Before running the projects for the first time, you need to ensure your local machine trusts the .NET development SSL certificate. This is a one-time setup per machine.

1.  Open your terminal/powershell **as an Administrator**.
2.  Run the following command:
    ```bash
    dotnet dev-certs https --trust
    ```
3.  If prompted by a security dialog, click **Yes** to install the certificate.

This command is required to allow the frontend to successfully connect to the backend API over a secure HTTPS connection during development.

### Setup

1.  **Clone the repository:**
    ```bash
    git clone https://github.com/ErisSusanto19/hr-core-suite-backend
    cd hr-core-suite-frontend
    ```

2.  **Configure Backend API Address:**
    *   Open `appsettings.Development.json` in the `HRCoreSuite.Frontend` project.
    *   Set the `BackendApiUrl` key to the address where your backend is running (e.g., `https://localhost:7001`).

3.  **Run the Application:**
    *   Open a terminal in the project's root directory (`src/HRCoreSuite.Frontend`).
    *   Run the following command:
    ```bash
    dotnet run --launch-profile https
    ```
    *   The frontend application is now running and can be accessed in your browser.

---

## How It Works

1.  **Data Fetching:** When a user visits the main page, the Razor Page's C# code-behind will use the `ApiClient` service to make `GET` requests to the backend API endpoints (`/api/employee`, `/api/branch`, etc.).
2.  **Data Display:** The fetched data is then bound to the page model and rendered into HTML tables using Razor syntax.
3.  **File Upload:** The upload form uses `multipart/form-data` to `POST` the selected Excel file. The C# code-behind receives this file and forwards it to the backend's `/api/employee/upload` endpoint using `HttpClient`.