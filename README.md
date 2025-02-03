# test

QA Automation Testing Project: Card Validation API

Overview

This project focuses on automated testing for a Card Validation API using integration tests, unit tests, and a fully automated CI/CD pipeline powered by GitHub Actions and Docker Compose. It simulates real-world validation scenarios, including checking credit card numbers, expiration dates, CVC codes, and more.

The project showcases key QA skills, including:

Automation testing using Reqnroll, NUnit, and SpecFlow.

API testing with POST requests for validating card details.

Containerized environment setup using Docker and Docker Compose.

CI/CD pipeline implementation with GitHub Actions.

Test coverage analysis using Coverlet.

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
