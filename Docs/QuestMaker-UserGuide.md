# QuestMaker — User Guide

QuestMaker is a node-based quest authoring tool for Unity. Designers build a quest visually as a graph, compile it into a plain `QuestSO` ScriptableObject asset, and the runtime systems (quest manager, event buses, quest givers, inventory, rewards) execute it in play mode without ever touching the graph again.

The pipeline, end to end:

```
.qmgraph file  ──►  Quest Compiler  ──►  QuestSO asset            ──►  QuestLoader / QuestManager  ──►  gameplay
(graph editor)      (editor window)      (Assets/Resources/Quests)     (loads all quests at startup)     (events drive progress)
```

- **Environment:** Unity `6000.4.5f1`, Graph Toolkit package `com.unity.graphtoolkit 0.5.0-exp.1` (experimental).
- **Assemblies:** `QuestMaker.Domain` (quest data, steps), `QuestMaker.Runtime` (managers, game systems, UI), `QuestMakerEditor` (graph, nodes, compiler).

---

## 1. Quick start (TL;DR)

1. Right-click in the Project window → **Create → QuestMaker → Quest Graph**. The file name you type becomes the quest's ID.
2. Double-click the `.qmgraph` file to open the graph editor.
3. Add a **Start** node, set *Quest Type* and *Description*.
4. Add an **Objective** node, connect it to the Start node's **Objectives** port, add step blocks inside it (Collect, Slay, …).
5. Optionally add **Prerequisite**, **Reward**, and **Acquisition** context nodes and connect them to the **Context** port. Add blocks inside each.
6. In the Acquisition node, drag a Quest Giver *prefab* into the Hand-in / Turn-in slots.
7. Open **QuestMaker → Compiler Window**, drag the `.qmgraph` asset into the *Graph* field — it compiles immediately.
8. Click **Save as prefab** — the quest is written to `Assets/Resources/Quests/<ID>/<ID>.asset`.
9. Make sure the scene has the manager prefabs (`Assets/Prefabs/Managers/`), the Player, the UI prefabs, and your Quest Giver instances.
10. Press Play. Walk to the giver, press **E**, accept the quest, play it through, return, press **E**, turn it in.

---

## 2. Creating a quest graph

**Create → QuestMaker → Quest Graph** (Project window context menu) creates a `.qmgraph` file. Existing examples live in [Assets/QuestGraphs/](../Assets/QuestGraphs/).

Two things happen behind the scenes:

- The file is a Unity Graph Toolkit graph of type `QMGraph` ([QMGraph.cs](../Assets/Scripts/QuestMaker/Editor/QMGraph/QMGraph.cs)).
- A scripted importer ([QMGraphImporter.cs](../Assets/Scripts/QuestMaker/Editor/QMGraph/QMGraphImporter.cs)) attaches a small `QMGraphAssetFile` metadata object holding the asset's GUID. This metadata object is what you later drag into the Compiler Window.

**The file name is the quest ID.** The compiler names the `QuestSO` after the graph (`quest.Rename(graph.Name)`), and `QuestSO.ID` is the asset name. The ID is what quest-prerequisite chains, the quest map, and the on-screen overlay all use — rename thoughtfully and recompile after renaming.

Double-clicking the file opens Unity's graph editor window, where you build the quest from nodes.

---

## 3. Graph anatomy

### 3.1 The Start node

Every graph must contain exactly one **Start** node ([QMStartingNode.cs](../Assets/Scripts/QuestMaker/Editor/Nodes/BaseNodes/QMStartingNode.cs)) — compilation throws an error without it. It carries two options:

| Option | Meaning |
|---|---|
| **Quest Type** | `Main`, `Side`, `Repeatable`, `Timed`, `Daily`, `Unlockable` — categorization metadata stored on the quest. |
| **Description** | The quest's description text. |

And three output ports:

