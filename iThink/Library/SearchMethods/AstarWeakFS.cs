using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{

	class AstarWeakFS : iThinkPlanner
	{
		public AstarWeakFS():base(){ depth = 5; }

		public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager,
		                                        List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		{
			int it = 0;
			iThinkPlan curStep, nextStep;
			iThinkState CurrentState = null;

			List<iThinkPlan> stateList = null;

			while ( OpenStates.Count != 0 )
			{
				List<iThinkAction> applicableActions = new List<iThinkAction>();

				stateList = new List<iThinkPlan>();

				curStep = new iThinkPlan( OpenStates[0] );
				CurrentState = OpenStates[0].getState();
				//VisitedStates.Add(CurrentState);

				OpenStates.RemoveAt( 0 );
				if (curStep.getActionCount() < depth)
				{
					applicableActions = getApplicable( CurrentState, ActionManager.getActions() );

					foreach ( iThinkAction action in applicableActions )
					{
						nextStep = progress( curStep, action );
						if ( compareStates( nextStep.getState(), GoalState ) )
						{
							Debug.Log( "Found Plan (A*-weak FS) after " + it + " iterations, of length " + nextStep.getPlanActions().Count + ", Nodes Expanded: " + nodesExpanded);
							Plan.setPlan( nextStep );
							repoFunct.completed=true;
							return Plan;
						}

						if ( !VisitedStates.Contains(nextStep.getState()) )
						{
							int Cost = hFunction( nextStep.getState(), GoalState );
							Cost = Cost + nextStep.getPlanActions().Count;
							nextStep.getState().setCost( Cost );
							stateList.Add( nextStep );
						}

					}
					OpenStates.AddRange( stateList );
					OpenStates.Sort( delegate( iThinkPlan obj1, iThinkPlan obj2 )
					                {
					                	if ( obj1.getState().getCost() == obj2.getState().getCost() )
					                		return 0;
					                	else if ( obj1.getState().getCost() < obj2.getState().getCost() )
					                		return -1;
					                	else
					                		return 1;
					                }
					               );
					++it;
				}

			}
			Debug.Log( "Didn't find Plan (A*-weak FS)" );
			return null;
		}

		public override int hFunction( iThinkState nextState, iThinkState GoalState )
		{
			return base.hFunction(nextState,GoalState);
		}

		public override iThinkPlan progress( iThinkPlan Step, iThinkAction Action )
		{
			iThinkPlan NewStep = new iThinkPlan( Step );
			List<iThinkAction> curActions;

			curActions = NewStep.getPlanActions();
			curActions.Add( Action );

			NewStep.setState( Action.applyEffects( Step.getState() ) );

			return NewStep;
		}
	}
}
