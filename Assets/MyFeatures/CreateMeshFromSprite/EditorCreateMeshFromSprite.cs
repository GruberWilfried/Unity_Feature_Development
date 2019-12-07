using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCreateMeshFromSprite : EditorWindow
{

    public Object source;

    [MenuItem("FeatureExtensions/Window")]
    public static void ShowWindow()
    {
        GetWindow<EditorCreateMeshFromSprite>("Create Prefab from Sprite");
    }

    private void OnGUI()
    {
        GUILayout.Label("This is a label", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        source = EditorGUILayout.ObjectField(source, typeof(Sprite), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Check"))
        {
            if (source != null)
            {

            }
        }

        if (GUILayout.Button("Create Prefab"))
        {
            string path = EditorUtility.SaveFilePanelInProject("","","","");
            string[] pathElements = path.Split(char.Parse("/"));

            string pathToFolder = "";
            for (int i = 0; i < (pathElements.Length - 1); i++)
            {
                if (i == 0)
                {
                    pathToFolder = pathElements[i];
                }
                else
                {
                    pathToFolder = pathToFolder + "/" + pathElements[i];
                }
                
            }

            AssetDatabase.CreateFolder(pathToFolder, pathElements[pathElements.Length-1]);

            Debug.Log(pathToFolder);
        }
    }
}