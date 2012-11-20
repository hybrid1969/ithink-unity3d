using UnityEngine;

class ActionBwStack : iThinkAction
{
    GameObject Top, Below;
    public ActionBwStack( string name, GameObject top, GameObject below )
        : base( name )
    {
        Top = top;
        Below = below;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        preconditions.Add( new iThinkFact( "clear", Below ) );
        preconditions.Add( new iThinkFact( "holding", Top ) );
    }

    public override void initEffects()
    {
        effects.Add( new iThinkFact( "holding", false, Top ) );
        effects.Add( new iThinkFact( "clear", false, Below ) );
        effects.Add( new iThinkFact( "clear", Top ) );
        effects.Add( new iThinkFact( "gripEmpty" ) );
        effects.Add( new iThinkFact( "on", Top, Below ) );
    }

}