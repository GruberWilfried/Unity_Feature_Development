using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeshFromSprite : MonoBehaviour
{
    public float depth = 1f;
    public Sprite sprite;
    public GameObject cube;
    public Material mat;

    void Start()
    {
        Debug.Log(sprite.rect);

        float width = sprite.rect.width;
        float height = sprite.rect.height;

        int count = 0;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float red = sprite.texture.GetPixel(i, j).r;
                float green = sprite.texture.GetPixel(i, j).g;
                float blue = sprite.texture.GetPixel(i, j).b;
                float alpha = sprite.texture.GetPixel(i, j).a;

                if (alpha > 0.2f)
                {
                    var pos = new Vector3(i, j, 0);

                    var instance = Instantiate(cube, pos, Quaternion.identity);
                    instance.gameObject.transform.localScale = new Vector3(1,1,depth);
                    SetUVs(instance,i,j,(int)width,(int)height);
                    var mr = instance.gameObject.GetComponent<MeshRenderer>();
                    mr.material = mat;
                    count += 1;
                }
            }
        }
    }

    public Vector2 ConvertPixelsToUVCoordinates(int x, int y, int textureWidth, int textureHeight)
    {
        return new Vector2((float)x / textureWidth, (float)y / textureHeight);
    }

    public void SetUVs(GameObject cube, int x, int y, int imageWidth, int imageHeight)
    {
        Mesh mesh = cube.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = ConvertPixelsToUVCoordinates(x,y, imageWidth, imageHeight);
        }
        mesh.uv = uvs;
    }
}
