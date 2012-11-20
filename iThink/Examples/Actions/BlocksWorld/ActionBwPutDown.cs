using UnityEngine;

class ActionBwPutDown : iThinkAction
{
    GameObject Obj;
    public ActionBwPutDown( string name, GameObject obj)
        : base( name )
    {
        Obj = obj;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        preconditions.Add( new iThinkFact( "holding", Obj ) );
    }

    public override void initEffects()
    {
        effects.Add( new iThinkFact( "clear", Obj ) );
        effects.Add( new iThinkFact( "onTable", Obj ) );
        effects.Add( new iThinkFact( "gripEmpty") );
        effects.Add( new iThinkFact( "holding", false, Obj ) );
    }

}
