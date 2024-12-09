using UnityEngine;

public class RotateObj : MonoBehaviour
{
    [SerializeField] private Transform centerPoint;
    [SerializeField] private float rotationSpeed = 15f; 
    [SerializeField] private Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        if (centerPoint == null)
        {
            Debug.LogWarning("Center Point is not assigned!");
            return;
        }

        RotateAroundCenter();
    }

    private void RotateAroundCenter()
    {
        transform.RotateAround(centerPoint.position, rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
