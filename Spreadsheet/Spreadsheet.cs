// <copyright file="Spreadsheet.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>

// Written by Joe Zachary for CS 3500, September 2013
// Update by Profs Kopta and de St. Germain
//     - Updated return types
//     - Updated documentation

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CS3500.Spreadsheet;

using CS3500.Formula;
using CS3500.DependencyGraph;
using System.Security.Cryptography;
using System.Dynamic;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using System.Transactions;
using System.IO;


/// <summary>
///   <para>
///     Thrown to indicate that a change to a cell will cause a circular dependency.
///   </para>
/// </summary>
public class CircularException : Exception
{
}

/// <summary>
/// <para>
///   Thrown to indicate that a read or write attempt has failed with
///   an expected error message informing the user of what went wrong.
/// </para>
/// </summary>
public class SpreadsheetReadWriteException : Exception
{
    /// <summary>
    ///   <para>
    ///     Creates the exception with a message defining what went wrong.
    ///   </para>
    /// </summary>
    /// <param name="msg"> An informative message to the user. </param>
    public SpreadsheetReadWriteException(string msg)
    : base(msg)
    {
    }
}

/// <summary>
///   <para>
///     Thrown to indicate that a name parameter was invalid.
///   </para>
/// </summary>
public class InvalidNameException : Exception
{
}

/// <summary>
///   <para>
///     An Spreadsheet object represents the state of a simple spreadsheet.  A
///     spreadsheet represents an infinite number of named Cells.
///   </para>
/// <para>
///     Valid Cell Names: A string is a valid cell name if and only if it is one or
///     more letters followed by one or more numbers, e.g., A5, BC27.
/// </para>
/// <para>
///    Cell names are case insensitive, so "x1" and "X1" are the same cell name.
///    Your code should normalize (uppercased) any stored name but accept either.
/// </para>
/// <para>
///     A spreadsheet represents a cell corresponding to every possible cell name.  (This
///     means that a spreadsheet contains an infinite number of Cells.)  In addition to
///     a name, each cell has a contents and a value.  The distinction is important.
/// </para>
/// <para>
///     The <b>contents</b> of a cell can be (1) a string, (2) a double, or (3) a Formula.
///     If the contents of a cell is set to the empty string, the cell is considered empty.
/// </para>
/// <para>
///     By analogy, the contents of a cell in Excel is what is displayed on
///     the editing line when the cell is selected.
/// </para>
/// <para>
///     In a new spreadsheet, the contents of every cell is the empty string. Note:
///     this is by definition (it is IMPLIED, not stored).
/// </para>
/// <para>
///     The <b>value</b> of a cell can be (1) a string, (2) a double, or (3) a FormulaError.
///     (By analogy, the value of an Excel cell is what is displayed in that cell's position
///     in the grid.)
/// </para>
/// <list type="number">
///   <item>If a cell's contents is a string, its value is that string.</item>
///   <item>If a cell's contents is a double, its value is that double.</item>
///   <item>
///     <para>
///       If a cell's contents is a Formula, its value is either a double or a FormulaError,
///       as reported by the Evaluate method of the Formula class.  For this assignment,
///       you are not dealing with values yet.
///     </para>
///   </item>
/// </list>
/// <para>
///     Spreadsheets are never allowed to contain a combination of Formulas that establish
///     a circular dependency.  A circular dependency exists when a cell depends on itself.
///     For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
///     A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
///     dependency.
/// </para>
/// </summary>
public class Spreadsheet
{
    public Spreadsheet(string name)
    {
        Name = name;
        Cells = new();
    }

    // "Empty" Default Constructor to allow Spreadsheet to be serialized.
    public Spreadsheet() : this("default") { }
    [JsonInclude]
    public string Name { get; set; }
    private DependencyGraph depGraph = new();
    [JsonInclude]
    private Dictionary<string, Cell> Cells { get; set; }
    private HashSet<string> activeCells = new();
    public bool Changed = false;



