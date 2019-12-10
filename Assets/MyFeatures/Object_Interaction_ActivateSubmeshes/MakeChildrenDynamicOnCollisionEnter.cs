using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildrenDynamicOnCollisionEnter : MonoBehaviour
{
    public string layerName = "ground";

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == layerName)
        {
            Debug.Log(other.gameObject.tag);
            Debug.Log(other.gameObject.name);
            var rb = GetComponentsInChildren<Rigidbody>();
            var mr = GetComponentsInChildren<BoxCollider>();

            foreach (var item in rb)
            {
                item.isKinematic = false;
                item.useGravity = true;
            }

            foreach (var item in mr)
            {
                item.enabled = true;
            }
        }

    }
}
