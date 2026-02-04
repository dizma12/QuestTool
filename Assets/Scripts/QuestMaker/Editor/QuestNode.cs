using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;
using QuestMaker.Runtime;

namespace QuestMaker.Editor.Nodes
{
    public abstract class QuestMakerNode : Node
    {
        public const string DEFAULT_OUTPORT_NAME = "Quest_export";
        public const string DEFAULT_INPORT_NAME = "Quest_import";

        /// <summary>
        /// Defines the default Input Port for All non-start Nodes
        /// </summary>
        /// <param name="context"></param>
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort(DEFAULT_INPORT_NAME)
                .WithDataType(typeof(QuestInfoSO))
                .WithDefaultValue(default)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    public class QuestInfoStartingNode : QuestMakerNode
    {
        private const string QUEST_ID = "QuestID";
        private const string QUEST_NAME = "QuestName";
        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {
            context.AddOption(QUEST_ID, typeof(string))
                .WithDisplayName("Quest ID")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The id of the quest")
                .Build();

            context.AddOption(QUEST_NAME, typeof(string))
                .WithDisplayName("Quest Name")
                .WithDefaultValue(string.Empty)
                .WithTooltip("The name of the quest")
                .Build();
        }


        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddOutputPort(DEFAULT_OUTPORT_NAME)
                .WithDisplayName(string.Empty)
                .WithDataType(typeof(QuestInfoSO))
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }

    public class QuestAcquisitionMethodNode : QuestMakerNode
    {

        private const string ACQUISITION_METHOD_OPTION = "AcquisitionMethod";

