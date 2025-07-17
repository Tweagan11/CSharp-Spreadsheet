```
Author:     Teagan Smith
Partner:    None
Course:     CS 3500
GitHub ID:  tweagan11
Repo:       https://github.com/uofu-cs3500-20-fall2024/spreadsheet-Tweagan11 
Date:       13-09-24 10:21 PM
Project:    Dependency Graph Class
Copyright:  CS 3500 and Teagan - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

I developed tests beforehand but didn't fully understand what the implementation looked like, so I altered them to keep the same concept but actually fit the code correctly.

# Assignment Specific Topics
This Dependency Graph connects and visualizes dependencies between variables for our future Spreadsheet. The inner workings of this class utilizes two dictionaries of a string key and a hash set value, allowing quick and efficient retrieval of our relationships between dependees and dependents.

A Dependee is defined as follows: If s is a string, the set of all strings t such that (t,s) is in DG is called dependee(s). (The set of things that s depends on.)

A Dependent is definde as follows: If s is a string, the set of all strings t such that (s,t) is in DG is called dependent(s). (The set of things that depend on s.)

Our Public Methods of usage are as follows:
- AddDependency(dependee, dependent)
- RemoveDependency(dependee, dependent)
- HasDependents(nodeName)
- HasDependees(nodeName)
- GetDependents(nodeName)
- GetDependees(nodeName)
- ReplaceDependees(nodeName, [newDependees])
- ReplaceDependents(nodeName, [newDependents])

# Consulted Peers:

1. Dennis Pancheka
1. Chase Hammond

# References:

    1. ChatGPT - chatgpt.com - Only consulted about implementation of C#, no code was taken.
    2. Microsoft Visual Studio Dictionary API usage - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2?view=net-8.0
    3. Microsoft HashSet Class and Methods - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1?view=net-8.0

Estimated Time: 6 hrs

Time Spent Learning:.5
Time Spent Creating: 6 hrs
Time Spent Debugging: 1 hr

Actual Time Spent: 7.5 hrs