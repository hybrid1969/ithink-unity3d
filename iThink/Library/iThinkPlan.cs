using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{
	/*! @class iThinkPlan
	 *  @brief A plan is a list of actions paired to a state, describing a (possibly incomplete) plan.
	 *
	 *  An iThinkPlan describes the sequence of "actions" needed to proceed to a "state"
	 *  When the state equals a goalstate, the actions lists equals a complete, valid plan.
	 */
	public class iThinkPlan
	{
		protected iThinkState state;
		protected List<iThinkAction> actions;

		public iThinkPlan()
		{
			this.state = null;
			this.actions = new List<iThinkAction>();
		}

		public iThinkPlan( iThinkState state )
		{
			this.state = new iThinkState( state );
			this.actions = new List<iThinkAction>();
		}

		public iThinkPlan( iThinkState state, List<iThinkAction> actions )
		{
			this.state = new iThinkState( state );
			this.actions = new List<iThinkAction>( actions );
		}

		public iThinkPlan( iThinkPlan Step )
		{
			this.state = new iThinkState( Step.state );
			this.actions = new List<iThinkAction>( Step.actions );
		}

		public iThinkState getState() { return state; }
		public void setState( iThinkState NewState ) { state = NewState; }

		public List<iThinkAction> getPlanActions() { return actions; }
		public int getActionCount() { return actions.Count; }

		public bool hasPlan() { return actions.Count != 0; }
		public void setPlan( iThinkPlan plan ) {
			state = new iThinkState(plan.getState());
			actions = new List<iThinkAction>( plan.getPlanActions() );
		}
		public void appendPlan( List<iThinkAction> newActions ) {
			this.actions.AddRange(newActions);
		}

		public void debugPrintPlan()
		{
			String msg = "[PrintPlan()]";
			foreach ( var action in actions )
			{ msg += " -> " + action.ToStringLite(); }
			Debug.Log( msg );
		}
		
		public string ToStringLite()
		{
			string msg = "[PrintPlan()]";
			foreach ( iThinkAction action in actions )
			{ 
				msg += " -> " + action.ToStringLite(); 
			}
			
			return msg;
		}
		
	}
}