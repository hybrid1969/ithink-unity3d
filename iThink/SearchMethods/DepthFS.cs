using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

public class DepthFS : iThinkPlanner {

	public DepthFS() : base(){}
	// Use this for initialization
	public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
	{
		int it = 0;
		iThinkPlan curStep, nextStep;
		iThinkState CurrentState;
	
		while ( OpenStates.Count != 0 )
		{
			//Debug.LogWarning( "Iteration #" + it );
			List<iThinkAction> applicableActions = new List<iThinkAction>();
	
			curStep = new iThinkPlan( OpenStates[0] );
			CurrentState = OpenStates[0].getState();
			VisitedStates.Add( CurrentState );
			OpenStates.RemoveAt( 0 );

			applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
		
			int count=0;
			foreach ( iThinkAction action in applicableActions )
			{
				bool found = false;             
		
				nextStep = progress( curStep, action );
	
				if ( compareStates( nextStep.getState(), GoalState ) )
				{
					Debug.Log( "Found Plan (DepthFS)" );
					Plan.setPlan( nextStep );
					return Plan;
				}
	
				foreach ( iThinkState state in VisitedStates )
				{
					if ( state == nextStep.getState() )
					{
						found = true;
						break;
					}
				}
	
				if ( found == false )
				{
					OpenStates.Insert( count, nextStep );
					count++;
				}
			}
	
			++it;
		}
		Debug.Log( "Didn't find Plan (DepthFS)" );
		return null;
	}
}
