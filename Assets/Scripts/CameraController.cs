using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 150f;
    private InputSystem_Actions controls;

    void Awake()
    {
        controls = new InputSystem_Actions();   
    }

    void OnEnable()
    {
        controls.Player.Enable();
    }

    void OnDisable()
    {
        controls.Player.Disable();
    }

    void Update()
    {
        Vector2 moveInput = controls.Player.Move.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;
        transform.Rotate(Vector3.up, horizontalInput * Time.deltaTime * rotateSpeed);
    }
}
