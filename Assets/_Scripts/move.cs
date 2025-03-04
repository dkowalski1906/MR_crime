using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnContact : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(0, 0, 1);
    public float moveSpeed = 1f;
    private HashSet<Collider> collidingObjects = new HashSet<Collider>();

    void Update()
    {
        foreach (var obj in collidingObjects)
        {
            obj.transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("element"))
        {
            collidingObjects.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("element"))
        {
            collidingObjects.Remove(other);
        }
    }
}
