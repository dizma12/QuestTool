
using QuestMaker.Data;
using QuestMaker.Runtime;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal interface IQuestModuleBuilder
    {
        void Build(QuestSO quest);
    }


    internal interface IQuestModule { }
    internal interface IItemModule : IQuestModule
    {
        void SetItem(Item item, int amount = 1);
    }
    internal interface ILevelModule : IQuestModule
    {
        void SetLevel(int level);
    }

    internal interface INpcModule : IQuestModule
    {
        int ID { get; protected set; }
        void SetNpcID(int id) => ID = id;
    }

    internal interface IExpModule : IQuestModule
    {
        void SetExp(int amount);
    }
}
