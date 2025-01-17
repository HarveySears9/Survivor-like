using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetRotation : MonoBehaviour
{

    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
