using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{

    // Variables
    public Transform player;

    private Vector2 cameraTurn;
    public float maxRotationAngleX;
    public float mouseSensitivity;

    public Transform cameraTransform;
    public InventoryManager inventoryManager;
    public bool isInventoryOpen;



    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        inventoryManager = FindObjectOfType<InventoryManager>();

    }

    void Update()
    {
        isInventoryOpen = inventoryManager.isInventoryOpen;

        if (!isInventoryOpen) 
        {
            cameraTurn.y += Input.GetAxisRaw("Mouse X") * mouseSensitivity;

            cameraTurn.x += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            cameraTurn.x = Mathf.Clamp(cameraTurn.x, -maxRotationAngleX, maxRotationAngleX);


            cameraTransform.localRotation = Quaternion.Euler(cameraTurn.x, -cameraTurn.y, 0f);

            player.localRotation = Quaternion.Euler(0f, -cameraTurn.y, 0f);
        }

      
    }
}