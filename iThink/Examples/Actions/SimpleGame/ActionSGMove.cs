using UnityEngine;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;

class ActionSGMove : iThinkAction
{
    GameObject From, To, Dir;
    public ActionSGMove( string name, GameObject from, GameObject to, GameObject dir )
        : base( name )
    {
        From = from;
        To = to;
        Dir = dir;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        base.initPreconditions();
        preconditions.Add( new iThinkFact( "npcAt", From ) );
        preconditions.Add( new iThinkFact( "npcFacing", Dir ) );
        preconditions.Add( new iThinkFact( "adjacent", From, To, Dir ) );
    }

    public override void initEffects()
    {
        base.initEffects();
        effects.Add( new iThinkFact( "npcAt", To ) );
        effects.Add( new iThinkFact( "npcAt", false, From ) );
    }
}
