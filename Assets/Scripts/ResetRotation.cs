using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{
    private Transform myTransform;

    void Start()
    {
        myTransform = GetComponent<Transform>();
    }

    void LateUpdate()
    {
        // Reset world rotation to (0, 0, 0)
        myTransform.rotation = Quaternion.identity;
    }
}
