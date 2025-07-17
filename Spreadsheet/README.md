```
Author:     Teagan Smith
Partner:    None
Course:     CS 3500
GitHub ID:  tweagan11
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-Tweagan11 
Date:       25-09-24 6:20 PM
Project:    Spreadsheet Class
Copyright:  CS 3500 and Teagan - This work may not be copied for use in Academic Coursework.
```

This Spreadsheet class was created to start to implement more functionality to our Spreadsheet Project by utilizing
aspects of our MVC. This focused on the model aspect of our MVC, and we will develop the GUI later to add more functionality.
This Spreadsheet class can create Cells that have a name and a value stored inside of them. You can access the cells through this
Spreadsheet. They update their dependencies themselves due to our Dependency Graph class, which we implemented earlier. This catches
both Invalid Cell names as well as Circular reasoning with cells.

# Comments to Evaluators:

No notes, just that when I first developed my tests there were some things I didn't fully understand, so I did tweak them later on.

# Assignment Specific Topics

This Spreadsheet maintains all previously defined functionality, My only question is why is SetCellContents expected to return a list,
but when we call GetCellsToRecalculate, it gives us a linked list? Could that be lossy if we cast it to a list?

# Consulted Peers:

- Chase Hammond
- Hale (Didn't catch last Name)
- Dennis Panchecka

# References:

- Notion AI: Consulted about specific minute questions (like comparing lists) and references. DID NOT TAKE ANY CODE FROM NOTION AI.
- https://www.w3schools.com/cs/cs_exceptions.php
- https://www.w3schools.com/cs/index.php
- https://learn.microsoft.com/en-us/troubleshoot/developer/visualstudio/csharp/language-compilers/read-write-text-file
 
Estimated Time: 7hrs (V1.0 of Spreadsheet Class)

Time Spent Learning: .5 hr
Time Spent Creating Tests: .5 hr
Time Spent Creating: 4 hrs
Time Spent Debugging:  1 hr

Actual Time Spent: 6 hrs

Estimated Time: 5 hrs (V2.0 of Spreadsheet Class)

Time Spent Learning:  .5hr
Time Spent Creating Tests:  1hr
Time Spent Creating:  4hrs
Time Spent Debugging:   1hr

Actual Time Spent:  6.5hrs