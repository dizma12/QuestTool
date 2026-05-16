using QuestMaker.Editor.Graph;
using UnityEditor;


[CustomEditor(typeof(QMGraphAssetFile))]
internal class QMGraphMetadataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var meta = (QMGraphAssetFile)target;

        EditorGUILayout.LabelField("QM Graph Metadata", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.TextField("GUID", meta.Guid);
        }
    }
}

