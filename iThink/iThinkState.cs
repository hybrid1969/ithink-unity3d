using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0660, 0661

/*! @class iThinkState
 * @brief iThinkState represents a state-node of the planning process.

  An iThinkState object is used in the planning process to describe a node of the planning graph (search space).
  It contains: 
  - its name
  - a list of \a facts (literals)
  - the evaluated \a cost of the state-node during the search process
*/

namespace iThinkLibrary{
	namespace KnowledgeRepresentation{
	
		public class iThinkState
		{
		    protected List<iThinkFact> facts;   /// The list of facts which constitute the state
		    protected int heuristicCost;        /// The total search cost up to this node, if needed by the search algorithm
		    protected string name;              /// The name of
		
		    public iThinkState( string Name, List<iThinkFact> factList )
		    {
		        facts = new List<iThinkFact>( factList );
		        heuristicCost = 0;
		        name = Name;
		    }
		
		    public iThinkState( iThinkState state )
		    {
		        facts = new List<iThinkFact>( state.facts );
		        heuristicCost = state.heuristicCost;
		        name = state.name;
		    }
		
		    public string getName() { return name; }
		    public void setName( string name ) { this.name = name; }
		    public int getCost() { return heuristicCost; }
		    public void setCost( int cost ) { heuristicCost = cost; }
		
		    public List<iThinkFact> getFactList() { return facts; }
		
		    /// If the fact isn't part of the list, it is inserted. Called by iThinkFact::applyFact()
		    public void addFact( iThinkFact fact )
		    {
		        foreach ( iThinkFact _fact in this.facts )
		        {
		            if ( _fact == fact )
		                return;
		        }
		        facts.Add( fact );
		    }
		
		    /// If the fact is part of the list, it is deleted. Called by iThinkFact::applyFact()
		    public void delFact( iThinkFact fact )
		    {
		        foreach ( iThinkFact _fact in this.facts )
		        {
		            if ( _fact == fact )
		            {
		                facts.Remove( _fact );
		                break;
		            }
		        }
		    }
		
		    public static bool operator ==( iThinkState state1, iThinkState state2 )
		    {
		        if ( System.Object.ReferenceEquals( state1, state2 ) )
		        {
		            return true;
		        }
		
		        if ( (object)state1 == null || (object)state2 == null )
		        {
		            return false;
		        }
		
		        foreach ( iThinkFact fact1 in state1.facts )
		        {
		            bool check = false;
		
		            foreach ( iThinkFact fact2 in state2.facts )
		            {
		                if ( fact1 == fact2 )
		                    check = true;
		            }
		
		            if ( check == false )
		                return false;
		        }
		
		        return true;
		    }
		
		    public static bool operator !=( iThinkState state1, iThinkState state2 )
		    {
		        return !( state1 == state2 );
		    }
		
		    public void debugPrint()
		    {
		        String msg = "[state] ";
		        msg += this.name + " //";
		        foreach ( var fact in facts )
		            msg += " " + fact.getName();
		        Debug.Log( msg );
		    }
		}	
	}
}