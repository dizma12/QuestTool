using QuestMaker.Editor.Graph;

using UnityEngine;

namespace Assets.Scripts.QuestMaker.Editor
{
    [CreateAssetMenu(menuName = "QuestMaker/Test Compiler")]
    internal class TestCompiler : ScriptableObject
    {
        [SerializeReference] QMGraph m_graph;
        
    }
}
