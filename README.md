# Ministry Manager

A web application for managing ministry-related tasks including calendar events, reflections, and notes.

- Members
    1. Anthony Raesch - Project Manager
    2. Harrison Robichaux - Communications Lead
    3. Joseph Ashton Berret - QA Tester
    4. Brandon Hoang - Git Master
    5. Jonathan Anderson - Design Lead

## Required Software Installation

1. Visual Studio Code or another preferred IDE: 
   - Download and install from: https://code.visualstudio.com/

2. .NET 8.0 SDK:
   - Download and install from: https://dotnet.microsoft.com/download/dotnet/8.0
   - Verify installation by opening terminal/command prompt and running:
     ```bash
     dotnet --version
     ```
   - Should show version 8.0.x

3. Node.js and npm:
   - Download and install from: https://nodejs.org/
   - Choose the LTS (Long Term Support) version
   - Verify installation:
     ```bash
     node --version
     npm --version
     ```

4. SQLite:
   - Windows: Download from https://www.sqlite.org/download.html
   - Mac: Potentially pre-installed, but if not you are able to download it from their website
   - Linux: Install via package manager:
     ```bash
     sudo apt-get install sqlite3
     ```
   - Verify installation:
     ```bash
     sqlite3 --version
     ```

## Setup Instructions

1. Clone the repository:
```bash
git clone [repository-url]
cd Team-6
```

2. Navigate to the web project:
```bash
cd src/Team6.Web
```

3. Install TypeScript and other dependencies:
```bash
npm install
```

4. Compile TypeScript files:
```bash
npx tsc
```

5. Install project dependencies:
```bash
dotnet restore
```

6. Build the project:
```bash
dotnet build
```

7. Run the application:
```bash
dotnet run
```

8. Open your web browser and navigate to:
   - http://localhost:5000

## Troubleshooting

If you encounter any errors:

- Build errors:
   - Make sure you're in the correct directory (Team6.Web)
   - Try cleaning the solution:
     ```bash
     dotnet clean
     dotnet build
     ```

- Database errors:
   - The database file (app.db) should be created automatically
   - If issues persist, delete app.db and restart the application
   - The database can be managed/accessed in the terminal via the command sqlite3 app.db
 
- Login errors:
   - If you encounter any login errors theres a few things you can do to resolve the issue:
   - make sure your username is at least 3 characters long
   - make sure your password is at least 6 characters long
   - make sure your email is in the form of foo@bar.com 

- TypeScript errors:
   - Verify Node.js installation
   - Try removing node_modules folder and running:
     ```bash
     npm install
     npx tsc
     ```

## Features

- User Authentication
  - Registration
  - Login/Logout
  - Password Reset
  - Account lockout protection

- Calendar
  - Event creation and management
  - Date navigation
  - Event details view

- Reflections/Notes
  - PDF document upload
  - Document viewing
  - Personal notes

- FAQ Page

- Dark Mode

## Technology Stack

- Backend:
  - ASP.NET Core 8.0
  - Razor Pages
  - Dapper
  - SQLite

- Frontend:
  - TypeScript
  - Bootstrap 5
  - CSS Variables for theming

## Dark/Light Mode

The application supports system-level dark/light mode preferences automatically. To change the appearance, you must modify your system wide settings.
