﻿// <copyright file="GradingTestsPS6.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

namespace CS3500.GradingTestsPS6;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using CS3500.Spreadsheet;
using CS3500.Formula;
using System.Text.Json;

/// <summary>
///   Authors:   Joe Zachary
///              Daniel Kopta
///              Jim de St. Germain
///   Date:      Fall 2024
///   Course:    CS 3500, University of Utah, School of Computing
///   Copyright: CS 3500 - This work may not be copied for use
///                      in Academic Coursework.  See below.
///
///   File Contents:
///
///     This file contains proprietary grading tests for CS 3500.  These tests cases
///     are for individual student use only and MAY NOT BE SHARED.  Do not back them up
///     nor place them in any online repository.  Improper use of these test cases
///     can result in removal from the course and an academic misconduct sanction.
///
///     These tests are for your private use, this semester, only to improve the quality of the
///     rest of your assignments.
///
///   Test Information
///      This file contains various classes for testing the full Spreadsheet
///      and is used in grading student success.
///   <para>
///     There are multiple classes in this file containing similar tests.
///     This first class (SimpleValidSpreadSheetExmaples) tests sheet
///     content assignment that should be valid.
///   </para>
/// </summary>
[TestClass]
public class SimpleValidSpreadSheetExamples
{
    /// <summary>
    ///   Test that we can create a spreadsheet and can add and retrieve a value.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "1" )]
    public void SetGetCellContents_SingleStringAdded_HasOneString( )
    {
        Spreadsheet s = new();
        s.SetContentsOfCell( "A1", "x" );
        Assert.AreEqual( s.GetNamesOfAllNonemptyCells().Count, 1 );
        Assert.AreEqual( "x", s.GetCellContents( "A1" ) );
    }

    /// <summary>
    ///   An empty sheet should not contain values.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "2" )]
    public void Constructor_Default_ShouldBeEmpty( )
    {
        Spreadsheet ss = new();

        var results = new Dictionary<string, object>
        {
            { "A1", string.Empty },
            { "B10", string.Empty },
            { "CC101", string.Empty },
        };

        ss.CompareToExpectedValues( results );
        ss.CompareCounts( results );
    }

