using QuestMaker.Editor.Compiler;
using System;
using UnityEngine;

namespace QuestMaker.Editor.CompilationModules
{
    /// <summary>
    /// A scoped resolver tied to one specific builder instance.
    /// Context nodes create a scope from their builder and pass it to block nodes via Compose().
    /// Block nodes call scope.Get&lt;IItemModule&gt;() etc. — they never know the concrete builder type.
    /// </summary>
    internal sealed class ModuleScope
    {
        private readonly IQuestModuleBuilder _builder = null;
        private readonly ModuleBuilderRegistry _registry = null;

        internal ModuleScope(IQuestModuleBuilder builder, ModuleBuilderRegistry registry)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        /// <summary>
        /// Returns the module interface TModule from the builder this scope is bound to.
        /// Returns null and logs an error if the builder does not implement TModule.
        /// </summary>
        public TModule Get<TModule>() where TModule : class, IQuestModule
        {
            if (_builder is TModule module)
                return module;

            Debug.LogError($"[ModuleScope] Builder {_builder.GetType().Name} does not implement {typeof(TModule).Name}");
            return null;
        }
    }
}