using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public MeshRenderer Renderer;

    [SerializeField] Vector3 position = new Vector3(3, 4, 1);
    [SerializeField] Vector3 scale = Vector3.one * 1.3f;
    [SerializeField] float angleToRotate = 10.0f;
    [SerializeField] float rotationSpeed = 1.0f;
    [SerializeField] Color color = new Color(0.5f, 1.0f, 0.3f, 0.4f);
    [SerializeField] float alpha = 0.4f;

    void Start()
    {
        transform.position = position;
        transform.localScale = scale;

        Material material = Renderer.material;

        color.a = alpha;
        material.color = color;
    }
    
    void Update()
    {
        transform.Rotate(angleToRotate * Time.deltaTime * rotationSpeed, 0.0f, 0.0f);
    }
}
