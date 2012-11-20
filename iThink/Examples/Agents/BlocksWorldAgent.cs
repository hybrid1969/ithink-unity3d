using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksWorldAgent : MonoBehaviour
{
    iThinkBrain brain;

    public string[] schemaList = {
                                     "ActionBwPickUp-1-Tag~block",
                                     "ActionBwUnStack-2-Tag~block-Tag~block",
                                     "ActionBwPutDown-1-Tag~block",
                                     "ActionBwStack-2-Tag~block-Tag~block",
                                 };

    public void Awake()
    {
        brain = new iThinkBrain();

        // Sensing GameObjects
        List<String> tags = new List<String>();
        tags.Add( "block" );
        brain.sensorySystem.OmniscientUpdate( this.gameObject, tags );

        // Building knowledge
        initKnowledge();
        brain.curState = brain.startState;

        // Generate actions from knowledge
        brain.ActionManager = new iThinkActionManager();
        brain.ActionManager.initActionList( this.gameObject, schemaList, brain.getKnownObjects(), brain.getKnownFacts() );

        // Init search        
        renderCurrentState();
        brain.planner.forwardSearch( brain.startState, brain.goalState, brain.ActionManager );
    }

    public void Update()
    {
        if ( Time.time - brain.lastUpdate >= 0.3 )
        {
            if ( brain.planner.getPlan().getActionCount() != 0 )
                applyNextAction();
            brain.lastUpdate = Time.time;

            renderCurrentState();
        }
    }

    private void renderCurrentState()
    {
        int i;
        BitArray stacks = new BitArray( brain.getKnownObjects().Count );

        foreach ( iThinkFact fact in brain.curState.getFactList() )
        {
            if ( fact.getName().Equals( "onTable" ) )
            {
                i = findNextFreeStack( stacks );
                fact.getObj( 0 ).transform.position = new Vector3( (float)( 1.5 - i * 1.5 ), 0, 0 );
                stacks.Set( i - 1, true );
            }
        }

        foreach ( iThinkFact fact in brain.curState.getFactList() )
        {
            if ( fact.getName().Equals( "on" ) )
            {
                fact.getObj( 0 ).transform.position = fact.getObj( 1 ).transform.position;
                fact.getObj( 0 ).transform.Translate( 0, (float)1.5, 0 );
            }

            else if ( fact.getName().Equals( "holding" ) )
            {
                fact.getObj( 0 ).transform.position = new Vector3( -15, 5, 0 );
            }
        }
    }
    
    static int findNextFreeStack( BitArray bitmap )
    {
        int count = 0;
        foreach ( bool bit in bitmap )
        {
            if ( bit )
                return count;
            count++;
        }
        return count;
    }
    
    void applyNextAction()
    {
        List<iThinkAction> actionList = brain.planner.getPlan().getPlanActions();

        brain.curState = new iThinkState( actionList[0].applyEffects( brain.curState ) );

        actionList.RemoveAt( 0 );
    }

    void initKnowledge()
    {
        ///////////////////////////
        // Building current state//
        ///////////////////////////
        List<iThinkFact> factList = new List<iThinkFact>();
        factList.Add( new iThinkFact( "onTable", GameObject.Find( "E" ) ) );
        factList.Add( new iThinkFact( "onTable", GameObject.Find( "D" ) ) );
        factList.Add( new iThinkFact( "onTable", GameObject.Find( "C" ) ) );
        factList.Add( new iThinkFact( "onTable", GameObject.Find( "F" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "A" ), GameObject.Find( "E" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "B" ), GameObject.Find( "D" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "G" ), GameObject.Find( "F" ) ) );
        factList.Add( new iThinkFact( "clear", GameObject.Find( "A" ) ) );
        factList.Add( new iThinkFact( "clear", GameObject.Find( "B" ) ) );
        factList.Add( new iThinkFact( "clear", GameObject.Find( "C" ) ) );
        factList.Add( new iThinkFact( "clear", GameObject.Find( "G" ) ) );
        factList.Add( new iThinkFact( "gripEmpty" ) );
        brain.startState = new iThinkState( "Initial", new List<iThinkFact>( factList ) );

        /////////////////
        // Create goal //
        /////////////////
        factList.Clear();
        factList.Add( new iThinkFact( "on", GameObject.Find( "E" ), GameObject.Find( "A" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "D" ), GameObject.Find( "F" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "C" ), GameObject.Find( "B" ) ) );
        factList.Add( new iThinkFact( "on", GameObject.Find( "F" ), GameObject.Find( "E" ) ) );
        factList.Add( new iThinkFact( "holding", GameObject.Find( "G" ) ) );

        brain.goalState = new iThinkState( "Goal", new List<iThinkFact>( factList ) );
    }
}