    /// <summary>
    ///    Test that you can add one string to a spreadsheet.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> If used alone, check the count as well as the values. </param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "3" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_AddStringToCell_CellContainsString( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "B1", "hello" },
            { "B2", string.Empty },
        };

        ss.SetContentsOfCell( "B1", "hello" );
        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that you can add one string to a spreadsheet.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "4" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_AddNumberToCell_CellContainsNumber( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "C1", 17.5 },
            { "C2", string.Empty },
        };

        ss.SetContentsOfCell( "C1", "17.5" );
        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that you can add a simple formula ("=5") to a spreadsheet.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "5" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_AddSimpleFormulaToCell_CellEvaluatesCorrectly( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 5.0 },
            { "A2", string.Empty },
        };

        ss.SetContentsOfCell( "A1", "=5" );
        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///   Test that you can add a formula that depends on one other cell.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "6" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_FormulaBasedOnSingleOtherCell_EvaluatesCorrectly( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "C1", 8.3 },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "C1", "=A1+4.2" );
        ss.CompareToExpectedValues( results );

        if (verifyCounts)
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that you can add one formula to a spreadsheet that depends on two other cells.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "7" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_FormulaDependsOnTwoCells_AllCellsComputeCorrectly( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "B1", 5.2 },
            { "C1", 9.3 },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "B1", "5.2" );
        ss.SetContentsOfCell( "C1", "=A1+B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that formulas work for addition, subtraction, multiplication, and division.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "8" )]
    [DataRow( null, true )]
    public void SetContentsFormulas_AddSubtractMultiplyDivide_AllWorkAsExpected( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.4 },
            { "B1", 2.2 },
            { "C1", 6.6 },
            { "D1", 2.2 },
            { "E1", 4.4 * 2.2 },
            { "F1", 2.0 },
        };

        ss.SetContentsOfCell( "A1", "4.4" );
        ss.SetContentsOfCell( "B1", "2.2" );
        ss.SetContentsOfCell( "C1", "= A1 + B1" );
        ss.SetContentsOfCell( "D1", "= A1 - B1" );
        ss.SetContentsOfCell( "E1", "= A1 * B1" );
        ss.SetContentsOfCell( "F1", "= A1 / B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "9" )]
    public void IncreaseGradingWeight1( )
    {
        SetContentsFormulas_AddSubtractMultiplyDivide_AllWorkAsExpected( null, true );
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "10" )]
    public void IncreaseGradingWeight2( )
    {
        SetContentsFormulas_AddSubtractMultiplyDivide_AllWorkAsExpected( null, true );
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "11" )]
    public void IncreaseGradingWeight3( )
    {
        SetContentsFormulas_AddSubtractMultiplyDivide_AllWorkAsExpected( null, true );
    }

    /// <summary>
    ///    Test that division by an empty cell is an error.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "12" )]
    [DataRow( null, true )]
    public void DivisionByEmptyCell( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "B1", string.Empty },
            { "C1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "C1", "=A1/B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that you cannot add a number to a string.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "13" )]
    [DataRow( null, true )]
    public void AddNumberToString( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "B1", "hello" },
            { "C1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "B1", "hello" );
        ss.SetContentsOfCell( "C1", "=A1+B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that a formula computed from a cell with a formula error value
    ///    is also a formula error.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "14" )]
    [DataRow( null, true )]
    public void FormulaFromFormulaError( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", "hello" },
            { "B1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
            { "C1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
        };

        ss.SetContentsOfCell( "A1", "hello" );
        ss.SetContentsOfCell( "B1", "=A1" );
        ss.SetContentsOfCell( "C1", "=B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Test that direct division by 0 in a formula is caught.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "15" )]
    [DataRow( null, true )]
    public void DivisionByZero1( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "B1", string.Empty },
            { "C1", new FormulaError( "Division by zero" ) },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "B1", string.Empty );
        ss.SetContentsOfCell( "C1", "=A1/0.0" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///    Check that division by a cell which contains zero is caught.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "16" )]
    [DataRow( null, true )]
    public void DivisionByZero2( Spreadsheet? ss, bool verifyCounts )
    {
        ss ??= new();

        var results = new Dictionary<string, object>
        {
            { "A1", 4.1 },
            { "B1", 0.0 },
            { "C1", new FormulaError( "Division by zero") },
        };

        ss.SetContentsOfCell( "A1", "4.1" );
        ss.SetContentsOfCell( "B1", "0.0" );
        ss.SetContentsOfCell( "C1", "= A1 / B1" );

        ss.CompareToExpectedValues( results );

        if ( verifyCounts )
        {
            ss.CompareCounts( results );
        }
    }

    /// <summary>
    ///   Repeats the simple tests all together.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "17" )]
    public void SpreadsheetOverall_CombineMultipleTests_AllCorrect( )
    {
        Spreadsheet ss = new();
        var results = new Dictionary<string, object>
        {
            { "A1", "hello" },
            { "B1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
            { "C1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
        };

        ss.SetContentsOfCell( "A1", "17.32" );
        ss.SetContentsOfCell( "B1", "This is a test" );
        ss.SetContentsOfCell( "C1", "=C2+C3" );

        SetContentsOfCell_AddStringToCell_CellContainsString( ss, false );
        SetContentsOfCell_AddNumberToCell_CellContainsNumber( ss, false );
        SetContentsOfCell_FormulaDependsOnTwoCells_AllCellsComputeCorrectly( ss, false );

        DivisionByZero1( ss, false );
        DivisionByZero2( ss, false );

        AddNumberToString( ss, false );
        FormulaFromFormulaError( ss, false );

        ss.CompareToExpectedValues ( results );
    }

    /// <summary>
    ///    Five cells related to each other.  Make sure original values are
    ///    correctly computed (Formula Errors), then change end cells, then check that the
    ///    new values are correct.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "18" )]
    [DataRow( null )]
    public void SetContentsOfCell_SimpleFormulas_ComputeCorrectResults( Spreadsheet? ss )
    {
        var expectedResults = new Dictionary<string, object>
        {
            { "A1", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
            { "A2", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
        };

        ss ??= new();

        ss.SetContentsOfCell( "A1", "= A2 + A3" );
        ss.SetContentsOfCell( "A2", "= B1 + B2" );

        ss.CompareToExpectedValues( expectedResults );
        ss.CompareCounts( expectedResults );

        ss.SetContentsOfCell( "A3", "5.0" );
        ss.SetContentsOfCell( "B1", "2.0" );
        ss.SetContentsOfCell( "B2", "3.0" );

        expectedResults[ "A1" ] = 10.0;
        expectedResults[ "A2" ] = 5.0;
        expectedResults[ "A3" ] = 5.0;
        expectedResults[ "B1" ] = 2.0;
        expectedResults[ "B2" ] = 3.0;

        ss.CompareToExpectedValues( expectedResults );
        ss.CompareCounts( expectedResults );

        ss.SetContentsOfCell( "B2", "4.0" );

        expectedResults[ "A1" ] = 11.0;
        expectedResults[ "A2" ] = 6.0;
        expectedResults[ "B2" ] = 4.0;

        ss.CompareToExpectedValues( expectedResults );
        ss.CompareCounts( expectedResults );
    }

    /// <summary>
    ///    Change the end cell of a three cell chain and check for
    ///    the correct computation of results.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "20" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_ThreeCellChainFormula_ComputesCorrectResults( Spreadsheet? ss, bool verifyCounts )
    {
        var expectedResults = new Dictionary<string, object>
        {
            { "A1", 12.0 },
            { "A2",  6.0 },
            { "A3",  6.0 },
        };

        ss ??= new();

        ss.SetContentsOfCell( "A1", "= A2 + A3" );
        ss.SetContentsOfCell( "A2", "= A3" );
        ss.SetContentsOfCell( "A3", "6.0" );

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }

        ss.SetContentsOfCell( "A3", "5.0" );
        expectedResults[ "A1" ] = 10.0;
        expectedResults[ "A2" ] = 5.0;
        expectedResults[ "A3" ] = 5.0;

        ss.CompareToExpectedValues( expectedResults );

        if (verifyCounts)
        {
            ss.CompareCounts( expectedResults );
        }
    }

    /// <summary>
    ///    Five cells chained together.  Make sure initial values are
    ///    computed correctly, then change end cells, then make sure
    ///    new values are computed correctly.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "21" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_FiveCellsWithValues_CorrectValuesComputed( Spreadsheet? ss, bool verifyCounts )
    {
        var expectedResults = new Dictionary<string, object>
        {
            { "A1", 18.0 },
            { "A2", 18.0 },
            { "A3",  9.0 },
            { "A4",  9.0 },
            { "A5",  9.0 },
        };

        ss ??= new();

        ss.SetContentsOfCell( "A1", "= A3 + A5" );
        ss.SetContentsOfCell( "A2", "= A5 + A4" );
        ss.SetContentsOfCell( "A3", "= A5" );
        ss.SetContentsOfCell( "A4", "= A5" );
        ss.SetContentsOfCell( "A5", "9.0" );

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }

        ss.SetContentsOfCell( "A5", "8.0" );
        expectedResults[ "A1" ] = 16.0;
        expectedResults[ "A2" ] = 16.0;
        expectedResults[ "A3" ] = 8.0;
        expectedResults[ "A4" ] = 8.0;
        expectedResults[ "A5" ] = 8.0;

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }
    }

    /// <summary>
    ///    Combine the other tests and make sure that they all work
    ///    in combination with each other.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "22" )]
    public void SpreadsheetOverall_CombineOtherTests_CorrectValuesComputed( )
    {
        var expectedResults = new Dictionary<string, object>
        {
            { "A1", 16.0 },
            { "A2", 16.0 },
            { "A3",  8.0 },
            { "A4",  8.0 },
            { "A5",  8.0 },
            { "B1",  2.0 },
            { "B2",  4.0 },
        };

        Spreadsheet ss = new();
        SetContentsOfCell_SimpleFormulas_ComputeCorrectResults( ss );
        SetContentsOfCell_ThreeCellChainFormula_ComputesCorrectResults( ss, false );
        SetContentsOfCell_FiveCellsWithValues_CorrectValuesComputed( ss, false );

        ss.CompareToExpectedValues( expectedResults );
        ss.CompareCounts( expectedResults );
    }

    /// <summary>
    ///   Increase the grading weight of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "23" )]
    public void IncreaseGradingWeight4( )
    {
        SpreadsheetOverall_CombineOtherTests_CorrectValuesComputed();
    }

    /// <summary>
    ///   Check that the base case (empty cell) index works.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "24" )]
    public void Indexer_EmptyCell_EmptyStringValue( )
    {
        Spreadsheet ss = new();

        Assert.AreEqual( ss[ "A1" ], string.Empty );
        Assert.AreEqual( ss[ "A1" ], ss.GetCellValue( "A1" ) );
    }

    /// <summary>
    ///   Check that a double value can be returned.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "25" )]
    public void Indexer_DoubleValue_Returns5( )
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell( "A1", "5" );
        Assert.AreEqual( (double) ss[ "A1" ], 5.0, .001 );
        Assert.AreEqual( (double) ss[ "A1" ], (double) ss.GetCellValue( "A1" ), .001 );
    }

    /// <summary>
    ///   Check that a string can be returned.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "26" )]
    public void Indexer_StringValue_ReturnsHelloWorld( )
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell( "A1", "hello world" );
        Assert.AreEqual( ss[ "A1" ], "hello world" );
        Assert.AreEqual( ss[ "A1" ], ss.GetCellValue( "A1" ) );
    }

    /// <summary>
    ///   Check that a formulas computed value can be returned,
    ///   both as a FormulaError and as a double.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "27" )]
    public void Indexer_GetFormulaValue_ReturnsErrorThenValue( )
    {
        Spreadsheet ss = new();

        ss.SetContentsOfCell( "A1", "=A2" );
        Assert.IsInstanceOfType<FormulaError>( ss[ "A1" ] );

        ss.SetContentsOfCell( "A2", "1.234" );
        Assert.AreEqual( (double) ss[ "A1" ], 1.234, 0.0000001 );
        Assert.AreEqual( (double) ss[ "A1" ], (double) ss.GetCellValue( "A1" ), 0.00000001 );
    }
}

/// <summary>
///    Check cell name normalization (up-casing).
/// </summary>
[TestClass]
public class SpreadSheetNormalizationTests
{
    /// <summary>
    ///   Check name normalization. Given a lower case
    ///   cell name, it should work and be up-cased.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "28" )]
    public void SetGetCellContents_LowerCaseCellName_NormalizedToUpper( )
    {
        Spreadsheet s = new();

        s.SetContentsOfCell( "B1", "hello" );
        Assert.AreEqual( "hello", s.GetCellContents( "B1" ) );
        s.GetNamesOfAllNonemptyCells().Matches( [ "B1" ] );
    }

    /// <summary>
    ///   Check name normalization. Given a formula with a mis-cased
    ///   cell name, it should still work.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "29" )]
    public void SetContentsGetValue_MisCasedVariableNamesInFormula_ShouldStillWork( )
    {
        Spreadsheet s = new();

        s.SetContentsOfCell( "A1", "6" );
        s.SetContentsOfCell( "B1", "= A1" );

        Assert.AreEqual( 6.0, (double) s.GetCellValue( "B1" ), 1e-9 );
    }
}

/// <summary>
///    Test some simple mistakes that a user might make,
///    including invalid formulas resulting in the appropriate contents and values,
///    as well as invalid names.
/// </summary>
[TestClass]
public class SimpleInvalidSpreadsheetTests
{
    /// <summary>
    ///   Test that a formula can be added and retrieved.  The
    ///   contents of the cell are a Formula and the value of
    ///   the cell is a Formula Error.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "30" )]
    public void SetContentsOfCell_InvalidFormula_CreatedFormulaValueIsErrorType( )
    {
        Spreadsheet s = new();
        s.SetContentsOfCell( "B1", "=A1+C1" );
        Assert.AreEqual( s.GetNamesOfAllNonemptyCells().Count, 1 );
        Assert.IsInstanceOfType<Formula>( s.GetCellContents( "B1" ) );
        Assert.IsInstanceOfType<FormulaError>( s.GetCellValue( "B1" ) );
    }

    /// <summary>
    ///   Test that an invalid cell name doesn't affect spreadsheet.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "31" )]
    public void SetGetCellContents_InvalidNameAdded_StillEmpty( )
    {
        Spreadsheet s = new();

        try
        {
            s.SetContentsOfCell( "1A1", "x" );
        }
        catch
        {
            Assert.AreEqual( s.GetNamesOfAllNonemptyCells().Count, 0 );
            Assert.AreEqual( string.Empty, s.GetCellContents( "A1" ) );
            return;
        }

        Assert.Fail();
    }
}

/// <summary>
///    Test the changed property of the spreadsheet.
/// </summary>
[TestClass]
public class SpreadsheetChangedTests
{
    /// <summary>
    ///   After setting a cell, the spreadsheet is changed.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "32" )]
    public void Changed_AfterModification_IsTrue( )
    {
        Spreadsheet ss = new();
        Assert.IsFalse( ss.Changed );
        ss.SetContentsOfCell( "C1", "17.5" );
        Assert.IsTrue( ss.Changed );
    }

    /// <summary>
    ///   After saving the spreadsheet to a file, the spreadsheet is not changed.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "33" )]
    public void Changed_AfterSave_IsFalse( )
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell( "C1", "17.5" );
        ss.Save( "changed.txt" );
        Assert.IsFalse( ss.Changed );
    }
}

/// <summary>
///    Test that you can have multiple spreadsheet objects
///    and they don't interact with each other (e.g., by using static
///    fields/properties).
/// </summary>
[TestClass]
public class SpreadsheetNonStaticFields
{
    /// <summary>
    ///    Check that two spreadsheet objects are independent.  If we add
    ///    a value to one, it doesn't influence the other.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "34" )]
    public void Constructor_SetCellContents_SpreadSheetsAreIndependent( )
    {
        Spreadsheet s1 = new();
        Spreadsheet s2 = new();

        var results = new Dictionary<string, object>
        {
            { "X1", "hello" },
            { "A1", string.Empty },
            { "B1", 5.0 },
        };

        s1.SetContentsOfCell( "X1", "hello" );
        s1.SetContentsOfCell( "B1", "5.0" );
        s2.SetContentsOfCell( "X1", "goodbye" );

        s1.CompareToExpectedValues( results );
        s1.CompareCounts( results );

        results[ "X1" ] = "goodbye";
        results.Remove( "B1" );

        s2.CompareToExpectedValues( results );
        s2.CompareCounts( results );
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "35" )]
    public void IncreaseGradingWeight1( )
    {
        Constructor_SetCellContents_SpreadSheetsAreIndependent();
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "36" )]
    public void IncreaseGradingWeight2( )
    {
        Constructor_SetCellContents_SpreadSheetsAreIndependent();
    }

    /// <summary>
    ///    Increase score for grading tests.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "37" )]
    public void IncreaseGradingWeight3( )
    {
        Constructor_SetCellContents_SpreadSheetsAreIndependent();
    }
}

