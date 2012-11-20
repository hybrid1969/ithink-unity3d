using UnityEngine;

class ActionBwUnStack : iThinkAction
{
    GameObject Top, Below;
    public ActionBwUnStack( string name, GameObject top, GameObject below )
        : base( name )
    {
        Top = top;
        Below = below;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        preconditions.Add( new iThinkFact( "on", Top, Below ) );
        preconditions.Add( new iThinkFact( "clear", Top ) );
        preconditions.Add( new iThinkFact( "gripEmpty") );
    }

    public override void initEffects()
    {
        effects.Add( new iThinkFact( "clear", Below ) );
        effects.Add( new iThinkFact( "on", false, Top, Below ) );
        effects.Add( new iThinkFact( "clear", false, Top ) );
        effects.Add( new iThinkFact( "gripEmpty", false ) );
        effects.Add( new iThinkFact( "holding", Top ) );
    }

}
