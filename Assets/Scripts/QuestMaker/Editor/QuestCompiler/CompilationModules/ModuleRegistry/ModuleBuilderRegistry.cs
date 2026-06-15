using QuestMaker.Domain;
using QuestMaker.Runtime.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestMaker.Editor.CompilationModules
{
    internal class ModuleBuilderRegistry
    {
        private readonly Dictionary<Type, Type[]> _reg = new();


        private readonly Dictionary<Type, IQuestModuleBuilder> _modules = new();


        private readonly List<IQuestModuleBuilder> _objectiveModules = new();


        /// <summary>
        /// All non-objective module builders.
        /// </summary>
        public IQuestModuleBuilder[] Modules
            => _modules.Count == 0 ? null : _modules.Values.ToArray();

        /// <summary>
        /// All objective module builders.
        /// </summary>
        public IQuestModuleBuilder[] ObjectiveModules
            => _objectiveModules.Count == 0 ? null : _objectiveModules.ToArray();


        /// <summary>
        /// Gets or creates the uniques builder instance of TBuilder.
        /// </summary>
        public TBuilder GetBuilder<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);

            if (!EnforceRegistration<TBuilder>())
                return null;

            if (!_modules.TryGetValue(type, out IQuestModuleBuilder builder))
                builder = CreateBuilder<TBuilder>();

            return builder as TBuilder;
        }

        /// <summary>
        /// Creates a new ObjectiveModule instance and registers it.
        /// </summary>
        public TBuilder RequestNewObjectiveModule<TBuilder>() where TBuilder : ObjectiveModule, IQuestModuleBuilder, new()
        {
            TBuilder module = new();
            _objectiveModules.Add(module);
            return module;
        }

        public void AddObjectiveModule(ObjectiveModule module)
        {
            if (module is not IQuestModuleBuilder bldr || _objectiveModules.Contains(bldr)) return;
            _objectiveModules.Add(bldr);
        }


        public void Clear(bool areYouSure)
        {
            if (!areYouSure) return;

            _modules.Clear();
            _reg.Clear();
            _objectiveModules.Clear();
        }


        private TBuilder CreateBuilder<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            TBuilder builder = new();
            _modules.Add(typeof(TBuilder), builder);
            ConsoleLogger.Log(this, $"Created builder: {typeof(TBuilder).Name}");
            return builder;
        }

        private bool EnforceRegistration<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);
            if (_reg.ContainsKey(type)) return true;

            var moduleTypes = type.GetInterfacesOfType(typeof(IQuestModule))
                                  .Where(t => t != typeof(IQuestModule)) // removes parent interface from list
                                  .ToArray();

            if (!moduleTypes.Any())
            {
                ConsoleLogger.LogError(this, $"{type.Name} implements no IQuestModule interfaces.");
                return false;
            }

            _reg.Add(type, moduleTypes);
            ConsoleLogger.Log(this, $"Registered {type.Name} with {moduleTypes.Length} modules: " +
                      string.Join(", ", moduleTypes.Select(t => t.Name)));
            return true;
        }
    }
}