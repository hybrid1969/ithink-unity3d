using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{

	class AstarFS : iThinkPlanner {

		public iThinkActionManager actionManager;

		public AstarFS() : base(){ depth = 16; }

		public override iThinkPlan SearchMethod ( iThinkState GoalState, iThinkActionManager ActionManager,
		                                         List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		{
			int it = 0;
			actionManager = ActionManager;
			iThinkPlan curStep, nextStep;
			iThinkState CurrentState = null;

			List<iThinkPlan> stateList = null;
			nodesVisited++;

			while ( OpenStates.Count != 0 )
			{
				List<iThinkAction> applicableActions = new List<iThinkAction>();

				stateList = new List<iThinkPlan>();

				curStep = new iThinkPlan( OpenStates[0] );
				CurrentState = OpenStates[0].getState();
				VisitedStates.Add(CurrentState);
				
				/*
				for ( int i = 0 ; i < ((SimpleFPSActionManager)ActionManager).totalAreas ; i++ )
					foreach (iThinkAction a in ((SimpleFPSActionManager)ActionManager).getActions(i))
						a.ToStringLite();
				*/

				OpenStates.RemoveAt( 0 );
				if (curStep.getActionCount() < depth)
				{
					applicableActions = getApplicable( curStep.getState(), ((SimpleFPSActionManager)ActionManager).getActions( Convert.ToInt32(curStep.getState().getFactList().Find(item => item.getName().Equals("npc-at")).getObj(0).name.Substring(4))));

					foreach ( iThinkAction action in applicableActions )
					{
						nextStep = progress( curStep, action );
						nodesVisited++;
						if ( compareStates( nextStep.getState(), GoalState ) )
						{
							nodesExpanded++;
							Debug.Log( "Found Plan (A* FS) after " + it + " iterations, of length " + nextStep.getPlanActions().Count + ", Nodes Expanded: " + nodesExpanded);
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
							nodesExpanded++;
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
			Debug.Log( "Didn't find Plan (A* FS)" );
			return null;
		}

		public int hFunction(iThinkState nextState, iThinkState GoalState, int area)
		{
			//Debug.LogError(area);
			iThinkState tempState = new iThinkState(GoalState);
			iThinkState curState = new iThinkState(nextState);
			for (int i=0 ; i<depth ; i++)
			{
				List<int> areas = new List<int>();
				foreach (iThinkFact f in curState.getFactList())
				{
					if (f.getName() == "npc-at")
					{
						string name = f.getObj(0).name;
						int a = Convert.ToInt32(name.Substring(4));
						areas.Add(a);
					}

					tempState.delFact(f);
				}

				if (tempState.getFactList().Count == 0)
					return i;
				
				///List<iThinkAction> applicableActions = getApplicable(curState,  ((SimpleFPSActionManager)actionManager).getActions(-1) );
				///List<iThinkAction> applicableActions = getApplicable(curState,  ((SimpleFPSActionManager)actionManager).getActions(area) );
				
				List<iThinkAction> allApplicableActions = new List<iThinkAction>();
				foreach (int k in areas)
				{
					List<iThinkAction> applicableActions = getApplicable(curState,  ((SimpleFPSActionManager)actionManager).getActions(k) );
					allApplicableActions.InsertRange(0, applicableActions);
				}
				
				iThinkPlan curStep = new iThinkPlan(curState);
				foreach (iThinkAction act in allApplicableActions)
				{
					iThinkPlan nextStep = progressPositive(curStep, act);
					//iThinkPlan nextStep = progress(curStep, act);
					curStep = nextStep;
				}
				curState = curStep.getState();
			}
			//in case the planning graph has #layers = depth, return -1, indicating that no solution can be found starting from current state)
			//consequenlty, the state will not be added in the fringe.
			return -1;
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