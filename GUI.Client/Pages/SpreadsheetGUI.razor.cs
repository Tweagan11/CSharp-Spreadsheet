// <copyright file="SpreadsheetGUI.razor.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// Ignore Spelling: Spreadsheeeeeeeeee

namespace SpreadsheetNS;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using CS3500.Spreadsheet;
using System.Text.RegularExpressions;
using CS3500.Formula;

/// <summary>
///  This is a partial class that helps the implementation of the Razor files.
///  <remarks>
///    <para>
///      This is a partial class because class SpreadsheetGUI is also automatically
///      generated from the SpreadsheetGUI.razor file.  Any code in that file, and variable in
///      that file can be referenced here, and vice versa.
///    </para>
///    <para>
///      It is usually better to put the code in a separate CS isolation file so that Visual Studio
///      can use intellisense better.
///    </para>
///    <para>
///      Note: only GUI related information should go in the sheet. All (Model) spreadsheet
///      operations should happen through the Spreadsheet class API.
///    </para>
///    <para>
///      The "backing stores" are strings that are used to affect the content of the GUI
///      display.  When you update the Spreadsheet, you will then have to copy that information
///      into the backing store variable(s).
///    </para>
///  </remarks>
/// </summary>
public partial class SpreadsheetGUI
{
    /// <summary>
    /// Spreadsheet object used to model the cells and dependencies.
    /// </summary>
    private static Spreadsheet spreadsheet = new Spreadsheet();

