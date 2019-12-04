using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMeshFromSprite : MonoBehaviour
{
    public Sprite sprite;
    public GameObject cube;

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
                    var mr = instance.gameObject.GetComponent<MeshRenderer>();
                    Color col = new Color(red, green, blue, alpha);
                    mr.material.SetColor("_Color", col);
                    count += 1;
                }

                
            }
            
        }
    }
}
