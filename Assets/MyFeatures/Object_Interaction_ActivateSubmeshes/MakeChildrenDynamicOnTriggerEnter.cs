using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeChildrenDynamicOnTriggerEnter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
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
