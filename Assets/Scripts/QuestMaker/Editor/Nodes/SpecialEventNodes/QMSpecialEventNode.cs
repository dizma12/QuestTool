using QuestMaker.Data.SpecialEvents;
using QuestMaker.Editor.CompilationModules;
using System;
using UnityEngine;

namespace QuestMaker.Editor.Nodes
{
    [Serializable]
    internal class QMSpecialEventNode : QMBaseNode, IComposableNode
    {
        public override Type PortType => typeof(ISpecialEventNode);
        public const string SPECIAL_EVENT_TRIGGER_PORT = "Special_Event_Trigger_Port";
        public const string SPECIAL_EVENT_ID_PORT = "Special_Event_ID_Port";

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            base.OnDefineOptions(context);

            context.AddOption(SPECIAL_EVENT_TRIGGER_PORT, typeof(SpecialEventTrigger))
                .WithDefaultValue(SpecialEventTrigger.OnCompleted)
                .WithDisplayName("Trigger Moment")
                .WithTooltip("The moment you want the special event to trigger!")
                .Build();

            context.AddOption(SPECIAL_EVENT_ID_PORT, typeof(string))
                .WithDefaultValue(string.Empty)
                .WithDisplayName("EventID")
                .WithTooltip("The id of the event you want to fire (ex: Spawn_super_boss).")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(INPUT_PORT)
                .WithDataType(PortType)
                .WithDisplayName("Flow in")
                .WithConnectorUI(Unity.GraphToolkit.Editor.PortConnectorUI.Circle)
                .Build();
        }

        public void Compose(ModuleScope scope)
        {
            ISpecialEventModule module = scope.Get<ISpecialEventModule>();
            if (module == null) return;

            GetNodeOptionByName(SPECIAL_EVENT_ID_PORT).TryGetValue(out string eventID);
            GetNodeOptionByName(SPECIAL_EVENT_TRIGGER_PORT).TryGetValue(out SpecialEventTrigger trigger);

            module.SetSpecialEvent(new(trigger, eventID));
            Debug.Log($"[QMSpecialEventNode] Set special event on {module.GetType().Name} with ID: {eventID} and Trigger: {trigger}");
        }
    }
}