using QuestMaker.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace QuestMaker.Editor.Compiler.CompilationModules
{
    internal class ModuleRegistry
    {
        private readonly Dictionary<Type, Type[]> reg = new();
        
        private readonly Dictionary<Type, IQuestModuleBuilder> modules = new();

        /// <summary>
        /// Array of all active IQuestModuleBuilders
        /// </summary>
        public IQuestModuleBuilder[] Modules { get => modules.Values.ToArray(); }

        /// <summary>
        /// Gets or Creates a building module based on TBuilder. 
        /// TBuilder = class that implements the IQuestModuleBuilder. 
        /// TModule = Specific module to retrieve from IQuestModule
        /// </summary>
        /// <typeparam name="TBuilder">class that implements the IQuestModuleBuilder</typeparam>
        /// <typeparam name="TModule">Specific module to retrieve from IQuestModule</typeparam>
        /// <returns>Builder instance as TModule</returns>
        public TModule GetModule<TBuilder, TModule>() where TBuilder : class, IQuestModuleBuilder, new() where TModule : class, IQuestModule
        {
            Type builderType = typeof(TBuilder);
            TBuilder builder = null;

            if (!reg.ContainsKey(builderType) || !modules.ContainsKey(builderType))
                builder = CreateModule<TBuilder>();

            if (builder == null)
            {
                Debug.Log($"[ModuleRegistry] Builder returned null from CreateModule ");
                return default;
            }

            if (!reg[builderType].Contains(typeof(TModule)))
            {
                Debug.Log($"[ModuleRegistry] No TModules found ");
                return default;
            }

            return modules[builderType] as TModule;

        }

        /// <summary>
        /// Creates and/or adds a new TBuilder to Type and module registries.
        /// TBuilder = class that implements the IQuestModuleBuilder.
        /// </summary>
        /// <typeparam name="TBuilder">Class that implements the IQuestModuleBuilder.</typeparam>
        /// <returns></returns>
        private TBuilder CreateModule<TBuilder>() where TBuilder : class, IQuestModuleBuilder, new()
        {
            Type type = typeof(TBuilder);
            TBuilder builder = null;

            if (!reg.ContainsKey(type))
            {                                                                               //gets assignable types to IQuestModule
                var questModuleTypes = type.GetInterfacesOfType(typeof(IQuestModule))
                                                 .Where(t => t != typeof(IQuestModule)) // Removes IQuestModule itself 
                                                 .ToArray();                                // so only subtypes remain
                reg.Add(type, questModuleTypes);
                Debug.Log($"[ModuleRegistry] Added module builder to registry of type: {type} and {questModuleTypes.Length} submodules");
            }
            if (modules.ContainsKey(type))
            {
                builder = modules[type] as TBuilder;
                Debug.Log($"[ModuleRegistry] Module of type: {type} already found to modules ");
            }
            else
            {
                builder = new TBuilder();
                modules.Add(type, builder);
                Debug.Log($"[ModuleRegistry] Added new module of type: {builder.GetType()} to modules ");

            }
            return builder;
        }
    }
}