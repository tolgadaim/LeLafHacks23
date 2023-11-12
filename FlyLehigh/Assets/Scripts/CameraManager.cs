using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Serialized variables
    [SerializeField]
    private Vector2 _mouseSensitivity = new Vector2(2f, 1.8f);
    [SerializeField]
    private Vector2 _yAngleClamp = new Vector2(-85f, 85f);

    // Private variables
    private Vector2 _cameraRotation = new Vector2(0f, 0f);
    private bool _canLookAround;

    void Start()
    {
        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C))
            _canLookAround = true;
        else
            _canLookAround = false;

        // If pressing down C
        if (_canLookAround)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity.x;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity.y;

            // Rotate the camera by mouse Y movement
            _cameraRotation.x -= mouseY;
            _cameraRotation.y += mouseX;
            _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, _yAngleClamp.x, _yAngleClamp.y);
            transform.localRotation = Quaternion.Euler(_cameraRotation.x, _cameraRotation.y, transform.localRotation.z);
        }
        else
        {
            // If the player cannot look around, smoothly slerp back to looking forward
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0f, 0f, 0f), Time.deltaTime * 5f);
            _cameraRotation.x = 0;
            _cameraRotation.y = 0;
        }
    }
}