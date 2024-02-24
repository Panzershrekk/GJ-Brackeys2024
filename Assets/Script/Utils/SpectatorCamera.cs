using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
  public float movementSpeed = 5.0f;
  public float mouseSensitivity = 2.0f;
  public float clampAngle = 80.0f;

  private float verticalRotation = 0.0f;
  private float horizontalRotation = 0.0f;

  public KeyCode moveForwardKey = KeyCode.Z;
  public KeyCode moveBackwardKey = KeyCode.S;
  public KeyCode moveLeftKey = KeyCode.Q;
  public KeyCode moveRightKey = KeyCode.D;
  public KeyCode moveUpKey = KeyCode.Space;
  public KeyCode moveDownKey = KeyCode.LeftShift;

  void Start()
  {
    Vector3 rot = transform.localRotation.eulerAngles;
    horizontalRotation = rot.y;
    verticalRotation = rot.x;
  }

  void Update()
  {
    // Mouse look
    float mouseX = Input.GetAxis("Mouse X");
    float mouseY = -Input.GetAxis("Mouse Y");

    horizontalRotation += mouseX * mouseSensitivity;
    verticalRotation += mouseY * mouseSensitivity;
    verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

    Quaternion localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0.0f);
    transform.rotation = localRotation;

    // Keyboard movement using configurable keys
    float forwardBackward = (Input.GetKey(moveForwardKey) ? 1 : Input.GetKey(moveBackwardKey) ? -1 : 0) * movementSpeed;
    float leftRight = (Input.GetKey(moveLeftKey) ? -1 : Input.GetKey(moveRightKey) ? 1 : 0) * movementSpeed;
    float upDown = (Input.GetKey(moveUpKey) ? 1 : Input.GetKey(moveDownKey) ? -1 : 0) * movementSpeed;

    Vector3 movementDirection = (transform.forward * forwardBackward) + (transform.right * leftRight) + (transform.up * upDown);
    transform.position += movementDirection * Time.deltaTime;
  }
}