/// <summary>
///    Test that circular exceptions are handled correctly.
/// </summary>
[TestClass]
public class SpreadsheetCircularExceptions
{
    /// <summary>
    ///    Check that a simple circular exception is thrown.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "38" )]
    [ExpectedException( typeof( CircularException ) )]
    public void SetCellContents_CircularException_Throws( )
    {
        Spreadsheet s1 = new();
        s1.SetContentsOfCell( "A1", "=A1" );
    }

    /// <summary>
    ///    Check that assigning a circular exception doesn't change rest of spreadsheet.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "39" )]
    public void SetCellContents_CircularException_DoesNotChangeRestOfSheet( )
    {
        var results = new Dictionary<string, object>
        {
            { "A1", "hello" },
            { "A2", 10.0 },
            { "A3", new FormulaError( "Only cells that evaluate to a number are valid for lookup." ) },
            { "A4", 9.0 },
        };

        Spreadsheet s1 = new();
        s1.SetContentsOfCell( "A1", "hello" );
        s1.SetContentsOfCell( "A2", "=5+5" );
        s1.SetContentsOfCell( "A3", "=A1" );
        s1.SetContentsOfCell( "A4", "9.0" );

        try
        {
            s1.SetContentsOfCell( "A1", "=A1" );
        }
        catch
        {
            s1.CompareToExpectedValues( results );
            return;
        }

        Assert.Fail();
    }
}

