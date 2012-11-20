using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{

	public class HeuristicDepthFS : iThinkPlanner
	{
		public iThinkActionManager actionManager;

		public HeuristicDepthFS() : base(){ depth = 16; }

		public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		{
			int it = 0;
			DateTime n1 = DateTime.Now;
			iThinkPlan curStep, nextStep;
			iThinkState CurrentState;
			nodesVisited++;
			actionManager = ActionManager;
			List<iThinkAction> applicableActions;

			if ( compareStates( OpenStates[0].getState(), GoalState ) )
			{
				Debug.Log( "Found Plan (HFS) - Already at goal state!" );
				Plan.setPlan( OpenStates[0] );
				repoFunct.completed=true;
				return Plan;
			}
			
			//Debug.Log("Available actions: " + ActionManager.getActions().Count);
			
			while ( OpenStates.Count != 0 )
			{
				curStep = new iThinkPlan( OpenStates[0] );
				CurrentState = OpenStates[0].getState();
				VisitedStates.Add( CurrentState );
				OpenStates.RemoveAt( 0 );
				int curArea = Convert.ToInt32(curStep.getState().getFactList().Find(item => item.getName().Equals("npc-at")).getObj(0).name.Substring(4));
				
				if (curStep.getActionCount() < depth)
				{
					List<iThinkPlan> successors = new List<iThinkPlan>();
					applicableActions = getApplicable( CurrentState, ((SimpleFPSActionManager)ActionManager).getActions(-1) );
					///applicableActions = getApplicable( CurrentState, ((SimpleFPSActionManager)ActionManager).getActions(curArea) );
					nodesExpanded++;
					foreach ( iThinkAction action in applicableActions )
					{
						nextStep = progress( curStep, action );
						
						if ( compareStates( nextStep.getState(), GoalState ) )
						{
							nodesVisited++;
							Debug.Log( "Found Plan (H-DepthFS) " + nextStep.getActionCount() +
							          " (Nodes: "+nodesVisited+"/"+nodesExpanded+")");
							Plan.setPlan( nextStep );
							return Plan;
						}

						//if ( !VisitedStates.Contains(nextStep.getState()) ) {
							int Cost = hFunction( nextStep.getState(), GoalState, curArea );
							if (Cost != -1)
							{
								nextStep.getState().setCost( Cost );
								successors.Add(nextStep);

								nodesVisited++;
							}
						//}
					}
					successors.Sort( delegate( iThinkPlan obj1, iThinkPlan obj2 )
					                {
					                	if ( obj1.getState().getCost() == obj2.getState().getCost() )
					                		return 0;
					                	else if ( obj1.getState().getCost() < obj2.getState().getCost() )
					                		return -1;
					                	else
					                		return 1;
					                }
					               );
					OpenStates.InsertRange(0, successors);
					TimeSpan timediff = n1 - DateTime.Now;
					if ( timediff.TotalSeconds > 60 )
						return OpenStates[0];
					++it;
				}
				else {
					Debug.LogWarning("Depth Sucks!");
				}
			}
			Debug.Log( "Didn't find Plan (H-DepthFS)" );
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
	}
}
