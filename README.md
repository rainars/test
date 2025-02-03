QA Automation Testing Project: Card Validation API
This project focuses on automated testing for a Card Validation API using integration tests, unit tests, and a fully automated CI/CD pipeline powered by GitHub Actions and Docker Compose. It simulates real-world validation scenarios, including checking credit card numbers, expiration dates, CVC codes, and more.

Table of Contents
Overview
Features
Technologies Used
Project Structure
Test Scenarios Covered
CI/CD Pipeline (GitHub Actions)
How to Run Locally
Test Coverage Report
Future Improvements
Contact
License
Overview
This project demonstrates key QA skills by automating tests for a Card Validation API. It includes:

Automation Testing using Reqnroll, NUnit, and SpecFlow.
API Testing with POST requests for validating card details.
Containerized Environment setup using Docker and Docker Compose.
CI/CD Pipeline implementation with GitHub Actions.
Test Coverage Analysis using Coverlet.
Features
End-to-End Automated Testing: Validates multiple credit card scenarios such as card owner validation, expiration date checks, and more.
API Testing: Uses RESTful POST requests to test card validation endpoints.
Integration Testing: Built with integration tests using Reqnroll and NUnit.
Unit Testing: Validates core logic using NUnit.
Dockerized Environment: Fully containerized testing setup.
CI/CD Integration: Automatically builds and runs tests on every pull request or push to the main branch.
Test Coverage Reports: Automatically generated coverage reports.
Technologies Used
C# .NET 8
NUnit for unit and integration testing
Reqnroll for BDD-style test definitions
SpecFlow for Given/When/Then syntax (if applicable)
Docker & Docker Compose
GitHub Actions for CI/CD
Coverlet for test coverage reporting
Project Structure
bash
Copy
qa-home-assignment2/
├── CardValidation.Core/                  # Core business logic
├── CardValidation.Web/                   # Web API project
├── IntegrationTests/                     # Integration tests
│   ├── Features/                         # SpecFlow features
│   ├── StepDefinitions/                  # Step definitions for BDD
├── UnitTests/                            # Unit tests
├── docker-compose.yml                    # Docker Compose setup
├── Dockerfile                            # Docker configuration for API
├── Dockerfile.integration-tests          # Docker configuration for tests
├── .github/workflows/ci-pipeline.yml     # CI pipeline
├── README.md                             # Project README file
Test Scenarios Covered
Unit Tests
Owner Validation: Ensures that the card owner name does not contain sensitive credit card data.
CVC Checks: Verifies that the CVC is correctly validated.
Expiration Date Checks: Tests valid and expired credit cards.
Integration Tests (BDD Approach)
Example Scenario
gherkin
Copy
Scenario Outline: Validate MasterCard expiration date checks
  Given a credit card with ONLY owner "<owner>" and number "<number>"
  Given a credit card with issue date "<issueDate>"
  When I send a POST request to "/CardValidation/card/credit/validate"
  Then the response status should be <statusCode>
  And the response should contain "<expectedMessage>"

Examples:
  | owner       | number           | issueDate | statusCode | expectedMessage         |
  |-------------|------------------|-----------|------------|-------------------------|
  | John Doe    | 5555555555554444 | 01/20     | 400        | "Credit card expired"   |
  | Jane Smith  | 5555555555554444 | 12/25     | 200        | "Credit card is valid"  |
CI/CD Pipeline (GitHub Actions)
The CI pipeline is designed to automatically build and test the application on every push or pull request to the main branch.

Key Steps
Checkout Code: Retrieves the latest code from the repository.
Set up .NET: Configures the environment with the necessary .NET version.
Build and Run Unit Tests: Compiles the solution and runs unit tests.
Build Docker Images: Builds images for both the API and the integration tests.
Run Integration Tests: Uses Docker Compose to spin up containers and execute tests.
Collect Test Results: Copies test result files from containers for reporting.
Upload Test Results: Provides easy access to the test logs and coverage reports.
CI Pipeline Code
yaml
Copy
name: CI Pipeline

on:
  push:
    branches:
      - main
  pull_request:
    branches:
      - main

jobs:
  build-and-test:
    runs-on: ubuntu-latest

    services:
      docker:
        image: docker:20.10.16
        options: --privileged

    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Set up .NET environment
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'

      - name: Install Docker Compose
        run: |
          sudo curl -L "https://github.com/docker/compose/releases/download/v2.17.2/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
          sudo chmod +x /usr/local/bin/docker-compose

      - name: Build and run unit tests
        run: |
          dotnet build CardValidation.sln --configuration Release
          dotnet test UnitTests/UnitTests.csproj --results-directory ./test-results/unit

      - name: Build Docker images
        run: |
          docker build -t card-validation-api -f CardValidation.Web/Dockerfile .
          docker build -t card-validation-integration-tests -f Dockerfile.integration-tests .

      - name: Run integration tests with Docker Compose
        run: docker-compose up --abort-on-container-exit --build

      - name: Copy integration test results from Docker
        if: always()
        run: |
          mkdir -p ./test-results/integration
          docker cp $(docker ps -aqf "name=integration-tests"):/app/ReqnrollProject1/TestResults ./test-results/integration

      - name: Upload test results (optional)
        uses: actions/upload-artifact@v4
        with:
          name: TestResults
          path: "**/*.trx"
How to Run Locally
Clone the Repository:

bash
Copy
git clone <repository-url>
cd qa-home-assignment2
Build the Project:

bash
Copy
dotnet build CardValidation.sln
Run Unit Tests:

bash
Copy
dotnet test UnitTests/UnitTests.csproj
Run Docker Compose:

bash
Copy
docker-compose up --build
Test Coverage Report
To measure test coverage using Coverlet, run the following command:

bash
Copy
coverlet ./UnitTests/bin/Debug/net8.0 --target "dotnet" --targetargs "test UnitTests/UnitTests.csproj"
Coverage reports will be generated in the specified directory and can be viewed locally or uploaded as artifacts in the CI pipeline.

Future Improvements
Expand test cases to cover additional edge cases.
Integrate API performance tests using Locust or similar tools.
Automate deployment using Docker Swarm or Kubernetes.
Enhance reporting through dashboards or HTML reports.
