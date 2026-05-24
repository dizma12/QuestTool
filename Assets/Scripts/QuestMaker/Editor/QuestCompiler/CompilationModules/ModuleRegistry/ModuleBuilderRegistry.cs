using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using QuestMaker.Core.Extensions;
namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class ModuleBuilderRegistry
    {
        private readonly Dictionary<Type, Type[]> reg = new();

        private readonly Dictionary<Type, IQuestModuleBuilder> modules = new();

        private readonly List<IQuestModuleBuilder> objectiveModules = new();

        /// <summary>
        /// Array of all active Objective IQuestModuleBuilders
        /// </summary>
        public IQuestModuleBuilder[] ObjectiveModules
        {
            get
            {
                if (objectiveModules == null || !objectiveModules.Any())
                {
                    return null;
                }

                return objectiveModules.ToArray();
            }
        }

        /// <summary>
        /// Array of all active IQuestModuleBuilders (Objective Modules NOT included).
        /// </summary>
        public IQuestModuleBuilder[] Modules
        {
            get
            {
                if (modules == null || !modules.Any())
                {
                    return null;
                }

                return modules.Values.ToArray();
            }
        }

        /// <summary>
        /// Gets or Creates a building module based on TBuilder. 
        /// TBuilder = class that implements the IQuestModuleBuilder. 
        /// TModule = Specific module to retrieve from IQuestModule
        /// </summary>
        /// <typeparam name="TBuilder">class that implements the IQuestModuleBuilder</typeparam>
        /// <typeparam name="TModule">Specific module to retrieve from IQuestModule</typeparam>
        /// <returns>Builder instance as TModule</returns>
        public TModule RequestModule<TBuilder, TModule>()
            where TBuilder : class, IQuestModuleBuilder, new()
            where TModule : class, IQuestModule
        {
            Type builderType = typeof(TBuilder);

            if (!ExistsOnRegistry<TBuilder>())
                return default;

            if (!modules.TryGetValue(builderType, out IQuestModuleBuilder builder))
                builder = CreateBuilder<TBuilder>();


            if (!ContainsModule(builderType, typeof(TModule)))
            {
                Debug.LogError($"[ModuleRegistry] Builder {builderType} does not implement {typeof(TModule)}");
                return default;
            }

            return builder as TModule;

        }


        public void AddObjectiveModule(ObjectiveModule module)
        {
            // VisualStudio did my 18 line ifs into 1 liner KEKW T_T
            if (module is not IQuestModuleBuilder bldr || objectiveModules.Contains(bldr)) return;

            objectiveModules.Add(bldr);
        }
        public TBuilder RequestNewObjectiveModule<TBuilder>() where TBuilder: ObjectiveModule, IQuestModuleBuilder, new()
        {
            ObjectiveModule module = new();
            objectiveModules.Add(module);

            return module as TBuilder;
        }

        public TBuilder GetBuilder<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type builderType = typeof(TBuilder);

            if (!ExistsOnRegistry<TBuilder>())
                return default;

            if (!modules.TryGetValue(builderType, out IQuestModuleBuilder builder))
                builder = CreateBuilder<TBuilder>();

            return builder as TBuilder;
        }

        public TModule GetModuleByType<TModule>(Type builderType) where TModule : class, IQuestModule, new()
        {
            if(!reg.ContainsKey(builderType))
                return default;

            if(!ContainsModule(builderType, typeof(TModule)))
                return default;

            if (!modules.TryGetValue(builderType, out IQuestModuleBuilder builder))
                return default;

            return builder as TModule;
        }

        public TModule GetModuleByBuilder<TBuilder, TModule>(TBuilder builder)
            where TBuilder : class, IQuestModuleBuilder, new()
            where TModule : class, IQuestModule
        {
            Type builderType = typeof(TBuilder);

            if (builder == null)
            {
                Debug.LogError($"[ModuleRegistry] The builder provided is null");
                return default;
            }

            if (!ExistsOnRegistry<TBuilder>())
                return default;

            if (!reg[builderType].Contains(typeof(TModule)))
            {
                Debug.LogError($"[ModuleRegistry] Builder {builderType} does not implement {typeof(TModule)}");
                return default;
            }
            return builder as TModule;
        }

        /// <summary>
        /// Checks registry if Builders List of IQuestModule contains module type.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        private bool ContainsModule(Type builder, Type module)
        {
            return reg[builder].Contains(module);
        }

        /// <summary>
        /// Checks if TBuilder implements interfaces of type IQuestModule, if it does is added to registry.
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns>True if added.</returns>
        private bool TryRegisterBuilderType<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);

            //gets assignable types to IQuestModule
            var questModuleTypes = type.GetInterfacesOfType(typeof(IQuestModule))
                                             .Where(t => t != typeof(IQuestModule)) // Removes IQuestModule itself 
                                             .ToArray();// so only subtypes remain

            if (!questModuleTypes.Any())
            {
                Debug.LogError($"[ModuleRegistry] Builder {type} does not implement {typeof(IQuestModule)}");
                return false;
            }

            reg.Add(type, questModuleTypes);
            Debug.Log($"[ModuleRegistry] Added module builder to registry of type: {type} and {questModuleTypes.Length} submodules");
            return true;
        }

        /// <summary>
        /// Checks the registry for Builder. If it doesnt exist tries to create a new one.
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns>False if doesnt exist and failed to create a new one.</returns>
        private bool ExistsOnRegistry<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);
            if (reg.ContainsKey(type))
                return true;

            return TryRegisterBuilderType<TBuilder>();
        }


        /// <summary>
        /// Creates A new IQuestModuleBuilder as TBuilder.
        /// </summary>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        private TBuilder CreateBuilder<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);

            TBuilder builder = new();

            modules.Add(type, builder);
            Debug.Log($"[ModuleRegistry] Added new module of type: {builder.GetType()} to modules ");
            return builder;
        }

        public void Clear(bool areYouSure)
        {
            if (!areYouSure)
                return;

            modules.Clear();
            reg.Clear();
            objectiveModules.Clear();
        }
    }
}