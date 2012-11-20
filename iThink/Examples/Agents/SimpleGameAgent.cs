using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;
using iThinkLibrary.iThinkActionRepresentation;

public class SimpleGameAgent : MonoBehaviour
{
	iThinkBrain brain;
	Vector3 vector;
	Vector3 initPosition;
	bool letsPlay;
	bool giveMeHint;
	bool resetEve;
	List<iThinkAction> actionList;
	List<iThinkAction> actionList2;

	public string[] schemaList = {
		"ActionSGMove-3-Fact::adjacent",
		"ActionSGTurn-2-Fact::canTurn",
		"ActionSGShoot-4-Fact::adjacent-Tag::gun",
		"ActionSGStab-2-Tag::location-Tag::knife",
		"ActionSGPickUp-2-Tag::knife-Tag::location",
		"ActionSGPickUp-2-Tag::gun-Tag::location"
	};

	public void Awake()
	{
		brain = new iThinkBrain();

		// Sensing GameObjects
		List<String> tags = new List<String>();
		tags.Add( "direction" ); tags.Add( "location" ); tags.Add( "player" ); tags.Add( "npc" ); tags.Add( "gun" ); tags.Add( "knife" );
		brain.sensorySystem.OmniscientUpdate( this.gameObject, tags );

		Debug.LogWarning( "knownObjects count: " + brain.sensorySystem.getKnownObjects().Count );

		// Building knowledge
		SimplegameTest();
		brain.curState = brain.startState;

		// Building Actions from knowledge
		brain.ActionManager = new iThinkActionManager();
		brain.ActionManager.initActionList( this.gameObject, schemaList, brain.getKnownObjects(), brain.startState.getFactList() );
		
		// Init search
		letsPlay = false;
		giveMeHint = false;
		resetEve = false;
		
		brain.planner.forwardSearch( brain.curState, brain.goalState, brain.ActionManager);
		brain.planner.getPlan().debugPrintPlan();
		actionList = brain.planner.getPlan().getPlanActions();
		actionList2 = new List<iThinkAction>(actionList);
		initPosition = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y,this.gameObject.transform.position.z);
	}
	

	void OnGUI()
	{
		var evt = Event.current;
		if(giveMeHint == false){
			if(GUI.Button(new Rect(0,0,100,40),"Show Plan"))
				giveMeHint = true;
		}
		else {
			if(GUI.Button(new Rect(0,0,100,40),"Hide Plan"))
				giveMeHint = false;
		}
		
		if(letsPlay == false){
			if(GUI.Button(new Rect(0,50,100,40),"Run Plan")){
				letsPlay = true;
				
			}
		}else{
			if(GUI.Button(new Rect(0,50,100,40),"Stop"))
				letsPlay = false;
		}
		
		if(GUI.Button(new Rect(110,50,100,40),"Reset")){
			brain.curState = brain.startState;
			actionList2 =  new List<iThinkAction>(actionList);
			resetEve = true;
		}
		
		if(evt.type == EventType.Repaint && giveMeHint == true){
			showPlan();
		}
	}
	
	void showPlan()
	{
		int i=0;
		int Counter=1;
		GUIStyle style  =  new GUIStyle();
		GUI.Box(new Rect(0,100,300,270),"");
		foreach(iThinkAction action in actionList)
		{
			if(action.getName().Equals("Move"))
			{
				iThinkFact obj = action.getPreconditions().Find(
					delegate(iThinkFact fact)
					{
						return fact.getName().Equals("npcFacing");
						
					}	);
				if(obj !=null){
					string lal = "Action No"+ Counter +" : Move "+obj.getObj(0).name.ToString();
					iThinkAction actio = actionList2.Find(
						delegate(iThinkAction a){
							
							return a.Equals(action);
							
						});
					if(actio == null){
						style.normal.textColor = Color.red;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
					else{
						style.normal.textColor = Color.yellow;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
				}
				
			}else if(action.getName().Equals("Turn"))
			{
				iThinkFact obj = action.getEffects().Find(
					delegate(iThinkFact fact)
					{
						return fact.getName().Equals("npcFacing") && fact.getPositive();
					}	);
				if(obj !=null){
					string lal = "Action No"+ Counter +" : Turn "+obj.getObj(0).name.ToString();
					iThinkAction actio = actionList2.Find(
						delegate(iThinkAction a){
							
							return a.Equals(action);
							
						});
					if(actio == null){
						style.normal.textColor = Color.red;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
					else{
						style.normal.textColor = Color.yellow;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
				}
			}else if(action.getName().Equals("PickUp"))
			{
				iThinkFact obj = action.getEffects().Find(
					delegate(iThinkFact fact)
					{
						return fact.getName().Equals("npcHolding");
					}	);
				if(obj !=null){
					string lal = "Action No"+ Counter +" : PickUp "+obj.getObj(0).name.ToString();
					iThinkAction actio = actionList2.Find(
						delegate(iThinkAction a){
							
							return a.Equals(action);
							
						});
					if(actio == null){
						style.normal.textColor = Color.red;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
					else{
						style.normal.textColor = Color.yellow;
						GUI.Label(new Rect(0,100+i,200,200),lal,style);
					}
				}
			}else if(action.getName().Equals("Shoot"))
			{
				string lal = "Action No"+ Counter +" : Shoot ";
				iThinkAction actio = actionList2.Find(
					delegate(iThinkAction a){
						
						return a.Equals(action);
						
					});
				if(actio == null){
					style.normal.textColor = Color.red;
					GUI.Label(new Rect(0,100+i,200,200),lal,style);
				}
				else{
					style.normal.textColor = Color.yellow;
					GUI.Label(new Rect(0,100+i,200,200),lal,style);
				}
			}else
			{
				string lal = "Action No"+ Counter +" : Stab ";
				iThinkAction actio = actionList2.Find(
					delegate(iThinkAction a){
						
						return a.Equals(action);
						
					});
				if(actio == null){
					style.normal.textColor = Color.red;
					GUI.Label(new Rect(0,100+i,200,200),lal,style);
				}
				else{
					style.normal.textColor = Color.yellow;
					GUI.Label(new Rect(0,100+i,200,200),lal,style);
				}
			}
			i+=30;
			Counter++;
		}
	}
	
	//Run the plan that planner created
	void Update()
	{
		
		if(resetEve ){
			letsPlay = false;
			resetEve = false;
			this.gameObject.transform.position = initPosition;
		}
		
		if(letsPlay){
			if ( Time.time - brain.lastUpdate >= 0.8 && repoFunct.completed == true ){
				if(brain.planner.getPlan().getActionCount()!=0){
					brain.lastUpdate = Time.time;
					renderCurrentState();
				}else{
					repoFunct.completed = false;
				}
			}
		}
		
	}
	
	void renderCurrentState(){
		if(actionList2.Count != 0){
			if(actionList2[0].getName().Equals("Move")){
				iThinkFact obj = actionList2[0].getPreconditions().Find(
					delegate(iThinkFact fact)
					{
						return fact.getName().Equals("adjacent");
						
					}	);
				if(obj!=null)
				{
					GameObject lal = obj.getObj(1);
					Vector3 vector = new Vector3(lal.transform.position.x,this.gameObject.transform.position.y,lal.transform.position.z);
					iTween.MoveTo(this.gameObject,vector,(float)0.5);
				}
			}
			else if(actionList2[0].getName().Equals("Turn"))
			{
				iThinkFact obj = actionList2[0].getPreconditions().Find(
					delegate(iThinkFact fact)
					{
						return fact.getName().Equals("canTurn");
						
					}	);
				if(obj!=null)
				{
					Vector3 vector = getCoords(obj.getObj(0),obj.getObj(1));
					iTween.RotateBy(this.gameObject,vector,(float)0.5);
				}
			}
			
			brain.curState = new iThinkState( actionList2[0].applyEffects( brain.curState ) );
			actionList2.RemoveAt( 0 );
		}
	}
	
	Vector3 getCoords(GameObject dr1,GameObject dr2)
	{
		switch(dr1.name){
			case "UP":
				if(dr2.name.Equals("RIGHT"))
					return new Vector3(0,(float)0.25,0);
				if(dr2.name.Equals("DOWN"))
					return new Vector3(0,(float)0.50,0);
				if(dr2.name.Equals("LEFT"))
					return new Vector3(0,(float)-0.25,0);
				break;
			case "DOWN":
				if(dr2.name.Equals("LEFT"))
					return new Vector3(0,(float)0.25,0);
				if(dr2.name.Equals("RIGHT"))
					return new Vector3(0,(float)-0.25,0);
				if(dr2.name.Equals("UP"))
					return new Vector3(0,(float)0.5,0);
				break;
			case "LEFT":
				if(dr2.name.Equals("UP"))
					return new Vector3(0,(float)0.25,0);
				if(dr2.name.Equals("RIGHT"))
					return new Vector3(0,(float)0.5,0);
				if(dr2.name.Equals("DOWN"))
					return new Vector3(0,(float)-0.25,0);
				break;
			case "RIGHT":
				if(dr2.name.Equals("UP"))
					return new Vector3(0,(float)-0.25,0);
				if(dr2.name.Equals("LEFT"))
					return new Vector3(0,(float)0.5,0);
				if(dr2.name.Equals("DOWN")){
					return new Vector3(0,(float)0.25,0);
				}
				break;
		}
		return new Vector3(0,0,0);
	}
	
	void SimplegameTest()
	{
		List<iThinkFact> factList;

		///////////////////////////
		// Building current state//
		///////////////////////////
		factList = new List<iThinkFact>();
		factList.Add( new iThinkFact( "npcAt", GameObject.Find( "LOC1" ) ) );
		factList.Add( new iThinkFact( "npcFacing", GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "npcEmptyHands" ) );
		factList.Add( new iThinkFact( "playerAt", GameObject.Find( "LOC8" ) ) );
		factList.Add( new iThinkFact( "knife", GameObject.Find( "KNIFE" ) ) );
		factList.Add( new iThinkFact( "gun", GameObject.Find( "GUN" ) ) );
		factList.Add( new iThinkFact( "objectAt", GameObject.Find( "KNIFE" ), GameObject.Find( "LOC3" ) ) );
		factList.Add( new iThinkFact( "objectAt", GameObject.Find( "GUN" ), GameObject.Find( "LOC6" ) ) );

		initAdjacents( factList );
		brain.startState = new iThinkState( "Initial", new List<iThinkFact>( factList ) );

		/////////////////
		// Create goal //
		/////////////////
		factList.Clear();
		//factList.Add( new iThinkFact( "npcHolding", GameObject.Find( "GUN" ) ) );
		//factList.Add( new iThinkFact( "npcAt", GameObject.Find( "LOC9" ) ) );
		factList.Add( new iThinkFact( "playerDown" ) );

		brain.goalState = new iThinkState( "Goal", new List<iThinkFact>( factList ) );
	}

	public void initAdjacents( List<iThinkFact> factList )
	{
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC1" ), GameObject.Find( "LOC4" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC1" ), GameObject.Find( "LOC2" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC2" ), GameObject.Find( "LOC5" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC2" ), GameObject.Find( "LOC1" ), GameObject.Find( "DOWN" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC2" ), GameObject.Find( "LOC3" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC3" ), GameObject.Find( "LOC6" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC3" ), GameObject.Find( "LOC2" ), GameObject.Find( "DOWN" ) ) );

		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC4" ), GameObject.Find( "LOC7" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC4" ), GameObject.Find( "LOC5" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC4" ), GameObject.Find( "LOC1" ), GameObject.Find( "LEFT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC5" ), GameObject.Find( "LOC8" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC5" ), GameObject.Find( "LOC6" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC5" ), GameObject.Find( "LOC2" ), GameObject.Find( "LEFT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC5" ), GameObject.Find( "LOC4" ), GameObject.Find( "DOWN" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC6" ), GameObject.Find( "LOC9" ), GameObject.Find( "RIGHT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC6" ), GameObject.Find( "LOC5" ), GameObject.Find( "DOWN" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC6" ), GameObject.Find( "LOC3" ), GameObject.Find( "LEFT" ) ) );

		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC7" ), GameObject.Find( "LOC4" ), GameObject.Find( "LEFT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC7" ), GameObject.Find( "LOC8" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC8" ), GameObject.Find( "LOC5" ), GameObject.Find( "LEFT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC8" ), GameObject.Find( "LOC7" ), GameObject.Find( "DOWN" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC8" ), GameObject.Find( "LOC9" ), GameObject.Find( "UP" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC9" ), GameObject.Find( "LOC6" ), GameObject.Find( "LEFT" ) ) );
		factList.Add( new iThinkFact( "adjacent", GameObject.Find( "LOC9" ), GameObject.Find( "LOC8" ), GameObject.Find( "DOWN" ) ) );
		
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "UP" ), GameObject.Find( "DOWN" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "UP" ), GameObject.Find( "RIGHT" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "UP" ), GameObject.Find( "LEFT" )) );
		
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "DOWN" ), GameObject.Find( "UP" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "DOWN" ), GameObject.Find( "LEFT" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "DOWN" ), GameObject.Find( "RIGHT" )) );
		
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "LEFT" ), GameObject.Find( "UP" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "LEFT" ), GameObject.Find( "DOWN" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "LEFT" ), GameObject.Find( "RIGHT" )) );
		
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "RIGHT" ), GameObject.Find( "UP" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "RIGHT" ), GameObject.Find( "DOWN" )) );
		factList.Add( new iThinkFact( "canTurn", GameObject.Find( "RIGHT" ), GameObject.Find( "LEFT" )) );
	}
}
