using QuestMaker.Data.SpecialEvents;
using QuestMaker.Editor.Compiler;
using QuestMaker.Editor.Compiler.CompilationModules;
using QuestMaker.Editor.Nodes;
using System;
using UnityEngine;


namespace QuestMaker.Editor.Assets.Scripts.QuestMaker.Editor.Nodes.SpecialEventNodes
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
                .WithTooltip("The momment you want the special event to trigger!")
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

        //Compose pattern starts to fall off here :/
        public void Compose<TBuilder>(TBuilder moduleBuilder, ModuleBuilderRegistry reg) where TBuilder : class, IQuestModuleBuilder, new()
        {
            
            if (moduleBuilder == null || moduleBuilder is not ISpecialEventModule module) return;

            SetSpecialEvent(module);
        }

        private void SetSpecialEvent(ISpecialEventModule specialEventModule)
        {
            GetNodeOptionByName(SPECIAL_EVENT_ID_PORT).TryGetValue(out string eventID);
            GetNodeOptionByName(SPECIAL_EVENT_TRIGGER_PORT).TryGetValue(out SpecialEventTrigger trigger);

            specialEventModule.SetSpecialEvent(new(trigger, eventID));
            Debug.LogWarning($"special event was set on module {specialEventModule.GetType()} with id{eventID} and trigger {trigger}");
        }
    }
    
}
