using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

public class BreadthFS : iThinkPlanner {

	public BreadthFS() : base(){}
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
		
	           CurrentState.debugPrint();
		
	           applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
		
	           foreach ( iThinkAction action in applicableActions )
	           {
	               bool found = false;
		
	               // todo: Add "statnode" for statistics retrieval
	               nextStep = progress( curStep, action );
		
	               if ( compareStates( nextStep.getState(), GoalState ) )
	               {
	                   Debug.Log( "Found Plan (BreadthFS)" );
	                   Plan.setPlan( nextStep );
	                   repoFunct.completed=true;
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
	                   OpenStates.Add( nextStep );
	               }
	           }
		
	           ++it;
	       }
	       Debug.Log( "Didn't find Plan (BreadthFS)" );
	       return null;
	}
}

