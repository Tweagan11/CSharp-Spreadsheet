```
Author:     Teagan Smith
Partner:    Chase Hammond
Start Date: 02-Sept-2024
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  tweagan11, 
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheetpair-bros-before-nodes
Commit Date: 22-Oct-2024
Solution:   Spreadsheet
Copyright:  CS 3500 and Teagan Smith, Chase Hammond - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet functionality

This Spreadsheet is currently functional as a library where it can create a Formula Class, check the correct syntax, and hold certain data.
that we will be using later to create a full fledged spreadsheet. 

It has been updated to also contain a DependencyGraph that can check dependees and dependents of variables, and create dependencies between them as well.

As of 09/19/24, the Formula Class has been updated to Evaluate given formulas and check equivalence between two formula objects.

As of 09/26/24, The Spreadsheet Class has been added to the project, which gives us a model of a Spreadsheet and Cell structure.
This can access cells stored in the spreadsheet and currently just return the contents stored inside the specific cell (Strings, Doubles, or Formulas).

Updating it on 10/20/24, the spreadsheet class can now take in any string input for a cell, and store it into a spreadsheet class with the correct contents and value.
This allows us to help the user control our spreadsheet.

As of 10/29/24, Chase Hammond and I worked together to bring out the full functionality, implementing a GUI that any user can interact with. It can create a working spreadsheet,
hold formulas, values and strings, as well as options to resize and rename the given spreadsheet.

# Good Software Practices

Some good applications of GSP is definitely DRY, using multiple abstractions in my code to not repeat myself. Such as my ChangeCell and AddCell function,
instead of doing that each time for each different kind of cell, I just abstracted it and made a helper function.

Another is using Well named, commmented and short methods to not clog up my entire code. This helps keeping things streamlined, and if I ever make
a call not to abstract it, I will leave a comment to help explain for me and others.

I reused old tests, but had to reconfigure some of them to better fit the functionality of the updated Spreadsheet class.

All in all, I do think there is a little bit of room to improve, but that will come with time and practice as I try to keep these habits up!

# Time Management Skills:

I do think I am slowing getting better at being able to predict how long an assignment will take me. Most of that comes down to how well I understand
the assignment, and the better I understand it the better I guess. Adding an Additional partner definitely took time getting used to, but overall I think it helped
improve our time and make us more efficient.

# Time Expenditures:

    1. Assignment One:   Predicted Hours:      10     Actual Hours:   8
    2. Assignment Two:   Predicted Hours:       8     Actual Hours:   12
    3. Assignment Three: Predicted Hours:       8     Actual Hours:   9
    4. Assignment Four:  Predicted Hours:       6     Actual Hours:   5
    5. Assignment Five:  Predicted Hours:       7     Actual Hours:   6
    6. Assignment Six:   Predicted Hours:       5     Actual Hours:   6.5
    7. Assignment Seven: Predicted Hours:      10     Actual Hours:   8.5