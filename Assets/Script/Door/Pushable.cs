using UnityEngine;

public class Pushable : MonoBehaviour
{
  public float pushForce = 20f; // Adjust the force applied to the door
  public float interactDistance = 5f; // Maximum distance from which the player can interact with the door
  public LayerMask layerMask;

  // Update is called once per frame
  void Update()
  {
    Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;
    Debug.DrawRay(transform.position, forward, Color.green);

    if (Input.GetKeyDown(KeyCode.E))
    {
      PushDoor();
    }
  }

  void PushDoor()
  {
    RaycastHit hit;

    if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDistance, layerMask))
    {
      if (hit.collider.GetComponent<Door>() != null)
      {
        Rigidbody doorRb = hit.collider.GetComponent<Rigidbody>();
        HingeJoint doorHinge = hit.collider.GetComponent<HingeJoint>();
        if (doorRb != null)
        {
          if (doorHinge != null)
          {
            Destroy(doorHinge);
            // TODO: verify it's not null
            hit.collider.transform.GetComponent<SuckableBehaviour>().enabled = true;
          }

          Vector3 forceDirection = Camera.main.transform.forward;

          doorRb.AddForceAtPosition(forceDirection * pushForce, hit.point, ForceMode.Impulse);
        }
      }
    }
  }
}
