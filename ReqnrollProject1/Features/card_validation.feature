Feature: Credit Card Validation
  As an API consumer
  I want to validate credit card details
  So that only valid credit card transactions are processed

Background:
	Given the API base URL is set


Scenario: Validate an invalid card number
	Given an invalid credit card number
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be 400
	And the response should contain "Wrong number"


Scenario Outline: Validate a credit card with a missing owner
	Given a credit card with a missing owner and number "<cardNumber>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be 400
	And the response should contain "Owner is required"

Examples:
	| cardNumber       |
	| 4111111111111111 | # Visa 16 digits
	| 4111111111111    | # Visa 13 digits
	| 5555555555554444 | # MasterCard
	#| 378282246310005  | # American Express
	#| 1234567890123456 | # Invalid card number