/// <summary>
///    Test file input and output.
/// </summary>
[TestClass]
public class SpreadsheetLoadSaveTests
{
    /// <summary>
    ///   Try to save to a bad file.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "40" )]
    [ExpectedException( typeof( SpreadsheetReadWriteException ) )]
    public void Save_InvalidMissingFolder_Throws( )
    {
        Spreadsheet s1 = new();
        s1.Save( "." ); // note: this test was updated and students will all be given a point for it.
    }

    /// <summary>
    ///   Try to save to a folder (i.e., ".").
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "40" )]
    [ExpectedException( typeof( SpreadsheetReadWriteException ) )]
    public void Save_ToCurrentFolderPeriod_Throws( )
    {
        Spreadsheet s1 = new();
        s1.Save( "." );
    }

    /// <summary>
    ///   Try to load from a bad file.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "41" )]
    [ExpectedException( typeof( SpreadsheetReadWriteException ) )]
    public void Load_FromMissingFile_Throws( )
    {
        // should not be able to read
        Spreadsheet ss = new();
        ss.Load( "q:\\missing\\save.txt" );
    }

    /// <summary>
    ///   Write a single string to a spreadsheet, save the file,
    ///   load the file, look to see if the value is back.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "42" )]
    public void SaveLoad_CreateAVerySimpleSheetSaveAndLoad_GetOriginalBack( )
    {
        var results = new Dictionary<string, object>
        {
            { "A1", "hello" },
            { "B1", string.Empty },
        };

        Spreadsheet s1 = new();
        s1.SetContentsOfCell( "A1", "hello" );
        s1.Save( "save1.txt" );
        s1.CompareToExpectedValues( results );
        s1.CompareCounts( results );

        Spreadsheet s2 = new();
        s2.Load( "save1.txt" );

        s2.CompareToExpectedValues( results );
        s2.CompareCounts( results );
    }

    /// <summary>
    ///   Should not be able to read a file that is not correct JSON.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "43" )]
    [ExpectedException( typeof( SpreadsheetReadWriteException ) )]
    public void Load_InvalidXMLFile_Throws( )
    {
        using ( StreamWriter writer = new( "save2.txt" ) )
        {
            writer.WriteLine( "This" );
            writer.WriteLine( "is" );
            writer.WriteLine( "a" );
            writer.WriteLine( "test!" );
        }

        Spreadsheet s1 = new();
        s1.Load( "save2.txt" );
    }

    /// <summary>
    ///   Save a sheet, load the file with it, make sure the new sheet has
    ///   the expected values.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "44" )]
    public void Load_ReadValidPreDefinedJSONFile_ContainsCorrectData( )
    {
        var results = new Dictionary<string, object>
        {
            { "A1", "hello" },
            { "A2", 5.0 },
            { "A3", 4.0 },
            { "A4", 9.0 },
        };

        var sheet = new
        {
            Cells = new
            {
                A1 = new { StringForm = "hello" },
                A2 = new { StringForm = "5.0" },
                A3 = new { StringForm = "4.0" },
                A4 = new { StringForm = "= A2 + A3" },
            },
        };

        File.WriteAllText( "save5.txt", JsonSerializer.Serialize( sheet ) );

        Spreadsheet ss = new();
        ss.Load( "save5.txt" );

        ss.CompareToExpectedValues( results );
        ss.CompareCounts( results );
    }

    /// <summary>
    ///    Save a spreadsheet with a string, two numbers, and a formula,
    ///    and see that the saved file contains the proper json.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "45" )]
    public void Save_SaveSheetWithDoubleStringAndFormula_GeneratesValidJSONFileSyntax( )
    {
        Spreadsheet ss = new();
        ss.SetContentsOfCell( "A1", "hello" );
        ss.SetContentsOfCell( "A2", "5.0" );
        ss.SetContentsOfCell( "A3", "4.0" );
        ss.SetContentsOfCell( "A4", "=A2+A3" );
        ss.Save( "save6.txt" );

        string fileContents = File.ReadAllText("save6.txt");

        try
        {
            Dictionary<string, object> root  = JsonSerializer.Deserialize<Dictionary<string, object>>(fileContents) ?? [];
            if (!root.TryGetValue( "Cells", out object? cellValues ))
            {
                Assert.Fail();
            }

            var cells = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>( cellValues.ToString() ?? "oops" );

            if ( cells != null )
            {
                Assert.AreEqual( "hello", cells[ "A1" ][ "StringForm" ] );
                Assert.AreEqual( 5.0, double.Parse( cells[ "A2" ][ "StringForm" ] ), 1e-9 );
                Assert.AreEqual( 4.0, double.Parse( cells[ "A3" ][ "StringForm" ] ), 1e-9 );
                Assert.AreEqual( "=A2+A3", cells[ "A4" ][ "StringForm" ].Replace( " ", string.Empty ) );
            }
            else
            {
                Assert.Fail();
            }
        }
        catch
        {
            Assert.Fail();
        }
    }
}

