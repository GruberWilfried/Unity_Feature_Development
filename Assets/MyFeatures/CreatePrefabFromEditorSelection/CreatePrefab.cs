using UnityEngine;
using UnityEditor;


class CreatePrefab
{


    [MenuItem("FeatureExtensions/Create Prefab from Selection")]
    static void Create()
    {
        GameObject selection = Selection.activeGameObject;
        string filePath = EditorUtility.SaveFilePanelInProject("Save Procedural Mesh", "pixel_mesh", "prefab", "");
        if (filePath == "") return;

        PrefabUtility.SaveAsPrefabAsset(selection, filePath);
    }


}