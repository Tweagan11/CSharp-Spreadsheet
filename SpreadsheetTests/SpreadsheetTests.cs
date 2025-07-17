using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;

namespace SpreadsheetTests;

using CS3500.Formula;
using CS3500.Spreadsheet;
using System.Security.Cryptography;

[TestClass]
public class SpreadsheetTests
{
    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This checks to make sure that the
    /// double value is recognized and put in correctly.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellsDouble_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5.0");
        Assert.AreEqual(s.GetCellContents("A1"), 5.0);
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This checks to make sure that the
    /// string value is recognized and put in correctly.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellsString_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Test");
        Assert.IsTrue(s.GetCellContents("A1") is string);
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This checks to make sure that the
    /// formula object is recognized and put in correctly.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellsFormula_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A2", "3.0");
        s.SetContentsOfCell("A1", "=A2+5");
        Assert.IsTrue(s.GetCellContents("A1") is Formula);
        Assert.IsTrue(s.GetCellValue("A1") is double);
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This goes through all object variants,
    /// and checks to make sure that they are the correct object, ensuring that
    /// when changing the content of the cells, it doesn't modify the objects.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellsAllObjects_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Test");
        Assert.IsTrue(s.GetCellContents("A1") is String);

        s.SetContentsOfCell("A1", "5.0");
        Assert.IsTrue(s.GetCellContents("A1") is Double);

        s.SetContentsOfCell("A1", "NewTest");
        Assert.IsTrue(s.GetCellContents("A1") is String);
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This checks to make sure that the
    /// string value is recognized and put in correctly.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellsDoubleAsString_Invalid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5.0");
        Assert.AreEqual(s.GetCellContents("A1"), 5.0);
        Assert.AreNotEqual(s.GetCellContents("A1"), "5.0");
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This Test looks and makes sure
    /// that both a list object is returned as well as the correct dependencies
    /// are contained in that list.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_SetGetCellList_Valid()
    {
        // Create different formula objects to create variables and dependencies.
        Formula bForm = new Formula("A1*2");
        Formula cForm = new Formula("A1+B1");

        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "2.0");
        s.SetContentsOfCell("B1", "=A1*2");
        s.SetContentsOfCell("C1", "=A1+B1");

        IList<string> depList = s.SetContentsOfCell("A1", "5.0");

        // Create a comparable List
        IList<string> expectedList = ["A1", "B1", "C1"];

        Assert.IsTrue(depList.SequenceEqual(expectedList));
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This test does have a cirucular dependency, (A1 -> B1 -> C1 -> A1, etc)
    /// which should throw our Circular Exception.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(CircularException))]
    public void SpreadsheetConstructor_SetGetCellsCircular_Invalid()
    {
        // Create different formula objects to create variables and dependencies.

        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=B1 + 2");
        s.SetContentsOfCell("B1", "=C1 * 3");

        // Should throw exception after C1 is constructed.
        s.SetContentsOfCell("C1", "=A1 - 4");
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This test invalidates the naming structure
    /// of cells, therefore will give us an invalid name exception.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_SetGetCellsInvalidName_Invalid()
    {

        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1A", "5.0");
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This test creates dependents for a certain
    /// cell, and should return the dependents, both direct and indirect.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_GetDirectDependents_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1","5.0");
        s.SetContentsOfCell("B1", "=A1 * A1");
        s.SetContentsOfCell("C1", "=A1 - B1");
        s.SetContentsOfCell("D1", "=B1 * C1");
        IList<string> depList = s.SetContentsOfCell("A1", "2.0");


        // Create a comparable List
        IList<string> expectedList = ["A1", "B1", "C1", "D1"];

        Assert.IsTrue(depList.SequenceEqual(expectedList));

    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This Test looks at the public GetNamesOfAllNonemptyCells
    /// method, and makes sure that all nonempty cells are returned.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_GetNonemptyCellsNone_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Count() == 0);
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This Test looks at the public GetNamesOfAllNonemptyCells
    /// method, and makes sure that all nonempty cells are returned.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_GetNonemptyCellsOne_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5.0");

        // Create a comparable List
        ISet<string> expectedSet = new HashSet<string>
        {
            "A1"
        };

        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().SetEquals(expectedSet));
    }

