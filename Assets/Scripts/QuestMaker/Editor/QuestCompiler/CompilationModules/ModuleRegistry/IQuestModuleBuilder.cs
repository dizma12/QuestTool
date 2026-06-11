
using QuestMaker.Data;
using QuestMaker.Data.SpecialEvents;


namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal interface IQuestModuleBuilder
    {
        void Build(QuestSO quest);
    }


    internal interface IQuestModule { }
    internal interface IItemModule : IQuestModule
    {
        public void SetItem(Item item, int amount = 1);
    }
    internal interface ILevelModule : IQuestModule
    {
        public void SetLevel(int level);
    }

    internal interface INpcModule : IQuestModule
    {
        public int ID { get; protected set; }
        public void SetNpcID(int id) => ID = id;
    }

    internal interface IExpModule : IQuestModule
    {
        public void SetExp(int amount);
    }
    internal interface IReputationModule : IQuestModule
    {
        public void SetReputationFaction(ReputationFaction rep);
    }
    internal interface IQuestInfoModule : IQuestModule
    {
        public void SetQuestName(string name);
        public void SetQuestDescription(string desc);
        public void SetQuestType(QuestType type);

    }
    internal interface IAbilityModule : IQuestModule
    {
        public void SetAbility(string abilityId);
    }

    internal interface IInGameTimeConstraintModule : IQuestModule
    {
        public void SetTimeConstraint(InGameTimeline time);
    }

    internal interface ISpecialEventModule : IQuestModule
    {
        public void SetSpecialEvent(SpecialEventData eventData);
    }
}
