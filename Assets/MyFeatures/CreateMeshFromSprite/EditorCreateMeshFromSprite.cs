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

        if (GUILayout.Button("Create Geometry in Scene"))
        {
            if (source != null)
            {
                CreatePrefab(source as Texture2D, "test");
            }
        }

        if (GUILayout.Button("Create Folder"))
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

        Debug.Log((int)width);
        Debug.Log((int)height);

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

                    SetUVs(voxel,x,y,(int)width,(int)height);
                }
            }
        }
    }

    public void SetUVs(GameObject cube, int x, int y, int imageWidth, int imageHeight)
    {
        Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = ConvertPixelsToUVCoordinates(x, y, imageWidth, imageHeight);
        }
        mesh.uv = uvs;
    }

    public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
    {
        return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    }

}