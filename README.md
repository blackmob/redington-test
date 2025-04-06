# Probability Calculator

React + C# Rest API 

## Description

This project is a simple web application that allows users to calculate probabilities.

## Technologies Used

### Frontend
- React
- Next.js
- TypeScript
- Tailwind CSS
- Axios
- SWR
- Zod

### Backend
- C#
- ASP.NET Core
- Mediatr
- FluentValidation
- Serilog
- OpenTelemetry


## Getting started docker-compose

### Prerequisites
- Docker dekstop

### Running the Application with Docker Compose

This is the simplest way to run the application. It uses Docker Compose to build and run both the frontend and backend services in containers.

1. Navigate to the project root:
2. Run the following command to start the application:
   ```bash
   docker-compose up --build
   ```
3. Open your browser and navigate to http://localhost:3000 to access the application.
4. Logs will be available on the host machine in Logs directory at the root of the repository.

## Getting started API

### Prerequisites
 
- net 8.0 SDK

### Running the API

1. Navigate to the project directory:
   ```bash 
   cd server/ProbabilityCalculator.Api
    ```
2. Install dependencies:
      ```bash
   dotnet restore
    ```
3. Build the project:
      ```bash
   dotnet build
    ```
4. Run the project:
      ```bash
   dotnet run
    ```
5. Logs will be available on the host machine in Logs directory.

## Getting Started Front-end

### Prerequisites

- Node.js
- npm or yarn

### Running the Application

1. Navigate to the project directory:
      ```bash
   cd client/probability-calculator
    ``` 
2. Install dependencies:
   ```bash
   npm install
    ```
3. Environment Variables
   The application requires the following environment variables:
   CALCULATOR_API_URL: The URL of the API used for calculations.
   To run this on your local machine ad the .env.local file in the root directory with the following content:
   ```
    CALCULATOR_API_URL=http://localhost:5004/
   ```
4. Running the Application
   Start the development server:
   ```bash
   npm run dev
    ```
   Open your browser and navigate to http://localhost:3000.
 
