## This section will change soon, and the whole project will move to github. ##


---


iThink is a classical planning library implemented in C# for the popular game engine Unity. The library is based on the popular paradigm of STRIPS planning but is implemented in a way that literals use directly game objects for representing facts about the game-world. The library can be used for any type of deliberation problem that can be represented using the STRIPS form of initial state, action schemas, and goal conditions, using sets of positive literals. In particular some specialized approaches are under development to allow a more efficient use of the planning library to non-player characters (NPCs) that are aware of the topology of the game-world.

For more information check out "iThink: A Library for Classical Planning in Video-games", Vassileios-Marios Anastassiou, Panagiotis Diamantopoulos, Stavros Vassos, Manolis Koubarakis, In Proceedings of the 7th Hellenic Conference on Artificial Intelligence (SETN), Lamia, Greece, 2012. ([PDF](http://stavros.lostre.org/files/ADVK12iThink.pdf))



---

ChangeLog:

  * **v0.1**: Prototype version has been released as part of the authors' B.Sc thesis.
  * **v0.2**: Several improvements in the iThinkPlanner subsystem and several performance tweaks overall.
  * **v0.2.1**: Implemented graphical wizards (script generation tools).
  * **v0.3**: Integrated FF-lite (a FastForward based heuristic) and various small fixes.
  * **v0.4**: Under development