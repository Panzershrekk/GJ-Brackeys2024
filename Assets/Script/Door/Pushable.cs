using UnityEngine;

public class Pushable : MonoBehaviour
{
  public float pushForce = 500f; // Adjust the force applied to the door
  public float interactDistance = 5f; // Maximum distance from which the player can interact with the door

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      PushDoor();
    }
  }

  void PushDoor()
  {
    RaycastHit hit;
    // Perform a raycast from the camera forward
    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance))
    {
      // Check if the raycast hit an object tagged as "Door"
      if (hit.collider.CompareTag("DOOR"))
      {
        Rigidbody doorRb = hit.collider.GetComponent<Rigidbody>();
        HingeJoint doorHinge = hit.collider.GetComponent<HingeJoint>();
        if (doorRb != null)
        {
          // Optional: Disable the Hinge Joint to allow the door to move freely
          if (doorHinge != null)
          {
            Destroy(doorHinge);
          }

          // Apply the force in the direction the player is looking
          Vector3 forceDirection = Camera.main.transform.forward;

          // Apply the force at the point where the raycast hit the door to simulate a kick
          doorRb.AddForceAtPosition(forceDirection * pushForce, hit.point, ForceMode.Impulse);
        }
      }
    }
  }
}
