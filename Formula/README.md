```
- - Author:	  Teagan Smith
- Date:		  08/30/24 7:43 PM
- Assignment: Project Spreadsheets 1 (PS1)
- Github ID:  tweagan11
- Copyright:  CS 3500 and Teagan Smith
```

This template is the starting place for the first
lab and the first assignment.  The Formula Tests project should
contain:

1) Formula.cs
2) FormulaSyntaxTests.cs
3) EvaluationTests.cs
4) stylecop.json

The solution folder should contain this README as well as the .editorconfig file.

This solution contains a testing suite given to us by a black-box means of testing.
Later we added functionality of the Formula class ourselves, as well as white-box testing giving us a grey-box testing suite.

# Formula Format
## Version 1
The Formula Class will take in a string following the given rules, and creates a Formula Object.
This has 50+ tests to ensure accuracy in the creation of a formula, regarding tokens and syntax such as:
- Valid Tokens
		- (, ), +, -, *, /, variables, and numbers
- One Token Rule
		- Must be one token
- Closing Parenthesis Rule
		- Left to right, no point should the number of closing parenthesis exceed the number of opening parenthesis.
- Balanced Parenthesis token
		- Total number of Open and Closing parentheses should match.
- First Token Rule
		- First token must be number, variable, or an opening parenthesis.
- Last Token Rule
		- Last token must be a number, variable, or a closing parenthesis.
- Parenthesis/Operator Following Rule
		- Any token that immediately follows an opening parenthesis or an operator must
		  either be a number , variable, or an opening parenthesis.
- Extra Following Rule
		- Any token that immediately follows a number, a variable, or a closing parenthesis
		  must either be an operator or a closing parenthesis.

Our formula class can also return all variables and give us a canonical representation of the Formula.

## Version 2
After adding the Evaluate functionality to our Formula class, we can now take in a formula and process the operators
and the values of the given formula and produce a value for the Formula class. Only Errors possible are a division by 0 or
a not known variable value, which we will account for later.

We have also added and overidden the methods for:

	1) Equals()
	2) ==
	3) !=
	4) GetHashCode()


# Consulted Peers:

- Chase Hammond
- Dennis Pancheka
- Jacob (Last Name Unavailable)

# References:

- https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
- Assignment One Specifications for Definitions and Syntax Definitions.
- Assignment Two Specifications
- Assignment Three Specifications

# Time Spent on Project

Estimated Time for Project 4: 6 hours
Time Spent (Learning): 1 hour
Time Spent (Creating Methods): 2 hour
Time Spent (Debugging): .5 hours
Actual Time Spent on Project Formula Class: 3.5 Hours