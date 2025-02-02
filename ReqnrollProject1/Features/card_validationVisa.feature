Feature: Visa Credit Card Validation
  As an API consumer
  I want to validate credit card details
  So that only valid credit card transactions are processed

Background:
	Given the API base URL is set


Scenario Outline: Validate credit card owner field
	Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Owner "<expectedMessage>"

Examples: 
    | owner                | statusCode | expectedMessage | number        | 
    | Visa                 | 200        | 10              | 4123111111111 | # One word is acceptable.
    | VISA                 | 200        | 10              | 4222111111111 | # Only capital letters.
    | john                 | 200        | 10              | 4333111111111 | # Only lowercase letters.
    | Visa Doe             | 200        | 10              | 4444111111111 | # Two words are valid.
    | Visa Mary Lee        | 200        | 10              | 4555111111111 | # Three words are valid.
    | Visa Doe Smith Brown | 400        | "Wrong owner"   | 4666111111111 | # Exceeds the 3-word limit.
    | Visa123              | 400        | "Wrong owner"   | 4777111111111 | # Contains numbers, not allowed.
    | Visa_Doe             | 400        | "Wrong owner"   | 4888111111111 | # Contains an underscore, which is invalid.
    | Visa@Smith           | 400        | "Wrong owner"   | 4999111111111 | # Special symbols not allowed.
    | "   Visa Doe"        | 400        | "Wrong owner"   | 4123412341234 | # Leading spaces are not allowed.
    | "Visa Doe   "        | 400        | "Wrong owner"   | 4234512341234 | # Trailing spaces are not allowed.
    | "12134 442"          | 400        | "Wrong owner"   | 4345612341234 | # Only numbers are not allowed.
    | ""                   | 400        | "Wrong owner"   | 4456712341234 | # Empty string is not allowed.
    | Visa  Doe            | 400        | "Wrong owner"   | 4567812341234 | # Contains a double space between words.
    | O'Visa               | 400        | "Wrong owner"   | 4678912341234 | # Contains an apostrophe (invalid).
    | Visa\\tBob           | 400        | "Wrong owner"   | 4789012341234 | # Contains a tab character between words.

  

Scenario Outline: Validate Visa card number field errors
    Given a credit card with ONLY owner "<owner>" and number "<number>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain an error for Number "<expectedMessage>"

Examples:
    | owner          | number          | statusCode | expectedMessage |
    | Visa Johnson   | 4111111111111   | 200        | 10              | # Valid 13-digit Visa card
    | Visa Smith     | 4000000000000456| 200        | 10              | # Valid 16-digit card
    
    # Edge cases for 13 digits
    | Visa Brown     | 411111111111    | 400        | "Wrong number"  | # 12 digits, missing 1 digit
    | Visa Taylor    | 41111111111111  | 400        | "Wrong number"  | # 14 digits, 1 extra digit
    
    # Edge cases for 16 digits
    | Visa Lee       | 411111111111111 | 400        | "Wrong number"  | # 15 digits, missing 1 digit
    | Visa White     | 41111111111111111| 400       | "Wrong number"  | # 17 digits, 1 extra digit
    
    # Invalid characters in the Visa number
    | Visa Clark     | 4111a11111111111| 400        | "Wrong number"  | # 16 digits, Contains a letter
    | Visa Garcia    | 4111a11111111   | 400        | "Wrong number"  | # 13 digits, Contains a letter
    | Visa Miller    | aaaaaaaaaaaaa   | 400        | "Wrong number"  | # 13 letters
    | Visa Davis     | 41111@1111111111| 400        | "Wrong number"  | # 16 digits, Contains a special character
    | Visa Lopez     | 41111@1111111   | 400        | "Wrong number"  | # 13 digits, Contains a special character
    
    # Invalid formatting cases
    | Visa Adams     | 4111-1111-1111-1111 | 400    | "Wrong number"  | # 16 digits, Hyphenated format is invalid
    | Visa Thompson  | 4111 1111 1111 1111 | 400    | "Wrong number"  | # 16 digits, Space-separated format is invalid



Scenario Outline: Validate Visa card expiration date checks
	Given a credit card with issue date "<issueDate>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain "<expectedMessage>"

