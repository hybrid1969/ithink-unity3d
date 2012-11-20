using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;

class ActionSGTurn : iThinkAction
{
    GameObject Fdir, Tdir;
    public ActionSGTurn( string name, GameObject fdir, GameObject tdir )
        : base( name )
    {
        Fdir = fdir;
        Tdir = tdir;

        initPreconditions();
        initEffects();
    }

    public override void initPreconditions()
    {
        base.initPreconditions();
        preconditions.Add( new iThinkFact("npcFacing", Fdir ) );
        preconditions.Add( new iThinkFact("canTurn", Fdir, Tdir ) );
    }

    public override void initEffects()
    {
        base.initEffects();
        effects.Add( new iThinkFact("npcFacing", Tdir ) );
        effects.Add( new iThinkFact("npcFacing", false, Fdir ) );
    }

}
