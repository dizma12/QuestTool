using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using QuestMaker.Runtime;

namespace QuestMaker.Editor.Nodes
{
    public enum QuestAcquisitionMethods
    {
        TalkToNpc,
        PickUpItem,
        EnterArea,
    }

    [Serializable]
    public class QuestMakerNode : Node
    {
        public const string DEFAULT_EXECUTION_PORT_NAME = "ExecutionPort";
    }


    [Serializable]
    public class QuestStartingNode : QuestMakerNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(DEFAULT_EXECUTION_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    [Serializable]
    public class QuestInfoNode : QuestMakerNode
    {
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(name: "QuestID", dataType: typeof(string));
            context.AddOption(name: "QuestName", dataType: typeof(string));
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_EXECUTION_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort(DEFAULT_EXECUTION_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    [Serializable]
    public class QuestAcquisitionNode : QuestMakerNode
    {
        public const string DEFAULT_ACQUISITION_METHOD_NAME = "QuestAcquisitionMethod";
        public const string DEFAULT_QUEST_GIVER_NAME = "QuestGiverName";
        public const string DEFAULT_QUEST_GIVER_ID = "QuestGiverID";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(DEFAULT_ACQUISITION_METHOD_NAME, typeof(QuestAcquisitionMethods))
            .WithDefaultValue(QuestAcquisitionMethods.TalkToNpc)
            .WithDisplayName("Quest Acquisition Method")
            .WithTooltip("The way you acquire the quest.")
            .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_EXECUTION_PORT_NAME)
                    .WithDisplayName(string.Empty)
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();

            var acqOption = GetNodeOptionByName(DEFAULT_ACQUISITION_METHOD_NAME);

            if (acqOption == null)
            {
                throw new InvalidOperationException(
                    $"[QuestAcquisitionNode] Cannot find port with name: {DEFAULT_ACQUISITION_METHOD_NAME}");
            }

            acqOption.TryGetValue(out QuestAcquisitionMethods acq);

            switch (acq)
            {
                case QuestAcquisitionMethods.TalkToNpc:
                    context.AddInputPort("NpcID")
                        .WithDataType(typeof(string))
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddInputPort("QuestGiverName")
                        .WithDataType(typeof(string))
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                case QuestAcquisitionMethods.PickUpItem:
                    context.AddInputPort("ItemID")
                        .WithDataType(typeof(string))
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                case QuestAcquisitionMethods.EnterArea:
                    context.AddInputPort("AreaID")
                        .WithDataType(typeof(string))
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();


                    context.AddInputPort("AreaGiverName")
                        .WithDataType(typeof(string))
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
            }



            context.AddOutputPort(DEFAULT_EXECUTION_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    [Serializable]
    public class QuestPrerequisiteAcquisitionNode : QuestMakerNode
    {
        private const string DEFAULT_PREREQUISITES_COUNT_NAME = "PrerequisiteCount";
        private const string DEFAULT_PREREQUISITES_PREFIX = "Prerequisite";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(DEFAULT_PREREQUISITES_COUNT_NAME, typeof(int))
                .WithDisplayName("Prerequisites Count")
                .WithDefaultValue(1)
                .Delayed()
                .Build();
        }


        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_EXECUTION_PORT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            INodeOption countPort = GetNodeOptionByName(DEFAULT_PREREQUISITES_COUNT_NAME);

            countPort.TryGetValue(out int count);

            for (int i = 0; i < count; i++)
            {
                context.AddInputPort($"{DEFAULT_PREREQUISITES_PREFIX}{i}")
                    .WithDisplayName($"{DEFAULT_PREREQUISITES_PREFIX} {i + 1}")
                    .WithDataType(typeof(QuestAcquisitionPrerequisite))
                    .Build();
            }
        }

        public class QuestPrerequisteNode : QuestMakerNode
        {
            protected override void OnDefinePorts(IPortDefinitionContext context)
            {
                context.AddInputPort("QuestID")
                    .WithDisplayName("QuestID")
                    .WithDataType(typeof(string))
                    .WithConnectorUI(PortConnectorUI.Circle)
                    .Build();

                context.AddOutputPort(DEFAULT_EXECUTION_PORT_NAME)
                    .WithDisplayName(string.Empty)
                    .WithDataType(typeof(string))
                    .WithConnectorUI(PortConnectorUI.Arrowhead)
                    .Build();
            }
        }

    }

}