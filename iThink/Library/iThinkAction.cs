using System;
using System.Collections.Generic;
using UnityEngine;

/*! @class iThinkAction
 *	@brief iThinkAction describes a base (abstract) Action and provides useful methods concerning
 *	its status, initialization and functionality.
 *
 *	An action in iThink can be described by its \a name, its \a preconditions and its \a effects.
 *	Its functions of interest are isApplicable() and applyEffect(), described below.
 *	If the user chooses the 1-parameter constructor, it is suggested to override the initPreconditions() and initEffects()
 *  functions and add the precondition and effect facts there.
 */
namespace iThinkLibrary{
	public class iThinkAction
	{
		protected string name;                      /// The name of the action
		protected List<iThinkFact> preconditions;   /// The list of precondition facts
		protected List<iThinkFact> effects;         /// The list of effect facts

		public iThinkAction( string name ) { this.name = name; }
		public iThinkAction( string name, List<iThinkFact> preconditions, List<iThinkFact> effects )
		{
			this.name = name;
		}

		/*!
		 * Returns a new iThinkState, based on \a State and applies the action's effects to it
		 * @param State The state to be effected
		 * @returns A new iThinkState
		 */
		public virtual iThinkState applyEffects( iThinkState State )
		{
			iThinkState NewState = new iThinkState( State );

			foreach ( iThinkFact effect in this.effects )
			{
				if (effect.getPositive())
					NewState.addFact(effect);
				else
					NewState.delFact(effect);
			}

			return NewState;
		}

		/*! Returns a new iThinkState, applying only positive effects of this action */
		public iThinkState applyPositiveEffects( iThinkState State )
		{
			iThinkState NewState = new iThinkState( State );
			
			foreach ( iThinkFact effect in this.effects )
			{
				if (effect.getPositive())
					NewState.addFact(effect);
			}
			
			return NewState;
		}
		
		/*!
		 * Checks whether the action can be applied on iThinkState \a curState
		 * @param curState The state to be checked
		 * @returns A boolean value
		 */
		public bool isApplicable( iThinkState curState )
		{
			int counter = 0;

			foreach ( iThinkFact fact in preconditions )
			{
				//! TODO Get facts of wanted type/name only
				foreach ( iThinkFact checkFact in curState.getFactList() )
				{
					if ( fact.Equals(checkFact) )
						counter++;
				}
			}
			if ( counter == preconditions.Count ){
				//Debug.LogError("Found applicable! " + this.ToString());
				return true;
			}
			//if ( this.name.StartsWith("SFPSPlace") )
			//	Debug.Log("Unequal: " + counter + " of " + preconditions.Count + " - act: " + this.ToString());
			return false;
		}

		/// Used for initialization of the action's preconditions
		public virtual void initPreconditions() { preconditions = new List<iThinkFact>(); }
		/// Used for initialization of the action's effects
		public virtual void initEffects() { effects = new List<iThinkFact>(); }

		public string getName() { return name; }
		public List<iThinkFact> getEffects() { return effects; }
		public List<iThinkFact> getPreconditions() { return preconditions; }
		public override string ToString()
		{
			String s = name + "() - Prec: ";
			foreach ( iThinkFact f in preconditions )
				s += f.ToString();
			s += " - Effects: ";
			foreach ( iThinkFact f in effects )
				s += f.ToString();
			return s;
		}
		
		public string ToStringLite()
		{
			String s = name + "()-";
			foreach ( iThinkFact f in preconditions )
			{
				s += f.ToString();
			}
			return s;
		}
		
		public bool areaCheck(string s) {
			foreach ( iThinkFact f in preconditions ) {
				if ( f.getName().Equals("npc-at") && f.getObj(0).name.StartsWith(s) )
					return true;
			}
			return false;
		}
		
		public bool nonAreaAction() {
			foreach ( iThinkFact f in preconditions ) {
				if ( f.getName().Equals("npc-at") )
					return false;
			}
			return true;
		}
	}
}