Examples:
	| issueDate | statusCode | expectedMessage |
	| 12/25     | 200        | 10              | # Valid future date
	| 12/2025   | 200        | 10              | # Valid extended year
	| 01/22     | 400        | Wrong date      | # Expired past date
	| 13/25     | 400        | Wrong date      | # Invalid month
	| 12-25     | 400        | Wrong date      | # Wrong separator
	#| 12/252525 | 400        | Wrong date      | # Too many digits in year
	| 12/5      | 400        | Wrong date      | # Insufficient digits in year
	#| 02/2024   | 200        | 10              | # Leap year valid date
	| 02/2020   | 400        | Wrong date      | # Expired leap year date
	#| 02/29     | 400        | Wrong date      | # February 29 invalid non-leap year
	| 02/28     | 200        | 10              | # February 28 in non-leap year
	#| 122025    | 400        | Wrong date      | # Incorrect format, no separator
	#| " 12/25 " | 200        | 10              | # Whitespace before/after date
	#| 01/25     | 200        | 10              | # Leading zero in month
	#| 1/25      | 200        | 10              | # No leading zero in month
	| abc/xyz   | 400        | Wrong date      | # Random characters as date
	| 00/25     | 400        | Wrong date      | # Invalid month
	| 12/2025   | 200        | 10              | # Boundary test: December
	| 12/2100   | 200        | 10              | # Far future but realistic
	#| 12/2500   | 400        | Wrong date      | # Year beyond practical limit

  
Scenario Outline: Validate credit card CVC
	Given a credit card with CVC "<cvc>"
	When I send a POST request to "/CardValidation/card/credit/validate"
	Then the response status should be <statusCode>
	And the response should contain "<expectedMessage>"

Examples:
	| cvc    | statusCode | expectedMessage |
	| 123    | 200        | 10              | # 3-digit valid CVC
	| 1234   | 200        | 10              | # 4-digit valid CVC
	| 12     | 400        | Wrong cvv       | # Too short
	| 12345  | 400        | Wrong cvv       | # Too long
	| 12a    | 400        | Wrong cvv       | # Non-numeric characters
	| 1 23   | 400        | Wrong cvv       | # Spaces within CVC
	| 12#    | 400        | Wrong cvv       | # Special characters
	| abc    | 400        | Wrong cvv       | # only abc
	| " 123" | 400        | Wrong cvv       | # Leading spaces are not allowed.
	| "123 " | 400        | Wrong cvv       | # Trial spaces are not allowed.
	| ""     | 400        | Wrong cvv       | # empty


Scenario Outline: Validate credit card with invalid CVC and owner
    Given a credit card with owner "<owner>" and CVC "<cvv>"
    When I send a POST request to "/CardValidation/card/credit/validate"
    Then the response status should be <statusCode>
    And the response should contain errors for owner "<ownerError>" and CVC "<cvvError>"

Examples:
    | owner         | cvv    | statusCode | ownerError     | cvvError      |
    | John123       | 12     | 400        | "Wrong owner"  | "Wrong cvv"   |
    | John_Doe      | 12345  | 400        | "Wrong owner"  | "Wrong cvv"   |
    | Jane@Smith    | 12a    | 400        | "Wrong owner"  | "Wrong cvv"   |
    | " John"       | 1 23   | 400        | "Wrong owner"  | "Wrong cvv"   |
    | John_Doe      | abc    | 400        | "Wrong owner"  | "Wrong cvv"   |
    | " John"       | " 123" | 400        | "Wrong owner"  | "Wrong cvv"   |
    | "John Doe "   | "123 " | 400        | "Wrong owner"  | "Wrong cvv"   |
    | ""            | ""     | 400        | "Wrong owner"  | "Wrong cvv"   |


##	scenario outline: validate credit card with invalid cvc, owner, date, and number
##    given a credit card with owner "<owner>", cvc "<cvv>", date "<date>", and number "<number>"
##    when i send a post request to "/cardvalidation/card/credit/validate"
##    then the response status should be <statuscode>
##    and the response should contain errors for owner "<ownererror>", cvc "<cvverror>", date "<dateerror>", and number "<numbererror>"
##
##
##examples:
##    | owner         | cvv    | date    | number             | statuscode | ownererror     | cvverror      | dateerror     | numbererror     |
##    | john123       | 12     | 13/25   | 12345678901234     | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # 4 all are wrong
##    | john_doe      | 12345  | 12-25   | 4111111111111      | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # 2 fields are wrong (owner and cvc)
##    | jane@smith    | 12a    | 01/22   | 41111111111111116  | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # owner and date are wrong
##    | john_doe      | abc    | 12/23   | 41111111111116     | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # 3 fields are wrong (owner, cvc, and date)
##    | "john doe "   | "123 " | 01/25   | 123456             | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # 3 fields are wrong (number, date, and cvc)
##    | ""            | ""     | ""      | ""                 | 400        | "wrong owner"  | "wrong cvv"   | "wrong date"  | "wrong number"  | # 4 fields are empty and wrong


