```
Author:     Teagan Smith
Partner:    Chase Hammond
Start Date: 29-Oct-2024
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  tweagan11, chammond123 
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheetpair-bros-before-nodes
Commit Date: 29-Oct-2024
Solution:   Spreadsheet GUI
Copyright:  CS 3500 and Teagan Smith, Chase Hammond - This work may not be copied for use in Academic Coursework.
```

# Overview of the Spreadsheet GUI functionality:

This GUI of the spreadsheet model.  It allows the user to input cell data to the spreadsheet through a table and view real time changes in formula cells.
The contents and value of a cell are displayed above the table. There are options to save, load and clear the spreadsheet as well as naming and resizing in the spreadsheet 
options menu.

# Comments to Evaluators:

Cells must be clicked twice to enter the contents editor. Also, after talking with a few other students, they said implementing the GetStringForm API to our spreadsheet would be ok
with the Professor as he cleared them in implementing that extended API. This was merely an extension of our already existing API, thus not compromising the current API of our Spreadsheet
class.

# Design Decisions:

We decided to implement the following design decisions:
	- To encapsulate any spreadsheet options into a drop down menu, including loading, saving, clearing, naming and resizing. This makes the GUI look cleaner.
	- To add the ability to edit a cells contents by clicking into a cell directly.
	- To add the ability to change the name of the spreadsheet and have that name become the default file save name. A spreadsheet's name is also loaded in the load method.

# Pair Programming:

All code was completed with pair programming.  There is a single branch called Messing-with-layout created by Chase to mess with the layout while not pair programming.
Minimal work was done in this branch, the changes kept were small tweaks to the overall layout of the spreadsheet so code merging was no issue.

Initial branch commit: b812ea9e2753f6062303ae640e495eb4664c6a40
Branch merge: 0deb1e86e85d66072bceac9de2f5694e2da0672d

# Consulted Peers:
Chase Hammond
Teagan Smith

# References: 

- https://www.w3schools.com/html/default.asp
- https://getbootstrap.com/docs/5.3/examples/dropdowns/

# Time Expenditures:

Times reflect the input of both partners as all time was spent pair programming.

Expected time: 20h total (10 hours of both partners either driving or navigating)
Actual time: 17h total (8.5 Hours of partner work)

Time Spent Learning: .5 hr x 2
Time Spent Manually Testing: .5h x 2
Time Spent Creating: 4.5 hrs x 2
Time Spent Debugging:  2.5 hr x 2

# Time Estimation Skills:

We believe our time estimations skills are improving with each project.  This project felt a little different as we had to estimate the time with the added
difficulty of factoring in pair programming.  We both agree that the project seemed to go faster than expected with pair programming.

# Partnership Success:

We believe this partnership to have been a success.  Both partners were driven to complete the project on time and at A quality.  Both partners made time available to 
complete the project.  At no time was the programming or project held back by any lack of commitment or effort from either partner.  Due to the diligence of both partners,
the development of the project was able to run smoothly.  Partners alternated driving and navigating during programming so syntax mistakes or logical errors could be noticed
and corrected quickly. Another aspect of Pair Programming that worked well was being able to use the other as a resource to look up syntax, how to implement other API, and other
questions the driver may have had were quickly answered instead of splitting the time between research and developing.

One area we believe could be improved in our teamwork is swapping roles more often. The load of driving and navigating was split fairly evenly in the end; however we didn't
do a very good job of swapping frequently.  There would be times where one partner would be at the keyboard for a longer period of time, so to compensate the other partner would 
have to match it.  If we had swapped at more regular intervals this could have been avoided.