| Port | Connector | Connect it to |
|---|---|---|
| **Special Event** | circle | Special Event nodes |
| **Context** | arrowhead | Prerequisite, Reward, and Acquisition context nodes |
| **Objectives** | arrowhead | The *first* Objective node |

### 3.2 Context nodes and block nodes

QuestMaker uses Graph Toolkit's context/block model: **context nodes** are containers you wire into the graph flow, and **block nodes** are rows you add *inside* a context node. Each block type declares which contexts accept it, so the editor only offers valid blocks.

There are four context node types:

| Context node | Connects to | Multiple allowed? | Accepts blocks |
|---|---|---|---|
| **Objective** | Objectives port (then chained) | Yes — one per objective | Step blocks |
| **Prerequisite** | Context port | No — one per graph | Level, Required Quest, Time Constraint, Item, Reputation |
| **Reward** | Context port | No — one per graph | Exp, Item, Ability, Reputation |
| **Acquisition** | Context port | No — one per graph | Hand-in Giver, Turn-in Giver |

The one-per-graph rule is enforced live: adding a second Prerequisite/Reward/Acquisition node logs a warning on the node ("Can not have multiple instances of …"), and the compiler skips duplicates.

### 3.3 Objectives and ordering

Each **Objective** node is one stage of the quest. Its blocks are the steps the player must complete — *all* steps of an objective must finish before the quest advances to the next objective.

- The objective connected to the Start node's **Objectives** port runs first.
- Chain further objectives from the **Next Objective** output port. They activate strictly in sequence.
- The **Description** option is shown on the quest overlay while the objective is active, above the per-step progress lines (e.g. *"Kill Boars"* then *"Boar slained 7/10"*).

### 3.4 Step blocks (inside Objective nodes)

| Block | Options | Completed when |
|---|---|---|
| **Collect** | Item (asset), Amount | The item count is reached (pickups and existing inventory both count). |
| **Slay** | Enemy ID (string), Amount | That many enemies with the matching ID die. |
| **Loot** | Item, Amount | Same event as Collect — currently identical behavior. |
| **Craft** | Item, Amount | An item-crafted event for that item reaches the amount.* |
| **Deliver** | Item, NPC ID | An item-delivered event fires for that item.* |
| **Talk** | NPC ID | An npc-talked event fires with that ID.* |
| **Explore** | Area ID | An area-entered event fires with that ID.* |
| **Interact** | Item (IInteractable) | ⚠ **Do not use** — the runtime step is not implemented and throws `NotImplementedException` when the quest starts. |

