// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta
// Version 1.3 - H. James de St. Germain Fall 2024
// Version 1.4 - Teagan Smith 
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)

// <copyright file="DependencyGraph.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <summary>
// Author:    Teagan Smith
// Partner:   None
// Date:      09/13/24
// Course:    CS 3500, University of Utah, School of Computing
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
//    This File contains our DependencyGraph Library, where it can 
//    create and Remove dependencies between variables, replace them, and check status of them as well.
//    When Constructed, it creates 2 dictionaries that has strings as keys and hash sets as values to hold
//    all dependencies both ways.
// </summary>
using System.Diagnostics;
using System.Xml.Linq;

namespace CS3500.DependencyGraph;
/// <summary>
/// <para>
/// (s1,t1) is an ordered pair of strings, meaning t1 depends on s1.
/// (in other words: s1 must be evaluated before t1.)
/// </para>
/// <para>
/// A DependencyGraph can be modeled as a set of ordered pairs of strings.
/// Two ordered pairs (s1,t1) and (s2,t2) are considered equal if and only
/// if s1 equals s2 and t1 equals t2.
/// </para>
/// <remarks>
/// Recall that sets never contain duplicates.
/// If an attempt is made to add an element to a set, and the element is already
/// in the set, the set remains unchanged.
/// </remarks>
/// <para>
/// Given a DependencyGraph DG:
/// </para>
/// <list type="number">
/// <item>
/// If s is a string, the set of all strings t such that (s,t) is in DG is called dependent(s).
/// (The set of things that depend on s.)
/// </item>
/// <item>
/// If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
/// (The set of things that s depends on.)
/// </item>
/// </list>
/// <para>
/// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d","d")}.
/// </para>
/// <code>
/// dependent("a") = {"b", "c"}
/// dependent("b") = {"d"}
/// dependent("c") = {}
/// dependent("d") = {"d"}
/// dependees("a") = {}
/// dependees("b") = {"a"}
/// dependees("c") = {"a"}
/// dependees("d") = {"b", "d"}
/// </code>
/// </summary>
public class DependencyGraph
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
    /// The initial DependencyGraph is empty.
    /// </summary>
    public DependencyGraph()
    {
        _size = 0;
    }
    /// <summary>
    /// The number of ordered pairs in the DependencyGraph.
    /// </summary>
    public int Size
    {
        get { return _size; }
    }
    /// <summary>
    /// Reports whether the given node has dependents (i.e., other nodes depend on it).
    /// </summary>
    /// <param name="nodeName"> The name of the node.</param>
    /// <returns> true if the node has dependents. </returns>
    public bool HasDependents(string nodeName)
    {
        return variableDependents.ContainsKey(nodeName);
    }
    /// <summary>
    /// Reports whether the given node has dependees (i.e., depends on one or more other nodes).
    /// </summary>
    /// <returns> true if the node has dependees.</returns>
    /// <param name="nodeName">The name of the node.</param>
    public bool HasDependees(string nodeName)
    {
        return variableDependees.ContainsKey(nodeName);
    }

    /// <summary>
    /// <para>
    /// Returns the dependents of the node with the given name.
    /// </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The variableDependents of nodeName. </returns>
    public IEnumerable<string> GetDependents(string nodeName)
    {
        if (HasDependents(nodeName))
        {
            return variableDependents[nodeName];
        }
        return [];
    }

    /// <summary>
    /// <para>
    /// Returns the dependees of the node with the given name.
    /// </para>
    /// </summary>
    /// <param name="nodeName"> The node we are looking at.</param>
    /// <returns> The dependees of nodeName. </returns>
    public IEnumerable<string> GetDependees(string nodeName)
    {
        if (HasDependees(nodeName))
        {
            return variableDependees[nodeName];
        
        }
        return [];
    }
    /// <summary>
    /// <para>
    /// Adds the ordered pair (dependee, dependent), if it doesn't already exist(otherwise nothing happens).
    /// </para>
    /// <para>
    /// This can be thought of as: dependee must be evaluated before dependent.
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first.</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until after the other node has been. </param>
    public void AddDependency(string dependee, string dependent)
    {

        if (!IsExistingDependency(dependee, dependent))
        {
            // First, add a Dependent Key to the Dictionary to form a set of dependees
            // Check to see if value already exists
            if (!HasDependees(dependent))
            {
                variableDependees[dependent] = [];
            }
            // Add Dependent as a value to the Dependee Key
            variableDependees[dependent].Add(dependee);

            // Second, add Dependee as a key to the Dependent's Dictionary to form a set of dependents
            if (!HasDependents(dependee))
            {
                variableDependents[dependee] = [];
            }
            // Add Dependeee as a value to the Dependent Key
            variableDependents[dependee].Add(dependent);

            // Increase size each time add is called
            _size++;
        }
    }
    /// <summary>
    /// <para>
    /// Removes the ordered pair (dependee, dependent), if it exists (otherwise nothing happens).
    /// </para>
    /// </summary>
    /// <param name="dependee"> The name of the node that must be evaluated first.</param>
    /// <param name="dependent"> The name of the node that cannot be evaluated until the other node has been. </param>
    public void RemoveDependency(string dependee, string dependent)
    {
        if (IsExistingDependency(dependee, dependent))
        {
            variableDependents[dependee].Remove(dependent);
            if (variableDependents[dependee].Count == 0)
            {
                variableDependents.Remove(dependee);
            }

            variableDependees[dependent].Remove(dependee);
            if (variableDependees[dependent].Count == 0)
            {
                variableDependees.Remove(dependent);
            }
            _size--;
        }
    }

    /// <summary>
    /// Removes all existing ordered pairs of the form (nodeName, *). Then, for each
    /// t in newDependents, adds the ordered pair (nodeName, t).
    /// </summary>
    /// <param name="nodeName"> The name of the node who's variableDependents are being replaced. </param>
    /// <param name="newDependents"> The new variableDependents for nodeName. </param>
    public void ReplaceDependents(string nodeName, IEnumerable<string> newDependents)
    {
        // Remove all dependents from given node name
        if (HasDependents(nodeName))
        {
            foreach (string dependent in variableDependents[nodeName])
            {
                RemoveDependency(nodeName, dependent);
            }
        }


        // Add in all new dependents from the given list
        foreach (string newDependent in newDependents)
        {
            AddDependency(nodeName, newDependent);
        }
    }
    /// <summary>
    /// <para>
    /// Removes all existing ordered pairs of the form (*, nodeName). Then,for each
    /// t in newDependees, adds the ordered pair (t, nodeName).
    /// </para>
    /// </summary>
    /// <param name="nodeName"> The name of the node who's variableDependees are being replaced. </param>
    /// <param name="newDependees"> The new variableDependees for nodeName. Could be empty.</param>
    public void ReplaceDependees(string nodeName, IEnumerable<string> newDependees)
    {
        // Remove all dependees from the given nodeName
        if (HasDependees(nodeName))
        {
            foreach (string dependee in variableDependees[nodeName])
            {
                RemoveDependency(dependee, nodeName);
            }
        }
        

        // Add in all new dependees from the given list to the nodeName
        foreach (string newDependee in newDependees)
        {
            AddDependency(newDependee, nodeName);
        }
    }

    /// <summary>
    /// This Helper function checks if this dependency already exists. This looks at a dependee
    /// and a dependent and checks both dictionaries if they both exists as a value respectively.
    /// </summary>
    /// <param name="dependee">Name of node that is a child or dependee of the other node.</param>
    /// <param name="dependent">Name of parent node or dependent of the dependee.</param>
    /// <returns></returns>
    private bool IsExistingDependency(string dependee, string dependent)
    {
        bool DependeeExists = false;
        bool DependentExists = false;
        // Check first if there is a dependee -> dependent relationship stored.
        if (HasDependents(dependee))
        {
            DependentExists = variableDependents[dependee].Contains(dependent);
        }
        
        // Check then if there is a dependent -> dependee relationship stored.
        if (HasDependees(dependent))
        {
            DependeeExists = variableDependees[dependent].Contains(dependee);
        }

        return DependeeExists && DependentExists;
    }

    /// <summary>
    /// Keeps track of size of the dependency graph, or order of numbered pairs.
    /// </summary>
    private int _size;

    /// <summary>
    /// <para>
    /// This privately stores all dependencies into a private hash set our class can access.
    /// </para>
    /// </summary>
    private readonly Dictionary<string, HashSet<string>> variableDependents = [];

    /// <summary>
    /// <para>
    /// This privately stores all dependencies into a private hash set our class can access.
    /// </para>
    /// </summary>
    private readonly Dictionary<string, HashSet<string>> variableDependees = [];
}
