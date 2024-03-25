using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldCamera : MonoBehaviour
{
    [SerializeField] private GameObject bee;
    [SerializeField] private Vector3 offset = new(0, 15, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = bee.transform.position + offset;
    }
}
