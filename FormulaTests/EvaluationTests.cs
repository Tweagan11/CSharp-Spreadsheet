// <copyright file = "FormulaSyntaxTests.cs" company = "UofU-CS3500">
// Copyright (c) UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Teagan Smith </authors>
// <date> 09.18.24 </date>
// <summary>
// Copyright: CS 3500 and Teagan Smith - This work may not be
//            copied for use in Academic Coursework.
//
// I, Teagan, certify that I wrote this code from scratch and
// did not copy it in part or whole from another source.  All
// references used in the completion of the assignments are cited
// in my README file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3500.Formula;

/// <summary>
///   <para>
///     The Following Test Class will help us understand and implement our Evaluation aspect
///     of our Formula Class. This evaluate will account for digits, variables, and operation order
///     to preserve formulas and functionality.
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class EvaluationTests
{
    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers into the formula and adds them together. 
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_TwoIntAdd_Valid()
    {
        Formula f = new("1+1");
        double result = double.Parse(f.Evaluate((s) => 0).ToString());
        Assert.AreEqual(result, 2.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in multiple integers into the formula and adds them together. This tests our operator stack and makes sure that it
    ///    correctly evaluates when given multiple addition or subtraction operators.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_IntAddMultiple_Valid()
    {
        Formula f = new("1+1+1+1");
        object result = f.Evaluate((s) => 0);
        Assert.AreEqual(result, 4.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers into the formula and subtracts them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_TwoIntSubtract_Valid()
    {
        Formula f = new("1-1");
        object result = f.Evaluate((s) => 0);
        Assert.AreEqual((double)result, 0.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers into the formula and divides them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_TwoIntDivide_Valid()
    {
        Formula f = new("6/3");
        double result = double.Parse(f.Evaluate((s) => 0).ToString());
        Assert.AreEqual(result, 2.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers into the formula and divides them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_OperationOrder_Valid()
    {
        Formula f = new("1 + 2 - 3 * 4 / 6");
        double result = double.Parse(f.Evaluate((s) => 0).ToString());
        Assert.AreEqual(result, 1.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers into the formula and multiplies them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_TwoIntMultiply_Valid()
    {
        Formula f = new("2*3");
        double result = double.Parse(f.Evaluate((s) => 0).ToString());
        Assert.AreEqual(result, 6.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers, one negative into the formula and multiplies them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_NegMultiply_Valid()
    {
        Formula f = new("2*(0-3)");
        object result = f.Evaluate((s) => 0);
        Assert.AreEqual(result, -6.0);
    }

    /// <summary>
    ///   <para>
    ///    This test takes in 2 integers, one negative variable into the formula and multiplies them.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_NegativeMultiply_Valid()
    {
        Formula f = new("2*A1");
        object result = f.Evaluate((s) => -3);
        Assert.AreEqual(result, -6.0);
    }

    /// <summary>
    ///   <para>
    ///    This test tracks multiple subtractions and should produce a negative number.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return 2.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_NegativeNumberSubtraction_Valid()
    {
        Formula f = new("3-3-3-3");
        object result = f.Evaluate((s) => 0);
        Assert.AreEqual(result, -6.0);
    }

    /// <summary>
    ///   <para>
    ///     This test makes the formula divide by 0, which should return a FormulaError Object.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return a Formula Object Error.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DivideByZero_Invalid()
    {
        Formula f = new("1/0");
        object result = f.Evaluate((s) => 0);
        Assert.IsTrue(result is FormulaError);
    }

    /// <summary>
    ///   <para>
    ///     This test makes the formula divide by 0 in a set of parenthesis, which should return a FormulaError Object.
    ///     This tests our algorithm and that it correctly throws an error when the algorithm in parenthesis is processed.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return a Formula Object Error.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_ParenDivideByZero_Invalid()
    {
        Formula f = new("(3/(1-1))");
        object result = f.Evaluate((s) => 0);
        Assert.IsTrue(result is FormulaError);
    }

    /// <summary>
    ///   <para>
    ///     This test makes the formula divide by 0 but using a variable, which should return a FormulaError Object.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return a Formula Object Error.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_VarDivideByZero_Invalid()
    {
        Formula f = new("1/A1");
        object result = f.Evaluate((s) => 0);
        Assert.IsTrue(result is FormulaError);
    }

    /// <summary>
    ///   <para>
    ///     This test takes in one variable and evaluates the formula, hopefully giving us our expected value of 5.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_SingleVariableAddition_Valid()
    {
        Formula f = new("A1-5");
        var result = f.Evaluate((s) => s == "A1" ? 10 : 0);
        Assert.AreEqual(result, 5.0);
    }

    /// <summary>
    ///   <para>
    ///     This Test takes in 2 variables and uses a Lambda function to "lookup" their values.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DoubleVariableLambda_Valid()
    {
        Formula f = new("A1*A2");
        object result = f.Evaluate((s) =>
        {
            if (s == "A1") { return 3.0; }
            else if (s == "A2") { return 2.0; }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        });
        Assert.AreEqual((double)result, 6.0);
    }

    /// <summary>
    ///   <para>
    ///   This Test takes in a function instead of a lambda to check validity of this delegate.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DoubleVariableMethod_Valid()
    {

        double MyVariables(string name)
        {
            if (name == "A1") return 2;
            else if (name == "B1") return 3;
            else
            {
                throw new ArgumentException();
            }
        }

        Formula f = new("A1 + B1");
        var result = f.Evaluate(MyVariables);
        Assert.AreEqual(result, 5.0);
    }

    /// <summary>
    ///   <para>
    ///     This test uses a complex formula with multiple steps to check the validity
    ///     of our algorithm to solve our equation. This test uses no variables.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DigitOnlyComplex_Valid()
    {
        Formula f = new("((15 * 8 - 42) / 3 + 17) * (25 / 5 - 2) * 6 - (81 / 9 + 7) * 4");
        var result = f.Evaluate((s) => 0);
        Assert.AreEqual(result, 710.0);
    }

    /// <summary>
    ///   <para>
    ///     This test is a complex formula with many steps as well as variables.
    ///     Currently their values are 16 and 8, which the lambda should take care of.
    ///   </para>
    ///   <remarks>
    ///     This test should provide an accuracy of 1e-6.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DigitAndVariableComplex_Valid()
    {
        // Using this to compare floating points later.
        double epsilon = 1e-6;
        double expectedValue = 58.4;

        Formula f = new("((A1 * 3 - A2) / 2 + A2) * (A1 / A2 + 1) - (A2 * A1 / 5)");
        object result = f.Evaluate((s) =>
        {
            if (s.Equals("A1"))
            {
                return 16.0;
            }
            else if (s.Equals("A2"))
            {
                return 8.0;
            }

            throw new ArgumentOutOfRangeException();
        });

        // Use Absolute Value and Subtraction to find if it is smaller than the given threshold.
        Assert.IsTrue(Math.Abs((double)result - expectedValue) < epsilon);
    }

    /// <summary>
    ///   <para>
    ///     This test is a complex formula with many steps as well as variables.
    ///     Currently their values are 18 and 7, which the lambda should take care of.
    ///     However, this formula has a variable with an unknown value. This should throw an exception.
    ///   </para>
    ///   <remarks>
    ///     This test should throw an Exception.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_VariableOutOfRange_Valid()
    {
        // Using this to compare floating points later.
        double epsilon = 1e-6;
        double expectedValue = 58.4;

        Formula f = new("((A1 * 3 - B2) / 2 + A2) * (A1 / A2 + 1) - (A2 * A1 / 5)");
        object result = f.Evaluate((s) =>
        {
            if (s.Equals("A1"))
            {
                return 16.0;
            }
            else if (s.Equals("A2"))
            {
                return 8.0;
            }

            throw new ArgumentException();
        });

        // Use Absolute Value and Subtraction to find if it is smaller than the given threshold.
        Assert.IsTrue(result is FormulaError);
    }

    /// <summary>
    ///   <para>
    ///     This test is similar to above, but this time using the actual values of the variables instead of the variables themselves.
    ///   </para>
    ///   <remarks>
    ///     This test should provide an accuracy of 1e-6.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DigitComplex2_Valid()
    {
        // Using this to compare floating points later.
        double epsilon = 1e-6;
        double expectedValue = 58.4;

        Formula f = new("((16 * 3 - 8) / 2 + 8) * (16 / 8 + 1) - (8 * 16 / 5)");
        object result = f.Evaluate((s) => 0);

        // Use Absolute Value and Subtraction to find if it is smaller than the given threshold.
        Assert.IsTrue(Math.Abs((double)result - expectedValue) < epsilon);
    }

    /// <summary>
    ///   <para>
    ///     This Test tests the functionality of decimal points when subtracting,
    ///     ensuring that equivalence is maintained.
    ///   </para>
    ///   <remarks>
    ///     This test should provide an accuracy of 1e-6.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("DigitEvaluation")]
    public void FormulaEvaluator_DecimalPointFormula_Valid()
    {
        // Using this to compare floating points later.
        double epsilon = 1e-6;
        double expectedValue = 0.2;

        Formula f = new("0.3 - .1");
        var result = f.Evaluate((s) => 0);
        double doubleResult = Convert.ToDouble(result);

        // Use Absolute Value and Subtraction to find if it is smaller than the given threshold.
        Assert.IsTrue(Math.Abs(doubleResult - expectedValue) < epsilon);
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_SimilarFormula_Valid()
    {
        Formula f1 = new("1+1");
        Formula f2 = new("1+1");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same. We will use 2
    ///     canonically different formulas and check if it is false.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_CanonicalDifferentFormula_Valid()
    {
        Formula f1 = new("1+1");
        Formula f2 = new("1-1");
        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same. We will use 2
    ///     variables, that canonically should match but inputs differ.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_CanonicalSameFormula_Valid()
    {
        Formula f1 = new("A1+B1");
        Formula f2 = new("a1+b1");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same. We will use scientific
    ///     notation to make sure it recognizes scientific notation both ways.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_SciNotation_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        Formula f2 = new("5e-2 * 5e2 - 5e1");
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same.
    ///     We will use a null object / empty variable and make sure it comes back false.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_NullObj_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        object f2 = null;
        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the Equals override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same.
    ///     We will use a different kind of object, such as a string to make sure it returns false.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaEquals")]
    public void FormulaEquals_DiffObj_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        string f2 = new(".05 * 500 - 50");
        Assert.IsFalse(f1.Equals(f2));
    }

    /// <summary>
    ///   <para>
    ///     This looks at the == override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same. We will use 2 similar Formulas
    ///     to check functionality
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("Formula==")]
    public void FormulaEqualsOperator_SimilarFormula_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        Formula f2 = new(".05 * 500 - 50");
        Assert.IsTrue(f1==f2);
    }

    /// <summary>
    ///   <para>
    ///     This looks at the == override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are the same. We will use 2 similar Formulas
    ///     to check functionality, but different in input.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("Formula==")]
    public void FormulaEqualsOperator_CanonEqual_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        Formula f2 = new("5e-2 * 5e2 - 5e1");
        Assert.IsTrue(f1 == f2);
    }

    /// <summary>
    ///   <para>
    ///     This looks at the != override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are different. We will use 2 similar Formulas
    ///     to check functionality, but different in input.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to succeed.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("Formula!=")]
    public void FormulaNotEquals_CanonEqual_Valid()
    {
        Formula f1 = new(".05 * 500 - 50");
        Formula f2 = new("5e-2 * 5e2 - 5e1");
        Assert.IsFalse(f1 != f2);
    }

    /// <summary>
    ///   <para>
    ///     This looks at the != override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are different. We will use 2 similar Formulas
    ///     to check functionality, but different in input.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return True.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("Formula!=")]
    public void FormulaNotEquals_DiffWithVariables_Valid()
    {
        Formula f1 = new("A1 + B1");
        Formula f2 = new("A1 - B1");
        Assert.IsTrue(f1 != f2);
    }

    /// <summary>
    ///   <para>
    ///     This looks at the != override in the Formula Class. When given 2 formula objects,
    ///     it compares the canonical strings and returns true if they are different.
    ///     We will use a formula that looks similar to a scientific notation but is different due to tokens.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return true.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("Formula!=")]
    public void FormulaNotEquals_SciNotationAgainstVaraibles_Valid()
    {
        Formula f1 = new("5e-9 + 6e9");
        Formula f2 = new("5-e9+6*e9");
        Assert.IsTrue(f1 != f2);
    }

    /// <summary>
    ///   <para>
    ///     The GetHashCode function of the Formula class returns a hash code of the
    ///     canonical representation of the formula. So, if f1.Equals(f2), then
    ///     f1.GetHashCode() == f2.GetHashCode() should be true.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return true.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaGetHashCode")]
    public void FormulaGetHashCode_SimilarFormulas_Valid()
    {
        Formula f1 = new("1+1");
        Formula f2 = new("1+1");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
    }

    /// <summary>
    ///   <para>
    ///     The GetHashCode function of the Formula class returns a hash code of the
    ///     canonical representation of the formula. So, if f1.Equals(f2), then
    ///     f1.GetHashCode() == f2.GetHashCode() should be true. We will use
    ///     2 different formulas that are canonically similar.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return true.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaGetHashCode")]
    public void FormulaGetHashCode_SimilarCanonicalFormulas_Valid()
    {
        Formula f1 = new("10+100+a1");
        Formula f2 = new("1e1+1e2+A1");
        Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        Assert.IsTrue(f1.Equals(f2));
    }

    /// <summary>
    /// This tests our performance, and when given many numbers, can our program still handle it.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    [TestCategory("PerformanceEvaluation")]
    public void FormulaEvaluator_LargeFormula_Valid()
    {
        // Wanted a big string, found the Enumerable.Repeat through ChatGPT.
        string f1 = string.Join("+", Enumerable.Repeat("1", 10000));
        Formula f = new(f1);
        object result = f.Evaluate((s) => 0);
        Assert.AreEqual(result, 10000.0);
    }

    /// <summary>
    ///   <para>
    ///     The GetHashCode function of the Formula class returns a hash code of the
    ///     canonical representation of the formula. So, if f1.Equals(f2), then
    ///     f1.GetHashCode() == f2.GetHashCode() should be true. We will use
    ///     2 different formulas that are canonically similar.
    ///   </para>
    ///   <remarks>
    ///     This test is expected to return true.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    [TestCategory("FormulaGetHashCode")]
    public void FormulaGetHashCode_StringHashAgainstFormula_Valid()
    {
        Formula f1 = new("10+100+A1");
        string hashtest = "10+100+A1";
        Assert.IsTrue(f1.GetHashCode() == hashtest.GetHashCode());
    }

    /// <summary>
    /// This given exception is a placeholder for our Lookup delegate. This resembles our later lookup function that will throw an exception anytime a
    /// value of a variable is not found or doesn't exist.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private Exception ArgumentOutOfRangeException()
    {
        throw new NotImplementedException();
    }
}
