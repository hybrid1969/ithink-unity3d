using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;


/*! @class iThinkActionManager
 *  @brief The ActionManager provides a list of actions that are usable by the agent via iThinkBrain.
 
 *  By using iThinkActionSchemas, the action manager facilitates the creation of available actions,
 *  which the user can access via \a getActions().
 */
namespace iThinkLibrary{
	namespace iThinkActionManagerUtility{
	
		public class iThinkActionManager
		{
		    protected List<iThinkAction> actionList;    /// List of available actions
		    public iThinkActionSchemas schemaManager;   /// Schema manager to be used for action generation
		
		    public List<iThinkAction> getActions() { return actionList; }
		
		    public iThinkActionManager()
		    {
		        actionList = new List<iThinkAction>();
		        schemaManager = new iThinkActionSchemas();
		    }
		
		    /// This function will be called by the user whenever he wants to generate all actions available to him.
		    public void initActionList( GameObject agent, string[] actionSchemas, List<GameObject> knownObjects, List<iThinkFact> factList )
		    {
		        List<iThinkAction> tempActionList = new List<iThinkAction>();
		        foreach ( string schema in actionSchemas )
		        {
		            tempActionList.Clear();
		            tempActionList = schemaManager.generateActions( knownObjects, factList, schema, 2 );
		            actionList.AddRange( tempActionList );
		        }
		    }
		}
	}
}