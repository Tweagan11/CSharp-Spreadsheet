// <copyright file="DependencyGraphTests.cs" company="UofU-CS3500">
// Copyright (c) UofU-CS3500. All rights reserved.
// </copyright>
// <authors> Teagan Smith </authors>
// <date> 09.13.24 </date>
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
//    This File Contains our DependencyGraphTests, checking functionality
//    of our DependencyGraph Library as well as effeciency.
// </summary>

namespace CS3500.DevelopmentTests;
using CS3500.DependencyGraph;
using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// This is a test class for DependencyGraphTest and is intended
/// to contain all DependencyGraphTest Unit Tests
/// </summary>
[TestClass]
public class DependencyGraphTests
{
    /// <summary>
    /// This Function ensures when initialized that the dependency graphy is empty and size is equal to 0
    /// </summary>
    [TestMethod]
    public void DependencyGraph_NewGraphSize0_Valid()
    {
        DependencyGraph dgTest = new();
        Assert.IsTrue(dgTest.Size == 0);
    }

    /// <summary>
    /// This function will add a dependency to the graph and check the size,
    /// ensuring that it equals 1
    /// </summary>
    [TestMethod]
    public void DependencyGraph_AddCheckSize_Valid()
    {
        DependencyGraph dgTest = new();
        dgTest.AddDependency("A1", "B1");
        Assert.IsTrue(dgTest.Size == 1);
    }

    /// <summary>
    /// This function will add multiple equivalent dependencies and checks the size
    /// to makes sure that it remains the same.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_AddCheckSizeMultiple_Valid()
    {
        DependencyGraph dgTest = new();
        dgTest.AddDependency("A1", "B1");
        dgTest.AddDependency("A1", "B1");
        dgTest.AddDependency("A1", "B1");
        Assert.IsTrue(dgTest.Size == 1);
    }

    /// <summary>
    /// This function tests the add and the remove functionality of our Graph,
    /// and then checks the size, making sure it is clear.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_RemoveToZeroCheckSize_Valid()
    {
        DependencyGraph dgTest = new();
        dgTest.AddDependency("A1", "B1");
        dgTest.RemoveDependency("A1", "B1");
        Assert.IsTrue(dgTest.Size == 0);
    }

    /// <summary>
    /// This function tests the add and the remove functionality of our Graph,
    /// but removes past 0 to make sure that the size isn't negative and errors aren't thrown.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_RemovePastZeroCheckSize_Valid()
    {
        DependencyGraph dgTest = new();
        dgTest.AddDependency("A1", "B1");
        dgTest.RemoveDependency("A1", "B1");
        dgTest.RemoveDependency("A1", "B1");
        dgTest.RemoveDependency("A1", "B1");
        Assert.IsTrue(dgTest.Size == 0);
    }

    /// <summary>
    /// This function tests the add and the remove functionality of our Graph,
    /// but this removes no previous existing dependencies, checking that nothing happens.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_RemoveNothingCheckSize_Valid()
    {
        DependencyGraph dgTest = new DependencyGraph();
        dgTest.RemoveDependency("A1", "B1");
        Assert.IsTrue(dgTest.Size == 0);
    }

    /// <summary>
    /// This tests the ReplaceDependents function, and ensures that the size stays the same
    /// when replacing an equivalent amount of dependents.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_ReplaceDependentsOneSize_Valid()
    {
        DependencyGraph dgTest = new();
        dgTest.AddDependency("A1", "B1");

        // Create another Dependency Graph with 1 dependent
        DependencyGraph dgReplaceDependent = new DependencyGraph();
        dgReplaceDependent.AddDependency("B1", "C1");
        dgTest.ReplaceDependents("A1", dgReplaceDependent.GetDependents("B1"));
        Assert.IsTrue(dgTest.Size == 1);
    }

    /// <summary>
    /// Like the test above, this test should replace the dependents of one variable
    /// with the dependents of another, so we should see an increase of size to match 
    /// the original size of the second Dependency Graph.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_ReplaceDependentsMultipleSize_Valid()
    {
        DependencyGraph dgTest = new DependencyGraph();
        dgTest.AddDependency("A1", "B1");

        // Create another Dependency Graph with 1 dependent
        DependencyGraph dgReplaceDependents = new DependencyGraph();
        dgReplaceDependents.AddDependency("B1", "C1");
        dgReplaceDependents.AddDependency("B1", "D1");
        dgReplaceDependents.AddDependency("B1", "E1");

        dgTest.ReplaceDependents("A1", dgReplaceDependents.GetDependents("B1"));
        Assert.IsTrue(dgTest.Size == 3);
    }

