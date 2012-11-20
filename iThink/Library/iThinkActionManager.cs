using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary{
	/*! @class iThinkActionManager
	 *  @brief The ActionManager provides a list of actions that are usable by the agent via iThinkBrain.
	 *
	 *  By using iThinkActionSchemas, the action manager facilitates the creation of available actions,
	 *  which the user can access via \a getActions().
	 */
	public class iThinkActionManager
	{
		protected List<iThinkAction> actionList;	/// List of available actions
		public iThinkActionGenerator generator;		/// Schema manager to be used for action generation

		public virtual List<iThinkAction> getActions() { return actionList; }

		public iThinkActionManager()
		{
			actionList = new List<iThinkAction>();
			generator = new iThinkActionGenerator();
		}

		public virtual iThinkAction actionInstantiator(string actionName,GameObject[] matrix)
		{
			return null;
		}

		/// This function will be called by the user whenever he wants to generate all actions available to him.
		public virtual void initActionList( GameObject agent, string[] actionAssemblyList, List<GameObject> knownObjects, List<iThinkFact> factList )
		{
			List<iThinkAction> tempActionList = new List<iThinkAction>();
			foreach ( string assembly in actionAssemblyList )
			{
				tempActionList.Clear();
				tempActionList = generator.actionGenerator( knownObjects, factList, assembly, actionInstantiator );
				actionList.AddRange( tempActionList );
			}
		}
	}
}