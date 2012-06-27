using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;

/*! @class iThinkPlanner
 *  @brief iThinkPlanner performs the planning process.

 *  By supplying an initial state, a goal state, and an iThinkActionManager object, iThinkPlanner
 *  performs planning and searches for the first valid plan.
 */
namespace iThinkLibrary{
	namespace iThinkPlannerUitility{
		
		 public static class repoFunct{
			
			public static bool loadData ;
			public static bool completed;
				
		}
		
		public class iThinkPlanner
		{
		    protected iThinkPlan Plan;
		    protected List<iThinkPlan> _OpenStates;
		    protected List<iThinkState> _VisitedStates;
		    
		    //repo data
		    protected List<iThinkPlan> repoOpenStates;
		    protected List<iThinkState> repoVisitedStates;
			protected List<iThinkPlan> repoStateList;
		    protected iThinkState repoCurrentState;
		    protected int repoIterations;
		
		    public iThinkPlanner()
		    {
		        Plan = new iThinkPlan();
		        _OpenStates = new List<iThinkPlan>();
		        _VisitedStates = new List<iThinkState>();
		    }
		    
		    public void InitRepo(){
		    	this.repoOpenStates = new List<iThinkPlan>();
				this.repoVisitedStates = new List<iThinkState>();
				this.repoStateList = new List<iThinkPlan>();
		    }
		    
		    public void ClearRepo(){
		    	this.repoOpenStates = null;
				this.repoVisitedStates = null;
				this.repoStateList = null;
		    }
		
		    public iThinkPlan getPlan() { return Plan; }
	
		    /// The function performing planning using Forward Search and the specified \a method
		    public bool forwardSearch( iThinkState InitialState, iThinkState GoalState, iThinkActionManager ActionManager)
		    {
		        iThinkPlan ReturnVal;
		        
		        _OpenStates.Clear();
		        _VisitedStates.Clear();
		
		        iThinkPlan step = new iThinkPlan( InitialState );
		        _OpenStates.Add( step );
		        _VisitedStates.Add( step.getState() );
		
		        ReturnVal = SearchMethod( GoalState, ActionManager, _OpenStates, _VisitedStates );
		
		        if ( ReturnVal == null )
		            return false;
		        else if ( ReturnVal.hasPlan() )
		            return true;
		        return false;
		    }
		
		    public virtual iThinkPlan SearchMethod(iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
		    {
		    	return null;
		    }
		
		    public virtual int hFunction( iThinkState nextState, iThinkState GoalState )
		    { 
		    	int counter = 0;
		        foreach ( iThinkFact fact in nextState.getFactList() )
		        {
		            foreach ( iThinkFact goalFact in GoalState.getFactList() )
		            {
		                if ( fact == goalFact )
		                {
		                    counter++;
		                    break;
		                }
		            }
		        }
		        return counter;
		    }
		
		    /// Generates the next plan step after the application of \a Action's effects.
		    public iThinkPlan progress( iThinkPlan Step, iThinkAction Action )
		    {
		        iThinkPlan NewStep = new iThinkPlan( Step );
		        List<iThinkAction> curActions;
		
		        curActions = NewStep.getPlanActions();
		        curActions.Add( Action );
		
		        NewStep.setState( Action.applyEffects( Step.getState() ) );
		
		        return NewStep;
		    }
		
		    /// Finds all actions applicable to the current \a State from the collection of available \a Actions
		    public List<iThinkAction> getApplicable( iThinkState State, List<iThinkAction> Actions )
		    {
		        List<iThinkAction> ApplicableActions = new List<iThinkAction>();
		
		        foreach ( iThinkAction action in Actions )
		        {
		            if ( action == null )
		                break;
		
		            if ( action.isApplicable( State ) )
		                ApplicableActions.Add( action );
		        }
		
		        return ApplicableActions;
		    }
		
		    /// Checks if goalState is a subset of curState
		    public bool compareStates( iThinkState curState, iThinkState goalState )
		    {
		        int counter = 0;
		        foreach ( iThinkFact fact in goalState.getFactList() )
		        {
		            foreach ( iThinkFact check in curState.getFactList() )
		            {
		                if ( check == null )
		                    return false;
		                else if ( check == fact )
		                    counter++;
		            }
		        }
		        if ( counter == goalState.getFactList().Count )
		            return true;
		        return false;
		    }
		}
	}
}