\* These steps work, but no shipped game system fires their events yet — see [§7 Event reference](#7-event-reference) for what your game code must call.

### 3.5 Prerequisite blocks

| Block | Options | Checked at runtime? |
|---|---|---|
| **Level** | Level (int) | ✔ Player level must be ≥ value. |
| **Required Quest** | Quest (QuestSO asset) | ✔ That quest must be completed first — this is how quest chains are built. |
| **Item** | Item, Amount | ✔ Inventory must contain the amount. |
| **Reputation** | Faction ID, Amount | ✖ Compiled into the asset but not evaluated yet. |
| **Time Constraint** | None / Day / Night | ✖ Compiled into the asset but not evaluated yet. |

### 3.6 Reward blocks

| Block | Options | Granted at runtime? |
|---|---|---|
| **Exp amount** | int | ✔ Added to the player; level-ups can unlock other quests immediately. |
| **Item** | Item, Amount | ✔ Added to the inventory. |
| **Ability** | Ability ID (string) | ✖ Compiled, but granting is not implemented (logs a warning). |
| **Reputation** | Faction ID, Amount | ✖ Compiled, but granting is not implemented (logs a warning). |

### 3.7 Acquisition blocks

| Block | Option | Meaning |
|---|---|---|
| **Hand-in Giver** | QuestGiver prefab | Who offers the quest. |
| **Turn-in Giver** | QuestGiver prefab | Who accepts the finished quest (may be the same giver). |

⚠ **Drag the prefab from the Project window into the slot.** The Unity object-picker search cannot find QuestGiver prefabs (the option tooltip says the same).

What is stored is not a scene reference but the prefab asset's **GUID** — the same GUID a `QuestGiver` component stamps on itself as its ID (see §6.2). That is how compiled quests and scene givers find each other.

### 3.8 Special Event node

Connect a **Special Event** node to the Start node's circle port. Options: **EventID** (free string, e.g. `Spawn_super_boss`) and **Trigger Moment** (`OnStarted` / `OnCompleted` / `OnFailed`).

⚠ The event data is compiled into the quest asset, but the runtime never fires it yet — there is a `FireSpecialEvent` hook on the quest event bus with no caller. Treat this as authoring-ready, runtime-pending.

---

## 4. Compiling a quest

Open **QuestMaker → Compiler Window** ([QuestCompilerEditorWindow.cs](../Assets/Scripts/QuestMaker/Editor/QuestMakerEditorWindow/QuestCompilerEditorWindow.cs)).

1. Drag the `.qmgraph` asset into the **Graph** field.
2. Compilation runs immediately on assignment. The *Node count* field fills in, and the Unity console logs each module the compiler builds (plus warnings for anything wrong).
3. If compilation produced a valid quest, **Save as prefab** enables. Click it to write the asset.

The asset is written to `Assets/Resources/Quests/<QuestID>/<QuestID>.asset`. If it already exists, the compiler **copies the new data over the old asset in place**, preserving the asset's GUID — so other graphs that reference this quest as a prerequisite never break when you recompile.

### What compilation actually does

[QuestCompiler.cs](../Assets/Scripts/QuestMaker/Editor/QuestCompiler/QuestCompiler.cs) walks the graph from the Start node:

1. The Start node writes quest type, description, and any special events into a `QuestInfoModule`.
2. The **Context** port is followed: each context node fetches its builder module (`PrerequisiteModule`, `RewardModule`, `AcquisitionModule`) from a registry and lets its blocks fill it in.
3. The **Objectives** port is followed: each Objective node creates its own `ObjectiveModule`, its step blocks add `QuestStepData` entries, and chained objectives are processed in connection order.
4. Every module's `Build(quest)` runs, assembling the final `QuestSO`.

Common compile-time warnings to watch for in the console: unconnected Context/Objectives ports, duplicate context node types, null Item/Quest/Giver references in blocks, amounts ≤ 0.

### The compiled QuestSO

The result ([QuestSO.cs](../Assets/Scripts/QuestMaker/Data/QuestData/QuestSO.cs)) is a plain ScriptableObject containing: quest type, description, the objective list (each with description and serialized step data), prerequisite data, reward data, hand-in/turn-in giver GUIDs, and optional special event data. Runtime never sees the graph — only this asset.

---

## 5. Items

Steps and rewards reference **Item** assets (**Create → QuestMaker → ScriptableObjects → Item**, [Item.cs](../Assets/Scripts/QuestMaker/Data/Items/Item.cs)).

- An item's **ID** is auto-assigned from its asset GUID and shown read-only; its **Name** syncs from the asset file name. Duplicating an item asset automatically gives the copy its own fresh ID.
- To make an item collectable in the world, put the `CollectableItem` component on a prefab with a trigger collider, assign the Item asset and set **Items Per Pickup** (how many items one pickup awards). The player collects it on touch — no key press.

---

## 6. Scene setup

The demo scene [SampleScene.unity](../Assets/Scenes/SampleScene.unity) has everything wired; use it as the reference. To build a scene from scratch, you need four groups of objects, all available as prefabs under [Assets/Prefabs/](../Assets/Prefabs/).

### 6.1 Managers (required, in this initialization order)

Script execution order is already declared in code — just make sure all of them exist in the scene:

| Prefab | Role |
|---|---|
| **ReferenceManager** | Service locator every other system registers with. Initializes first. |
| **EventManager** (`GameEventManager`) | Owns the typed event buses (game, quest, player, UI). |
| **QuestManager** | Loads all quests from `Resources/Quests`, tracks statuses, starts/turns-in quests, checks prerequisites. |
| **InventoryManager** | Owns the runtime `Inventory`; listens for item events. |
| **RewardGiver** | Grants exp/item rewards when a quest completes. |

### 6.2 Quest givers

1. Duplicate `Prefabs/QuestGivers/BaseQuestGiver_ONLY_USED_FOR_DUPLICATION.prefab` and give the copy a name (e.g. `George`).
2. The `QuestGiver` component stamps its own prefab GUID into its read-only **ID** field automatically (`OnValidate`). This GUID is what Acquisition blocks store — so **each distinct giver must be its own prefab asset**, and the same prefab you drag into the graph must be the one you place in the scene.
3. Place instances in the scene. The child `QuestIcons` object (`QuestGiverIndicators`) drives the floating status icons:

| Icon | Meaning |
|---|---|
| Yellow **!** | Has a quest ready to hand out. |
| White **!** | Has a quest, requirements not met yet. |
| Yellow **?** | Ready to accept a turn-in. |
| White **?** | Player has the quest in progress; giver awaits completion. |

### 6.3 Player

`Prefabs/Player/Player.prefab` bundles: `Player` (level/exp), `PlayerController` (WASD/arrow movement, **Space** spin attack), `PlayerInteractions` (**E** interacts with the nearest QuestGiver in range; auto-collects collectables on touch), `PlayerHitbox` (damage during the spin).

### 6.4 UI

| Prefab | Role |
|---|---|
| **QuestGiverWindow** | Opens on interacting with a giver: *accept* buttons for available quests, *turn-in* buttons for finishable ones, disabled entries for quests with missing requirements. Closes when the player walks away. |
| **QuestOverlay** | On-screen tracker: quest ID, active objective description, live per-step progress; turns green with "Completed!" when ready to turn in. |
| **PlayerInfo** | Level / exp readout. |

### 6.5 Enemies (for Slay quests)

- `Prefabs/Enemy/Enemy.prefab` has an **ID** string — it must match the *Enemy ID* typed into Slay blocks. On death it fires the enemy-killed event.
- `DummyEnemySpawner` is a demo convenience: whenever an objective with Slay steps activates, it spawns one enemy per step configured to satisfy the whole step (one kill fires the event *Amount* times). Replace it with real spawning in a real game.

### 6.6 Collectables (for Collect quests)

- The collectible `Item` asset's **Prefab** field must point at a prefab with a `CollectableItem` component and a trigger collider.
- `DummyCollectableSpawner` is the collect-side counterpart of the enemy spawner: whenever an objective with Collect steps activates, it spawns the step item's prefab at its spawn points.
- **Spawn Single** on (default): spawns one pickup boosted to grant the step's full amount — one touch completes the step, mirroring the enemy spawner. Off: spawns `ceil(step amount / Items Per Pickup)` pickups.
- Same demo-tier caveats as the enemy spawner — replace it with real placement in a real game.

---

## 7. Runtime flow

### 7.1 Startup

`QuestManager` loads **every** `QuestSO` under `Resources/Quests` (anything the compiler saved is auto-discovered — no registration step) and wraps each in a runtime `Quest`. Quest-prerequisite links are inverted into *next-in-chain* references so completing a quest immediately re-evaluates its dependents.

### 7.2 Quest status lifecycle

```
MISSING_REQUIRMENTS ──► CAN_START ──► IN_PROGRESS ──► CAN_FINISH ──► COMPLETED
     (prereq check)      (accept at      (all objectives   (turn in at
                          hand-in giver)  finished)          turn-in giver)
```

Prerequisites are re-checked at scene start, whenever the player levels up, and for chained quests whenever a prerequisite quest is turned in. Level, item, and completed-quest requirements are enforced; reputation and time-of-day are not yet.

### 7.3 Accepting → progressing → turning in

1. **Accept:** walk into a giver's trigger radius, press **E**, click the quest button. `QuestManager.TryStartQuest` re-validates prerequisites, instantiates runtime steps for every objective, and activates the first objective's steps.
2. **Progress:** each active step subscribes to its game event (see table below). Events update counters, the overlay repaints, and when *all* steps of the objective are complete the next objective activates. Collect-type steps also pre-fill from items already in the inventory the moment the objective activates.
3. **Turn in:** when the last objective finishes, status becomes `CAN_FINISH` (overlay goes green, giver shows the yellow **?**). Interact with the *turn-in* giver and click the quest: rewards are granted (exp, items), collected quest items are **consumed from the inventory**, and chained quests get their prerequisites re-checked — potentially lighting up the next giver on the spot.

### 7.4 Event reference

Runtime steps progress exclusively through the game event bus. What fires each event today:

| Event | Progresses step | Currently fired by |
|---|---|---|
| Item collected | Collect, Loot | `CollectableItem.Collect()` (player touches a pickup) |
| Enemy killed | Slay | `Enemy.Die()` |
| Item crafted | Craft | **nothing yet** — your crafting system must call `GameEventBus.FireItemCrafted(stack)` |
| Item delivered | Deliver | **nothing yet** — call `GameEventBus.FireItemDelivered(stack)` |
| NPC talked | Talk | **nothing yet** — call `GameEventBus.FireNpcTalked(npcID)` from your dialogue system |
| Area entered | Explore | **nothing yet** — call `GameEventBus.FireAreaEntered(areaID)` from a zone trigger |

To integrate one of the pending ones, get the bus and fire the event:

```csharp
GameEventBus bus = ReferenceManager.Instance
    .RequestReference<GameEventManager>()
    .RequestBus<GameEventBus>();

bus.FireNpcTalked("npc_george");
```

### 7.5 Testing tips

- The project ships with Quantum Console. `Player.AddExp` and `Player.ResetLevel` are console commands — useful for testing level prerequisites without grinding.
- Every system logs verbosely through `ConsoleLogger`; the Unity console tells you exactly which steps were created, activated, and finished.

---

## 8. Current limitations (as built)

- **Interact steps crash** at quest start (`NotImplementedException`) — don't author them yet.
- **Talk / Deliver / Explore / Craft** steps need game systems to fire their events (§7.4); none ship with the demo.
- **Special events** compile into the asset but nothing fires or listens to them at runtime.
- **Reputation and Ability rewards** are stored but only log "not implemented" when granted; **reputation and time-constraint prerequisites** are stored but never evaluated.
- **No save system** — quest progress lives in memory and resets on play-mode exit.
- **Quest Type** is metadata only; no runtime behavior differs per type (e.g. `Repeatable` quests are not actually repeatable).
- **Loot** currently listens to the same event as Collect, so they behave identically.

---

## 9. Extending the tool

QuestMaker is built around plug-in seams: graph nodes are auto-discovered from the editor assembly, compiler modules self-register through the capability interfaces they implement, event buses are created lazily on first request, and managers are resolved through the `ReferenceManager` service locator. Extending the tool almost never means editing existing systems — you add new types next to them.

Every extensible aspect, in one list:

| # | Extension point | You add |
|---|---|---|
| 9.1 | New step types | Step data + runtime step + block node (+ event) |
| 9.2 | New prerequisite blocks | Capability interface + module change + block node + runtime check |
| 9.3 | New reward blocks | Capability interface + module change + block node + grant handler |
| 9.4 | New context node categories | Context node + builder module + blocks + QuestSO field |
| 9.5 | New options on existing nodes | `OnDefineOptions` entries |
| 9.6 | New game events / buses | Bus events or a `CustomEventBus` subclass |
| 9.7 | New managers / services | `IGameReference` + registration |
| 9.8 | World objects | `CollectableItem` / `IInteractable` / `IDamagable` implementations |
| 9.9 | Custom quest UI | Subscribers to `QuestEventBus` / `UIEventBus` |
| 9.10 | World systems reacting to quest data | `GetCurrentStepsOfType<T>()` consumers |
| 9.11 | Runtime special events | Wiring the existing `FireSpecialEvent` hook |

### 9.1 Adding a new step type

The most common extension. Four pieces, following the Collect/Slay pattern (example names for a fishing step: `FishStepData`, `FishQuestStep`, `FishStepBlockNode`).

**Step data** ([Assets/Scripts/QuestMaker/Data/Steps/](../Assets/Scripts/QuestMaker/Data/Steps/), `QuestMaker.Domain` assembly):

- Subclass `QuestStepData` and mark it `[Serializable]`.
- Add `[SerializeField]` fields for everything the designer configures (IDs, amounts, `Item` references).
- Follow the `StepID` convention used by Collect/Slay (e.g. `Fish_[Trout]_5`) so objectives get readable auto-generated IDs.
- Implement `CreateRuntimeStep(IQuestEventSource eventbus) => new FishQuestStep(this, eventbus)` — the factory the runtime `Quest` calls for every step of every objective.

**Runtime step** (same folder):

- Subclass `QuestStep`. In the constructor validate the incoming data and log problems through `ConsoleLogger` instead of throwing, matching the existing steps.
- Implement `Subscribe`/`Unsubscribe` against one event on `IQuestEventSource`.
- In the handler: filter by ID, advance a counter, call `FireOnChanged()` so the UI repaints, and call `Finish()` once `Validate()` returns true.
- `ProgressText` is what the overlay prints — return something like *"Trout fished 2/5"*.
- Shortcut: if the step counts Items, subclass `ItemQuestStep` instead — it ships `HandleItemProgress`, `Validate` and `CheckInventoryForExisting` (inventory pre-fill) for free; you only choose the event in `Subscribe`/`Unsubscribe`. That choice is the entire difference between Collect and Loot.

**Block node** ([Assets/Scripts/QuestMaker/Editor/Nodes/](../Assets/Scripts/QuestMaker/Editor/Nodes/), `QuestMakerEditor` assembly):

- Subclass `QMBaseStepBlockNode` with `[UseWithContext(typeof(ObjectiveNode))]` and `[Serializable]`.
- Declare designer fields in `OnDefineOptions` (§9.5).
- Implement `ComposeStep(IStepModule module)`: read values with `RetrieveBlockValue<T>(optionName)` and call `module.AddStep(new FishStepData { ... })`.
- No registration needed — any node type in the graph assembly is picked up automatically.

**Event + game code:**

- If no existing event fits, add one to `IQuestEventSource` and implement it on `GameEventBus` (event + `Fire` method with validation guards).
- Fire it from gameplay. Done — author the block, compile, play.

### 9.2 Adding a new prerequisite block

Example: a gold requirement.

1. Capability interface: add `IGoldModule : IQuestModule` with `void SetGold(int amount)` next to the other module interfaces in [IQuestModuleBuilder.cs](../Assets/Scripts/QuestMaker/Editor/QuestCompiler/CompilationModules/ModuleRegistry/IQuestModuleBuilder.cs).
2. `PrerequisiteModule`: implement `IGoldModule`, store the value, and pass it into the `PrerequisiteData` it builds. `PrerequisiteData` is constructor-based — add a field, a getter and a constructor parameter.
3. Block node: subclass `QMBaseBlockNode` with `[UseWithContext(typeof(PrerequisiteNode))]`. In `Compose(ModuleScope scope)` call `scope.Get<IGoldModule>()?.SetGold(RetrieveBlockValue<int>())`.
4. Runtime check: add the comparison in `QuestManager.PrereqsMet`.
5. Re-check triggers: prerequisites are re-evaluated on scene start, player level-up and chained turn-ins. If your new condition changes outside those moments (gold changes constantly), also call `QuestManager.CheckAllPrerequisites()` from the event that changes it — otherwise giver indicators will not refresh until the next existing trigger fires.

### 9.3 Adding a new reward block

Same shape as prerequisites, ending in `RewardGiver` instead of `QuestManager`.

1. Capability interface extending `IQuestModule` (e.g. `IGoldRewardModule`).
2. `RewardModule`: implement it and include the value in the `RewardData` it builds (field + getter + constructor parameter).
3. Block node: `QMBaseBlockNode` with `[UseWithContext(typeof(RewardNode))]`; `Compose` calls `scope.Get<IGoldRewardModule>()?.SetGold(...)`.
4. Granting: add a `Handle...Rewards` branch in `RewardGiver.GrantRewards`. The Reputation and Ability handlers already exist as "not implemented" stubs — completing those is exactly this job.

A block can serve both contexts: `ItemBlockNode` and `ReputationBlockNode` declare `[UseWithContext(typeof(PrerequisiteNode), typeof(RewardNode))]` and work in either, because both builder modules implement the same capability interface.

### 9.4 Adding a new context node category

For an entirely new quest aspect (failure conditions, timers, dialogue) create a new context node plus its own builder module. The compiler needs **no changes** — anything connected to the Start node's Context port that derives from `QMBaseContextNode` is processed generically.

- Context node: subclass `QMBaseContextNode`. Set `PortType => typeof(IContextFlowHelper)` so it can connect to the Context port, and choose `AllowMultipleContextNodesOfSameType` — `false` gives you the live duplicate warning and compiler dedup that Prerequisite/Reward/Acquisition have.
- `ProcessNode(ModuleBuilderRegistry reg)`: fetch your builder with `reg.GetBuilder<YourModule>()` and call `ComposeBlocks(builder, reg)` — the base helper wraps the builder in a `ModuleScope` and hands it to every block inside the node.
- Builder module: a plain class implementing `IQuestModuleBuilder` plus at least one `IQuestModule` capability interface. Registration is automatic — `ModuleBuilderRegistry` scans the interfaces the builder implements; a builder with no `IQuestModule` interface is rejected with an error.
- `Build(QuestSO quest)`: write the collected data onto the quest. Add a `[SerializeReference]` field and a setter on `QuestSO` for it, and expose it on the runtime `Quest` wrapper if gameplay needs it.
- Blocks: `QMBaseBlockNode` subclasses tagged `[UseWithContext(typeof(YourContextNode))]`.

### 9.5 New options on existing nodes

Options are the fields designers edit on nodes and blocks, declared in `OnDefineOptions`:

```csharp
context.AddOption(OPTION_KEY, typeof(int))
    .WithDisplayName("Amount")
    .WithDefaultValue(1)
    .WithTooltip("How many")
    .Build();
```

- Supported types: primitives (`int`, `float`, `string`, `bool`), enums (`QuestType`, `InGameTimeline`, `SpecialEventTrigger`) and `UnityEngine.Object` references (`Item`, `QuestSO`, `QuestGiver`).
- Read values with `GetNodeOptionByName(KEY).TryGetValue(out T value)`; blocks and context nodes have the `RetrieveBlockValue<T>` / `RetrieveOptionValue<T>` helpers.
- Keep option keys as `const` strings. Serialized values are stored **by key** — renaming a key orphans the data already saved in graphs, so treat keys as stable identifiers.

### 9.6 New game events and event buses

- New event on an existing bus: add the C# event plus a `Fire` method with validation guards to `GameEventBus` (and to `IQuestEventSource` if quest steps must subscribe to it). `QuestEventBus`, `PlayerEventBus` and `UIEventBus` follow the same pattern for their own domains.
- Whole new bus: subclass `CustomEventBus`. There is no registration — `GameEventManager.RequestBus<YourBus>()` creates and caches it on first request.
- Getting a bus: `ReferenceManager.Instance.RequestReference<GameEventManager>().RequestBus<YourBus>()`.
- Conventions: events are past-tense facts (`OnItemCollected`), `Fire` methods validate before invoking, subscribers unsubscribe in `OnDisable`.

### 9.7 New managers / services

- Implement `IGameReference` on the MonoBehaviour.
- In `Awake` call `ReferenceManager.Instance.SubScribeReference<YourManager>(this)`; unsubscribe in `OnDisable`. Any script can then resolve it with `RequestReference<YourManager>()`.
- Execution order matters: ReferenceManager runs at −20, GameEventManager at −19, QuestManager at −18, InventoryManager at −17. Give a new manager a `[DefaultExecutionOrder]` later than everything it resolves in `Awake`, or resolve in `Start` like `Player` and `QuestGiver` do.

### 9.8 World objects: collectables, interactables, damageables

- **Collectables**: put the `CollectableItem` component on a prefab (or subclass it for custom behaviour), assign the Item asset and Items Per Pickup, give the prefab a trigger collider. `PlayerInteractions` collects on touch and `Collect()` fires the item-collected event — Collect and Loot steps react with zero extra code. Override `Collect()` for custom VFX or sounds and call `base.Collect()`.
- **Interactables**: implement `IInteractable` on a `Component` with a trigger collider. `PlayerInteractions` tracks everything in range and calls `Interact(player)` on the nearest one when **E** is pressed — `QuestGiver` is the reference implementation. Use this for doors, levers or dialogue NPCs, and fire the matching bus event inside `Interact` to progress Talk or custom steps.
- **Damageables**: implement `IDamagable` to receive `TakeDamage` from `PlayerHitbox` during the spin attack. On death fire the enemy-killed event with your ID like `Enemy` does so Slay steps count it.

### 9.9 Custom quest UI

All shipped UI is replaceable — it only consumes events.

- `QuestEventBus` signals: `OnQuestStarted`, `OnQuestObjectiveChanged`, `OnQuestCanFinish`, `OnQuestCompleted`, `OnPrerequisitesChanged`, `OnNewQuestAdded`.
- Per-quest events: `Quest.StepChanged`, `Quest.ObjectiveChanged`, `Quest.CanFinish`; read `Quest.CurrentObjective.Description` and `step.ProgressText` for display strings — `QuestOverlay` is the reference implementation.
- Giver-window flows go through `UIEventBus` (`ShowQuestGiverWindow` / `CloseQuestGiverWindow`) and call back into `QuestManager.TryStartQuest` / `TryTurnInQuest` — `QuestGiverWindow` shows the pattern.

### 9.10 World systems reacting to quest data

Gameplay systems can inspect active quests and react to their content:

- `Quest.GetCurrentStepsOfType<TStepData>()` returns the typed step data of the *active* objective; `GetAllStepsOfType<TStepData>()` scans every objective.
- `DummyEnemySpawner` is the reference: it listens to `OnQuestObjectiveChanged` and spawns enemies for every `SlayStepData` in the new objective.
- The same pattern fits area unlocks for Explore steps, NPC schedule changes for Talk steps, or ambient and music directors reacting to quest starts.

### 9.11 Finishing the special-event hook

Special events are authored and compiled but not fired at runtime — the hook exists and is waiting:

- `QuestEventBus` already has `FireSpecialEvent(string)` and `OnSpecialEvent`.
- Wire the firing side where quests change status (`QuestManager.TryStartQuest` / `TryTurnInQuest`): read `quest.SpecialEvent`, compare its `Trigger` (`OnStarted` / `OnCompleted` / `OnFailed`) to the transition that just happened, and call `FireSpecialEvent(quest.SpecialEvent.EventID)`.
- Gameplay systems subscribe to `OnSpecialEvent` and match IDs to actions — spawn a boss, unlock a door, start a cutscene.

### A note on subgraphs

`QMGraph` enables `GraphOptions.SupportsSubgraphs`, so the editor lets you create subgraph assets — but the compiler does not traverse into subgraph nodes yet. Keep each quest in a single graph for now.
