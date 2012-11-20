using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{
	public class BestFS : iThinkPlanner
	{
		public BestFS() : base(){}

		public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager,
		                                        List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		{
			int it = 0;
			iThinkPlan curStep, nextStep;
			iThinkState CurrentState = null;

			List<iThinkPlan> stateList = null;

			this.totalActions = ActionManager.getActions().Count;
			this.nodes += OpenStates.Count;

			/*Debug.Log("Available Actions = " + totalActions);
			foreach ( iThinkAction act in ActionManager.getActions() )
				Debug.Log(act.ToString());*/
			
			List<iThinkAction> applicableActions = new List<iThinkAction>();
			stateList = new List<iThinkPlan>();

			if ( compareStates( OpenStates[0].getState(), GoalState ) )
			{
				Debug.Log( "Found Plan (BestFS) - Already at goal state!" );
				Plan.setPlan( OpenStates[0] );
				repoFunct.completed=true;
				return Plan;
			}
			
			while ( OpenStates.Count != 0 )
			{
				if(it % 200 == 0 && it!=0){
					//InitRepo();
					this.repoOpenStates = new List<iThinkPlan>(OpenStates);
					this.repoVisitedStates = new List<iThinkState>(VisitedStates);
					this.repoStateList = new List<iThinkPlan>(stateList);
					this.repoCurrentState = new iThinkState(CurrentState);
					this.repoIterations = it;
					repoFunct.loadData = true;
					return null;
				}else if( repoFunct.loadData ){
					OpenStates = this.repoOpenStates;
					VisitedStates = this.repoVisitedStates;
					stateList = this.repoStateList;
					CurrentState = this.repoCurrentState;
					it = this.repoIterations;
					repoFunct.loadData = false;
					//ClearRepo();
				}else{
					stateList = new List<iThinkPlan>();
				}

				curStep = new iThinkPlan( OpenStates[0] );
				CurrentState = OpenStates[0].getState();
				
				//Debug.Log(GoalState.ToString());
				//Debug.Log(CurrentState.ToString());
				
				//Debug.LogWarning( "Iteration #" + it + " - " + curStep.getState().ToString() );
				
				VisitedStates.Add( OpenStates[0].getState() );
				OpenStates.RemoveAt( 0 );
				applicableActions = getApplicable( CurrentState, ActionManager.getActions() );

				this.nodesExpanded++;
				this.totalApplicable += applicableActions.Count;

				foreach ( iThinkAction action in applicableActions )
				{
					//Debug.Log(action.ToString());

					nextStep = progress( curStep, action );

					if ( compareStates( nextStep.getState(), GoalState ) )
					{
						Debug.Log( "Found Plan (BestFS) after " + it + " iterations, of length " + nextStep.getPlanActions().Count );
						Plan.setPlan( nextStep );
						repoFunct.completed=true;
						return Plan;
					}

					if ( VisitedStates.Contains(nextStep.getState()) == false )
					{
						int Cost = hFunction( nextStep.getState(), GoalState );
						nextStep.getState().setCost( Cost );
						stateList.Add( nextStep );
						this.nodes ++;
						this.totalActionsUsed++;
					}
				}

				OpenStates.AddRange( stateList );
				OpenStates.Sort( delegate( iThinkPlan obj1, iThinkPlan obj2 )
				                {
				                	if ( obj1.getState().getCost() == obj2.getState().getCost() )
				                		return 0;
				                	else if ( obj1.getState().getCost() > obj2.getState().getCost() )
				                		return -1;
				                	else
				                		return 1;
				                }
				               );
				++it;
			}
			Debug.Log( "Didn't find plan (BestFS) " + it );
			return null;
		}
	}
}