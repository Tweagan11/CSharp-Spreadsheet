// <copyright file = "FormulaSyntaxTests.cs" company = "UofU-CS3500">
// Copyright (c) UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Teagan Smith </authors>
// <date> 08.30.24 </date>
// <summary>
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
//    This File contains our Formula Tests, checking Syntax as well
//    as specific edge cases. Tests for public methods as well, to
//    ensure that when speaking with other code it still functions as expected.
// </summary>

namespace CS3500.Formula;

using CS3500.Formula;

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid( )
    {
        _ = new Formula( string.Empty );  // note: it is arguable that you should replace "" with string.

                                          // Empty for readability and clarity of intent (e.g., not a cut-and-paste error or a "I forgot to put something there" error).
    }

    // --- Tests for Valid Token Rule ---

    /// <summary>
    ///   <para>
    ///     Testing valid number tokens to make sure they are recognized in a formula, for this one we will use a single digit.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenDigit_Valid()
    {
        _ = new Formula("2");
    }

    /// <summary>
    ///   <para>
    ///     Testing valid number tokens to make sure they are recognized in a formula, for this one we will use a decimal.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenDecimalLone_Valid()
    {
        _ = new Formula("1.5");
    }

    /// <summary>
    ///   <para>
    ///     Testing valid number tokens to make sure they are recognized in a formula, for this one we will use a decimal.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenDecimalFormula_Valid()
    {
        _ = new Formula("1.5 + 3.5");
    }

    /// <summary>
    ///   <para>
    ///     This checks to make sure all forms of Scientific notation works within the formula.
    ///   </para>
    ///   <remarks>
    ///     Scientific notation is one digit followed by an optional decimal, then an uppercase or lowercase e, then another number to indicate the exponent.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationLowerCaseSingle_Valid()
    {
        _ = new Formula("2e6 + 1");
    }

    /// <summary>
    ///   <para>
    ///     This checks to make sure all forms of Scientific notation works within the formula.
    ///   </para>
    ///   <remarks>
    ///     Scientific notation is one digit followed by an optional decimal, then an uppercase or lowercase e, then another number to indicate the exponent.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationUpperCaseSingle_Valid()
    {
        _ = new Formula("2E6 + 1");
    }

    /// <summary>
    ///   <para>
    ///     This checks to make sure all forms of Scientific notation works within the formula.
    ///   </para>
    ///   <remarks>
    ///     Scientific notation is one digit followed by an optional decimal, then an uppercase or lowercase e, then another number to indicate the exponent.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationDecimal_Valid()
    {
        _ = new Formula("2.5E6 + 1");
    }

    /// <summary>
    ///   <para>
    ///     This checks to make sure all forms of Scientific notation works within the formula.
    ///   </para>
    ///   <remarks>
    ///     Scientific notation is one digit followed by an optional decimal, then an uppercase or lowercase e, then another number to indicate the exponent.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestScientificNotationDecimalExponent_Invalid()
    {
        _ = new Formula("2E6.5 + 1");
    }

    /// <summary>
    ///   <para>
    ///     This checks to make sure all forms of Scientific notation works within the formula.
    ///   </para>
    ///   <remarks>
    ///     Scientific notation is one digit followed by an optional decimal, then an uppercase or lowercase e, then another number to indicate the exponent.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestScientificNotationNegative_Valid()
    {
        _ = new Formula("2E-6 + 1");
    }

    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well-formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenVariable_Valid()
    {
        _ = new Formula( "A1+1" );
    }

    /// <summary>
    ///   <para>
    ///     Check that with a simple function, the formula recognizes that the first token is a closing parenthesis, which is invalid.
    ///     We do that by switching the order of parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosedParenthesis_Invalid()
    {
        _ = new Formula(")1+1(");
    }

    // ------------ Tests for Variables ---------------

    /// <summary>
    ///   <para>
    ///     Considers a simple formula and checks to make sure that a valid variable works
    ///     in the considered formula class as a token.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///         <item>A valid variable must consist of one or more letters followed by one or more digits.
    ///         Letters can be lowercase or uppercase. For example, a1, AAab1, and abc123 are all valid variables.
    ///         a, 1, 1a, and a1a are not.
    ///         </item>
    ///     </list>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenVariableUpperCase_Valid()
    {
        _ = new Formula("1 + A1");
    }

        /// <summary>
        ///     <para>
        ///         Tests Variable Tokens with lowercase letters and a digit.
        ///     </para>
        ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
        ///   <remarks>
        ///     This test is expected to succeed.
        ///   </remarks>
        /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenVariableLowerCase_Valid()
    {
        _ = new Formula("1 + a1");
    }

    /// <summary>
    ///     <para>
    ///         Tests Variable Tokens with lowercase letters and a digit.
    ///     </para>
    ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestTokenVariableLowerCaseWithDigitReversed_Valid()
    {
        _ = new Formula("a1 + 1");
    }

    /// <summary>
    ///     <para>
    ///         Tests Variable Tokens with  multiple lowercase letters and multiple digits.
    ///     </para>
    ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultipleVariableMultiDigit_Valid()
    {
        _ = new Formula("abc123 + 1");
    }

    /// <summary>
    ///     <para>
    ///         Tests Variable Tokens with  multiple lowercase letters and multiple digits.
    ///     </para>
    ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultipleVariableMultiDigitReversed_Valid()
    {
        _ = new Formula("1 + abc123");
    }

    /// <summary>
    ///     <para>
    ///         Tests Variable Tokens with  multiple lowercase and uppercase letters and one digit.
    ///     </para>
    ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultipleVariableOneDigit_Valid()
    {
        _ = new Formula("AAab1 + 1");
    }

    /// <summary>
    ///     <para>
    ///         Tests Variable Tokens with  multiple lowercase and uppercase letters and one digit.
    ///     </para>
    ///   <seealso cref="FormulaConstructor_TestTokenVariableUpperCase_Valid()">Upper Case Test for Description of Variables</seealso>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestMultipleVariableOneDigitReversed_Valid()
    {
        _ = new Formula("1 + AAab1");
    }

    /// <summary>
    ///   <para>
    ///     This test checks for invalid variables that violate the rules of a valid variable.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///         <item>A valid variable must consist of one or more letters followed by one or more digits.
    ///         Letters can be lowercase or uppercase. For example, a1, AAab1, and abc123 are all valid variables.
    ///         a, 1, 1a, and a1a are not.
    ///         </item>
    ///     </list>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestTokenVariableExtraLetter_Invalid()
    {
        _ = new Formula("A1a + 1");
    }

    /// <summary>
    ///   <para>
    ///     This test checks for invalid variables that violate the rules of a valid variable, this rule being a single letter with no digit following.
    ///   </para>
    ///   <see cref="FormulaConstructor_TestTokenVariableLoneLetter_Invalid">See Valid Variable Rules</see>.
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestTokenVariableLoneLetter_Invalid()
    {
        _ = new Formula("1+a");
    }

    /// <summary>
    ///   <para>
    ///     This test checks for invalid variables that violate the rules of a valid variable, this rule being a single letter with no digit following.
    ///   </para>
    ///   <see cref="FormulaConstructor_TestTokenVariableSpacedOut_Invalid">See Valid Variable Rules</see>.
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestTokenVariableSpacedOut_Invalid()
    {
        _ = new Formula("x 23 + 1");
    }

    // ------------------------- Tests for Closing Parenthesis ----------------

    /// <summary>
    ///   <para>
    ///     Considering a formula that works, when a closing parenthesis is on the end when it shouldn't,
    ///     this will fail the Closing Parentheses Rule.
    ///   </para>
    ///   <para>
    ///   This is closely related to the balanced parenthesis rule, but ensures that the Closing Parentheses are accounted for.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ClosingParenthesisRule_Invalid()
    {
        _ = new Formula("1+1)");
    }

    /// <summary>
    ///   <para>
    ///     This tests to make sure that the formula accounts for all parenthesis, opening and closing, and that they are balanced. (One on each side of the equation).
    ///   </para>
    ///   <para>
    ///   This is closely related to the Closing parenthesis rule, but ensures that the balanced Parentheses are accounted for.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_BalancedParenthesisRule_Valid()
    {
        _ = new Formula("1 + (1 + (1 +(1 + (1 + 1))))");
    }

    /// <summary>
    ///   <para>
    ///     This tests to make sure that the formula accounts for all parenthesis, opening and closing, and that they are balanced. (One on each side of the equation).
    ///   </para>
    ///   <para>
    ///   This is closely related to the Closing parenthesis rule, but ensures that the balanced Parentheses are accounted for.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_BalancedParenthesisRule_Invalid()
    {
        _ = new Formula("((2) + 1");
    }

    /// <summary>
    ///   <para>
    ///     This tests to make sure that the formula accounts for all parenthesis, opening and closing, and that they are balanced. (One on each side of the equation).
    ///   </para>
    ///   <para>
    ///   This is closely related to the Closing parenthesis rule, but ensures that the balanced Parentheses are accounted for.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ClosedParenthesisInvalidBalancedParenthesis_Invalid()
    {
        _ = new Formula("(2+2) + ) + 3+3 + ( + (1 + 1)");
    }

    // ----------------- Tests for Valid Operators --------------------

    /// <summary>
    ///   <para>
    ///     Considering all possible operators, this checks to make sure it is syntactically correct to use these as operators.
    ///     This test checks the addition operator (+).
    ///   </para>
    ///   <para>
    ///   This does not check for a logical formula, but instead a syntax error.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenOperatorAddition_Valid()
    {
        _ = new Formula("1+1");
    }

    /// <summary>
    ///   <para>
    ///     Considering all possible operators, this checks to make sure it is syntactically correct to use these as operators.
    ///     This test checks the Multiplication Operator(*).
    ///   </para>
    ///   <para>
    ///   This does not check for a logical formula, but instead a syntax error.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenOperatorMultiplication_Valid()
    {
        _ = new Formula("1*1");
    }

    /// <summary>
    ///   <para>
    ///     Considering all possible operators, this checks to make sure it is syntactically correct to use these as operators.
    ///     This test checks the division operator (/).
    ///   </para>
    ///   <para>
    ///   This does not check for a logical formula, but instead a syntax error.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenOperatorDivision_Valid()
    {
        _ = new Formula("1/1");
    }

    /// <summary>
    ///   <para>
    ///     Considering all possible operators, this checks to make sure it is syntactically correct to use these as operators.
    ///     This test checks the Subtraction Operator (-).
    ///   </para>
    ///   <para>
    ///   This does not check for a logical formula, but instead a syntax error.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenOperatorSubtraction_Valid()
    {
        _ = new Formula("1-1");
    }

    /// <summary>
    ///   <para>
    ///     Considering all possible operators, this checks to make sure it is syntactically correct to use these as operators.
    ///     This test checks all operators in one equation.
    ///   </para>
    ///   <para>
    ///   This does not check for a logical formula, but instead a syntax error.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenOperatorCombined_Valid()
    {
        _ = new Formula("1+1-1*1/1");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <para>
    ///   We cannot check every known invalid operator,
    ///   but testing common ones may increase bug detection in future development.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenSlash_Invalid()
    {
        _ = new Formula("5\\5");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations. <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula</see>.
    ///   </para>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenModulo_Invalid()
    {
        _ = new Formula("5%5");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenEqualSign_Invalid()
    {
        _ = new Formula("5=5");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula</see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenSquareBracketOpen_Invalid()
    {
        _ = new Formula("[");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenSquareBracketClosed_Invalid()
    {
        _ = new Formula("]");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenSquareBrackets_Invalid()
    {
        _ = new Formula("[test]");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenCurlyBracketClosed_Invalid()
    {
        _ = new Formula("}");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenCurlyBracketOpen_Invalid()
    {
        _ = new Formula("{");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenAtSign_Invalid()
    {
        _ = new Formula("5@5");
    }

    /// <summary>
    ///   <para>
    ///     These series of tests account for common operators that may be used in a mathematical sense,
    ///     but is invalid according to our regulations.
    ///   </para>
    ///   <see cref="FormulaConstructor_InvalidTokenSlash_Invalid">See this Formula
    ///   </see>.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenCaret_Invalid()
    {
        _ = new Formula("2^1");
    }

    // --- Tests for  Last Token Rule ---

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <para>
    ///   </para>
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_LastTokenRuleOperator_Invalid()
    {
        _ = new Formula("1 + 1 +");
    }

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_LastTokenRuleOpeningParenthesis_Invalid()
    {
        _ = new Formula("1 + 1 + 1(");
    }

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to fail.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_LastTokenRuleClosingParenthesis_Invalid()
    {
        _ = new Formula("1 + 1 + 1)");
    }

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleDigit_Valid()
    {
        _ = new Formula("5 + 5");
    }

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleVariable_Valid()
    {
        _ = new Formula("5 + A1");
    }

    /// <summary>
    ///   <para>
    ///     This rule checks to make sure the last token is a number, variable, or closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleClosingParenthesis_Valid()
    {
        _ = new Formula("(5 + A1)");
    }

    // -------------- Tests for Parenthesis/Operator Following Rule

    /// <summary>
    ///   <para>
    ///     This Following rule is Any token that immediately follows an opening parenthesis or an operator
    ///     must be either a number, a variable, or an opening parenthesis.
    ///   </para>
    ///   <para>
    ///     We have had previous tests that have satisfied many of these conditions,
    ///     but to test edge cases we will run through similar but slightly different tests.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleParenthesis_Valid()
    {
        _ = new Formula("((1*1)+1)");
    }

    /// <summary>
    ///   <para>
    ///     This Following rule is Any token that immediately follows an opening parenthesis or an operator
    ///     must be either a number, a variable, or an opening parenthesis. This rule tests Variables after an open parenthesis.
    ///   </para>
    ///   <para>
    ///     We have had previous tests that have satisfied many of these conditions,
    ///     but to test edge cases we will run through similar but slightly different tests.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleVariable_Valid()
    {
        _ = new Formula("(A1)");
    }

    /// <summary>
    ///   <para>
    ///     This Following rule is Any token that immediately follows an opening parenthesis or an operator
    ///     must be either a number, a variable, or an opening parenthesis. This rule tests Variables after an open parenthesis, but does not close the parenthesis to check edge cases.
    ///   </para>
    ///   <para>
    ///     We have had previous tests that have satisfied many of these conditions,
    ///     but to test edge cases we will run through similar but slightly different tests.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException (typeof(FormulaFormatException))]
    public void FormulaConstructor_ParenthesisFollowingRuleInvalidBalancedParenthesis_Invalid()
    {
        _ = new Formula("(A1");
    }

    /// <summary>
    ///   <para>
    ///     This Following rule is Any token that immediately follows an opening parenthesis or an operator
    ///     must be either a number, a variable, or an opening parenthesis. This rule tests an operator after another operator.
    ///   </para>
    ///   <para>
    ///     We have had previous tests that have satisfied many of these conditions,
    ///     but to test edge cases we will run through similar but slightly different tests.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException (typeof(FormulaFormatException))]
    public void FormulaConstructor_OperatorFollowingRuleOperator_Invalid()
    {
        _ = new Formula("5+/5");
    }

    /// <summary>
    ///   <para>
    ///     This Following rule is Any token that immediately follows an opening parenthesis or an operator
    ///     must be either a number, a variable, or an opening parenthesis. This rule tests an operator after a parenthesis.
    ///   </para>
    ///   <para>
    ///     We have had previous tests that have satisfied many of these conditions,
    ///     but to test edge cases we will run through similar but slightly different tests.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ParenthesisFollowingRuleOperator_Invalid()
    {
        _ = new Formula("5 + ( + 5)");
    }

    // --------- Tests for Extra Following Rule ------

    /// <summary>
    ///   <para>
    ///     The Extra Following rule is Any token that immediately follows a number,
    ///     a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ExtraFollowingRuleOperator_Valid()
    {
        _ = new Formula("(1*1) * 1");
    }

    /// <summary>
    ///   <para>
    ///     The Extra Following rule is Any token that immediately follows a number,
    ///     a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ExtraFollowingRuleClosingParenthesis_Valid()
    {
        _ = new Formula("(1 + (1*1))");
    }

    /// <summary>
    ///   <para>
    ///     The Extra Following rule is Any token that immediately follows a number,
    ///     a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an error.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingRuleOpeningParenthesis_Invalid()
    {
        _ = new Formula("(1*1)(1+1)");
    }

    /// <summary>
    ///   <para>
    ///     The Extra Following rule is Any token that immediately follows a number,
    ///     a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to throw an error.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingRuleDigit_Invalid()
    {
        _ = new Formula("(1+1)5");
    }

    /// <summary>
    /// Formulas the constructor get variables valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToString_Valid()
    {
        string testFormula = new Formula("X1+5").ToString();
        Assert.AreEqual("X1+5", testFormula);
    }

    /// <summary>
    /// Tests to make sure the Formula normalizes all variables and gets rid of whitespace.
    /// </summary>
    /// <remarks>
    /// This test asserts that they are equal, which they should be.
    /// </remarks>
    [TestMethod]
    public void FormulaConstructor_TestNormalizingVariables_Valid()
    {
        string testFormula = new Formula("x1 + y1 + z1").ToString();
        Assert.AreEqual("X1+Y1+Z1", testFormula);
    }

    /// <summary>
    /// Tests to make sure the Formula normalizes all variables and gets rid of whitespace.
    /// </summary>
    /// <remarks>
    /// This test asserts that they are equal, which they should be.
    /// </remarks>
    [TestMethod]
    public void ToString_ScientificNotation_Valid()
    {
        string test = new Formula("5e2 + 3e-2").ToString();
        Assert.AreEqual("500+0.03", test);
    }

    /// <summary>
    /// Tests to make sure the Formula normalizes all variables and gets rid of whitespace.
    /// </summary>
    /// <remarks>
    /// This test asserts that they are equal, which they should be.
    /// </remarks>
    [TestMethod]
    public void ToString_VariablesAndSciNotation_Valid()
    {
        string test = new Formula("e9 + 4E4 + (ab1 * 5e-1)").ToString();
        Assert.AreEqual("E9+40000+(AB1*0.5)", test);
    }

    /// <summary>
    /// Tests to make sure the Formula normalizes all variables and gets rid of whitespace.
    /// </summary>
    /// <remarks>
    /// This test asserts that they are equal, which they should be.
    /// </remarks>
    [TestMethod]
    public void ToString_Digits_Valid()
    {
        string test = new Formula("5.0 + 5.00 + 1.2500 + 409.0900").ToString();
        Assert.AreEqual("5+5+1.25+409.09", test);
    }

    /// <summary>
    /// Tests the GetVariables function to verify that it is correctly storing variables.
    /// </summary>
    /// <remarks>
    /// This test asserts that the Set containing variables should contain one instance of an Uppercase version of each variable.
    /// </remarks>
    [TestMethod]
    public void GetVariables_SameVariable_Valid()
    {
        int counter = 0;
        ISet<string> test = new Formula("x1+X1").GetVariables();
        foreach (string variable in test)
        {
            Assert.IsTrue(variable == "X1");
            counter++;
        }

        Assert.AreEqual(counter, 1);
    }

    /// <summary>
    /// Tests the GetVariables function to verify that it is correctly storing variables.
    /// </summary>
    /// <remarks>
    /// This test asserts that the Set containing variables should contain
    /// one instance of an Uppercase version of each variable, regardless of configurations.
    /// </remarks>
    [TestMethod]
    public void GetVariables_SameVariableMultiple_Valid()
    {
        int counter = 0;
        ISet<string> test = new Formula("xy1+XY1 * xY1 - Xy1").GetVariables();
        foreach (string variable in test)
        {
            Assert.IsTrue(variable == "XY1");
            counter++;
        }

        // Check that it only enumerated one variable.
        Assert.AreEqual(counter, 1);
    }

    /// <summary>
    /// Tests the GetVariables function to verify that it is correctly storing variables.
    /// </summary>
    /// <remarks>
    /// This test asserts that when no variables are stored, then the HashSet will be empty.
    /// </remarks>
    [TestMethod]
    public void GetVariables_Null_Valid()
    {
        ISet<string> test = new Formula("1+1").GetVariables();
        Assert.AreEqual(test.Count, 0);
    }
}