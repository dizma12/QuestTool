Welcome to QuestMaker tool.

You can find the documentation and and quickstart guide on:
Unity Toolbar -> QuestMaker -> Documentation

Quick Start Guide

QuestMaker is a node-based quest authoring tool. You build a quest visually as a graph 
compile it into a QuestSO asset, and the runtime systems execute it in play mode
the game never touches the graph.
Pipeline: .qmgraph  →  Quest Compiler  →  QuestSO asset  →  QuestManager  →  gameplay.

10 step guide:

1. Right-click in the Project window → Create → QuestMaker → Quest Graph. The file name becomes the quest's ID.
2. Double-click the .qmgraph file to open the graph editor.
3. Add a Start node, set Quest Type and Description.
4. Add an Objective node, connect it to the Start node's Objectives port, add step blocks inside it (Collect, Slay, ...).
5. Optionally add Prerequisite, Reward and Acquisition context nodes and connect them to the Context port. Add blocks inside each.
6. In the Acquisition node, drag a Quest Giver PREFAB into the Hand-in / Turn-in slots.
7. Open QuestMaker → Compiler Window, drag the .qmgraph asset into the Graph field, it compiles immediately.
8. Click 'Save as prefab', the quest is written to Assets/QuestMaker/Resources/Quests/<ID>/<ID>.asset.
9. Make sure the scene has the manager prefabs (Assets/QuestMaker/Prefabs/Managers/), the Player, the UI prefabs and your Quest Giver instances.
10. Press Play. Walk to the giver, press E, accept the quest, play it through, return, press E, turn it in.