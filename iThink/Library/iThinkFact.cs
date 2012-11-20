using System;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0660, 0661

namespace iThinkLibrary{
	/*! @class iThinkFact
	 *	@brief iThinkFact describes a fact that can be used in the planning process and in the description of actions.
	 *
	 *	A fact describes a logic predicate. It consists of:
	 *	- its \a name
	 *	- the sign of the literal (whether it is a \a positive literal or not)
	 *	- a list of its propositions (which are actually in-game \a GameObjects).
	 */
	public class iThinkFact
	{
		protected List<GameObject> objects; /// The list of instances-parameters of the logic literal.
		protected string name;              /// The name of the literal.
		protected bool positive;            /// Describes whether the literal is positive or not.

		/// A new fact is positive by default
		public iThinkFact( string Name, params GameObject[] objs )
		{
			name = Name;
			positive = true;
			objects = new List<GameObject>();

			for ( int i = 0 ; i < objs.Length ; i++ )
				addObj( objs[i] );
		}

		/// If a new fact is negative, it must be specified with a second parameter in its constructor, with value 'false'.
		public iThinkFact( string Name, bool pos, params GameObject[] objs )
		{
			name = Name;
			positive = pos;
			objects = new List<GameObject>();

			for ( int i = 0 ; i < objs.Length ; i++ )
				addObj( objs[i] );
		}

		public iThinkFact( List<String> factTokens )
		{
			int len = factTokens.Count;

			// TODO: Exceptions
			if (len < 1)
			{
				Debug.LogWarning("Invalid iThinkFact generated");
				name = "NULL";
				positive = true;
				objects = new List<GameObject>();
			}

			name = factTokens[0];
			positive = true;
			objects = new List<GameObject>();

			for ( int i = 1 ; i < len ; i++ )
				addObj( GameObject.Find(factTokens[i]) );
		}

		public string getName() { return name; }
		public bool getPositive() { return positive; }

		public void addObj( GameObject obj ) { objects.Add( obj ); }
		public GameObject getObj( int index ) { return objects[index]; }
		public int getObjCount() { return objects.Count; }

		public List<GameObject> getObjects() {return objects;}

		/// A positive iThinkFact is added to the \a State, and a negative one is removed from it.
		public void applyFact( iThinkState State )
		{
			if ( this.positive == false )
				State.delFact( this );
			else
				State.addFact( this );
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			iThinkFact other = obj as iThinkFact;
			if (other == null)
				return false;

			return this.Equals( (iThinkFact)obj );
		}

		public bool Equals(iThinkFact fact2)
		{
			if ( !this.getName().Equals( fact2.getName() ) )
				return false;

			if ( this.objects.Count != fact2.objects.Count )
				return false;

			for ( int i = 0 ; i < this.objects.Count ; i++ )
			{
				if ( !this.getObj( i ).Equals(fact2.getObj( i )) )
					return false;
			}

			return true;
		}

		public static bool operator ==( iThinkFact fact1, iThinkFact fact2 )
		{
			if ( System.Object.ReferenceEquals( fact1, fact2 ) )
				return true;

			if ( (object)fact1 == null || (object)fact2 == null )
				return false;

			if ( !fact1.getName().Equals( fact2.getName() ) )
				return false;

			if ( fact1.objects.Count != fact2.objects.Count )
				return false;

			for ( int i = 0 ; i < fact1.objects.Count ; i++ )
			{
				if ( !fact1.getObj( i ).Equals(fact2.getObj( i )) )
					return false;
			}

			return true;
		}

		public static bool operator !=( iThinkFact fact1, iThinkFact fact2 )
		{
			return !( fact1 == fact2 );
		}

		public override string ToString()
		{
			string s;
			if (!positive)
				s = "[(not " + name+")";
			else
				s = "[("+name+")";

			foreach ( GameObject o in objects )
			{
				//if ( o != null)
				s += "+" + o.name;
			}
			return s+"]";
		}
	}

}