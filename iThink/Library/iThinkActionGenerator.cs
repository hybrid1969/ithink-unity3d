using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

namespace iThinkLibrary{
	/** @class iThinkActionSchemas
	 *  @brief Parses action schemas and generates actions.
	 *
	 *  Each agent describes its available actions in the iThinkBrain::schemaList variable.
	 *  Whenever it needs to plan, it generates its available actions via the iThinkManager,
	 *  a process which is internally managed and completed by iThinkActionSchemas.
	 */
	public class iThinkActionGenerator
	{

		void printEverything(List<GameObject[]> combinations)
		{
			foreach(GameObject[] matrix in combinations)
			{
				string test = schemaElements[0] + " ";
				for ( int i = 0 ; i < matrix.Length ; i++ )
					test += matrix[i].ToString();
				Debug.Log(test);
			}
		}

		/*! Action generator
		 */
		public virtual void actionAssembler( List<GameObject[]> combinations, Func<string, GameObject[], iThinkAction> actionInstantiator )
		{
			iThinkAction action;
			//printEverything(combinations);
			foreach ( GameObject[] matrix in combinations )
			{
				action = actionInstantiator(schemaElements[0],matrix);
				tempActionList.Add( action );
			}
		}

		int globalCounter = 0;
		int factCounter = 0;
		Dictionary<int, List<GameObject>> objectCollector { get; set; }
		Dictionary<int, List<GameObject>> factCollector{get; set;}
		List<iThinkAction> tempActionList;
		List<GameObject[]> combinations;
		List<GameObject[]> finalizeCombos;
		string[] schemaElements;
		string[] actionElements;

		public iThinkActionGenerator()
		{
			objectCollector = new Dictionary<int, List<GameObject>>();
			factCollector = new Dictionary<int, List<GameObject>>();
			finalizeCombos = new List<GameObject[]>();
			tempActionList = new List<iThinkAction>();
		}

		/*! Parses the action schema string, searches for needed data, and creates a list applicable actions
		 * 	@return A list of available/applicable actions.
		 */
		public List<iThinkAction> actionGenerator( List<GameObject> knownObjects, List<iThinkFact> FactList, string schema, Func<string, GameObject[],iThinkAction> actionInstantiator  )
		{
			initData();
			schemaParser( schema );
			searchComponents( knownObjects, FactList );
			if(objectCollector.Count != 0)
				unificator( 2 );
			completeKnowledge();
			actionAssembler(finalizeCombos, actionInstantiator);
			return tempActionList;
		}

		private void completeKnowledge()
		{
			if(factCollector.Count == 0){
				foreach(GameObject[] obj in combinations){
					finalizeCombos.Add(obj);
				}
				return;
			}

			if( objectCollector.Count == 0 ) {
				for(int i=0;i<factCollector.Count;i++)
				{
					List<GameObject> tempList = factCollector[i];
					finalizeCombos.Add(tempList.ToArray());
				}
				return;
			}

			for(int i=0 ; i < factCollector.Count ; i++)
			{
				foreach(GameObject[] objs in combinations)
				{
					GameObject[] test = factCollector[i].ToArray();
					GameObject[] final = new GameObject[test.Length+objs.Length];
					test.CopyTo(final, 0);
					objs.CopyTo(final, test.Length);
					finalizeCombos.Add(final);
				}
			}
		}

		/// Splits the action schema string
		private void schemaParser( string schema )
		{
			try{
				schemaElements = schema.Split( '-' );
			}catch(Exception e){
				Debug.LogError("Error: Cannot parse schemas!" + e.StackTrace);
			}
		}

		/// Searches for the needed information, specified in the action schema string.
		private void searchComponents( List<GameObject> knownObjects, List<iThinkFact> FactList )
		{
			for ( int i = 2 ; i < schemaElements.Length ; i++ )
			{
				actionElements = Regex.Split(schemaElements[i], "::" );

				switch ( actionElements[0] )
				{
					case "Tag":
						searchknownObjects( knownObjects );
						break;
					case "Fact":
						/*Debug.LogWarning("Fact-based schema");
						foreach ( string s in actionElements)
							Debug.Log(s);*/
						searchFactList(FactList);
						break;
					default:
						break;
				}
			}
		}

		/// Searches for GameObjects of specified tag.
		private void searchknownObjects( List<GameObject> knownObjects )
		{
			List<GameObject> foundObjects = new List<GameObject>();
			foundObjects = knownObjects.FindAll( delegate( GameObject checkedObject )
			                                    {
			                                    	if ( checkedObject.tag.ToString().Equals( actionElements[1] ) )
			                                    		return true;
			                                    	return false;
			                                    } );

			objectCollector.Add( globalCounter, foundObjects );
			globalCounter++;
		}

		private void searchFactList(List<iThinkFact> FactList)
		{
			List<iThinkFact> foundObjects = new List<iThinkFact>();
			foundObjects = FactList.FindAll(delegate(iThinkFact checkedFacts)
			                                {
			                                	if(checkedFacts.getName().Equals(actionElements[1]))
			                                		return true;
			                                	return false;
			                                });

			foreach(iThinkFact fact in foundObjects){
				//Debug.LogWarning(fact.ToString());
				factCollector.Add(factCounter,fact.getObjects());
				factCounter++;
			}
		}

		/// Executes unification by cartesian product (iterative or recursive).
		private void unificator( int method )
		{
			switch ( method )
			{
				case 1:
					combinations = CartesianProductIterative();
					break;
				case 2:
					combinations = CartesianProductRecursive();
					break;
			}
		}

		private List<GameObject[]> CartesianProductIterative()
		{
			var accum = new List<GameObject[]>();
			if ( objectCollector.Count > 0 )
			{
				var enumStack = new Stack<List<GameObject>.Enumerator>();
				var itemStack = new Stack<GameObject>();
				int index = objectCollector.Count - 1;
				var enumerator = objectCollector[index].GetEnumerator();
				while ( true )
					if ( enumerator.MoveNext() )
				{
					itemStack.Push( enumerator.Current );
					if ( index == 0 )
					{
						accum.Add( itemStack.ToArray() );
						itemStack.Pop();
					}
					else
					{
						enumStack.Push( enumerator );
						enumerator = objectCollector[--index].GetEnumerator();
					}
				}
				else
				{
					if ( ++index == objectCollector.Count )
						break;
					itemStack.Pop();
					enumerator = enumStack.Pop();
				}
			}
			return accum;
		}

		private List<GameObject[]> CartesianProductRecursive()
		{
			List<GameObject[]> accum = new List<GameObject[]>();
			if ( objectCollector.Count > 0 )
			{
				CartesianRecursion( accum, new Stack<GameObject>(), objectCollector.Count - 1 );
			}
			return accum;
		}

		private void CartesianRecursion( List<GameObject[]> accum, Stack<GameObject> stack, int index )
		{
			foreach ( GameObject item in objectCollector[index] )
			{
				stack.Push( item );
				if ( index == 0 )
					accum.Add( stack.ToArray() );
				else
				{
					CartesianRecursion( accum, stack, index - 1 );
				}
				stack.Pop();
			}
		}

		/// Clears the object's data values.
		private void initData()
		{
			objectCollector.Clear();
			factCollector.Clear();
			tempActionList.Clear();
			finalizeCombos.Clear();
			globalCounter = 0;
			factCounter = 0;
		}
	}
}