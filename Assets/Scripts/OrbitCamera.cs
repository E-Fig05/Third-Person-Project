using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float rotSpeed = 1.5f;

    private float rotY;
    private Vector3 offset;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotY = transform.localEulerAngles.y;

        offset = target.position - transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if(!Mathf.Approximately(horizontalInput, 0f))
        {
            rotY += horizontalInput * rotSpeed;
        }
        else
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed * 3;
        }

        Quaternion rotation = Quaternion.Euler(0, rotY, 0);
        transform.position = target.position - (rotation * offset);

        transform.LookAt(target);
    }
}
