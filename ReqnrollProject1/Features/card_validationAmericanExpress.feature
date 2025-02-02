Feature: AmericanExpress Credit Card Validation
  As an API consumer
  I want to validate credit card details
  So that only valid credit card transactions are processed

Background:
	Given the API base URL is set

	  
Scenario Outline: Validate AmericanExpress card owner field
	Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Owner "<expectedMessage>"

Examples:
	| owner                   | statusCode | expectedMessage | number          |
	| America                 | 200        | 30              | 371449635398401 | # One word is acceptable.
	| AMERICA                 | 200        | 30              | 371449635398402 | # Only capital letters.
	| america                 | 200        | 30              | 371449635398403 | # Only lowercase letters.
	| America Doe             | 200        | 30              | 371449635398404 | # Two words are valid.
	| America Mary Lee        | 200        | 30              | 371449635398405 | # Three words are valid.
	| America Doe Smith Brown | 400        | "Wrong owner"   | 371449635398406 | # Exceeds the 3-word limit.
	| America123              | 400        | "Wrong owner"   | 371449635398407 | # Contains numbers, not allowed.
	| America_Doe             | 400        | "Wrong owner"   | 371449635398408 | # Contains an underscore, which is invalid.
	| America@Smith           | 400        | "Wrong owner"   | 371449635398409 | # Special symbols not allowed.
	| "   America Doe"        | 400        | "Wrong owner"   | 371449635398410 | # Leading spaces are not allowed.
	| "America Doe   "        | 400        | "Wrong owner"   | 371449635398411 | # Trailing spaces are not allowed.
	| "12134 442"             | 400        | "Wrong owner"   | 371449635398412 | # Only numbers are not allowed.
	| ""                      | 400        | "Wrong owner"   | 371449635398413 | # Empty string is not allowed.
	| America  Doe            | 400        | "Wrong owner"   | 371449635398414 | # Contains a double space between words.
	| O'America               | 400        | "Wrong owner"   | 371449635398415 | # Contains an apostrophe (invalid).
	| America\\tBob           | 400        | "Wrong owner"   | 371449635398416 | # Contains a tab character between words.



Scenario Outline: Validate AmericanExpress card number fields
	Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Number "<expectedMessage>"

Examples:
	| owner           | number             | statusCode | expectedMessage |

    # Valid American Express numbers (starting with 34 or 37, 15 digits)
	| America Johnson | 341111111111111    | 200        | 30              | # Valid 15-digit American Express, starting with 34.
	| America Smith   | 371234567890123    | 200        | 30              | # Valid 15-digit American Express, starting with 37.

    # Edge cases for length issues
	| America Brown   | 34111111111111     | 400        | "Wrong number"  | # 14 digits, missing 1 digit.
	| America Taylor  | 3711111111111111   | 400        | "Wrong number"  | # 16 digits, 1 extra digit.
	| America White   | 341111111111       | 400        | "Wrong number"  | # 12 digits, missing 3 digits.

    # Invalid starting digits (should not match 34 or 37)
	| America Garcia  | 361234567890123    | 400        | "Wrong number"  | # Starts with 36, invalid for American Express.
	| America Lopez   | 391234567890123    | 400        | "Wrong number"  | # Starts with 39, invalid for American Express.

    # Invalid characters in the number
	| America Clark   | 34111a111111111    | 400        | "Wrong number"  | # Contains a letter (a).
	| America Davis   | 37123#567890123    | 400        | "Wrong number"  | # Contains a special character (#).
	| America Miller  | aaaaaaaaaaaaaaa    | 400        | "Wrong number"  | # 15 letters, invalid format.
	| America Lopez   | 34111111111aa123   | 400        | "Wrong number"  | # Last 2 characters are letters (invalid).

    # Invalid formatting cases
	| America Brown   | 3411-1111-1111-111 | 400        | "Wrong number"  | # Hyphenated format is invalid.
	| America Lee     | 3712 3456 7890 123 | 400        | "Wrong number"  | # Space-separated format is invalid.