    /// <summary>
    /// This test evaluates both the setting and getting of cells in
    /// the Spreadsheet constructor. This Test looks at the public GetNamesOfAllNonemptyCells
    /// method, and makes sure that all nonempty cells are returned.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_GetNonemptyCellsMulti_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5.0");
        s.SetContentsOfCell("B1", "5.0");
        s.SetContentsOfCell("C1", "5.0");


        // Create a comparable List
        ISet<string> expectedSet = new HashSet<string>
        {
            "A1", "B1", "C1"
        };

        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().SetEquals(expectedSet));
    }

    /// <summary>
    /// When our cells have not been set yet, we need to make sure that
    /// the cells return an empty string. This tests that, so A1 should return an empty string.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_GetEmptyCell_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        Assert.AreEqual(s.GetCellContents("A1"), string.Empty);
    }

    /// <summary>
    /// This test will take in 3 cells of 3 different valid types, and then delete them
    /// to make sure they are removed from the list of Nonempty cells.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_DeleteCells_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "1.0");
        s.SetContentsOfCell("B1", "hello");
        s.SetContentsOfCell("C1", "=7+8");

        s.SetContentsOfCell("A1", "");
        s.SetContentsOfCell("B1", string.Empty);
        s.SetContentsOfCell("C1", "");

        Assert.IsTrue(s.GetNamesOfAllNonemptyCells().Count() == 0);
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().Contains("A1"));
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().Contains("B1"));
        Assert.IsFalse(s.GetNamesOfAllNonemptyCells().Contains("C1"));
    }

    /// <summary>
    /// This test will take in 3 cells of 3 different valid types, and then delete them
    /// to make sure they are removed from the list of Nonempty cells.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_CellsListManyCells_Valid()
    {
        Spreadsheet s = new Spreadsheet();

        s.SetContentsOfCell("A1", "=B1+C1");
        s.SetContentsOfCell("B1", "=D1+C1");
        s.SetContentsOfCell("C1", "=D1+E1");
        s.SetContentsOfCell("D1", "=E1+F1");
        s.SetContentsOfCell("E1", "=F1+88");

        IList<string> cells = s.SetContentsOfCell("F1", "29");
        IList<string> expectedCells = ["F1", "E1", "D1", "C1", "B1", "A1"];

        Assert.IsTrue(expectedCells.SequenceEqual(cells));
        Assert.IsFalse(cells.SequenceEqual(expectedCells.Reverse()));

        cells = s.SetContentsOfCell("AB1", "889.22");
        expectedCells = ["AB1"];
        Assert.IsTrue(expectedCells.SequenceEqual(cells));
    }

    /// <summary>
    /// This test checks when getting a cell that has an invalid name that it throws an error.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(InvalidNameException))]
    public void SpreadsheetConstructor_GetInvalidCell_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.GetCellContents("A1A");
    }

    /// <summary>
    /// When Setting a cell, we expect the formula to be stored, so if the fromula is wrong
    /// then we should expect a formulaFormat exception
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(FormulaFormatException))]
    public void SpreadsheetConstructor_CellStoreInvalidFormula_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "=B1++3");
    }

    /// <summary>
    /// Testing the Indexer of a cell, which should have similar functionality to a
    /// GetCellValue function.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_CellIndexerDouble_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5");
        Assert.AreEqual(s["A1"], 5.0);
    }

    /// <summary>
    /// Testing the Indexer of a cell, which should have similar functionality to a
    /// GetCellValue function. This tests it with a string 
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_CellIndexerString_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Hello");
        Assert.AreEqual(s["A1"], "Hello");
    }

    /// <summary>
    /// Testing the Indexer of a cell, which should have similar functionality to a
    /// GetCellValue function. This Tests it with a formula, converting it to a string and then seeing if the values match. 
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_CellIndexerFormula_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        Formula f = new Formula("A1");
        s.SetContentsOfCell("A1", "5.0");
        s.SetContentsOfCell("B1", "="+f.ToString());
        Assert.AreEqual(s["A1"], 5.0);
    }

    /// <summary>
    /// This tests when saving a file if the contents match the given format.
    /// This format should be a classic serialization of JSON, when each cell has a value, a stringform, and a cell name.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void Spreadsheet_SaveFile_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5");
        s.Save("test.txt");

        using (StreamReader reader = new StreamReader("test.txt"))
        {
            string allText = reader.ReadToEnd();
            Console.WriteLine(allText);
        }

        Spreadsheet newS = new Spreadsheet();
        newS.Load("test.txt");

        Assert.AreEqual(s["A1"], newS["A1"]);
    }

    /// <summary>
    /// When loading a file, it should match the given format of JSON.
    /// This format should be a classic serialization of JSON, when each cell has a value, a stringform, and a cell name.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void Spreadsheet_LoadFile_Valid()
    {
        string expectedOutput = "{\r\n    \t\"Cells\": {\r\n    \t\t\"A1\": {\r\n    \t\t\t\"StringForm\": \"5\"\r\n    \t\t\t},\r\n    \t\t\"B3\": {\r\n    \t\t\t\"StringForm\": \"=A1+2\"\r\n    \t\t\t},\r\n    \t\t\"C4\": {\r\n    \t\t\t\"StringForm\": \"hello\"\r\n    \t\t\t}\r\n    \t\t}\r\n    }\r\n";

        File.WriteAllText("known_values.txt", expectedOutput);

        Spreadsheet s = new Spreadsheet();
        s.Load("known_values.txt");

        Assert.AreEqual(s["A1"], 5.0);
        Assert.AreEqual(s["B3"], 7.0);
        Assert.AreEqual(s["C4"], "hello");

    }

    /// <summary>
    /// Given an invalid name, this should throw an exception due to the file being invalid when trying to save a file.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Spreadsheet_SaveFile_Invalid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "5");
        s.Save(".");
    }

    /// <summary>
    /// Given an invalid name, this should throw an exception due to the file being invalid when trying to load a file.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Spreadsheet_LoadFile_Invalid()
    {
        Spreadsheet s = new Spreadsheet();
        s.Load(".");
    }

    /// <summary>
    /// When any changes are made to the spreadsheet, this variable should be set to true, and then after saving, should be false.
    /// This lets us know and aid the user in not losing any changes, helping them save the contents in the file.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void Spreadsheet_ChangedStatus_Valid()
    {
        Spreadsheet s = new Spreadsheet();
        // On initialization, should be set to false.
        Assert.IsFalse(s.Changed);
        s.SetContentsOfCell("A1", "5.0");

        // Something changed, should be true now
        Assert.IsTrue(s.Changed);
        s.Save("test.txt");

        // Saved, should be back to false.
        Assert.IsFalse(s.Changed);
    }

    /// <summary>
    /// When loading a file, it should match the given format of JSON.
    /// This format should be a classic serialization of JSON, when each cell has a value, a stringform, and a cell name.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Spreadsheet_LoadFileEmptyJSON_Invalid()
    {
        string expectedOutput = "";

        File.WriteAllText("known_values.txt", expectedOutput);

        Spreadsheet s = new Spreadsheet();
        s.Load("known_values.txt");

    }

    /// <summary>
    /// When loading a file, it should match the given format of JSON.
    /// This format should be a classic serialization of JSON, when each cell has a string form, and a cell name.
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    [ExpectedException(typeof(SpreadsheetReadWriteException))]
    public void Spreadsheet_LoadFileNoJSON_Invalid()
    {
        string expectedOutput = "This is a random text file";

        File.WriteAllText("known_values.txt", expectedOutput);

        Spreadsheet s = new Spreadsheet();
        s.Load("known_values.txt");

    }

    /// <summary>
    /// Testing the Indexer of a cell, which should have similar functionality to a
    /// GetCellValue function. This Tests it with a formula, converting it to a string and then seeing if the values match. 
    /// </summary>
    [TestMethod]
    [TestCategory("Constructor")]
    public void SpreadsheetConstructor_InvalidInputCellFormula_Invalid()
    {
        Spreadsheet s = new Spreadsheet();
        s.SetContentsOfCell("A1", "Hello");
        s.SetContentsOfCell("B1", "=A1");
        Assert.IsTrue(s["B1"] is FormulaError);
    }

}