    /// <summary>
    /// This function looks at the boolean function "HasDependents" and makes sure
    /// when a dependent is added, then it returns true.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependentsTrue_Valid()
    {
        DependencyGraph dgTest = new DependencyGraph();
        dgTest.AddDependency("A1", "B1");
        Assert.IsTrue(dgTest.HasDependents("A1"));
    }

    /// <summary>
    /// This function looks at the boolean function "HasDependents" and makes sure
    /// when a dependent is added, then it returns false when it may be in the list but doesn't
    /// have any dependents.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependentsFalse_Valid()
    {
        DependencyGraph dgTest = new DependencyGraph();
        dgTest.AddDependency("A1", "B1");
        Assert.IsFalse(dgTest.HasDependents("B1"));
    }

    /// <summary>
    /// This function looks at the boolean function "HasDependees" and makes sure
    /// when a dependee is added, then it returns true.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependeesTrue_Valid()
    {
        DependencyGraph dgTest = new DependencyGraph();
        dgTest.AddDependency("A1", "B1");
        Assert.IsTrue(dgTest.HasDependees("B1"));
    }

    /// <summary>
    /// This function looks at the boolean function "HasDependees" and makes sure
    /// when a dependee is added, then it returns false when it may be in the list but doesn't
    /// have any dependees.
    /// </summary>
    [TestMethod]
    public void DependencyGraph_HasDependeesFalse_Valid()
    {
        DependencyGraph dgtest = new DependencyGraph();
        dgtest.AddDependency("A1", "B1");
        Assert.IsFalse(dgtest.HasDependees("A1"));
    }

    /// <summary>
    /// <para>This test looks at the GetDependents functionality, passing in a couple
    /// dependencies and making sure that they are represented correctly when GetDependents is called.
    /// </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_GetDependentsMultiple_Valid()
    {
        DependencyGraph dgtest = new DependencyGraph();
        dgtest.AddDependency("A1", "B1");
        dgtest.AddDependency("A1", "C1");
        dgtest.AddDependency("A1", "D1");

        // Create Hashset of dependents to compare

        Assert.IsTrue(dgtest.GetDependents("A1").Contains("B1"));
        Assert.IsTrue(dgtest.GetDependents("A1").Contains("C1"));
        Assert.IsTrue(dgtest.GetDependents("A1").Contains("D1"));
    }

    /// <summary>
    /// <para>This test looks at the GetDependents functionality, passing in a couple
    /// dependencies and making sure that they are represented correctly when GetDependents is called.
    /// </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_GetDependeesMultiple_Valid()
    {
        DependencyGraph dgtest = new DependencyGraph();
        dgtest.AddDependency("B1", "A1");
        dgtest.AddDependency("C1", "A1");
        dgtest.AddDependency("D1", "A1");

        Assert.IsTrue(dgtest.GetDependees("A1").Contains("B1"));
        Assert.IsTrue(dgtest.GetDependees("A1").Contains("C1"));
        Assert.IsTrue(dgtest.GetDependees("A1").Contains("D1"));
    }

    /// <summary>
    /// <para>Tests the Replace Dependees functions and tests to make sure that dependees replaced are correct.
    /// We do this by making a Graph where A1 has multiple Dependees, and then making E1 a Parent or Dependent of A1.
    /// We want to shorten the graph and have E1 inherit all of A1's dependees.
    /// </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_ReplaceDependeesMultiple_Valid()
    {
        DependencyGraph dgDependees = new DependencyGraph();
        // Add Dependees to the Dependent, A1
        dgDependees.AddDependency("B1", "A1");
        dgDependees.AddDependency("C1", "A1");
        dgDependees.AddDependency("D1", "A1");

        // Add A1 as dependee to Dependent E1
        dgDependees.AddDependency("A1", "E1");


        // Replace all of the dependees of E1 with the dependees of A1, essentially shortening the graph
        dgDependees.ReplaceDependees("E1", dgDependees.GetDependees("A1"));
        dgDependees.ReplaceDependees("A1", []);

        Assert.IsFalse(dgDependees.GetDependees("A1").Contains("B1"));
        Assert.IsFalse(dgDependees.GetDependees("A1").Contains("C1"));
        Assert.IsFalse(dgDependees.GetDependees("A1").Contains("D1"));

        Assert.IsTrue(dgDependees.GetDependees("E1").Contains("B1"));
        Assert.IsTrue(dgDependees.GetDependees("E1").Contains("C1"));
        Assert.IsTrue(dgDependees.GetDependees("E1").Contains("D1"));

    }

