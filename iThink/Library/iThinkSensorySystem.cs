using System;
using System.Collections.Generic;
using UnityEngine;

namespace iThinkLibrary
{
	/*! @class iThinkSensorySystem
	 *	@brief A system implementing GameObject detection via Tags
	 *	Implements a basic system for declaring sensors usable by iThink agents.
	 *	Currently, it only provides two kinds of sensors:
	 *	- an active sensor \a (OmniscientUpdate) which returns all objects of specified \a Tags.
	 *	- an active sensor \a (ProximityUpdate) which returns all objects within a specified \a radius
	 *
	 *	It's up to the user to extend the class with more kinds of sensors.
	 */

	public class iThinkSensorySystem
	{
		protected List<GameObject> knownObjects;   /// A list of all the GameObjects sensed by the agent

		public iThinkSensorySystem() { knownObjects = new List<GameObject>(); }

		public List<GameObject> getKnownObjects() { return knownObjects; }

		/// Performs a search for GameObjects within a specified \a radius from the agent
		public virtual void ProximityUpdate( GameObject agent, UInt32 radius )
		{
			LayerMask layerMask = 1 << LayerMask.NameToLayer( "GamePart" );
			Collider[] interactiveObjs = Physics.OverlapSphere( agent.transform.position, radius, layerMask.value );

			foreach ( Collider col in interactiveObjs )
				knownObjects.Add( col.gameObject );
		}

		/// Performs a universal search in the whole scene for GameObjects of specified \a Tags
		public virtual void OmniscientUpdate( GameObject agent, List<String> Tags )
		{
			GameObject[] objects;

			foreach ( String tag in Tags )
			{
				objects = GameObject.FindGameObjectsWithTag( tag );

				foreach ( GameObject obj in objects )
					knownObjects.Add( obj );
			}
		}

		public virtual iThinkState translateKnowledge(){return new iThinkState("null");}
		
		public virtual void tagObjects(){}
		
		public void ignoreObject(GameObject obj)
		{
			knownObjects.Remove(obj);
		}

		public void addObject(GameObject obj)
		{
			if (!knownObjects.Contains(obj))
			{
				knownObjects.Add(obj);
			}
		}
	}
}