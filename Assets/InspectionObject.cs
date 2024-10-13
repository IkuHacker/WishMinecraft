using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionObject : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject objectToInspect;
    public GameObject head;


    public float rotationSpeed = 100f;
    public float rotationHeadSpeed = 100f;


    // Limites de rotation sur les axes X et Y
    public float minRotationX = -45f;  // Limite minimale de rotation sur l'axe X
    public float maxRotationX = 45f;   // Limite maximale de rotation sur l'axe X
    public float minRotationY = -45f;  // Limite minimale de rotation sur l'axe Y
    public float maxRotationY = 45f;   // Limite maximale de rotation sur l'axe Y

    public float minRotationHeadX = -45f;  // Limite minimale de rotation sur l'axe X
    public float maxRotationHeadX = 45f;   // Limite maximale de rotation sur l'axe X
    public float minRotationHeadY = -45f;  // Limite minimale de rotation sur l'axe Y
    public float maxRotationHeadY = 45f;   // Limite maximale de rotation sur l'axe Y

    private Vector3 previousMousePosition;
    private float currentRotationX = 0f;  // Pour stocker la rotation actuelle sur X
    private float currentRotationY = 0f;  // Pour stocker la rotation actuelle sur Y


    private float currentRotationHeadX = 0f;  // Pour stocker la rotation actuelle sur X
    private float currentRotationHeadY = 0f;  // Pour stocker la rotation actuelle sur Y

    private void Start()
    {
        Quaternion rotation = Quaternion.Euler(-0.1f, 71f, 9.4f);
        objectToInspect.transform.rotation = rotation;
        previousMousePosition = Input.mousePosition; // Initialisation de la position de la souris
    }

    void Update()
    {
        objectToInspect.SetActive(inventoryPanel.activeSelf);

        if (inventoryPanel.activeSelf) // Rotation seulement si le panneau est actif et le bouton est enfoncé
        {
            Vector3 deltaMousePosition = Input.mousePosition - previousMousePosition;
            float deltaRotationX = deltaMousePosition.y * rotationSpeed * Time.deltaTime;
            float deltaRotationY = -deltaMousePosition.x * rotationSpeed * Time.deltaTime;

            // Applique le delta à la rotation actuelle
            currentRotationX += deltaRotationX;
            currentRotationY += deltaRotationY;

            currentRotationHeadX += deltaRotationX;
            currentRotationHeadY += deltaRotationY;

            // Limite les rotations sur les axes X et Y avec Mathf.Clamp
            currentRotationX = Mathf.Clamp(currentRotationX, minRotationX, maxRotationX);
            currentRotationY = Mathf.Clamp(currentRotationY, minRotationY, maxRotationY);

            currentRotationHeadX = Mathf.Clamp(currentRotationHeadX, minRotationHeadX, maxRotationHeadX);
            currentRotationHeadY = Mathf.Clamp(currentRotationHeadY, minRotationHeadY, maxRotationHeadY);

            // Applique les nouvelles rotations à l'objet à inspecter
            Quaternion targetRotation = Quaternion.Euler(currentRotationX, currentRotationY, 0);
            Quaternion targetRotationHead = Quaternion.Euler(currentRotationHeadX, currentRotationHeadY, 0);

            objectToInspect.transform.rotation = Quaternion.Slerp(objectToInspect.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, targetRotationHead, Time.deltaTime * rotationHeadSpeed);

        }

        previousMousePosition = Input.mousePosition; // Mise à jour de la position de la souris
    }
}