    /// <summary>
    ///    Gets the alphabet for ease of creating columns.
    /// </summary>
    private static char[ ] Alphabet { get; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    ///   Gets or sets the javascript object for this web page that allows
    ///   you to interact with any javascript in the associated file.
    /// </summary>
    private IJSObjectReference? JSModule { get; set; }

   /// <summary>
    ///   Gets or sets the Filename to match the given spreadsheet name.
    ///   If the spreadsheet name is updated, so is the FileSaveName.
    /// </summary>
    private string FileSaveName { get; set; } = $"{spreadsheet.Name}.sprd";

    /// <summary>
    ///   <para> Gets or sets the data for the Tool Bar Cell Contents text area, e.g., =57+A2. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string ToolBarCellContents { get; set; } = string.Empty;

    /// <summary>
    ///   <para> Gets or sets the data for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML</remarks>
    /// </summary>
    private string[,] CellsBackingStore { get; set; } = new string[ 99, 26 ];

    /// <summary>
    ///   <para> Gets or sets the html class string for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML CLASS strings</remarks>
    /// </summary>
    private string[,] CellsClassBackingStore { get; set; } = new string[ 99, 26 ];

    /// <summary>
    ///   <para> Gets or sets the html class string for all of the cells in the spreadsheet GUI. </para>
    ///   <remarks>Backing Store for HTML CLASS strings</remarks>
    /// </summary>
    private ElementReference[,] CellsRefBackingStore { get; set; } = new ElementReference[99, 26];

    /// <summary>
    ///   Gets or sets a value indicating whether we are showing the save "popup" or not.
    /// </summary>
    private bool SaveGUIView { get; set; }

    /// <summary>
    ///   Gets or sets a value indicating whether we are showing the resize popup or not.
    /// </summary>
    private bool ResizeGUI { get; set; }

    /// <summary>
    ///   Gets or sets a value indicating whether we are showing the resize popup or not.
    /// </summary>
    private bool RenameGUI { get; set; }

    /// <summary>
    ///   Query the spreadsheet to see if it has been changed.
    ///   <remarks>
    ///     Any method called from JavaScript must be public
    ///     and JSInvokable!
    ///   </remarks>
    /// </summary>
    /// <returns>
    ///   true if the spreadsheet is changed.
    /// </returns>
    [JSInvokable]
    public bool HasSpreadSheetChanged()
    {
        return spreadsheet.Changed;
    }

    /// <summary>
    ///   Example of how JavaScript can talk "back" to the C# side.
    /// </summary>
    /// <param name="message"> string from javascript side. </param>
    [JSInvokable]
    public void TestBlazorInterop(string message)
    {
        Debug.WriteLine($"JavaScript has send me a message: {message}");
    }

    /// <summary>
    ///   Set up initial state and event handlers.
    ///   <remarks>
    ///     This is somewhat like a constructor for a Blazor Web Page (object).
    ///     You probably don't need to do anything here.
    ///   </remarks>
    /// </summary>
    protected override void OnInitialized( )
    {
        CellsClassBackingStore[0, 0] = "selected"; // Sets the default to 0,0
    }

    /// <summary>
    ///   Called anytime in the lifetime of the web page were the page is re-rendered.
    ///   <remarks>
    ///     You probably don't need to do anything in here beyond what is provided.
    ///   </remarks>
    /// </summary>
    /// <param name="firstRender"> true the very first time the page is rendered.</param>
    protected async override void OnAfterRender( bool firstRender )
    {
        base.OnAfterRender( firstRender );

        if ( firstRender )
        {
            /////////////////////////////////////////////////
            //
            // The following three lines setup and test the
            // ability for Blazor to talk to javascript and vice versa.
            JSModule = await JS.InvokeAsync<IJSObjectReference>( "import", "./Pages/SpreadsheetGUI.razor.js" ); // create/read the javascript
            await JSModule.InvokeVoidAsync( "SetDotNetInterfaceObject", DotNetObjectReference.Create( this ) ); // tell the javascript about us (dot net)

            await FormulaContentEditableInput.FocusAsync(); // when we start up, put the focus on the input. you will want to do this anytime a cell is clicked.
        }
    }

    /// <summary>
    ///  cells should be of the form "A5" or "B1".  The matrix of cells (the backing store) is zero
    ///  based but the first row in the spreadsheet is 1.
    /// </summary>
    /// <param name="cellName"> The name of the cell. </param>
    /// <param name="row"> The returned conversion between row and zero based index. </param>
    /// <param name="col"> The returned conversion between column letter and zero based matrix index. </param>
    private static void ConvertCellNameToRowCol( string cellName, out int row, out int col )
    {
        // Split the Cell name into 2 parts, column letter(s) and digit(s)
        var colMatch = Regex.Match(cellName, @"[A-Z]+");
        var rowMatch = Regex.Match(cellName, @"\d+");

        // Change String to Character
        char charCol = char.Parse(colMatch.Value);
        col = Array.IndexOf(Alphabet, charCol);  // A1 --> (0,0)
        row = int.Parse(rowMatch.Value)-1;
    }

    /// <summary>
    ///   Given a row,col such as "(0,0)" turn this into the appropriate
    ///   cell name, such as: "A1".
    /// </summary>
    /// <param name="row"> The row number (0-A, 1-B, ...).</param>
    /// <param name="col"> The column number (0 based).</param>
    /// <returns>A string defining the cell name, where the col is A-Z and row is not zero based.</returns>
    private static string CellNameFromRowCol( int row, int col )
    {
        return $"{Alphabet[col]}{row + 1}";
    }

    /// <summary>
    ///   Called when the input widget (representing the data in a particular cell) is modified.
    /// </summary>
    /// <param name="newInput"> The new value to put at row/col. </param>
    /// <param name="row"> The matrix row identifier. </param>
    /// <param name="col"> The matrix column identifier. </param>
    private async void HandleUpdateCellInSpreadsheet( string newInput, int row, int col )
    {
        try
        {
            InputWidgetBackingStore = $"{row},{col}";

            string cellName = CellNameFromRowCol(row, col);

            // Gets a List of dependents when updating the Contents of any cell.
            IList<string> cellsToUpdate = spreadsheet.SetContentsOfCell(cellName, newInput);

            foreach ( string cell in cellsToUpdate)
            {
                object cellValue = spreadsheet.GetCellValue(cell);

                // Use these to update the corresponding cells.
                int cellRow;
                int cellCol;
                ConvertCellNameToRowCol(cell, out cellRow, out cellCol);

                if (cellValue is FormulaError cellError)
                {
                    CellsBackingStore[cellRow, cellCol] = cellError.Reason;
                }
                else
                {
                    CellsBackingStore[cellRow, cellCol] = cellValue.ToString()!;
                }
            }
        }
        catch
        {
            // a way to communicate to the user that something went wrong.
            await JS.InvokeVoidAsync( "alert", "Something went wrong." );
        }
    }

    /// <summary>
    ///   <para>
    ///     Using a Web Input ask the user for a file and then process the
    ///     data in the file.
    ///   </para>
    ///   <remarks>
    ///     Unfortunately, this happens after the file is chosen, but we will live with that.
    ///   </remarks>
    /// </summary>
    /// <param name="args"> Information about the file that has been selected. </param>
    private async void HandleLoadFile( EventArgs args )
    {
        try
        {
            // See if spreadsheet has Changed
            if (spreadsheet.Changed)
            {
                bool success = await JS.InvokeAsync<bool>("confirm", "Do you want to override current spreadsheet?");
                if (!success)
                {
                    return;    // user canceled the action.
                }
            }

            string fileContent = string.Empty;

            InputFileChangeEventArgs eventArgs = args as InputFileChangeEventArgs ?? throw new Exception("that didn't work");
            if ( eventArgs.FileCount == 1 )
            {
                var file = eventArgs.File;
                if ( file is null )
                {
                    return;
                }

                using var stream = file.OpenReadStream();
                using var reader = new System.IO.StreamReader(stream);
                fileContent = await reader.ReadToEndAsync();

                await JS.InvokeVoidAsync( "alert", fileContent );
                CellsBackingStore = new string[99, 26]; // clears backingstore before loading new spreadsheet.

                // Using FileContents, update Spreadsheet
                spreadsheet.InstantiateFromJSON(fileContent);
                ISet<string> cells = spreadsheet.GetNamesOfAllNonemptyCells();
                int row;
                int col;

                // Copy Data from spreadsheet cells into the GUI
                foreach( string cell in cells)
                {
                    ConvertCellNameToRowCol(cell, out row, out col);
                    HandleUpdateCellInSpreadsheet(spreadsheet.GetCellStringForm(cell), row, col);
                }

                FocusMainInput(0, 0);
                spreadsheet.Changed = false;
                StateHasChanged();
            }
        }
        catch ( Exception e )
        {
            Debug.WriteLine( "something went wrong with loading the file..." + e );
        }
    }

    /// <summary>
    ///   Switch between the file save view or main view.
    /// </summary>
    /// <param name="show"> if true, show the file save view. </param>
    private void ShowHideSaveGUI(bool show)
    {
        SaveGUIView = show;
        StateHasChanged();
    }

    /// <summary>
    ///   Switch between the file save view or main view.
    /// </summary>
    /// <param name="show"> if true, show the file save view. </param>
    private void ShowHideResizeGUI(bool show)
    {
        ResizeGUI = show;
        StateHasChanged();
    }

    /// <summary>
    ///   Switch between the file save view or main view.
    /// </summary>
    /// <param name="show"> if true, show the file save view. </param>
    private void ShowHideRenameGUI(bool show)
    {
        RenameGUI = show;
        StateHasChanged();
    }

    /// <summary>
    ///   Call the JavaScript necessary to download the data via the Browser's Download
    ///   Folder.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleSaveFile(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        // <remarks> this null check is done because Visual Studio doesn't understand
        // the Blazor life cycle and cannot assure of non-null. </remarks>
        if ( JSModule is not null )
        {
            var success = await JSModule.InvokeAsync<bool>("saveToFile", SaveFileName, spreadsheet.GetJSON());
            if (success)
            {
                ShowHideSaveGUI( false );
                StateHasChanged();
            }
        }
    }

    /// <summary>
    ///   Clear the spreadsheet if not modified.
    /// </summary>
    /// <param name="e"> Ignored. </param>
    private async void HandleClear(Microsoft.AspNetCore.Components.Web.MouseEventArgs e)
    {
        if ( JSModule is not null )
        {
            bool success = await JS.InvokeAsync<bool>( "confirm", "Clear the sheet?" );
        }

        spreadsheet.InstantiateFromJSON("{}");
        CellsBackingStore = new string[99, 26];
        InputWidgetBackingStore = string.Empty;
        spreadsheet.Name = "default";
        StateHasChanged();
    }

    /// <summary>
    /// Renames the spreadsheet. Also modifies the SaveFileName
    /// to appropriately match the name of the spreadsheet when
    /// saving.
    /// </summary>
    private void RenameSpreadsheet()
    {
        spreadsheet.Name = tempName;

        // Rename placeholder to match the Spreadsheet name
        if (spreadsheet.Name == "default")
        {
            SaveFileName = "Spreadsheet.sprd";
        }
        else
        {
            SaveFileName = $"{spreadsheet.Name}.sprd";
        }

        ShowHideRenameGUI(false);
    }
}
