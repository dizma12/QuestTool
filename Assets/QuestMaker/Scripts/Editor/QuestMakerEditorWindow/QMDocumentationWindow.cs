using UnityEditor;
using UnityEngine;

namespace QuestMaker.Editor.Window
{
    // This script was based on the documentation window of: https://assetstore.unity.com/packages/tools/game-toolkits/economy-engine-1-4-357958
    // Changes made with AI.
    public class QMDocumentationWindow : EditorWindow
    {
        private enum InfoSection
        {
            QuickStart,
            CreatingGraphs,
            StartNode,
            ContextAndBlocks,
            Objectives,
            PrerequisiteBlocks,
            RewardBlocks,
            Acquisition,
            SpecialEvents,
            Compiling,
            Items,
            SceneSetup,
            RuntimeFlow,
            EventReference,
            Limitations,
            Extending
        }

        private InfoSection currentSection = InfoSection.QuickStart;
        private Vector2 scrollPos;

        [MenuItem("QuestMaker/Documentation")]
        public static void ShowWindow()
        {
            var wnd = GetWindow<QMDocumentationWindow>();
            wnd.titleContent = new GUIContent("QuestMaker Documentation");

            Vector2 windowSize = new(760, 580);
            wnd.minSize = windowSize;
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            GUIStyle title = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18
            };
            GUILayout.Label("QUESTMAKER DOCUMENTATION", title);
            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            DrawSidebar();

            EditorGUILayout.BeginVertical("box");
            DrawSectionHeader();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            DrawSectionContent();
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        #region Sidebar

        private void DrawSidebar()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(190));
            GUILayout.Space(8);

            GUILayout.Label("Getting Started", EditorStyles.boldLabel);
            GUILayout.Space(2);
            DrawSidebarButton(InfoSection.QuickStart, "Quick Start");
            DrawSidebarButton(InfoSection.CreatingGraphs, "Creating a Quest Graph");

            GUILayout.Space(6);
            GUILayout.Label("Graph Editor", EditorStyles.boldLabel);
            GUILayout.Space(2);
            DrawSidebarButton(InfoSection.StartNode, "The Start Node");
            DrawSidebarButton(InfoSection.ContextAndBlocks, "Context & Block Nodes");
            DrawSidebarButton(InfoSection.Objectives, "Objectives & Steps");
            DrawSidebarButton(InfoSection.PrerequisiteBlocks, "Prerequisite Blocks");
            DrawSidebarButton(InfoSection.RewardBlocks, "Reward Blocks");
            DrawSidebarButton(InfoSection.Acquisition, "Acquisition / Givers");
            DrawSidebarButton(InfoSection.SpecialEvents, "Special Events");

            GUILayout.Space(6);
            GUILayout.Label("Pipeline", EditorStyles.boldLabel);
            GUILayout.Space(2);
            DrawSidebarButton(InfoSection.Compiling, "Compiling a Quest");
            DrawSidebarButton(InfoSection.Items, "Items");

            GUILayout.Space(6);
            GUILayout.Label("Runtime", EditorStyles.boldLabel);
            GUILayout.Space(2);
            DrawSidebarButton(InfoSection.SceneSetup, "Scene Setup");
            DrawSidebarButton(InfoSection.RuntimeFlow, "Runtime Flow");
            DrawSidebarButton(InfoSection.EventReference, "Event Reference");
            DrawSidebarButton(InfoSection.Limitations, "Current Limitations");

            GUILayout.Space(6);
            GUILayout.Label("Extending", EditorStyles.boldLabel);
            GUILayout.Space(2);
            DrawSidebarButton(InfoSection.Extending, "Extending the Tool");

