using QuestMaker.Domain;
using QuestMaker.Editor.Graph;
using Unity.GraphToolkit.Editor;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

[ScriptedImporter(1, "qmgraph")]
public partial class QMGraphImporter : ScriptedImporter
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
        ConsoleLogger.Log(this, "Asset Saved!");

        ctx.AddObjectToAsset("metadata", assetFile);

        ctx.SetMainObject(assetFile);

    }
}
