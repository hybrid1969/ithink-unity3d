/*!
 *  @mainpage iThink 0.1 documentation page
 *
 *  @section intro_sec Introduction
 *
 *  \subsection intro1 What is iThink?
 *  iThink is an AI library for Unity3D, implementing a STRIPS-like classical planning system.
 * 
 *  \section install Installation instructions
 *
 *  \section usage Usage
 *  \subsection usage1 Using the iThinkBrain
 * 
 *  @todo iThinkBrain usage info
 * 
 *  \subsection usage2 Using the iThinkPlanner
 * 
 *  @todo iThinkPlanner usage info
 * 
 *  \section license License
 *  GNU Lesser General Public License v3.0
 * 
 *  \section credits Credits
 *  Code written by Vassilis Anasstassiou and Panagiotis Diamantopoulos
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

/** @class iThinkBrain
    @brief iThinkBrain contains all the needed components and sample interface to implement an iThink agent.

    The iThink brain contains all planning-related systems and information.
    It provides the agent with:
    - a sensory system to populate the list of know GameObjects
    - a list of possible actions
    - its local knowledge (collection of facts (iThinkFact))
 */
public class iThinkBrain
{
    protected List<iThinkFact> factList;        /// A list of all the Facts known to the agent.

    public iThinkState startState;              /// The first state before planning begins.
    public iThinkState curState;                /// The current state.
    public iThinkState goalState;               /// The goal state.

    public iThinkActionManager ActionManager;   /// The list of available actions.
    public iThinkSensorySystem sensorySystem;   /// The system which updates the list of known \a objects.
    public iThinkPlanner planner;               /// The iThink planner.

    public float lastUpdate;                    /// The last Time.time a check has been performed.

    /// Getter for the list of objects known to the agent via his \a {sensory system}.
    public List<GameObject> getKnownObjects() { return sensorySystem.getKnownObjects(); }

    /// Getter for the list of facts known to the agent
    public List<iThinkFact> getKnownFacts() { return factList; }

    /// iThinkBrain constructor
    public iThinkBrain()
    {
        lastUpdate = Time.time;

        planner = new BestFS();
        factList = new List<iThinkFact>();
        sensorySystem = new iThinkSensorySystem();
    }
}
