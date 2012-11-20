using UnityEngine;

class ActionBwPickUp : iThinkAction
{
    GameObject Obj;
    public ActionBwPickUp( string name, GameObject obj )
        : base( name )
    {
        Obj = obj;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        preconditions.Add( new iThinkFact( "clear", Obj ) );
        preconditions.Add( new iThinkFact( "onTable", Obj ) );
        preconditions.Add( new iThinkFact( "gripEmpty" ) );
    }

    public override void initEffects()
    {
        effects.Add( new iThinkFact( "clear", false, Obj ) );
        effects.Add( new iThinkFact( "onTable", false, Obj ) );
        effects.Add( new iThinkFact( "gripEmpty", false ) );
        effects.Add( new iThinkFact( "holding", Obj ) );
    }
}