    /// <summary>
    ///   <para>
    ///     Shortcut syntax to for getting the value of the cell
    ///     using the [] operator.
    ///   </para>
    ///   <para>
    ///     See: <see cref="GetCellValue(string)"/>.
    ///   </para>
    ///   <para>
    ///     Example Usage:
    ///   </para>
    ///   <code>
    ///      sheet.SetContentsOfCell( "A1", "=5+5" );
    ///
    ///      sheet["A1"] == 10;
    ///      // vs.
    ///      sheet.GetCellValue("A1") == 10;
    ///   </code>
    /// </summary>
    /// <param name="cellName"> Any valid cell name. </param>
    /// <returns>
    ///   Returns the value of a cell.  Note: If the cell is a formula, the value should
    ///   already have been computed.
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///     If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    public object this[string cellName]
    {
        get
        {
            return GetCellValue(cellName);
        }
    }

    /// <summary>
    ///   <para>
    ///     Writes the contents of this spreadsheet to the named file using a JSON format.
    ///     If the file already exists, overwrite it.
    ///   </para>
    ///   <para>
    ///     The output JSON should look like the following.
    ///   </para>
    ///   <para>
    ///     For example, consider a spreadsheet that contains a cell "A1" 
    ///     with contents being the double 5.0, and a cell "B3" with contents 
    ///     being the Formula("A1+2"), and a cell "C4" with the contents "hello".
    ///   </para>
    ///   <para>
    ///      This method would produce the following JSON string:
    ///   </para>
    ///   <code>
    ///   {
    ///     "Cells": {
    ///       "A1": {
    ///         "StringForm": "5"
    ///       },
    ///       "B3": {
    ///         "StringForm": "=A1+2"
    ///       },
    ///       "C4": {
    ///         "StringForm": "hello"
    ///       }
    ///     }
    ///   }
    ///   </code>
    ///   <para>
    ///     You can achieve this by making sure your data structure is a dictionary 
    ///     and that the contained objects (Cells) have property named "StringForm"
    ///     (if this name does not match your existing code, use the JsonPropertyName 
    ///     attribute).
    ///   </para>
    ///   <para>
    ///     There can be 0 Cells in the dictionary, resulting in { "Cells" : {} } 
    ///   </para>
    ///   <para>
    ///     Further, when writing the value of each cell...
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///       If the contents is a string, the value of StringForm is that string
    ///     </item>
    ///     <item>
    ///       If the contents is a double d, the value of StringForm is d.ToString()
    ///     </item>
    ///     <item>
    ///       If the contents is a Formula f, the value of StringForm is "=" + f.ToString()
    ///     </item>
    ///   </list>
    ///   <para>
    ///     After saving the file, the spreadsheet is no longer "changed".
    ///   </para>
    /// </summary>
    /// <param name="filename"> The name (with path) of the file to save to.</param>
    /// <exception cref="SpreadsheetReadWriteException">
    ///   If there are any problems opening, writing, or closing the file, 
    ///   the method should throw a SpreadsheetReadWriteException with an
    ///   explanatory message.
    /// </exception>
    public void Save(string filename)
    {
        try
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonSpreadsheet = JsonSerializer.Serialize(this, options);
            using (StreamWriter filewriter = new StreamWriter(filename))
            {
                filewriter.WriteLine(jsonSpreadsheet);
            }

            Changed = false;
        }
        catch (Exception e)
        {
            throw new SpreadsheetReadWriteException($"Error saving to file: {e}");
        }

    }

    /// <summary>
    /// This Function will return a string of JSON that stores our 
    /// current spreadsheets data in order to save this JSON with our GUI.
    /// </summary>
    /// <returns>A string resembling JSON of Spreadsheet</returns>
    public string GetJSON()
    {
        try
        {
            string jsonSpreadsheet = JsonSerializer.Serialize(this);
            Changed = false;
            return jsonSpreadsheet;
        }
        catch
        {
            throw new JsonException("Something went wrong with reading JSON");
        }
    }

    /// <summary>
    ///   <para>
    ///     Read the data (JSON) from the file and instantiate the current
    ///     spreadsheet.  See <see cref="Save(string)"/> for expected format.
    ///   </para>
    ///   <para>
    ///     Note: First deletes any current data in the spreadsheet.
    ///   </para>
    ///   <para>
    ///     Loading a spreadsheet should set changed to false.  External
    ///     programs should alert the user before loading over a changed sheet.
    ///   </para>
    /// </summary>
    /// <param name="filename"> The saved file name including the path. </param>
    /// <exception cref="SpreadsheetReadWriteException"> When the file cannot be opened or the json is bad.</exception>
    public void Load(string filename)
    {
        //Save all data
        DependencyGraph oldDepGraph = depGraph;
        Dictionary<string, Cell> oldCells = Cells;
        HashSet<string> oldActiveCells = activeCells;

        // Reset and clean out the spreadsheet
        depGraph = new();
        Cells = new();
        activeCells = new();

        try
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                string json = reader.ReadToEnd();
                Spreadsheet? jsonSpreadsheet;
                jsonSpreadsheet = JsonSerializer.Deserialize<Spreadsheet>(json);
                if (jsonSpreadsheet == null)
                {
                    throw new Exception("Deserialization returned null");
                }
                else
                {
                    foreach (var cell in jsonSpreadsheet.Cells)
                    {
                        SetContentsOfCell(cell.Key, cell.Value.StringForm);
                    }

                    this.Name = jsonSpreadsheet.Name;
                }
            }

            Changed = false;
        }
        catch (Exception e)
        {

            //Restore the old Spreadsheet data
            depGraph = oldDepGraph;
            Cells = oldCells;
            activeCells = oldActiveCells;
            throw new SpreadsheetReadWriteException($"File Error: {e}");
        }
    }

    /// <summary>
    /// FIXME:
    /// </summary>
    /// <param name="json"></param>
    /// <exception cref="ArgumentException"></exception>
    public void InstantiateFromJSON(string json) // Can throw an Argument Exception
    {
        //Save all data
        DependencyGraph oldDepGraph = depGraph;
        Dictionary<string, Cell> oldCells = Cells;
        HashSet<string> oldActiveCells = activeCells;

        // Reset and clean out the spreadsheet
        depGraph = new();
        Cells = new();
        activeCells = new();

        try
        {
            Spreadsheet? jsonSpreadsheet;
            jsonSpreadsheet = JsonSerializer.Deserialize<Spreadsheet>(json);
            if (jsonSpreadsheet == null)
            {
                throw new Exception("Deserialization returned null");
            }
            else
            {
                foreach (var cell in jsonSpreadsheet.Cells)
                {
                    SetContentsOfCell(cell.Key, cell.Value.StringForm);
                }
            }

            this.Name = jsonSpreadsheet.Name;
            Changed = false;
        }
        catch (Exception e)
        {

            //Restore the old Spreadsheet data
            depGraph = oldDepGraph;
            Cells = oldCells;
            activeCells = oldActiveCells;
            throw new ArgumentException($"File Error: {e}");
        }
    }

    /// <summary>
    ///   <para>
    ///     Return the value of the named cell.
    ///   </para>
    /// </summary>
    /// <param name="cellName"> The cell in question. </param>
    /// <returns>
    ///   Returns the value (as opposed to the contents) of the named cell.  The return
    ///   value's type should be either a string, a double, or a CS3500.Formula.FormulaError.
    ///   If the cell contents are a formula, the value should have already been computed
    ///   at this point.  
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the provided name is invalid, throws an InvalidNameException.
    /// </exception>
    public object GetCellValue(string cellName)
    {
        string name = CheckCellName(cellName);

        return Cells.ContainsKey(name) ? Cells[name].Value : string.Empty;
    }

    /// <summary>
    ///   <para>
    ///       Sets the contents of the named cell to the appropriate object
    ///       based on the string in <paramref name="content"/>.
    ///   </para>
    ///   <para>
    ///       First, if the <paramref name="content"/> parses as a double, the contents of the named
    ///       cell becomes that double.
    ///   </para>
    ///   <para>
    ///       Otherwise, if the <paramref name="content"/> begins with the character '=', an attempt is made
    ///       to parse the remainder of content into a Formula.
    ///   </para>
    ///   <para>
    ///       There are then three possible outcomes when a formula is detected:
    ///   </para>
    ///
    ///   <list type="number">
    ///     <item>
    ///       If the remainder of content cannot be parsed into a Formula, a
    ///       FormulaFormatException is thrown.
    ///     </item>
    ///     <item>
    ///       If changing the contents of the named cell to be f
    ///       would cause a circular dependency, a CircularException is thrown,
    ///       and no change is made to the spreadsheet.
    ///     </item>
    ///     <item>
    ///       Otherwise, the contents of the named cell becomes f.
    ///     </item>
    ///   </list>
    ///   <para>
    ///     Finally, if the content is a string that is not a double and does not
    ///     begin with an "=" (equal sign), save the content as a string.
    ///   </para>
    ///   <para>
    ///     On successfully changing the contents of a cell, the spreadsheet will be <see cref="Changed"/>.
    ///   </para>
    /// </summary>
    /// <param name="name"> The cell name that is being changed.</param>
    /// <param name="content"> The new content of the cell.</param>
    /// <returns>
    ///   <para>
    ///     This method returns a list consisting of the passed in cell name,
    ///     followed by the names of all other Cells whose value depends, directly
    ///     or indirectly, on the named cell. The order of the list MUST BE any
    ///     order such that if Cells are re-evaluated in that order, their dependencies
    ///     are satisfied by the time they are evaluated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list {A1, B1, C1} is returned.  If the Cells are then evaluate din the order:
    ///     A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
    ///   </para>
    /// </returns>
    /// <exception cref="InvalidNameException">
    ///   If the name parameter is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   If changing the contents of the named cell to be the formula would
    ///   cause a circular dependency, throw a CircularException.
    ///   (NOTE: No change is made to the spreadsheet.)
    /// </exception>
    public IList<string> SetContentsOfCell(string name, string content)
    {
        // Check name, make sure it is valid
        name = CheckCellName(name);
        IList<string> RecalcCells = new List<string>();
        double number;

        // Try to see if content is a double
        if (double.TryParse(content, out number))
        {
            Changed = true;
            RecalcCells = SetCellContents(name, number);
        }

        // Try to see if content is a Formula
        else if (content.StartsWith("="))
        {
            string formulaSubString = content.Substring(1);
            Formula cellFormula = new Formula(formulaSubString);
            try
            {
                Changed = true;
                RecalcCells = SetCellContents(name, cellFormula);
            }
            catch
            {
                Changed = false;
                throw;
            }
        }
        // Else, it is a string
        else
        {
            Changed = true;
            RecalcCells = SetCellContents(name, content);
        }

        // Recalculate all cells that need to be recalculated
        if (RecalcCells.Count != 0)
        {
            RecalcValues(name, RecalcCells);
        }
        return RecalcCells;
    }

    /// <summary>
    /// This Helper method filters through the list of returned cells and recalculates
    /// the values of the formulas inside of the cells. This should also follow
    /// the order of cells given by the SetCellContents.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="RecalcCells"></param>
    private void RecalcValues(string name, IList<string> RecalcCells)
    {
        foreach (string cell in RecalcCells)
        {
            if (cell != name && Cells[cell].Content is Formula)
            {
                Formula cellFormula = (Formula)Cells[cell].Content;
                Cells[cell].Value = cellFormula.Evaluate(CellValueDouble);
            }
        }
    }

    /// <summary>
    ///   Provides a copy of the names of all of the Cells in the spreadsheet
    ///   that contain information (i.e., not empty Cells).
    /// </summary>
    /// <returns>
    ///   A set of the names of all the non-empty Cells in the spreadsheet.
    /// </returns>
    public ISet<string> GetNamesOfAllNonemptyCells()
    {
        return activeCells;
    }

    /// <summary>
    ///   Returns the contents (as opposed to the value) of the named cell.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   Thrown if the name is invalid.
    /// </exception>
    ///
    /// <param name="name">The name of the spreadsheet cell to query. </param>
    /// <returns>
    ///   The contents as either a string, a double, or a Formula.
    ///   See the class header summary.
    /// </returns>
    public object GetCellContents(string name)
    {
        // Normalize Variable
        name = name.ToUpper();
        CheckCellName(name);

        if (Cells.ContainsKey(name))
        {
            return Cells[name].Content;
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets the string form of a given cell.
    /// </summary>
    /// <param name="name">A valid cell name.</param>
    /// <returns></returns>
    public string GetCellStringForm(string name)
    {
        name = CheckCellName(name);
        if (Cells.ContainsKey(name))
        {
            return Cells[name].StringForm;
        }
        return string.Empty;
    }

    /// <summary>
    ///  Set the contents of the named cell to the given number.
    /// </summary>
    ///
    /// <param name="name"> The name of the cell. </param>
    /// <param name="number"> The new content of the cell. </param>
    /// <returns>
    ///   <para>
    ///     This method returns an ordered list consisting of the passed in name
    ///     followed by the names of all other Cells whose value depends, directly
    ///     or indirectly, on the named cell.
    ///   </para>
    ///   <para>
    ///     The order must correspond to a valid dependency ordering for recomputing
    ///     all of the Cells, i.e., if you re-evaluate each cell in the order of the list,
    ///     the overall spreadsheet will be correctly updated.
    ///   </para>
    ///   <para>
    ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
    ///     list [A1, B1, C1] is returned, i.e., A1 was changed, so then A1 must be
    ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
    ///   </para>
    /// </returns>
    private IList<string> SetCellContents(string name, double number)
    {
        // Find new cell, and if not in there then create it.
        if (Cells.ContainsKey(name))
        {
            ChangeCell(name, number, number, number.ToString());
            depGraph.ReplaceDependees(name, []);
        }
        else
        {
            AddCell(name, number, number, number.ToString());
        }

        return GetCellsToRecalculate(name).ToList();
    }

    /// <summary>
    ///   The contents of the named cell becomes the given text.
    /// </summary>
    ///
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="text"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetContentsOfCell(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, string text)
    {

        // Find Cell, if it exists and text is empty, delete it from the dictionary
        // as well as replace all dependents with an empty set.
        if (Cells.ContainsKey(name))
        {
            if (text.Equals(string.Empty))
            {
                Cells.Remove(name);
                depGraph.ReplaceDependees(name, []);
                activeCells.Remove(name);
            }
            else
            {
                ChangeCell(name, text, text, text);
                depGraph.ReplaceDependees(name, []);
            }
        }

        // Cell not found, adding a new one.
        else if (!text.Equals(string.Empty))
        {
            AddCell(name, text, text, text);
        }

        return GetCellsToRecalculate(name).ToList();

    }

    /// <summary>
    ///   Set the contents of the named cell to the given formula.
    /// </summary>
    /// <exception cref="InvalidNameException">
    ///   If the name is invalid, throw an InvalidNameException.
    /// </exception>
    /// <exception cref="CircularException">
    ///   <para>
    ///     If changing the contents of the named cell to be the formula would
    ///     cause a circular dependency, throw a CircularException.
    ///   </para>
    ///   <para>
    ///     No change is made to the spreadsheet.
    ///   </para>
    /// </exception>
    /// <param name="name"> The name of the cell. </param>
    /// <param name="formula"> The new content of the cell. </param>
    /// <returns>
    ///   The same list as defined in <see cref="SetContentsOfCell(string, double)"/>.
    /// </returns>
    private IList<string> SetCellContents(string name, Formula formula)
    {
        List<string> recalcCells = new List<string>();

        // Deep copy of the original Dependencies
        HashSet<string> originalDependees =
            new HashSet<string>(depGraph.GetDependees(name));
        HashSet<string> originalDependents =
            new HashSet<string>(depGraph.GetDependents(name));

        // Iterate through variables, then add them to dependency graph
        ISet<string> varList = formula.GetVariables();
        depGraph.ReplaceDependees(name, varList);

        // Check and make sure that we don't get a CircularException
        // If so, redo the changes to the dependency graph, and throw exception.
        try
        {
            recalcCells = GetCellsToRecalculate(name).ToList();
        }
        catch (CircularException)
        {
            RevertDependencies(name, originalDependees, originalDependents);

            throw;
        }

        if (Cells.ContainsKey(name))
        {
            ChangeCell(name, formula, formula.Evaluate(CellValueDouble), "=" + formula.ToString());
        }
        else
        {
            AddCell(name, formula, formula.Evaluate(CellValueDouble), "=" + formula.ToString());
        }

        return recalcCells;
    }

    /// <summary>
    /// This function is used in Tandem with the GetCellValue function, but checks that it only returns a double, and if the
    /// Cells contain something other than a double, a FormulaError object will be returned, setting that cell's value to the
    /// Formula Error Object
    /// </summary>
    /// <param name="cellName">Name of the variable.</param>
    /// <returns></returns>
    private double CellValueDouble(string cellName)
    {
        return (double)Cells[cellName].Value;
    }

    /// <summary>
    /// Helper Method for when a circular dependency is found, therefore we must revert the original dependencies back.
    /// </summary>
    /// <param name="name">The name of a given cell</param>
    /// <param name="originalDependees">The original dependees.</param>
    /// <param name="originalDependents">The original dependents.</param>
    /// <returns></returns>
    private void RevertDependencies(string name, HashSet<string> originalDependees, HashSet<string> originalDependents)
    {
        depGraph.ReplaceDependees(name, originalDependees);
        depGraph.ReplaceDependents(name, originalDependents);
    }

    /// <summary>
    ///   Returns an enumeration, without duplicates, of the names of all Cells whose
    ///   values depend directly on the value of the named cell.
    /// </summary>
    /// <param name="name"> This <b>MUST</b> be a valid name.  </param>
    /// <returns>
    ///   <para>
    ///     Returns an enumeration, without duplicates, of the names of all Cells
    ///     that contain formulas containing name.
    ///   </para>
    ///   <para>For example, suppose that: </para>
    ///   <list type="bullet">
    ///      <item>A1 contains 3</item>
    ///      <item>B1 contains the formula A1 * A1</item>
    ///      <item>C1 contains the formula B1 + A1</item>
    ///      <item>D1 contains the formula B1 - C1</item>
    ///   </list>
    ///   <para> The direct dependents of A1 are B1 and C1. </para>
    /// </returns>
    private IEnumerable<string> GetDirectDependents(string name)
    {
        return depGraph.GetDependents(name);
    }

    /// <summary>
    ///   <para>
    ///     This method is implemented for you, but makes use of your GetDirectDependents.
    ///   </para>
    ///   <para>
    ///     Returns an enumeration of the names of all Cells whose values must
    ///     be recalculated, assuming that the contents of the cell referred
    ///     to by name has changed.  The cell names are enumerated in an order
    ///     in which the calculations should be done.
    ///   </para>
    ///   <exception cref="CircularException">
    ///     If the cell referred to by name is involved in a circular dependency,
    ///     throws a CircularException.
    ///   </exception>
    ///   <para>
    ///     For example, suppose that:
    ///   </para>
    ///   <list type="number">
    ///     <item>
    ///       A1 contains 5
    ///     </item>
    ///     <item>
    ///       B1 contains the formula A1 + 2.
    ///     </item>
    ///     <item>
    ///       C1 contains the formula A1 + B1.
    ///     </item>
    ///     <item>
    ///       D1 contains the formula A1 * 7.
    ///     </item>
    ///     <item>
    ///       E1 contains 15
    ///     </item>
    ///   </list>
    ///   <para>
    ///     If A1 has changed, then A1, B1, C1, and D1 must be recalculated,
    ///     and they must be recalculated in an order which has A1 first, and B1 before C1
    ///     (there are multiple such valid orders).
    ///     The method will produce one of those enumerations.
    ///   </para>
    ///   <para>
    ///      PLEASE NOTE THAT THIS METHOD DEPENDS ON THE METHOD GetDirectDependents.
    ///      IT WON'T WORK UNTIL GetDirectDependents IS IMPLEMENTED CORRECTLY.
    ///   </para>
    /// </summary>
    /// <param name="name"> The name of the cell.  Requires that name be a valid cell name.</param>
    /// <returns>
    ///    Returns an enumeration of the names of all Cells whose values must
    ///    be recalculated.
    /// </returns>
    private IEnumerable<string> GetCellsToRecalculate(string name)
    {
        LinkedList<string> changed = new();
        HashSet<string> visited = [];
        Visit(name, name, visited, changed);
        return changed;
    }

    /// <summary>
    ///   A helper for the GetCellsToRecalculate method. It recursively calls itself,
    ///   goes deeper to each level of dependents and ensures that it doesn't see an 
    ///   already used dependent in this level of dependency. Changed functions as an
    ///   overall linked list that stores 
    /// </summary>
    private void Visit(string start, string name, ISet<string> visited, LinkedList<string> changed)
    {
        visited.Add(name);
        foreach (string dependent in GetDirectDependents(name))
        {
            if (dependent.Equals(start))
            {
                throw new CircularException();
            }
            else if (!visited.Contains(dependent))
            {
                Visit(start, dependent, visited, changed);
            }
        }

        changed.AddFirst(name);
    }

    /// <summary>
    /// This checks to make sure that a variable is a valid name.
    /// </summary>
    /// <param name="name">Name of potential cell.</param>
    /// <returns></returns>
    private bool IsVar(string name)
    {
        string standaloneVarPattern = $"^[a-zA-Z]+\\d+$";
        return Regex.IsMatch(name, standaloneVarPattern);
    }

    /// <summary>
    /// Checks the name, and if it isn't a valid name, throws an exception.
    /// </summary>
    /// <param name="name">Name of Cell</param>
    /// <exception cref="InvalidNameException"></exception>
    private string CheckCellName(string name)
    {
        if (!IsVar(name))
        {
            throw new InvalidNameException();
        }

        return name.ToUpper();
    }

    /// <summary>
    /// Changes the given Cells Contents
    /// </summary>
    /// <param name="name">Name of Cell</param>
    /// <param name="value">Value of Object to change</param>
    private void ChangeCell(string name, object content, object value, string stringForm)

    {
        Cell cell = Cells[name];
        cell.Content = content;
        cell.Value = value;
        cell.StringForm = stringForm;
    }

    /// <summary>
    /// This takes in a name and a value and creates a new cell.
    /// </summary>
    /// <param name="name"> Name of Cell</param>
    /// <param name="value">Contents of Cell</param>

    private void AddCell(string name, object content, object value, string stringForm)
    {
        Cell cell = new(name, content, value, stringForm);
        Cells.Add(name, cell);
        activeCells.Add(name);
    }

}

/// <summary>
/// <para>
/// This cell class is a private/internal class to our Spreadsheet class.
/// This is to help us follow a very functional mindset when programming.
/// </para>
/// <para>
/// This class is to take in a "key", or name of a cell,
/// and return the contents that have been stored inside of that cell.
/// </para>
/// <list type="bullet"> Values can be
/// <item>Double
/// </item>
/// <item>String
/// </item>
/// <item>Formula
/// </item>
/// </list>
/// 
/// Later, we will add value and evaluation to the spreadsheet, which will
/// return a string, double, or formulaError object.
/// </summary>
/// <param name="contents">
/// This object is a double, string, or a formula
/// </param>
internal class Cell(string name, object content, object value, string stringForm)
{
    [JsonInclude]
    public string Name { get; set; } = name;

    [JsonInclude]
    public string StringForm { get; set; } = stringForm;

    [JsonIgnore]
    public object Content { get; set; } = content;

    [JsonIgnore]
    public object Value { get; set; } = value;
}