/// <summary>
///    Test methods on larger spreadsheets.
/// </summary>
[TestClass]
public class LargerSpreadsheetTests
{
    /// <summary>
    ///    Create 7 cells, put some formulas in, change the values,
    ///    and verify that the final and intermediate results are good.
    /// </summary>
    /// <param name="ss"> For use with other tests, allow passing in of a spreadsheet. </param>
    /// <param name="verifyCounts"> if used alone, check the count as well as the values.</param>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "46" )]
    [DataRow( null, true )]
    public void SetContentsOfCell_SevenCellsWithModifications_CorrectValuesProduced( Spreadsheet? ss, bool verifyCounts )
    {
        var expectedResults = new Dictionary<string, object>
        {
            { "A1", 1.0 },
            { "A2", 2.0 },
            { "A3", 3.0 },
            { "A4", 4.0 },
            { "B1", 3.0 },
            { "B2", 12.0 },
            { "C1", 9.0 },
        };

        ss ??= new();

        ss.SetContentsOfCell( "A1", "1.0" );
        ss.SetContentsOfCell( "A2", "2.0" );
        ss.SetContentsOfCell( "A3", "3.0" );
        ss.SetContentsOfCell( "A4", "4.0" );
        ss.SetContentsOfCell( "B1", "= A1 + A2" );
        ss.SetContentsOfCell( "B2", "= A3 * A4" );
        ss.SetContentsOfCell( "C1", "= B2 - B1" );

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }

