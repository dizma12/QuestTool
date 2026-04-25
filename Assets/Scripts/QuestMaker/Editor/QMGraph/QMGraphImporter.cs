using QuestMaker.Editor.Graph;
using QuestMaker.Editor.Utility;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "qmgraph")]
public class QMGraphImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        QMGraph graph = GraphDatabase.LoadGraph<QMGraph>(ctx.assetPath);
        //converting graph path to guid
        string guid = AssetDatabase.AssetPathToGUID(ctx.assetPath);

        //Hides the importer Window on inspector
        hideFlags = HideFlags.HideInInspector;

        QMGraphAssetFile assetFile = ScriptableObject.CreateInstance<QMGraphAssetFile>();
        assetFile.name = "metadata";
        assetFile.SetTargetGUID(guid);
        Debug.Log("Created new AssetFile");

        ctx.AddObjectToAsset("metadata", assetFile);

        ctx.SetMainObject(assetFile);

    }

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


            //if (GUI.changed)
            //    EditorUtility.SetDirty(meta);
        }
    }
}
