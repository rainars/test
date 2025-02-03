Feature: MasterCard Credit Card Validation
  As an API consumer
  I want to validate credit card details
  So that only valid credit card transactions are processed

Background:
	Given the API base URL is set


Scenario Outline: Validate MasterCard owner field
	Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Owner "<expectedMessage>"

Examples:
	| owner                  | statusCode | expectedMessage | number           |
	| Master                 | 200        | 20              | 5555555555554444 | # One word is acceptable.
	| MASTER                 | 200        | 20              | 5555555555554123 | # Only capital letters.
	| master                 | 200        | 20              | 5555555555554444 | # Only lowercase letters.
	| Master Doe             | 200        | 20              | 5555555555554444 | # Two words are valid.
	| Master Mary Lee        | 200        | 20              | 5555555555554444 | # Three words are valid.
	| Master Doe Smith Brown | 400        | "Wrong owner"   | 5555555555554444 | # Exceeds the 3-word limit.
	| Master123              | 400        | "Wrong owner"   | 5555555555554444 | # Contains numbers, not allowed.
	| Master_Doe             | 400        | "Wrong owner"   | 5555555555554444 | # Contains an underscore, which is invalid.
	| Master@Smith           | 400        | "Wrong owner"   | 5555555555554444 | # Special symbols not allowed.
	| "   Master Doe"        | 400        | "Wrong owner"   | 5555555555554444 | # Leading spaces are not allowed.
	| "Master Doe   "        | 400        | "Wrong owner"   | 5555555555554444 | # Trailing spaces are not allowed.
	| "12134 442"            | 400        | "Wrong owner"   | 5555555555555444 | # Only numbers are not allowed.
	| ""                     | 400        | "Wrong owner"   | 5555555555555444 | # Empty string is not allowed.
	| Master  Doe            | 400        | "Wrong owner"   | 5555555555554444 | # Contains a double space between words.
	| O'Master               | 400        | "Wrong owner"   | 5555555555554444 | # Contains an apostrophe (invalid).
	| Master\\tBob           | 400        | "Wrong owner"   | 5555555555554444 | # Contains a tab character between words.




Scenario Outline: Validate MasterCard card number fields
	Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Number "<expectedMessage>"

Examples:
	| owner           | number              | statusCode | expectedMessage |
    
    # Valid MasterCard numbers (covering various starting patterns and lengths)
	| Master Johnson  | 5111111111111111    | 200        | 20              | # Valid 16-digit MasterCard, starting with 51.
	| Master Smith    | 5212345678901234    | 200        | 20              | # Valid 16-digit MasterCard, starting with 52.
	| Master Brown    | 5312345678901234    | 200        | 20              | # Valid 16-digit MasterCard, starting with 53.
	| Master Taylor   | 5412345678901234    | 200        | 20              | # Valid 16-digit MasterCard, starting with 54.
	| Master Lee      | 5512345678901234    | 200        | 20              | # Valid 16-digit MasterCard, starting with 55.
	| Master White    | 2221001234567890    | 200        | 20              | # Valid 16-digit MasterCard, starting with 2221 (2-series range).
	| Master Garcia   | 2720999999999999    | 200        | 20              | # Valid 16-digit MasterCard, starting with 2720 (2-series boundary).

    # Edge cases for length and format issues
	| Master Clark    | 511111111111111     | 400        | "Wrong number"  | # 15 digits, missing 1 digit.
	| Master Davis    | 51111111111111111   | 400        | "Wrong number"  | # 17 digits, 1 extra digit.
	| Master Lopez    | 22210012345678901   | 400        | "Wrong number"  | # 17 digits, 1 extra digit in 2-series range.
	| Master Adams    | 53123456789012      | 400        | "Wrong number"  | # 14 digits, missing 2 digits.

    # Invalid characters in the MasterCard number
	| Master Thompson | 5111a11111111111    | 400        | "Wrong number"  | # Contains a letter (a).
	| Master Miller   | 53123#5678901234    | 400        | "Wrong number"  | # Contains a special character (#).
	| Master Brown    | aaaaaaaaaaaaaaaa    | 400        | "Wrong number"  | # 16 letters, invalid format.
	| Master Lopez    | 27209999999999aa    | 400        | "Wrong number"  | # Last 2 characters are letters (invalid).

    # Invalid formatting cases
	| Master Brown    | 5111-1111-1111-1111 | 400        | "Wrong number"  | # Hyphenated format is invalid.
	| Master Lee      | 5312 3456 7890 1234 | 400        | "Wrong number"  | # Space-separated format is invalid.



Scenario Outline: Validate Master card expiration date checks
	Given a credit card Master with issue date "<issueDate>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain "<expectedMessage>"

Examples:
	| issueDate | statusCode | expectedMessage |
	| 12/25     | 200        | 20              | # Valid future date
	| 12/2025   | 200        | 20              | # Valid extended year
	| 01/22     | 400        | Wrong date      | # Expired past date
	| 13/25     | 400        | Wrong date      | # Invalid month
	| 12-25     | 400        | Wrong date      | # Wrong separator
	| 12/252525 | 400        | Wrong date      | # Too many digits in year
	| 12/5      | 400        | Wrong date      | # Insufficient digits in year
	| 02/2020   | 400        | Wrong date      | # Expired leap year date
	| abc/xyz   | 400        | Wrong date      | # Random characters as date
	| 00/25     | 400        | Wrong date      | # Invalid month
	| 12/2025   | 200        | 20              | # Boundary test: December
	| 12/2100   | 200        | 20              | # Far future but realistic



	Scenario Outline: Validate credit Master card CVC
	Given a credit card Master with CVC "<cvc>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain "<expectedMessage>"

Examples:
	| cvc    | statusCode | expectedMessage |
	| 123    | 200        | 20              | # 3-digit valid CVC
	| 1234   | 200        | 20              | # 4-digit valid CVC
	| 12     | 400        | Wrong cvv       | # Too short
	| 12345  | 400        | Wrong cvv       | # Too long
	| 12a    | 400        | Wrong cvv       | # Non-numeric characters
	| 1 23   | 400        | Wrong cvv       | # Spaces within CVC
	| 12#    | 400        | Wrong cvv       | # Special characters
	| abc    | 400        | Wrong cvv       | # only abc
	| " 123" | 400        | Wrong cvv       | # Leading spaces are not allowed.
	| "123 " | 400        | Wrong cvv       | # Trial spaces are not allowed.
	| ""     | 400        | Wrong cvv       | # empty





Scenario: Validate a valid MasterCard credit card
	Given a valid MasterCard credit card
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be 200
	And the response should contain "20"
	 
Scenario: Validate a MasterCard with incorrect CVC
	Given a valid MasterCard with an incorrect CVC "<cvvNumber>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be 400
	And the response should contain "Wrong cvv"


Examples:
	| cvvNumber |
	| 12        | # Too short (less than 3 digits)
	| 12345     | # Too long (more than 4 digits)
	| abc       | # Contains non-numeric characters)
	| 12a       | # Too long (more than 4 digits)
	


