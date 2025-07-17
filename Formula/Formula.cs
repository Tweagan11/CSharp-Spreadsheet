// <copyright file="Formula.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <summary>
// Author:    Teagan Smith
// Partner:   None
// Date:      09/19/24
// Course:    CS 3500, University of Utah, School of Computing
// Copyright: CS 3500 and Teagan Smith - This work may not be
//            copied for use in Academic Coursework.
//
// I, Teagan, certify that I wrote this code from scratch and
// did not copy it in part or whole from another source.  All
// references used in the completion of the assignments are cited
// in my README file.
//
// File Contents
//
//    This File contains our Formula Class that we will be using
//    for our Spreadsheet Project. When constructed, the Formula
//    stores and modifies certain information to help canonically
//    represent our formula. It also checks the Formula provided is
//    syntactically correct.
// </summary>

namespace CS3500.Formula;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

/// <summary>
/// <para>
/// This class represents formulas written in standard infix notation using
/// standard precedence
/// rules. The allowed symbols are non-negative numbers written using double-precision
/// floating-point syntax; variables that consist of one or more letters
/// followed by
/// one or more numbers; parentheses; and the four operator symbols +, -, *,
/// and /.
/// </para>
/// <para>
/// Spaces are significant only insofar that they delimit tokens. For example,
/// "xy" is
/// a single variable, "x y" consists of two variables "x" and y; "x23" is a
/// single variable;
/// and "x 23" consists of a variable "x" and a number "23". Otherwise, spaces
/// are to be removed.
/// </para>
/// <para>
/// For Assignment Two, you are to implement the following functionality:
/// </para>
/// <list type="bullet">
/// <item>
/// Formula Constructor which checks the syntax of a formula.
/// </item>
/// <item>
/// Get Variables
/// </item>
/// <item>
/// ToString
/// </item>
/// </list>
/// </summary>
public class Formula
{
    /// <summary>
    /// All variables are letters followed by numbers. This pattern
    /// represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    /// Pattern to find operators, (+, -, / or *).
    /// </summary>
    private const string OperatorPattern = @"^[+-/*]$";

    /// <summary>
    /// Initializes a new instance of the <see cref="Formula"/> class.
    /// <para>
    /// Creates a Formula from a string that consists of an infix expression
    /// written as
    /// described in the class comment. If the expression is syntactically
    /// incorrect,
    /// throws a FormulaFormatException with an explanatory Message. See the
    /// assignment
    /// specifications for the syntax rules you are to implement.
    /// </para>
    /// <para>
    /// Non-Exhaustive Example Errors:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// Invalid variable name, e.g., x, x1x (Note: x1 is valid, but would
    /// be normalized to X1)
    /// </item>
    /// <item>
    /// Empty formula, e.g., string.Empty
    /// </item>
    /// <item>
    /// Mismatched Parentheses, e.g., "(("
    /// </item>
    /// <item>
    /// Invalid Following Rule, e.g., "2x+5"
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be
    /// created.</param>
    public Formula(string formula)
    {
        List<string> formulaTokens = GetTokens(formula);
        this.FormulaTokens = formulaTokens;
        CheckEmptyFormula(this.FormulaTokens);

        // When formula is not empty, we will make a tokenized list of
        // all the elements to check them for certain rules.
        CheckSyntax(formulaTokens);
        NormalizeTokens(ref formulaTokens);

        this.FormulaString = string.Join(string.Empty, formulaTokens);
        this.FormulaVariables = this.GetVariables();
    }

    /// <summary>
    /// Gets or sets a canonical representation of a string to display the formula.
    /// </summary>
    private string FormulaString { get; set; }

    /// <summary>
    /// Gets or sets a list of valid tokens in order of the formula.
    /// </summary>
    private List<string> FormulaTokens { get; set; }

    /// <summary>
    /// Gets or sets a Hashset of valid variables of the formula.
    /// </summary>
    private ISet<string> FormulaVariables { get; set; }

    /// <summary>
    /// <para>
    /// Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    /// </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2)
    {
        return f1.Equals(f2);
    }