        ss.SetContentsOfCell( "A1", "2.0" );
        expectedResults[ "A1" ] = 2.0;
        expectedResults[ "A2" ] = 2.0;
        expectedResults[ "A3" ] = 3.0;
        expectedResults[ "B1" ] = 4.0;
        expectedResults[ "B2" ] = 12.0;
        expectedResults[ "C1" ] = 8.0;

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }

        ss.SetContentsOfCell( "B1", "= A1 / A2" );
        expectedResults[ "B1" ] = 1.0;
        expectedResults[ "B2" ] = 12.0;
        expectedResults[ "C1" ] = 11.0;

        ss.CompareToExpectedValues( expectedResults );

        if ( verifyCounts )
        {
            ss.CompareCounts( expectedResults );
        }
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "47" )]
    public void IncreaseGradingWeight_MediumSheet1( )
    {
        SetContentsOfCell_SevenCellsWithModifications_CorrectValuesProduced( null, true );
    }

    /// <summary>
    ///   See if we can write and read a (slightly) larger spreadsheet.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "48" )]
    public void SpreadSheetSaveLoad_OverallTestOfMediumSize_Correct( )
    {
        Dictionary<string, object> expectedResults = new()
        {
            [ "A1" ] = 2.0,
            [ "A2" ] = 2.0,
            [ "A3" ] = 3.0,
            [ "A4" ] = 4.0,
            [ "B1" ] = 1.0,
            [ "B2" ] = 12.0,
            [ "C1" ] = 11.0,
        };

        Spreadsheet s1 = new();
        SetContentsOfCell_SevenCellsWithModifications_CorrectValuesProduced( s1, true );
        s1.Save( "save7.txt" );

        Spreadsheet s2 = new();
        s2.Load( "save7.txt" );

        s2.CompareToExpectedValues( expectedResults );
        s2.CompareCounts( expectedResults );
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "49" )]
    public void IncreaseGradingWeight_MediumSave1( )
    {
        SpreadSheetSaveLoad_OverallTestOfMediumSize_Correct();
    }

    /// <summary>
    ///   <para>
    ///     A long chained formula. Solutions that re-evaluate
    ///     cells on every request, rather than after a cell changes,
    ///     will timeout on this test.
    ///   </para>
    ///   <para>
    ///     A1 = A3+A5
    ///     A2 = A3+A4
    ///     A3 = A5+A6
    ///     A4 = A5+A6
    ///     A5 = A7+A8
    ///     A6 = A7+A8
    ///     etc.
    ///   </para>
    ///   <para>
    ///     In the end we compute the 2^depth.
    ///   </para>
    ///   <para>
    ///     Then we set the end cells to zero and the whole sum goes to 0.
    ///   </para>
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "50" )]
    public void SetContentsOfCell_LongChainOfExponentialNumbers_ComputesPowersCorrectly( )
    {
        Spreadsheet s1 = new();

        s1.SetContentsOfCell( "sum1", "=A1+A2" );

        int i;
        int depth = 100;
        for ( i = 1; i <= depth * 2; i += 2 )
        {
            s1.SetContentsOfCell( "A" + i, "= A" + ( i + 2 ) + " + A" + ( i + 3 ) );
            s1.SetContentsOfCell( "A" + ( i + 1 ), "= A" + ( i + 2 ) + "+ A" + ( i + 3 ) );
        }

        s1.SetContentsOfCell( "A" + i, "1" );
        s1.SetContentsOfCell( "A" + ( i + 1 ), "1" );
        Assert.AreEqual( Math.Pow( 2, depth + 1 ), (double) s1.GetCellValue( "sum1" ), 1.0 );

        s1.SetContentsOfCell( "A" + i, "0" );
        Assert.AreEqual( Math.Pow( 2, depth ), (double) s1.GetCellValue( "sum1" ), 1.0 );

        s1.SetContentsOfCell( "A" + ( i + 1 ), "0" );
        Assert.AreEqual( 0.0, (double) s1.GetCellValue( "sum1" ), 0.1 );
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "51" )]
    public void IncreaseGradingWeight_Long1( )
    {
        SetContentsOfCell_LongChainOfExponentialNumbers_ComputesPowersCorrectly();
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "52" )]
    public void IncreaseGradingWeight_Long2( )
    {
        SetContentsOfCell_LongChainOfExponentialNumbers_ComputesPowersCorrectly();
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "53" )]
    public void IncreaseGradingWeight_Long3( )
    {
        SetContentsOfCell_LongChainOfExponentialNumbers_ComputesPowersCorrectly();
    }

    /// <summary>
    ///   Increases the value of the given test.
    /// </summary>
    [TestMethod]
    [Timeout( 2000 )]
    [TestCategory( "54" )]
    public void IncreaseGradingWeight_Long4( )
    {
        SetContentsOfCell_LongChainOfExponentialNumbers_ComputesPowersCorrectly();
    }
}

