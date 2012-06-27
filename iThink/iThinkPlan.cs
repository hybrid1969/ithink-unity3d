using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
/*! @class iThinkPlan
 *  @brief A plan is a list of actions paired to a state, describing an (incomplete) plan.

 *  An iThinkPlan describes the sequence of "Actions" needed to proceed to a "State"
 *  When the State equals a goalstate, the Actions lists equals the Plan.
 */
namespace iThinkLibrary{
	namespace iThinkPlannerUitility{
		public class iThinkPlan
		{
		    protected iThinkState State;
		    protected List<iThinkAction> Actions;
		
		    public iThinkPlan()
		    {
		        this.State = null;
		        this.Actions = new List<iThinkAction>();
		    }
		
		    public iThinkPlan( iThinkState State )
		    {
		        this.State = new iThinkState( State );
		        this.Actions = new List<iThinkAction>();
		    }
		
		    public iThinkPlan( iThinkState State, List<iThinkAction> Actions )
		    {
		        this.State = new iThinkState( State );
		        this.Actions = new List<iThinkAction>( Actions );
		    }
		
		    public iThinkPlan( iThinkPlan Step )
		    {
		        this.State = new iThinkState( Step.State );
		        this.Actions = new List<iThinkAction>( Step.Actions );
		    }
		
		    public iThinkState getState() { return State; }
		    public void setState( iThinkState NewState ) { State = NewState; }
		
		    public List<iThinkAction> getPlanActions() { return Actions; }
		    public int getActionCount() { return Actions.Count; }
		    
		    public bool hasPlan() { return Actions.Count != 0; }
		    public void setPlan( iThinkPlan plan ) { Actions = new List<iThinkAction>( plan.getPlanActions() ); }
		    
		    public void debugPrintPlan()
		    {
		        String msg = "[PrintPlan()]";
		        foreach ( var action in Actions )
		        { msg += " -> " + action.getName(); }
		        Debug.Log( msg );
		    }
		}
	}
}