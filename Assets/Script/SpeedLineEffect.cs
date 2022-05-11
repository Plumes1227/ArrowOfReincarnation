using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLineEffect : MonoBehaviour
{
    [SerializeField] Vector2 scrillVelocity;

    Material material;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        material.mainTextureOffset += scrillVelocity * Time.deltaTime;
    }
}