        private const string NPC_ID_INPUT = "NpcID";
        private const string ITEM_ID_INPUT = "ItemID";
        private const string AREA_ID_INPUT = "AreaID";
        public enum AcquisitionMethods
        {
            NpcInteraction,
            ItemPickUp,
            AreaTrigger
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {

            context.AddOption( ACQUISITION_METHOD_OPTION , typeof(AcquisitionMethods))
                .WithDisplayName("Acquisition Method")
                .WithDefaultValue(AcquisitionMethods.NpcInteraction)
                .WithTooltip("The method that you acquire the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            INodeOption acqOption = GetNodeOptionByName(ACQUISITION_METHOD_OPTION);
            acqOption.TryGetValue(out AcquisitionMethods acquisitionMethod);

            switch (acquisitionMethod)
            {
                case AcquisitionMethods.NpcInteraction:

                    context.AddInputPort(NPC_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Npc ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                    case AcquisitionMethods.ItemPickUp:

                    context.AddInputPort(ITEM_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Item ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                    case AcquisitionMethods.AreaTrigger:

                    context.AddInputPort(AREA_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Area ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                    default:

                    break;
            }
            
        }
    }

    public class QuestPrerequisitesNode : QuestMakerNode
    { 
        private const string PREREQUISITE_TYPE_OPTION = "PrerequisiteType";


        private const string LEVEL_INPUT = "Level";
        private const string QUEST_ID_INPUT = "QuestID";
        private const string ITEM_ID_INPUT = "ItemID";
        private const string NPC_ID_INPUT = "NpcID";
        private const string AREA_ID_INPUT = "AreaID";
        public enum PrerequisiteType
        {   
            None,
            Level,
            Quest,
            ItemOwnership,
            NpcInteraction,
            AreaFound
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {

            context.AddOption(PREREQUISITE_TYPE_OPTION, typeof(PrerequisiteType))
                .WithDisplayName("Acquisition Method")
                .WithDefaultValue(PrerequisiteType.None)
                .WithTooltip("The type of Prerequisite for acquiring the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            INodeOption acqOption = GetNodeOptionByName(PREREQUISITE_TYPE_OPTION);
            acqOption.TryGetValue(out PrerequisiteType PrereqType);

            switch (PrereqType)
            {
                case PrerequisiteType.Level:
                    context.AddInputPort(LEVEL_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Level")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                    case PrerequisiteType.Quest:
                    context.AddInputPort(QUEST_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Quest ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                case PrerequisiteType.NpcInteraction:

                    context.AddInputPort(NPC_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Npc ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case PrerequisiteType.ItemOwnership:

                    context.AddInputPort(ITEM_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Item ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case PrerequisiteType.AreaFound:

                    context.AddInputPort(AREA_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Area ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                default:

                    break;
            }

        }
    }

    public class QuestStepNode : QuestMakerNode
    {

        private const string QUEST_OBJECTIVE_OPTION = "QuestObjective";

        private const string SLAY_INPUT = "EnemyID";
        private const string ITEM_ID_INPUT = "ItemID";
        private const string NPC_ID_INPUT = "NpcID";
        private const string AREA_ID_INPUT = "AreaID";

        private const string AMOUNT_INPUT = "Amount";
        public enum QuestObjective
        {
            Slay,
            Fetch,
            Interact,
            Escort,
            Craft,
            Explore
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {

            context.AddOption(QUEST_OBJECTIVE_OPTION, typeof(QuestObjective))
                .WithDisplayName("Acquisition Method")
                .WithDefaultValue(QuestObjective.Slay)
                .WithTooltip("The Objective of the quest")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            INodeOption acqOption = GetNodeOptionByName(QUEST_OBJECTIVE_OPTION);
            acqOption.TryGetValue(out QuestObjective qObj);
            
            switch (qObj)
            {
                
                case QuestObjective.Slay:
                    context.AddInputPort(SLAY_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Enemy ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                case QuestObjective.Fetch:
                    context.AddInputPort(ITEM_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Item ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                case QuestObjective.Interact:
                    context.AddInputPort(NPC_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Npc ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case QuestObjective.Escort:
                    context.AddInputPort(NPC_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Npc ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    break;

                case QuestObjective.Craft:
                    context.AddInputPort(ITEM_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Item ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                    case QuestObjective.Explore:
                    context.AddInputPort(AREA_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Area ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
            }

        }

    }
    
    public class QuestRewardNode : QuestMakerNode
    {

        private const string REWARD_TYPE_OPTION = "AcquisitionMethod";

        private const string AMOUNT_INPUT = "Amount";
        private const string EXP_INPUT = "ExpReward";
        private const string CURRENCY_INPUT = "CurrencyReward";
        private const string ITEM_ID_INPUT = "ItemID";
        private const string SKILL_ID_INPUT = "SkillID";
        public enum RewardType
        {
            Exp,
            Currency,
            Item,
            Skill,
        }

        protected override void OnDefineOptions(IOptionDefinitionContext context)
        {

            context.AddOption(REWARD_TYPE_OPTION, typeof(RewardType))
                .WithDisplayName("Acquisition Method")
                .WithDefaultValue(RewardType.Exp)
                .WithTooltip("The type of the reward.")
                .Build();
        }

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            base.OnDefinePorts(context);

            INodeOption acqOption = GetNodeOptionByName(REWARD_TYPE_OPTION);
            acqOption.TryGetValue(out RewardType rewardType);

            switch (rewardType)
            {
                case RewardType.Exp:
                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case RewardType.Currency:

                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case RewardType.Item:

                    context.AddInputPort(ITEM_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Item ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();

                    context.AddInputPort(AMOUNT_INPUT)
                        .WithDataType(typeof(int))
                        .WithDefaultValue(0)
                        .WithDisplayName("Amount")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;

                case RewardType.Skill:
                    context.AddInputPort(SKILL_ID_INPUT)
                        .WithDataType(typeof(string))
                        .WithDefaultValue(default)
                        .WithDisplayName("Skill ID")
                        .WithConnectorUI(PortConnectorUI.Circle)
                        .Build();
                    break;
                default:

                    break;
            }

        }
    }
}