/// <summary>
///   Helper methods for the tests above.
/// </summary>
public static class TestExtensions
{
    /// <summary>
    ///   Check to see if the two "sets" (source and items) match, i.e.,
    ///   contain exactly the same values. Note: we do not check for sequencing.
    /// </summary>
    /// <param name="source"> original container.</param>
    /// <param name="items"> elements to match against.</param>
    /// <returns> true if every element in source is in items and vice versa. They are the "same set".</returns>
    public static bool Matches( this IEnumerable<string> source, params string[ ] items )
    {
        return ( source.Count() == items.Length ) && items.All( item => source.Contains( item ) );
    }

    /// <summary>
    ///   This function takes in a spreadsheet object and a List
    ///   of expected Cell values which are compared with the spreadsheet.
    ///   Failure to match results in an error.
    ///   <para>
    ///     It is valid to have additional values in the spreadsheet which are not checked.
    ///   </para>
    /// </summary>
    /// <param name="sheet"> The spreadsheet being tested. </param>
    /// <param name="expectedValues"> Key-value pairs for what should be in the spreadsheet. </param>
    public static void CompareToExpectedValues( this Spreadsheet sheet, Dictionary<string, object> expectedValues )
    {
        foreach ( var cellName in expectedValues.Keys )
        {
            if ( expectedValues[ cellName ] is double number )
            {
                Assert.AreEqual( number, (double) sheet.GetCellValue( cellName ), 1e-9 );
            }
            else if ( expectedValues[ cellName ] is string entry )
            {
                Assert.AreEqual( entry, sheet.GetCellValue( cellName ) );
            }
            else if ( expectedValues[ cellName ] is FormulaError error )
            {
                Assert.IsInstanceOfType( error, typeof( FormulaError ) );
            }
            else
            {
                throw new Exception( "Invalid data in expected value dictionary!" );
            }
        }
    }

    /// <summary>
    ///   This function takes in a spreadsheet object and a List
    ///   of expected Cell values and makes sure that there are not
    ///   any extra values in the sheet (e.g., more non-empty cells
    ///   than expected).  Failure to match results in an error.
    ///   <para>
    ///     It is valid to have additional values in the spreadsheet which are not checked.
    ///   </para>
    /// </summary>
    /// <param name="sheet"> The spreadsheet being tested. </param>
    /// <param name="expectedValues"> Key-value pairs for what should be in the spreadsheet. </param>
    public static void CompareCounts( this Spreadsheet sheet, Dictionary<string, object> expectedValues )
    {
        int numberOfExpectedResults = expectedValues.Values.Where(o=> o.ToString() != string.Empty).Count();
        int numberOfNonEmptyCells   = sheet.GetNamesOfAllNonemptyCells().Count;

        Assert.AreEqual( numberOfExpectedResults, numberOfNonEmptyCells );
    }
}