            EditorGUILayout.EndVertical();
        }

        private void DrawSidebarButton(InfoSection section, string label)
        {
            bool isActive = currentSection == section;
            GUIStyle style = new GUIStyle(EditorStyles.miniButtonLeft)
            {
                alignment = TextAnchor.MiddleLeft,
                fontSize = 11,
                fixedHeight = 24
            };

            bool pressed = GUILayout.Toggle(isActive, label, style);
            if (pressed && !isActive)
            {
                currentSection = section;
                scrollPos = Vector2.zero;
            }
        }

        #endregion

        #region Header

        private void DrawSectionHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(GetSectionTitle(), EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private string GetSectionTitle()
        {
            switch (currentSection)
            {
                case InfoSection.QuickStart: return "Quick Start";
                case InfoSection.CreatingGraphs: return "Creating a Quest Graph";
                case InfoSection.StartNode: return "The Start Node";
                case InfoSection.ContextAndBlocks: return "Context & Block Nodes";
                case InfoSection.Objectives: return "Objectives & Steps";
                case InfoSection.PrerequisiteBlocks: return "Prerequisite Blocks";
                case InfoSection.RewardBlocks: return "Reward Blocks";
                case InfoSection.Acquisition: return "Acquisition / Quest Givers";
                case InfoSection.SpecialEvents: return "Special Events";
                case InfoSection.Compiling: return "Compiling a Quest";
                case InfoSection.Items: return "Items";
                case InfoSection.SceneSetup: return "Scene Setup";
                case InfoSection.RuntimeFlow: return "Runtime Flow";
                case InfoSection.EventReference: return "Event Reference";
                case InfoSection.Limitations: return "Current Limitations";
                case InfoSection.Extending: return "Extending the Tool";
                default: return currentSection.ToString();
            }
        }

        #endregion

        #region Section Content

        private void DrawSectionContent()
        {
            GUIStyle header = new GUIStyle(EditorStyles.boldLabel) { fontSize = 14 };
            GUIStyle subHeader = new GUIStyle(EditorStyles.boldLabel);
            GUIStyle body = new GUIStyle(EditorStyles.wordWrappedLabel);

            GUILayout.Space(6);

            switch (currentSection)
            {
                case InfoSection.QuickStart:
                    GUILayout.Label("From Graph to Gameplay in 10 Steps", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "QuestMaker is a node-based quest authoring tool. You build a quest visually as a graph, " +
                        "compile it into a QuestSO asset, and the runtime systems execute it in play mode — " +
                        "the game never touches the graph.\n\n" +
                        "Pipeline: .qmgraph  →  Quest Compiler  →  QuestSO asset  →  QuestManager  →  gameplay.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Steps", subHeader);
                    GUILayout.Label(
                        "1. Right-click in the Project window → Create → QuestMaker → Quest Graph. The file name becomes the quest's ID.\n" +
                        "2. Double-click the .qmgraph file to open the graph editor.\n" +
                        "3. Add a Start node, set Quest Type and Description.\n" +
                        "4. Add an Objective node, connect it to the Start node's Objectives port, add step blocks inside it (Collect, Slay, ...).\n" +
                        "5. Optionally add Prerequisite, Reward and Acquisition context nodes and connect them to the Context port. Add blocks inside each.\n" +
                        "6. In the Acquisition node, drag a Quest Giver PREFAB into the Hand-in / Turn-in slots.\n" +
                        "7. Open QuestMaker → Compiler Window, drag the .qmgraph asset into the Graph field — it compiles immediately.\n" +
                        "8. Click 'Save as prefab' — the quest is written to Assets/QuestMaker/Resources/Quests/<ID>/<ID>.asset.\n" +
                        "9. Make sure the scene has the manager prefabs (Assets/QuestMaker/Prefabs/Managers/), the Player, the UI prefabs and your Quest Giver instances.\n" +
                        "10. Press Play. Walk to the giver, press E, accept the quest, play it through, return, press E, turn it in.",
                        body);
                    break;

                case InfoSection.CreatingGraphs:
                    GUILayout.Label("Creating a Quest Graph", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Create → QuestMaker → Quest Graph (Project window context menu) creates a .qmgraph file. " +
                        "Existing examples live in Assets/QuestMaker/QuestGraphs/.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("What Happens Behind the Scenes", subHeader);
                    GUILayout.Label(
                        "• The file is a Unity Graph Toolkit graph of type QMGraph.\n" +
                        "• A scripted importer (QMGraphImporter) attaches a small QMGraphAssetFile metadata object " +
                        "holding the asset's GUID. This metadata object is what you later drag into the Compiler Window.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Naming Matters", subHeader);
                    GUILayout.Label(
                        "The file name IS the quest ID. The compiler names the QuestSO after the graph, and the ID " +
                        "is what prerequisite chains, the quest map and the on-screen overlay all use. " +
                        "Rename thoughtfully and recompile after renaming.\n\n" +
                        "Double-clicking the file opens Unity's graph editor window, where you build the quest from nodes.",
                        body);
                    break;

                case InfoSection.StartNode:
                    GUILayout.Label("The Start Node", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Every graph must contain exactly one Start node — compilation throws an error without it.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Options", subHeader);
                    GUILayout.Label(
                        "Quest Type\n" +
                        "    Main, Side, Repeatable, Timed, Daily, Unlockable — categorization metadata stored on the quest.\n\n" +
                        "Description\n" +
                        "    The quest's description text.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Output Ports", subHeader);
                    GUILayout.Label(
                        "Special Event  (circle connector)\n" +
                        "    Connect Special Event nodes here.\n\n" +
                        "Context  (arrowhead connector)\n" +
                        "    Connect Prerequisite, Reward and Acquisition context nodes here.\n\n" +
                        "Objectives  (arrowhead connector)\n" +
                        "    Connect the FIRST Objective node here. Further objectives chain from that node.",
                        body);
                    break;

                case InfoSection.ContextAndBlocks:
                    GUILayout.Label("Context & Block Nodes", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "QuestMaker uses Graph Toolkit's context/block model: context nodes are containers you wire " +
                        "into the graph flow, and block nodes are rows you add INSIDE a context node. Each block type " +
                        "declares which contexts accept it, so the editor only offers valid blocks.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("The Four Context Node Types", subHeader);
                    GUILayout.Label(
                        "Objective  →  connects to the Objectives port, then chains.\n" +
                        "    Multiple allowed — one per quest stage. Accepts step blocks.\n\n" +
                        "Prerequisite  →  connects to the Context port.\n" +
                        "    One per graph. Accepts: Level, Required Quest, Time Constraint, Item, Reputation.\n\n" +
                        "Reward  →  connects to the Context port.\n" +
                        "    One per graph. Accepts: Exp, Item, Ability, Reputation.\n\n" +
                        "Acquisition  →  connects to the Context port.\n" +
                        "    One per graph. Accepts: Hand-in Giver, Turn-in Giver.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("One-Per-Graph Rule", subHeader);
                    GUILayout.Label(
                        "Adding a second Prerequisite/Reward/Acquisition node logs a warning on the node " +
                        "('Can not have multiple instances of ...') and the compiler skips duplicates.",
                        body);
                    break;

                case InfoSection.Objectives:
                    GUILayout.Label("Objectives & Steps", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Each Objective node is one stage of the quest. Its blocks are the steps the player must " +
                        "complete — ALL steps of an objective must finish before the quest advances to the next objective.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Ordering", subHeader);
                    GUILayout.Label(
                        "• The objective connected to the Start node's Objectives port runs first.\n" +
                        "• Chain further objectives from the 'Next Objective' output port. They activate strictly in sequence.\n" +
                        "• The Description option is shown on the quest overlay while the objective is active, above the " +
                        "per-step progress lines (e.g. 'Kill Boars' then 'Boar slained 7/10').",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Step Blocks", subHeader);
                    GUILayout.Label(
                        "Collect — Item, Amount.\n" +
                        "    Completed when the item count is reached (pickups and existing inventory both count).\n\n" +
                        "Slay — Enemy ID (string), Amount.\n" +
                        "    Completed when that many enemies with the matching ID die.\n\n" +
                        "Loot — Item, Amount.\n" +
                        "    Same event as Collect — currently identical behavior.\n\n" +
                        "Craft — Item, Amount.  (*)\n" +
                        "Deliver — Item, NPC ID.  (*)\n" +
                        "Talk — NPC ID.  (*)\n" +
                        "Explore — Area ID.  (*)\n\n" +
                        "(*) These steps work, but no shipped game system fires their events yet — see the " +
                        "Event Reference section for what your game code must call.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Warning: Interact Steps", subHeader);
                    GUILayout.Label(
                        "Do NOT author Interact blocks. The runtime step is not implemented and throws " +
                        "NotImplementedException the moment the quest starts.",
                        body);
                    break;

                case InfoSection.PrerequisiteBlocks:
                    GUILayout.Label("Prerequisite Blocks", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Prerequisite blocks go inside the Prerequisite context node and gate when a quest becomes " +
                        "available at its hand-in giver.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Checked at Runtime", subHeader);
                    GUILayout.Label(
                        "Level — Level (int)\n" +
                        "    Player level must be greater than or equal to the value.\n\n" +
                        "Required Quest — Quest (QuestSO asset)\n" +
                        "    That quest must be completed first. This is how quest chains are built.\n\n" +
                        "Item — Item, Amount\n" +
                        "    Inventory must contain the amount.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Compiled but NOT Evaluated Yet", subHeader);
                    GUILayout.Label(
                        "Reputation — Faction ID, Amount\n" +
                        "Time Constraint — None / Day / Night\n\n" +
                        "Both are stored in the QuestSO but the runtime never checks them.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("When Prerequisites Are Re-Checked", subHeader);
                    GUILayout.Label(
                        "• At scene start.\n" +
                        "• Whenever the player levels up.\n" +
                        "• For chained quests, whenever a prerequisite quest is turned in.",
                        body);
                    break;

                case InfoSection.RewardBlocks:
                    GUILayout.Label("Reward Blocks", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Reward blocks go inside the Reward context node and are granted by the RewardGiver " +
                        "when the quest is turned in.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Granted at Runtime", subHeader);
                    GUILayout.Label(
                        "Exp amount — int\n" +
                        "    Added to the player. Level-ups can unlock other quests immediately.\n\n" +
                        "Item — Item, Amount\n" +
                        "    Added to the inventory.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Compiled but NOT Granted Yet", subHeader);
                    GUILayout.Label(
                        "Ability — Ability ID (string)\n" +
                        "Reputation — Faction ID, Amount\n\n" +
                        "Both are stored in the QuestSO but granting only logs a 'not implemented' warning.",
                        body);
                    break;

                case InfoSection.Acquisition:
                    GUILayout.Label("Acquisition / Quest Givers", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Acquisition blocks go inside the Acquisition context node and define who offers the quest " +
                        "and who accepts the finished quest.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Blocks", subHeader);
                    GUILayout.Label(
                        "Hand-in Giver — QuestGiver prefab\n" +
                        "    Who offers the quest.\n\n" +
                        "Turn-in Giver — QuestGiver prefab\n" +
                        "    Who accepts the finished quest. May be the same giver.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Drag, Don't Search", subHeader);
                    GUILayout.Label(
                        "Drag the prefab from the Project window into the slot — the Unity object-picker search " +
                        "cannot find QuestGiver prefabs (the option tooltip says the same).",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("How the Link Works", subHeader);
                    GUILayout.Label(
                        "What is stored is not a scene reference but the prefab asset's GUID — the same GUID a " +
                        "QuestGiver component stamps on itself as its ID. That is how compiled quests and scene " +
                        "givers find each other. See Scene Setup for the giver-side rules.",
                        body);
                    break;

                case InfoSection.SpecialEvents:
                    GUILayout.Label("Special Events", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Connect a Special Event node to the Start node's circle port.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Options", subHeader);
                    GUILayout.Label(
                        "EventID\n" +
                        "    Free string, e.g. 'Spawn_super_boss'.\n\n" +
                        "Trigger Moment\n" +
                        "    OnStarted / OnCompleted / OnFailed.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Authoring-Ready, Runtime-Pending", subHeader);
                    GUILayout.Label(
                        "The event data is compiled into the quest asset, but the runtime never fires it yet — " +
                        "there is a FireSpecialEvent hook on the quest event bus with no caller.",
                        body);
                    break;

                case InfoSection.Compiling:
                    GUILayout.Label("Compiling a Quest", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Open QuestMaker → Compiler Window.\n\n" +
                        "1. Drag the .qmgraph asset into the Graph field.\n" +
                        "2. Compilation runs immediately on assignment. The Node count field fills in, and the Unity " +
                        "console logs each module the compiler builds (plus warnings for anything wrong).\n" +
                        "3. If compilation produced a valid quest, 'Save as prefab' enables. Click it to write the asset.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Where the Asset Goes", subHeader);
                    GUILayout.Label(
                        "Assets/QuestMaker/Resources/Quests/<QuestID>/<QuestID>.asset\n\n" +
                        "If it already exists, the compiler copies the new data over the old asset IN PLACE, preserving " +
                        "the asset's GUID — so other graphs that reference this quest as a prerequisite never break " +
                        "when you recompile.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("What Compilation Actually Does", subHeader);
                    GUILayout.Label(
                        "1. The Start node writes quest type, description and any special events into a QuestInfoModule.\n" +
                        "2. The Context port is followed: each context node fetches its builder module (PrerequisiteModule, " +
                        "RewardModule, AcquisitionModule) from a registry and lets its blocks fill it in.\n" +
                        "3. The Objectives port is followed: each Objective node creates its own ObjectiveModule, its step " +
                        "blocks add QuestStepData entries, and chained objectives are processed in connection order.\n" +
                        "4. Every module's Build(quest) runs, assembling the final QuestSO.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Warnings to Watch in the Console", subHeader);
                    GUILayout.Label(
                        "• Unconnected Context/Objectives ports.\n" +
                        "• Duplicate context node types.\n" +
                        "• Null Item/Quest/Giver references in blocks.\n" +
                        "• Amounts less than or equal to 0.",
                        body);
                    break;

                case InfoSection.Items:
                    GUILayout.Label("Items", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Steps and rewards reference Item assets: Create → QuestMaker → ScriptableObjects → Item.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Behaviour", subHeader);
                    GUILayout.Label(
                        "• An item's ID is auto-assigned from its asset GUID and shown read-only.\n" +
                        "• Its Name syncs from the asset file name.\n" +
                        "• Duplicating an item asset automatically gives the copy its own fresh ID.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Making Items Collectable in the World", subHeader);
                    GUILayout.Label(
                        "Put the CollectableItem component on a prefab with a trigger collider, assign the Item asset " +
                        "and set Items Per Pickup (how many items one pickup awards). " +
                        "The player collects it on touch — no key press.",
                        body);
                    break;

                case InfoSection.SceneSetup:
                    GUILayout.Label("Scene Setup", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "The demo scene Assets/QuestMaker/Scenes/Demo_Scene.unity has everything wired; use it as the reference. " +
                        "All required prefabs live under Assets/QuestMaker/Prefabs/.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Managers (required)", subHeader);
                    GUILayout.Label(
                        "ReferenceManager — service locator every other system registers with. Initializes first.\n" +
                        "EventManager — owns the typed event buses (game, quest, player, UI).\n" +
                        "QuestManager — loads all quests from Resources/Quests, tracks statuses, starts/turns-in quests, checks prerequisites.\n" +
                        "InventoryManager — owns the runtime Inventory; listens for item events.\n" +
                        "RewardGiver — grants exp/item rewards when a quest completes.\n\n" +
                        "Script execution order is already declared in code — just make sure all of them exist in the scene.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Quest Givers", subHeader);
                    GUILayout.Label(
                        "1. Duplicate Prefabs/QuestGivers/BaseQuestGiver_ONLY_USED_FOR_DUPLICATION.prefab and name the copy (e.g. 'George').\n" +
                        "2. The QuestGiver component stamps its own prefab GUID into its read-only ID field automatically. " +
                        "This GUID is what Acquisition blocks store — so each distinct giver must be its OWN prefab asset, " +
                        "and the same prefab you drag into the graph must be the one you place in the scene.\n" +
                        "3. Place instances in the scene. The child QuestIcons object drives the floating status icons:\n\n" +
                        "    Yellow !  — has a quest ready to hand out.\n" +
                        "    White !   — has a quest, requirements not met yet.\n" +
                        "    Yellow ?  — ready to accept a turn-in.\n" +
                        "    White ?   — player has the quest in progress; giver awaits completion.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Player", subHeader);
                    GUILayout.Label(
                        "Prefabs/Player/Player.prefab bundles:\n" +
                        "• Player — level/exp.\n" +
                        "• PlayerController — WASD/arrow movement, Space spin attack.\n" +
                        "• PlayerInteractions — E interacts with the nearest QuestGiver in range; auto-collects collectables on touch.\n" +
                        "• PlayerHitbox — damage during the spin.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("UI Prefabs", subHeader);
                    GUILayout.Label(
                        "QuestGiverWindow — opens on interacting with a giver: accept buttons for available quests, " +
                        "turn-in buttons for finishable ones, disabled entries for quests with missing requirements. " +
                        "Closes when the player walks away.\n\n" +
                        "QuestOverlay — on-screen tracker: quest ID, active objective description, live per-step progress; " +
                        "turns green with 'Completed!' when ready to turn in.\n\n" +
                        "PlayerInfo — level / exp readout.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Enemies (for Slay quests)", subHeader);
                    GUILayout.Label(
                        "• Prefabs/Enemy/Enemy.prefab has an ID string — it must match the Enemy ID typed into Slay blocks. " +
                        "On death it fires the enemy-killed event.\n" +
                        "• DummyEnemySpawner is a demo convenience: whenever an objective with Slay steps activates, it spawns " +
                        "one enemy per step configured to satisfy the whole step (one kill fires the event Amount times). " +
                        "Replace it with real spawning in a real game.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Collectables (for Collect quests)", subHeader);
                    GUILayout.Label(
                        "• The collectible Item asset's Prefab field must point at a prefab with a CollectableItem " +
                        "component and a trigger collider.\n" +
                        "• DummyCollectableSpawner is the collect-side counterpart of the enemy spawner: whenever an " +
                        "objective with Collect steps activates, it spawns the step item's prefab at its spawn points.\n" +
                        "• Spawn Single ON (default): spawns one pickup boosted to grant the step's full amount — one " +
                        "touch completes the step, mirroring the enemy spawner.\n" +
                        "• Spawn Single OFF: spawns ceil(step amount / Items Per Pickup) pickups.\n" +
                        "• Same demo-tier caveats as the enemy spawner — replace it with real placement in a real game.",
                        body);
                    break;

                case InfoSection.RuntimeFlow:
                    GUILayout.Label("Runtime Flow", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "QuestManager loads EVERY QuestSO under Resources/Quests (anything the compiler saved is " +
                        "auto-discovered — no registration step) and wraps each in a runtime Quest. Quest-prerequisite " +
                        "links are inverted into next-in-chain references so completing a quest immediately re-evaluates " +
                        "its dependents.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Status Lifecycle", subHeader);
                    GUILayout.Label(
                        "MISSING_REQUIRMENTS  →  CAN_START  →  IN_PROGRESS  →  CAN_FINISH  →  COMPLETED\n\n" +
                        "prereq check → accept at hand-in giver → all objectives finished → turn in at turn-in giver",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Accept", subHeader);
                    GUILayout.Label(
                        "Walk into a giver's trigger radius, press E, click the quest button. TryStartQuest re-validates " +
                        "prerequisites, instantiates runtime steps for every objective, and activates the first objective's steps.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Progress", subHeader);
                    GUILayout.Label(
                        "Each active step subscribes to its game event (see Event Reference). Events update counters, the " +
                        "overlay repaints, and when ALL steps of the objective are complete the next objective activates. " +
                        "Collect-type steps also pre-fill from items already in the inventory the moment the objective activates.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Turn In", subHeader);
                    GUILayout.Label(
                        "When the last objective finishes, status becomes CAN_FINISH (overlay goes green, giver shows the " +
                        "yellow ?). Interact with the turn-in giver and click the quest: rewards are granted (exp, items), " +
                        "collected quest items are CONSUMED from the inventory, and chained quests get their prerequisites " +
                        "re-checked — potentially lighting up the next giver on the spot.",
                        body);
                    break;

                case InfoSection.EventReference:
                    GUILayout.Label("Event Reference", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "Runtime steps progress exclusively through the game event bus. What fires each event today:",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Wired End-to-End", subHeader);
                    GUILayout.Label(
                        "Item collected  →  Collect, Loot steps\n" +
                        "    Fired by CollectableItem.Collect() when the player touches a pickup.\n\n" +
                        "Enemy killed  →  Slay steps\n" +
                        "    Fired by Enemy.Die().",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Waiting for Your Game Code", subHeader);
                    GUILayout.Label(
                        "Item crafted  →  Craft steps\n" +
                        "    Your crafting system must call GameEventBus.FireItemCrafted(stack).\n\n" +
                        "Item delivered  →  Deliver steps\n" +
                        "    Call GameEventBus.FireItemDelivered(stack).\n\n" +
                        "NPC talked  →  Talk steps\n" +
                        "    Call GameEventBus.FireNpcTalked(npcID) from your dialogue system.\n\n" +
                        "Area entered  →  Explore steps\n" +
                        "    Call GameEventBus.FireAreaEntered(areaID) from a zone trigger.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("How to Fire an Event", subHeader);
                    GUILayout.Label(
                        "GameEventBus bus = ReferenceManager.Instance\n" +
                        "    .RequestReference<GameEventManager>()\n" +
                        "    .RequestBus<GameEventBus>();\n\n" +
                        "bus.FireNpcTalked(\"npc_george\");",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("Testing Tips", subHeader);
                    GUILayout.Label(
                        "• The project ships with Quantum Console. Player.AddExp and Player.ResetLevel are console " +
                        "commands — useful for testing level prerequisites without grinding.\n" +
                        "• Every system logs verbosely through ConsoleLogger; the Unity console tells you exactly which " +
                        "steps were created, activated and finished.",
                        body);
                    break;

                case InfoSection.Limitations:
                    GUILayout.Label("Current Limitations (As Built)", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "• Interact steps CRASH at quest start (NotImplementedException) — don't author them yet.\n\n" +
                        "• Talk / Deliver / Explore / Craft steps need game systems to fire their events; none ship with the demo.\n\n" +
                        "• Special events compile into the asset but nothing fires or listens to them at runtime.\n\n" +
                        "• Reputation and Ability rewards are stored but only log 'not implemented' when granted; " +
                        "reputation and time-constraint prerequisites are stored but never evaluated.\n\n" +
                        "• No save system — quest progress lives in memory and resets on play-mode exit.\n\n" +
                        "• Quest Type is metadata only; no runtime behavior differs per type (e.g. Repeatable quests are " +
                        "not actually repeatable).\n\n" +
                        "• Loot currently listens to the same event as Collect, so they behave identically.",
                        body);
                    break;

                case InfoSection.Extending:
                    GUILayout.Label("Extending the Tool", header);
                    GUILayout.Space(4);
                    GUILayout.Label(
                        "QuestMaker is built around plug-in seams: graph nodes are auto-discovered from the editor " +
                        "assembly, compiler modules self-register through the capability interfaces they implement, " +
                        "event buses are created lazily on first request, and managers are resolved through the " +
                        "ReferenceManager service locator. Extending the tool almost never means editing existing " +
                        "systems — you add new types next to them.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("Extension Point Map", subHeader);
                    GUILayout.Label(
                        "1. New step types — new things the player can be asked to do.\n" +
                        "2. New prerequisite blocks — new conditions gating quest availability.\n" +
                        "3. New reward blocks — new things granted on turn-in.\n" +
                        "4. New context node categories — entire new quest aspects with their own blocks.\n" +
                        "5. New options on existing nodes — extra fields on any node or block.\n" +
                        "6. New game events and event buses — new signals steps and systems can react to.\n" +
                        "7. New managers / services — systems registered on the ReferenceManager.\n" +
                        "8. World objects — collectables, interactables and damageables.\n" +
                        "9. Custom quest UI — alternative overlays, logs and giver windows.\n" +
                        "10. World systems reacting to quest data — spawners, directors, music, etc.\n" +
                        "11. Runtime special events — finishing the authored-but-unwired hook.",
                        body);

                    GUILayout.Space(10);
                    GUILayout.Label("1. Adding a New Step Type", subHeader);
                    GUILayout.Label(
                        "The most common extension. Four pieces, following the Collect/Slay pattern (example names for " +
                        "a fishing step: FishStepData, FishQuestStep, FishStepBlockNode).\n\n" +
                        "Step data — Data/Steps, QuestMaker.Domain assembly:\n" +
                        "• Subclass QuestStepData and mark it [System.Serializable].\n" +
                        "• Add [SerializeField] fields for everything the designer configures (IDs, amounts, Item references).\n" +
                        "• Follow the StepID convention used by Collect/Slay (e.g. 'Fish_[Trout]_5') so objectives get " +
                        "readable auto-generated IDs.\n" +
                        "• Implement CreateRuntimeStep(IQuestEventSource eventbus) => new FishQuestStep(this, eventbus). " +
                        "This is the factory the runtime Quest calls for every step of every objective.\n\n" +
                        "Runtime step — Data/Steps, QuestMaker.Domain assembly:\n" +
                        "• Subclass QuestStep. In the constructor validate the incoming data and log problems through " +
                        "ConsoleLogger instead of throwing, matching the existing steps.\n" +
                        "• Implement Subscribe/Unsubscribe against one event on IQuestEventSource.\n" +
                        "• In the handler: filter by ID, advance a counter, call FireOnChanged() so the UI repaints, and " +
                        "call Finish() once Validate() returns true.\n" +
                        "• ProgressText is what the overlay prints — return something like 'Trout fished 2/5'.\n" +
                        "• Shortcut: if the step counts Items, subclass ItemQuestStep instead — it ships HandleItemProgress, " +
                        "Validate and CheckInventoryForExisting (inventory pre-fill) for free; you only choose the event in " +
                        "Subscribe/Unsubscribe. That choice is the entire difference between Collect and Loot.\n\n" +
                        "Block node — Editor/Nodes, QuestMakerEditor assembly:\n" +
                        "• Subclass QMBaseStepBlockNode with [UseWithContext(typeof(ObjectiveNode))] and [System.Serializable].\n" +
                        "• Declare designer fields in OnDefineOptions (see section 5 below).\n" +
                        "• Implement ComposeStep(IStepModule module): read values with RetrieveBlockValue<T>(optionName) and " +
                        "call module.AddStep(new FishStepData { ... }).\n" +
                        "• No registration needed — any node type in the graph assembly is picked up automatically.\n\n" +
                        "Event + game code:\n" +
                        "• If no existing event fits, add one to IQuestEventSource and implement it on GameEventBus " +
                        "(event + Fire method with validation guards).\n" +
                        "• Fire it from gameplay. Done — author the block, compile, play.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("2. Adding a New Prerequisite Block", subHeader);
                    GUILayout.Label(
                        "Example: a gold requirement.\n\n" +
                        "• Capability interface: add IGoldModule : IQuestModule with void SetGold(int amount) next to the " +
                        "other module interfaces in IQuestModuleBuilder.cs.\n" +
                        "• PrerequisiteModule: implement IGoldModule, store the value, and pass it into the PrerequisiteData " +
                        "it builds. PrerequisiteData is constructor-based — add a field, a getter and a constructor parameter.\n" +
                        "• Block node: subclass QMBaseBlockNode with [UseWithContext(typeof(PrerequisiteNode))]. In " +
                        "Compose(ModuleScope scope) call scope.Get<IGoldModule>()?.SetGold(RetrieveBlockValue<int>()).\n" +
                        "• Runtime check: add the comparison in QuestManager.PrereqsMet.\n" +
                        "• Re-check triggers: prerequisites are re-evaluated on scene start, player level-up and chained " +
                        "turn-ins. If your new condition changes outside those moments (gold changes constantly), also call " +
                        "QuestManager.CheckAllPrerequisites() from the event that changes it — otherwise giver indicators " +
                        "will not refresh until the next existing trigger fires.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("3. Adding a New Reward Block", subHeader);
                    GUILayout.Label(
                        "Same shape as prerequisites, ending in RewardGiver instead of QuestManager.\n\n" +
                        "• Capability interface extending IQuestModule (e.g. IGoldRewardModule).\n" +
                        "• RewardModule: implement it and include the value in the RewardData it builds (field + getter + " +
                        "constructor parameter).\n" +
                        "• Block node: QMBaseBlockNode with [UseWithContext(typeof(RewardNode))]; Compose calls " +
                        "scope.Get<IGoldRewardModule>()?.SetGold(...).\n" +
                        "• Granting: add a Handle...Rewards branch in RewardGiver.GrantRewards. The Reputation and Ability " +
                        "handlers already exist as 'not implemented' stubs — completing those is exactly this job.\n" +
                        "• A block can serve both contexts: ItemBlockNode and ReputationBlockNode declare " +
                        "[UseWithContext(typeof(PrerequisiteNode), typeof(RewardNode))] and work in either, because both " +
                        "builder modules implement the same capability interface.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("4. Adding a New Context Node Category", subHeader);
                    GUILayout.Label(
                        "For an entirely new quest aspect (failure conditions, timers, dialogue) create a new context node " +
                        "plus its own builder module. The compiler needs NO changes — anything connected to the Start " +
                        "node's Context port that derives from QMBaseContextNode is processed generically.\n\n" +
                        "• Context node: subclass QMBaseContextNode. Set PortType => typeof(IContextFlowHelper) so it can " +
                        "connect to the Context port, and choose AllowMultipleContextNodesOfSameType — false gives you the " +
                        "live duplicate warning and compiler dedup that Prerequisite/Reward/Acquisition have.\n" +
                        "• ProcessNode(ModuleBuilderRegistry reg): fetch your builder with reg.GetBuilder<YourModule>() and " +
                        "call ComposeBlocks(builder, reg) — the base helper wraps the builder in a ModuleScope and hands it " +
                        "to every block inside the node.\n" +
                        "• Builder module: a plain class implementing IQuestModuleBuilder plus at least one IQuestModule " +
                        "capability interface. Registration is automatic — ModuleBuilderRegistry scans the interfaces the " +
                        "builder implements; a builder with no IQuestModule interface is rejected with an error.\n" +
                        "• Build(QuestSO quest): write the collected data onto the quest. Add a [SerializeReference] field " +
                        "and a setter on QuestSO for it, and expose it on the runtime Quest wrapper if gameplay needs it.\n" +
                        "• Blocks: QMBaseBlockNode subclasses tagged [UseWithContext(typeof(YourContextNode))].",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("5. New Options on Existing Nodes", subHeader);
                    GUILayout.Label(
                        "Options are the fields designers edit on nodes and blocks, declared in OnDefineOptions:\n\n" +
                        "context.AddOption(OPTION_KEY, typeof(int))\n" +
                        "    .WithDisplayName(\"Amount\")\n" +
                        "    .WithDefaultValue(1)\n" +
                        "    .WithTooltip(\"How many\")\n" +
                        "    .Build();\n\n" +
                        "• Supported types: primitives (int, float, string, bool), enums (QuestType, InGameTimeline, " +
                        "SpecialEventTrigger) and UnityEngine.Object references (Item, QuestSO, QuestGiver).\n" +
                        "• Read values with GetNodeOptionByName(KEY).TryGetValue(out T value); blocks and context nodes " +
                        "have the RetrieveBlockValue<T> / RetrieveOptionValue<T> helpers.\n" +
                        "• Keep option keys as const strings. Serialized values are stored BY KEY — renaming a key orphans " +
                        "the data already saved in graphs, so treat keys as stable identifiers.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("6. New Game Events and Event Buses", subHeader);
                    GUILayout.Label(
                        "• New event on an existing bus: add the C# event plus a Fire method with validation guards to " +
                        "GameEventBus (and to IQuestEventSource if quest steps must subscribe to it). QuestEventBus, " +
                        "PlayerEventBus and UIEventBus follow the same pattern for their own domains.\n" +
                        "• Whole new bus: subclass CustomEventBus. There is no registration — " +
                        "GameEventManager.RequestBus<YourBus>() creates and caches it on first request.\n" +
                        "• Getting a bus: ReferenceManager.Instance.RequestReference<GameEventManager>()" +
                        ".RequestBus<YourBus>().\n" +
                        "• Conventions: events are past-tense facts (OnItemCollected), Fire methods validate before " +
                        "invoking, subscribers unsubscribe in OnDisable.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("7. New Managers / Services", subHeader);
                    GUILayout.Label(
                        "• Implement IGameReference on the MonoBehaviour.\n" +
                        "• In Awake call ReferenceManager.Instance.SubScribeReference<YourManager>(this); unsubscribe in " +
                        "OnDisable. Any script can then resolve it with RequestReference<YourManager>().\n" +
                        "• Execution order matters: ReferenceManager runs at -20, GameEventManager at -19, QuestManager " +
                        "at -18, InventoryManager at -17. Give a new manager a [DefaultExecutionOrder] later than " +
                        "everything it resolves in Awake, or resolve in Start like Player and QuestGiver do.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("8. World Objects: Collectables, Interactables, Damageables", subHeader);
                    GUILayout.Label(
                        "• Collectables: put the CollectableItem component on a prefab (or subclass it for custom " +
                        "behaviour), assign the Item asset and Items Per Pickup, give the prefab a trigger collider. " +
                        "PlayerInteractions collects on touch and Collect() fires the item-collected event — Collect " +
                        "and Loot steps react with zero extra code. Override Collect() for custom VFX or sounds and " +
                        "call base.Collect().\n" +
                        "• Interactables: implement IInteractable on a Component with a trigger collider. " +
                        "PlayerInteractions tracks everything in range and calls Interact(player) on the nearest one when " +
                        "E is pressed — QuestGiver is the reference implementation. Use this for doors, levers or dialogue " +
                        "NPCs, and fire the matching bus event inside Interact to progress Talk or custom steps.\n" +
                        "• Damageables: implement IDamagable to receive TakeDamage from PlayerHitbox during the spin " +
                        "attack. On death fire the enemy-killed event with your ID like Enemy does so Slay steps count it.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("9. Custom Quest UI", subHeader);
                    GUILayout.Label(
                        "All shipped UI is replaceable — it only consumes events.\n\n" +
                        "• QuestEventBus signals: OnQuestStarted, OnQuestObjectiveChanged, OnQuestCanFinish, " +
                        "OnQuestCompleted, OnPrerequisitesChanged, OnNewQuestAdded.\n" +
                        "• Per-quest events: Quest.StepChanged, Quest.ObjectiveChanged, Quest.CanFinish; read " +
                        "Quest.CurrentObjective.Description and step.ProgressText for display strings — QuestOverlay is " +
                        "the reference implementation.\n" +
                        "• Giver-window flows go through UIEventBus (ShowQuestGiverWindow / CloseQuestGiverWindow) and " +
                        "call back into QuestManager.TryStartQuest / TryTurnInQuest — QuestGiverWindow shows the pattern.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("10. World Systems Reacting to Quest Data", subHeader);
                    GUILayout.Label(
                        "Gameplay systems can inspect active quests and react to their content:\n\n" +
                        "• Quest.GetCurrentStepsOfType<TStepData>() returns the typed step data of the ACTIVE objective; " +
                        "GetAllStepsOfType<TStepData>() scans every objective.\n" +
                        "• DummyEnemySpawner is the reference: it listens to OnQuestObjectiveChanged and spawns enemies " +
                        "for every SlayStepData in the new objective.\n" +
                        "• The same pattern fits area unlocks for Explore steps, NPC schedule changes for Talk steps, or " +
                        "ambient and music directors reacting to quest starts.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("11. Finishing the Special Event Hook", subHeader);
                    GUILayout.Label(
                        "Special events are authored and compiled but not fired at runtime — the hook exists and is waiting:\n\n" +
                        "• QuestEventBus already has FireSpecialEvent(string) and OnSpecialEvent.\n" +
                        "• Wire the firing side where quests change status (QuestManager.TryStartQuest / TryTurnInQuest): " +
                        "read quest.SpecialEvent, compare its Trigger (OnStarted / OnCompleted / OnFailed) to the " +
                        "transition that just happened, and call FireSpecialEvent(quest.SpecialEvent.EventID).\n" +
                        "• Gameplay systems subscribe to OnSpecialEvent and match IDs to actions — spawn a boss, unlock " +
                        "a door, start a cutscene.",
                        body);

                    GUILayout.Space(8);
                    GUILayout.Label("A Note on Subgraphs", subHeader);
                    GUILayout.Label(
                        "QMGraph enables GraphOptions.SupportsSubgraphs, so the editor lets you create subgraph assets — " +
                        "but the compiler does not traverse into subgraph nodes yet. Keep each quest in a single graph " +
                        "for now.",
                        body);
                    break;
            }
        }

        #endregion
    }
}
