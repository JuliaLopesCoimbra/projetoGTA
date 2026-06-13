using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingUpDown : MonoBehaviour
{
    public float amplitude = 0.05f;
    public float frequency = 1f;
    public float degreesPerSecond = 15f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Flutua para cima e para baixo
        transform.position = startPos + new Vector3(0,
            Mathf.Sin(Time.time * frequency) * amplitude, 0);

        // Gira
        transform.Rotate(0, degreesPerSecond * Time.deltaTime, 0);
    }
}