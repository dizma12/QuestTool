
using QuestMaker.Editor.Compiler.CompilationModules;
using System;

namespace QuestMaker.Editor.Compiler
{
    internal interface IComposableNode
    {
        public void Compose<TBuilder>(TBuilder moduleBuilder, ModuleBuilderRegistry reg) where TBuilder : class, IQuestModuleBuilder, new();
    }
}
