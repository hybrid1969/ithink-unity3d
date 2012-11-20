using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{

	public class BreadthFS : iThinkPlanner
	{
		public BreadthFS() : base(){}
		
		public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		{
			//Debug.LogWarning(GoalState.ToString());
			//Debug.LogWarning(ActionManager.getActions().Count);
			//Debug.LogWarning(OpenStates.Count);
			//Debug.LogWarning(VisitedStates.Count);
			/*Debug.Log("Available Actions = " + totalActions);
			foreach ( iThinkAction act in ActionManager.getActions() )
				Debug.Log(act.ToString());*/

			int it = 0;
			iThinkPlan curStep, nextStep;
			iThinkState CurrentState;

			while ( OpenStates.Count != 0 )
			{
				List<iThinkAction> applicableActions = new List<iThinkAction>();

				curStep = new iThinkPlan( OpenStates[0] );
				CurrentState = OpenStates[0].getState();
				VisitedStates.Add( CurrentState );
				OpenStates.RemoveAt( 0 );

				//Debug.LogWarning( "Iteration #" + it + " - " + curStep.getState().ToString() );
				
				applicableActions = getApplicable( CurrentState, ActionManager.getActions() );

				foreach ( iThinkAction action in applicableActions )
				{
					//Debug.Log(action.ToString());
					nextStep = progress( curStep, action );

					if ( compareStates( nextStep.getState(), GoalState ) )
					{
						Debug.Log( "Found Plan (BreadthFS) after " + it + " iterations, of length " + nextStep.getPlanActions().Count );
						Plan.setPlan( nextStep );
						repoFunct.completed = true;
						return Plan;
					}

					if( !VisitedStates.Contains(nextStep.getState()))
						OpenStates.Add( nextStep );
				}

				++it;
			}
			Debug.Log( "Didn't find Plan (BreadthFS) " + it );
			return null;
		}
	}
}