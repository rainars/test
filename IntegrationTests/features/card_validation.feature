Feature: Credit Card Validation
  As an API consumer
  I want to validate credit card details
  So that only valid credit card transactions are processed

  Background:
    Given the API base URL is set

  Scenario: Validate a valid Visa credit card
    Given a valid Visa credit card
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status should be 200
    And the response should return "Visa"

  Scenario: Validate a MasterCard with incorrect CVC
    Given a valid MasterCard with an incorrect CVC
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status should be 400
    And the response should contain an error message "Wrong cvv"

  Scenario: Validate a card with missing owner name
    Given a credit card with missing owner name
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status should be 400
    And the response should contain an error message "Owner is required"

  Scenario: Validate an invalid card number
    Given an invalid card number "1234567890123456"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status should be 400
    And the response should contain an error message "Wrong number"
