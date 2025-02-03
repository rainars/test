# QA Automation Testing Project: Card Validation API

This project focuses on automated testing for a Card Validation API using integration tests, unit tests, and a fully automated CI/CD pipeline powered by GitHub Actions and Docker Compose. It simulates real-world validation scenarios, including checking credit card numbers, expiration dates, CVC codes, and more.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Project Structure](#project-structure)
- [Test Scenarios Covered](#test-scenarios-covered)
- [CI/CD Pipeline (GitHub Actions)](#cicd-pipeline-github-actions)
- [How to Run Locally](#how-to-run-locally)
- [Test Coverage Report](#test-coverage-report)
---

## Overview

This project demonstrates key QA skills by automating tests for a Card Validation API. It includes:

- **Automation Testing** using Reqnroll, NUnit, and SpecFlow.
- **API Testing** with POST requests for validating card details.
- **Containerized Environment** setup using Docker and Docker Compose.
- **CI/CD Pipeline** implementation with GitHub Actions.
- **Test Coverage Analysis** using Coverlet.

---

## Features

- **End-to-End Automated Testing:** Validates multiple credit card scenarios such as card owner validation, expiration date checks, and more.
- **API Testing:** Uses RESTful POST requests to test card validation endpoints.
- **Integration Testing:** Built with integration tests using Reqnroll and NUnit.
- **Unit Testing:** Validates core logic using NUnit.
- **Dockerized Environment:** Fully containerized testing setup.
- **CI/CD Integration:** Automatically builds and runs tests on every pull request or push to the main branch.
- **Test Coverage Reports:** Automatically generated coverage reports.

---

## Technologies Used

- **C# .NET 8**
- **NUnit** for unit and integration testing
- **Reqnroll** for BDD-style test definitions
- **SpecFlow** for Given/When/Then syntax (if applicable)
- **Docker** & **Docker Compose**
- **GitHub Actions** for CI/CD
- **Coverlet** for test coverage reporting

---

# CI/CD Pipeline (GitHub Actions)

The CI pipeline automatically builds and tests the application on every push or pull request to the main branch.

## Key Steps

1. **Checkout Code:** Retrieves the latest code from the repository.
2. **Set up .NET:** Configures the environment with the necessary .NET version.
3. **Build and Run Unit Tests:** Compiles the solution and runs unit tests.
4. **Build Docker Images:** Builds images for both the API and the integration tests.
5. **Run Integration Tests:** Uses Docker Compose to spin up containers and execute tests.
6. **Collect Test Results:** Copies test result files from containers for reporting.
7. **Upload Test Results:** Provides easy access to the test logs and coverage reports.
---
## How to Run Locally

1. **Clone the Repository:**

   ```bash
   git clone <repository-url>
   cd qa-home-assignment2
   dotnet build CardValidation.sln
   Build the Project:
   dotnet build CardValidation.sln
   Run Unit Tests:
   dotnet test UnitTests/UnitTests.csproj
   Run Docker Compose:
   docker-compose up --build
   Test Coverage Report
   To measure test coverage using Coverlet, run the following command:
   coverlet ./UnitTests/bin/Debug/net8.0 --target "dotnet" --targetargs "test UnitTests/UnitTests.csp

---
## Test Coverage Report

   ```bash
   To measure test coverage, we use Coverlet. Run the following command to generate coverage reports:

   coverlet ./UnitTests/bin/Debug/net8.0 --target "dotnet" --targetargs "test UnitTests/UnitTests.csproj"

   Reports will be generated in the specified directory and can be viewed locally or uploaded as artifacts in the CI pipeline.

---


