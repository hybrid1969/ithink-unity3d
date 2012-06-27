using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

public class BestFS : iThinkPlanner {

	public BestFS() : base(){}
	// Use this for initialization
	public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
    {
        int it = 0;
        iThinkPlan curStep, nextStep;
        iThinkState CurrentState = null;
		
		List<iThinkPlan> stateList = null;
		        
		while ( OpenStates.Count != 0 )
        {
			if ( it % 10 == 0 )
					Debug.LogWarning( "Iteration #" + it );
            List<iThinkAction> applicableActions = new List<iThinkAction>();
		            
            stateList = new List<iThinkPlan>();
		            
            curStep = new iThinkPlan( OpenStates[0] );
            CurrentState = OpenStates[0].getState();
            VisitedStates.Add(CurrentState); 
		
            OpenStates.RemoveAt( 0 );
	            
            applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
		                
            foreach ( iThinkAction action in applicableActions )
            {
            	bool found = false;
                // todo: Add "statnode" for statistics retrieval
                nextStep = progress( curStep, action );

                if ( compareStates( nextStep.getState(), GoalState ) )
                {
                    Debug.Log( "Found Plan (BestFS) "+nextStep.getPlanActions().Count );
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
                    int Cost = hFunction( nextStep.getState(), GoalState );
                    nextStep.getState().setCost( Cost );
                    stateList.Add( nextStep );
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
		Debug.Log( "Didn't find Plan (BestFS)" );
        return null;
    }
}