    /// <summary>
    /// <para>This Test will replace a central node that has multiple dependents and multiple dependees.
    /// We will use 2 Dependents and 2 Dependees of one central node. We will then use a new node to replace the central node.
    /// </para>
    /// </summary>
    [TestMethod]
    public void DependencyGraph_ReplaceCentralNode_Valid()
    {
        DependencyGraph centralTest = new DependencyGraph();
        centralTest.AddDependency("center", "dependent1");
        centralTest.AddDependency("center", "dependent2");
        centralTest.AddDependency("dependee1", "center");
        centralTest.AddDependency("dependee2", "center");

        // Replace Dependees and Dependents of newCenter to match old center
        centralTest.ReplaceDependees("newCenter", centralTest.GetDependees("center"));
        centralTest.ReplaceDependents("newCenter", centralTest.GetDependents("center"));

        // Replace Dependees and Dependents with an empty list to remove entirely
        centralTest.ReplaceDependees("center", []);
        centralTest.ReplaceDependents("center", []);

        // Checks to make sure newCenter has a dependent and it matches old center's value
        Assert.IsTrue(centralTest.GetDependents("newCenter").Contains("dependent1"));
        Assert.IsTrue(centralTest.GetDependents("newCenter").Contains("dependent2"));

        Assert.IsTrue(centralTest.GetDependees("newCenter").Contains("dependee1"));
        Assert.IsTrue(centralTest.GetDependees("newCenter").Contains("dependee2"));

        // Check old center if it has any dependents or dependencies

        Assert.IsFalse(centralTest.HasDependents("center"));
        Assert.IsFalse(centralTest.HasDependees("center"));

    }
}

// Stress test given from the Professor
[TestClass]
public class DependencyGraphExampleStressTests
{
    /// <summary>
    /// This test checks to make sure our data structures aren't taking a long time to compile and 
    /// that they are effecient when computing a lot of cells. This gives us a time of 2000 ms, or 2 seconds.
    /// </summary>
    [TestMethod]
    [Timeout(2000)]
    public void StressTest()
    {
        DependencyGraph dg = new();
        // A bunch of strings to use
        const int SIZE = 200;
        string[] letters = new string[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            letters[i] = string.Empty + ((char)('a' + i));
        }
        // The correct answers
        HashSet<string>[] dependents = new HashSet<string>[SIZE];
        HashSet<string>[] dependees = new HashSet<string>[SIZE];
        for (int i = 0; i < SIZE; i++)
        {
            dependents[i] = [];
            dependees[i] = [];
        }
        // Add a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j++)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }
        // Remove a bunch of dependencies
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 4; j < SIZE; j += 4)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }
        // Add some back
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = i + 1; j < SIZE; j += 2)
            {
                dg.AddDependency(letters[i], letters[j]);
                dependents[i].Add(letters[j]);
                dependees[j].Add(letters[i]);
            }
        }
        // Remove some more
        for (int i = 0; i < SIZE; i += 2)
        {
            for (int j = i + 3; j < SIZE; j += 3)
            {
                dg.RemoveDependency(letters[i], letters[j]);
                dependents[i].Remove(letters[j]);
                dependees[j].Remove(letters[i]);
            }
        }
        // Make sure everything is right
        for (int i = 0; i < SIZE; i++)
        {
            Assert.IsTrue(dependents[i].SetEquals(new
            HashSet<string>(dg.GetDependents(letters[i]))));
            Assert.IsTrue(dependees[i].SetEquals(new
            HashSet<string>(dg.GetDependees(letters[i]))));
        }
    }
}