    /// <summary>
    /// <para>
    /// Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    /// </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        return !f1.Equals(f2);
    }

    /// <summary>
    /// <para>
    /// Determines if two formula objects represent the same formula.
    /// </para>
    /// <para>
    /// By definition, if the parameter is null or does not reference
    /// a Formula Object then return false.
    /// </para>
    /// <para>
    /// Two Formulas are considered equal if their canonical string representations
    /// (as defined by ToString) are equal.
    /// </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    /// True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Formula))
        {
            return false;
        }

        if (this.ToString() == obj.ToString())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// <para>
    /// Returns a set of all the variables in the formula.
    /// </para>
    /// <remarks>
    /// Important: no variable may appear more than once in the returned set,
    /// even if it is used more than once in the Formula.
    /// </remarks>
    /// <para>
    /// For example, if N is a method that converts all the letters in a string
    /// to upper case:
    /// </para>
    /// <list type="bullet">
    /// <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and
    /// "Z1".</item>
    /// <item>new("x1+X1" ).GetVariables() should enumerate "X1".</item>
    /// </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables
    /// referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        if (this.FormulaVariables == null)
        {
            this.FormulaVariables = new HashSet<string>();

            foreach (string s in this.FormulaTokens)
            {
                if (IsVar(s))
                {
                    this.FormulaVariables.Add(s);
                }
            }
        }

        return this.FormulaVariables;
    }

    /// <summary>
    /// <para>
    /// Returns a string representation of a canonical form of the formula.
    /// </para>
    /// <para>
    /// The string will contain no spaces.
    /// </para>
    /// <para>
    /// If the string is passed to the Formula constructor, the new Formula f
    /// will be such that this.ToString() == f.ToString().
    /// </para>
    /// <para>
    /// All the variables in the string will be normalized. This
    /// means capital letters.
    /// </para>
    /// <para>
    /// For example:
    /// </para>
    /// <code>
    /// new("x1 + y1").ToString() should return "X1+Y1"
    /// new("X1 + 5.0000").ToString() should return "X1+5".
    /// </code>
    /// <para>
    /// This code should execute in O(1) time.
    /// </para>
    /// </summary>
    /// <returns>
    /// A canonical version (string) of the formula. All "equal" formulas
    /// should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return this.FormulaString;
    }

    /// <summary>
    /// <para>
    /// Evaluates this Formula, using the lookup delegate to determine the values of
    /// variables.
    /// </para>
    /// <remarks>
    /// When the lookup method is called, it will always be passed a Normalize (capitalized)
    /// variable name. The lookup method will throw an ArgumentException if there is
    /// not a definition for that variable token.
    /// </remarks>
    /// <para>
    /// If no undefined variables or divisions by zero are encountered when evaluating
    /// this Formula, the numeric value of the formula is returned. Otherwise, a
    /// FormulaError is returned (with a meaningful explanation as the Reason property).
    /// </para>
    /// <para>
    /// This method should never throw an exception.
    /// </para>
    /// </summary>
    /// <param name="lookup">
    /// <para>
    /// Given a variable symbol as its parameter, lookup returns the variable's (double) value
    /// (if it has one) or throws an ArgumentException (otherwise). This method
    /// should expect
    /// variable names to be capitalized.
    /// </para>
    /// </param>
    /// <returns> Either a double or a formula error, based on evaluating the
    /// formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        Stack<double> valueStack = new Stack<double>();
        Stack<string> operatorStack = new Stack<string>();
        double value = 0;
        object result;

        foreach (string s in this.FormulaTokens)
        {
            if (IsDigit(s))
            {
                value = double.Parse(s);
                valueStack.Push(value);

                if (OperatorOnTop(operatorStack, "*", "/"))
                {
                    result = OperationAlgorithm(ref valueStack, ref operatorStack);
                    if (result is FormulaError)
                    {
                        return result;
                    }

                    valueStack.Push((double)result);
                }
            }
            else if (IsVar(s))
            {
                try
                {
                    value = lookup(s);
                    valueStack.Push(value);
                }
                catch
                {
                    return new FormulaError("Unknown Variable");
                }

                if (OperatorOnTop(operatorStack, "*", "/"))
                {
                    result = OperationAlgorithm(ref valueStack, ref operatorStack);
                    if (result is FormulaError)
                    {
                        return result;
                    }

                    valueStack.Push((double)result);
                }
            }
            else if (s == "+" || s == "-")
            {
                if (OperatorOnTop(operatorStack, "+", "-"))
                {
                    result = OperationAlgorithm(ref valueStack, ref operatorStack);
                    valueStack.Push((double)result);
                }

                operatorStack.Push(s);
            }
            else if (s == "*" || s == "/")
            {
                operatorStack.Push(s);
            }
            else if (s == "(")
            {
                operatorStack.Push(s);
            }
            else if (s == ")")
            {
                if (OperatorOnTop(operatorStack, "+", "-"))
                {
                    result = OperationAlgorithm(ref valueStack, ref operatorStack);
                    valueStack.Push((double)result);
                }

                if (operatorStack.Peek().Equals("("))
                {
                    operatorStack.Pop();
                }

                if (OperatorOnTop(operatorStack, "*", "/"))
                {
                    result = OperationAlgorithm(ref valueStack, ref operatorStack);
                    if (result is FormulaError)
                    {
                        return result;
                    }

                    valueStack.Push((double)result);
                }
            }
        }

        if (operatorStack.Count == 0)
        {
            return valueStack.Pop();
        }
        else
        {
            result = OperationAlgorithm(ref valueStack, ref operatorStack);
            return (double)result;
        }
    }

    /// <summary>
    /// <para>
    /// Returns a hash code for this Formula. If f1.Equals(f2), then it must be the
    /// case that f1.GetHashCode() == f2.GetHashCode(). Ideally, the probability
    /// that two
    /// randomly-generated unequal Formulas have the same hash code should be
    /// extremely small.
    /// </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return this.FormulaString.GetHashCode();
    }

    // TODO: Add Comments to OperatorOnTop

    /// <summary>
    /// This checks the stack of Operators and peeks to see if the elements on top match the given elements.
    /// </summary>
    /// <param name="operatorStack">The operator stack.</param>
    /// <param name="elem1">First element to compare.</param>
    /// <param name="elem2">Second Element to Compare.</param>
    /// <returns>This should return true or false if the top matches the given elements.</returns>
    private static bool OperatorOnTop(Stack<string> operatorStack, string elem1, string elem2)
    {
        return operatorStack.Count > 0 &&
                           (operatorStack.Peek().Equals(elem1) ||
                           operatorStack.Peek().Equals(elem2));
    }

    /// <summary>
    /// This function aids in the process of evaluating the given formula. It filters through the operators
    /// and processes them accordingly. It also catches division by 0 errors.
    /// </summary>
    /// <param name="values">This is the stack of values in the formula, and we modify this in this function, hence the ref.</param>
    /// <param name="operators">This is the stack of operators in the formula, giving us our next operation. We modify this, hence the ref.</param>
    /// <returns>Returns an object, either a double from the evaluation or an error if there is a division by 0.
    /// </returns>
    private static object OperationAlgorithm(ref Stack<double> values, ref Stack<string> operators)
    {
        double value;
        double secondValue = values.Pop();
        double firstValue = values.Pop();
        string oldOperator = operators.Pop();

        // Check and Apply correct operator
        if (oldOperator == "+")
        {
            value = firstValue + secondValue;
        }
        else if (oldOperator.Equals("-"))
        {
            value = firstValue - secondValue;
        }
        else if (oldOperator.Equals("*"))
        {
            value = firstValue * secondValue;
        }
        else
        {
            if (secondValue == 0)
            {
                return new FormulaError("Division by 0, not valid");
            }

            value = firstValue / secondValue;
        }

        return value;
    }

    /// <summary>
    /// Reports whether "token" is a variable. It must be one or more letters
    /// followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1.
    /// </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched
        // is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    /// Reports whether "token" is a digit, that is possibly an integer, double, or scientific notation.
    /// </summary>
    /// <param name="token"> A token that may be a digit. </param>
    /// <returns> true if the string matches the requirements, e.g., 0-9, 3.5, 2e6.
    /// </returns>
    private static bool IsDigit(string token)
    {
        string singleDigitPattern = @"^(\d+(\.\d*)?|\.\d+)([eE][+-]?\d+)?$";
        return Regex.IsMatch(token, singleDigitPattern);
    }

    /// <summary>
    /// In our Formula Class, The only tokens in the expression are (, ), +, -, *, /, variables, and numbers.
    /// This checks to make sure that the tokens given qualify under given descriptions.
    /// </summary>
    /// <param name="token"> This string is used to verify that it is a valid token as described.</param>
    /// <returns> true if the string passes all given patterns/valid token definitions.
    /// </returns>
    private static bool IsValidToken(string token)
    {
        // Re-use pattern to check for valid tokens
        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*\/]";
        string doublePattern = @"^(?:\d+\.\d*|\d*\.\d+|\d+)(?:[eE][\+-]?\d+)?$";

        // Overall pattern
        string pattern = string.Format(
            "({0})|({1})|({2})|({3})|({4})",
            lpPattern,
            rpPattern,
            opPattern,
            VariableRegExPattern,
            doublePattern);

        return Regex.IsMatch(token, pattern);
    }

    /// <summary>
    /// In our Formula Class, the first token must be a number, a variable, or an opening parenthesis.
    /// The last token must be The last token of an expression must be a number, a variable, or a closing parenthesis.
    /// </summary>
    /// <param name="firstToken"> This is the first token of our tokenized list, or the first token in the formula.</param>
    /// <param name="lastToken"> This is the last token of our tokenized list, or the last valid token in the formula.</param>
    /// <returns> true if the string violates conditions of Extra Following Rule.
    /// </returns>
    private static bool ValidFirstAndLast(string firstToken, string lastToken)
    {
        // Check that the first token is a number, variable, or open parenthesis.
        bool firstValid = firstToken.Equals("(") ||
                           IsVar(firstToken) ||
                           IsDigit(firstToken);

        // Check that the last token is a number, variable, or closing parenthesis.
        bool lastValid = lastToken.Equals(")") ||
                          IsVar(lastToken) ||
                          IsDigit(lastToken);

        return firstValid && lastValid;
    }

    /// <summary>
    /// In our Formula Class, Any token that immediately follows an opening parenthesis
    /// or an operator must be either a number, a variable, or an opening parenthesis.
    /// </summary>
    /// <param name="previousToken"> This is the token that proceeds the current token.</param>
    /// <param name="currentToken"> This must be a Variable, Digit, or opening parenthesis.</param>
    /// <returns> true if the string holds conditions of Operator/Parenthesis Following rule.
    /// </returns>
    private static bool OperParenFollowingRule(string previousToken, string currentToken)
    {
        if (previousToken.Equals("(") ||
            Regex.IsMatch(previousToken, OperatorPattern))
        {
            return IsVar(currentToken) ||
                    IsDigit(currentToken) ||
                    currentToken == "(";
        }
        else
        {
            // We return true because if it doesn't apply, then we cannot say it violates the rule.
            return true;
        }
    }

    /// <summary>
    /// In our Formula Class, any token that immediately follows a number, a variable, or a
    /// closing parenthesis must be either an operator or a closing parenthesis.
    /// </summary>
    /// <param name="previousToken"> This is the token that proceeds the current token.</param>
    /// <param name="currentToken"> This must be a closing parenthesis or operator, or the rule will be violated.</param>
    /// <returns> true if the string holds conditions of Extra Following Rule, or does not apply.
    /// </returns>
    private static bool ExtraFollowingRule(string previousToken, string currentToken)
    {
        if (IsVar(previousToken) ||
            IsDigit(previousToken) ||
            previousToken == ")")
        {
            return currentToken == ")" ||
                   Regex.IsMatch(currentToken, OperatorPattern);
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// <para>
    /// Given an expression, enumerates the tokens that compose it.
    /// </para>
    /// <para>
    /// Tokens returned are:
    /// </para>
    /// <list type="bullet">
    /// <item>left paren</item>
    /// <item>right paren</item>
    /// <item>one of the four operator symbols</item>
    /// <item>a string consisting of one or more letters followed by one or
    /// more numbers</item>
    /// <item>a double literal</item>
    /// <item>and anything that doesn't match one of the above patterns</item>
    /// </list>
    /// <para>
    /// There are no empty tokens; white space is ignored (except to separate
    /// other tokens).
    /// </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as
    /// 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = new List<string>();
        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*\/]";
        string doublePattern = @"(?:\d+\.\d*|\d*\.\d+|\d+)(?:[eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
        lpPattern,
        rpPattern,
        opPattern,
        VariableRegExPattern,
        doublePattern,
        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }

    /// <summary> This simple test checks to make sure that no empty formulas are being passed after
    /// the formula has been tokenized and split into different valid tokens.
    /// </summary>
    /// <param name="formulaTokensList">A list of tokens that has not yet been syntactically checked.</param>
    private static void CheckEmptyFormula(List<string> formulaTokensList)
    {
        if (formulaTokensList.Count == 0)
        {
            throw new FormulaFormatException("Formula cannot be empty, must contain at least one token.");
        }
    }

    /// <summary> Before initializing the Formula, we must verify that it is syntactically correct. By Definition, that means
    /// that it must follow certain rules.
    /// </summary>
    /// <item> One Token Rule.
    /// </item>
    /// <item> Valid Token Rule.
    /// </item>
    /// <item> Closing Parenthesis Rule.
    /// </item>
    /// <item> Balanced Parenthesis Rule.
    /// </item>
    /// <item> First and Last Token Rule.
    /// </item>
    /// <item> Following Rules (Parenthesis/Operator and Following).
    /// </item>
    /// <param name="formulaTokensList"> This is a list of our Tokens that we will verify.
    /// It also holds the order of the tokens to check for certain parameters and rules.</param>
    private static void CheckSyntax(List<string> formulaTokensList)
    {
        int parenCounter = 0; // Counter to check the balance of parenthesis in the equation.
        string previousToken = string.Empty; // Keeps track of previous token.

        if (!ValidFirstAndLast(formulaTokensList.First(), formulaTokensList.Last()))
        {
            throw new FormulaFormatException("First and/or Last tokens are not valid.");
        }

        foreach (string token in formulaTokensList)
        {
            // Check each token to make sure it is a valid token.
            if (!IsValidToken(token))
            {
                throw new FormulaFormatException($"Invalid Tokens. ({token})");
            }

            // Check to make sure that two operators aren't next to eachother.
            if (Regex.IsMatch(token, OperatorPattern) &&
                Regex.IsMatch(previousToken, OperatorPattern))
            {
                throw new FormulaFormatException("Operators do not have a valid token between them.");
            }

            // Checks the Operator Following Rule that after an opening parenthesis or operator,
            // the following token is a number, variable, or opening parenthesis.
            if (!OperParenFollowingRule(previousToken, token))
            {
                throw new FormulaFormatException($"Formula does not contain a number, variable, or open parenthesis after open parenthesis. See {token}");
            }

            // Check to see if an operator or a closing parenthesis follows a variable, digit, or closing parenthesis.
            else if (!ExtraFollowingRule(previousToken, token))
            {
                throw new FormulaFormatException(
                    "Formula must have an operator or closing parenthesis after a number, variable, or closing parenthesis.");
            }

            // Count up for opening parenthesis
            if (token.Equals("("))
            {
                parenCounter++;
            }

            // Count down for closing parenthesis.
            else if (token.Equals(")"))
            {
                parenCounter--;
                if (parenCounter <= -1)
                {
                    throw new FormulaFormatException("Formula has too many closing parenthesis.");
                }
            }

            previousToken = token;
        }

        if (parenCounter > 0)
        {
            throw new FormulaFormatException("Formula has too many Opening Parenthesis.");
        }
    }

    /// <summary> This takes in a list of Tokens and finds all variables and digits.
    /// Then, depending on their form, it normalizes them to be read and understood everywhere.
    /// Digits are simplified or pulled out of scientific notation, and variables are made all uppercase.
    /// </summary>
    /// <param name="formulaTokensList"> This list is an ordered list of all syntactically verified tokens.</param>
    private static void NormalizeTokens(ref List<string> formulaTokensList)
    {
        // Loop through the list and check for Variables and Numbers
        for (int i = 0; i < formulaTokensList.Count; i++)
        {
            // Checks for Variables and Capitalizes them
            if (IsVar(formulaTokensList[i]))
            {
                formulaTokensList[i] = formulaTokensList.ElementAt(i).ToUpper();
            }

            // Checks for Numbers and creates a canonical representation of them.
            if (IsDigit(formulaTokensList[i]))
            {
                // Hold the double value in a temporary variable so that we can convert it into a string..
                double temp;
                double.TryParse(formulaTokensList.ElementAt(i), out temp);
                formulaTokensList[i] = Convert.ToString(temp);
            }
        }
    }
}

/// <summary>
/// Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    /// <para>
    /// Constructs a FormulaFormatException containing the explanatory
    /// message.
    /// </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the
    /// exception occured.</param>
    public FormulaFormatException(string message)
    : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FormulaError"/> class.
    /// <para>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error
    /// occurred.</param>
    public FormulaError(string message)
    {
        this.Reason = message;
    }

    /// <summary>
    /// Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
/// Any method meeting this type signature can be used for
/// looking up the value of a variable. In general the expected behavior is that
/// the Lookup method will "know" about all variables in a formula
/// and return their appropriate value.
/// </summary>
/// <exception cref="ArgumentException">
/// If a variable name is provided that is not recognized by the implementing
/// method,
/// then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
/// The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);
