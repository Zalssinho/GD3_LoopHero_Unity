using System.Data;
using UnityEngine;
using UnityEngine.Rendering;

public class SightPerception : MonoBehaviour
{
    [SerializeField] private bool IsDetected = false;
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private GameObject detectionObject;
    private Vector3 targetDirection;

    private void Update()
    {
        ActivateDetection();
    }
    private void ActivateDetection()
    {
        targetDirection = detectionObject.transform.position - transform.position;
        if (Vector3.Dot(transform.forward, Vector3.Normalize(targetDirection)) > 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetDirection, out hit, detectionRadius))
            {
                if (hit.collider.gameObject == detectionObject) //il faudrait avoir un test basé sur un component ou un tag
                {
                    IsDetected = true;
                    return;
                }
            }
        }
        IsDetected = false;
    }
}
