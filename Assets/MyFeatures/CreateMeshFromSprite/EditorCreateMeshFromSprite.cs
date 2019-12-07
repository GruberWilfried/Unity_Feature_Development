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
        source = EditorGUILayout.ObjectField(source, typeof(Texture2D), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Check"))
        {
            if (source != null)
            {
                CreatePrefab(source as Texture2D, "test");
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

    public void CreatePrefab(Texture2D tex, string name)
    {
        float width = tex.width;
        float height = tex.height;

        GameObject parent = new GameObject();
        parent.name = name;

        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = tex as Texture2D;

        // iterate through all pixels and create cubes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // get color of the individuel pixel
                float red = tex.GetPixel(x, y).r;
                float green = tex.GetPixel(x, y).g;
                float blue = tex.GetPixel(x, y).b;
                float alpha = tex.GetPixel(x, y).a;

                if (alpha > 0.1f)
                {
                    Vector3 spawnPos = new Vector3(x,y,0);

                    GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    voxel.transform.position = spawnPos;

                    voxel.transform.SetParent(parent.transform);

                    voxel.GetComponent<MeshRenderer>().material = mat;
                }
            }
        }
    }
}