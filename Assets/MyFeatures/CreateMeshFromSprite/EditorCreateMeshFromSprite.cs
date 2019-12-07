using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorCreateMeshFromSprite : EditorWindow
{

    public Object source;

    [MenuItem("FeatureExtensions/Create Prefab from Texture")]
    public static void ShowWindow()
    {
        GetWindow<EditorCreateMeshFromSprite>("Create Prefab from Texture");
    }

    private void OnGUI()
    {
        GUILayout.Label("Instructions", EditorStyles.boldLabel);
        GUILayout.Label("1. Assign Sprite", EditorStyles.label);
        GUILayout.Label("2. Press Button", EditorStyles.label);
        GUILayout.Label("3. Use created Prefab from Projectwindow", EditorStyles.label);

        EditorGUILayout.BeginHorizontal();
        source = EditorGUILayout.ObjectField(source, typeof(Texture2D), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create Prefab from Texture"))
        {
            if (source != null)
            {
                string[] paths = CreateAssetFolders();
                CreatePrefab(source as Texture2D, paths[0], paths);
            }
        }
    }

    public string[] CreateAssetFolders()
    {
        string[] info = new string[5];

        string path = EditorUtility.SaveFilePanelInProject("", "", "", "");
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

        AssetDatabase.CreateFolder(pathToFolder, pathElements[pathElements.Length - 1]);
        AssetDatabase.CreateFolder(path, "Meshes");
        AssetDatabase.CreateFolder(path, "Material");
        AssetDatabase.CreateFolder(path, "Prefab");

        
        string pathMeshes = path + "/Meshes";
        string pathMaterial = path + "/Material";
        string pathPrefab = path + "/Prefab";
        string name = pathElements[pathElements.Length - 1];

        Debug.Log("name: " + name);
        Debug.Log("path: " + path);
        Debug.Log("pathToFolder: " + pathToFolder);
        Debug.Log("pathMeshes: " + pathMeshes);
        Debug.Log("pathMaterial: " + pathMaterial);
        Debug.Log("pathPrefab: " + pathPrefab);

        info[0] = name;
        info[1] = path;
        info[2] = pathMeshes;
        info[3] = pathMaterial;
        info[4] = pathPrefab;

        return info;
    }

    public void CreatePrefab(Texture2D tex, string name, string[] paths)
    {
        float width = tex.width;
        float height = tex.height;

        Debug.Log((int)width);
        Debug.Log((int)height);

        GameObject parent = new GameObject();
        parent.name = name;

        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = tex as Texture2D;
        AssetDatabase.CreateAsset(mat, paths[3] + "/" + name + ".mat");

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



                    // Create the Meshes as Asset
                    Mesh mesh = voxel.GetComponent<MeshFilter>().mesh;
                    AssetDatabase.CreateAsset(mesh, paths[2] + "/" + name + "x" + x + "y" + y + ".asset");

                    // Set UVs
                    SetUVs(voxel,x,y,(int)width,(int)height);

                }
            }
        }

        // Create the Prefab as Asset
        PrefabUtility.SaveAsPrefabAsset(parent, paths[4] + "/" + name + ".prefab");
    }

    public void SetUVs(GameObject cube, int x, int y, int imageWidth, int imageHeight)
    {
        Mesh mesh = cube.GetComponent<MeshFilter>().sharedMesh;
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