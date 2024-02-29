using UnityEngine;

public class DoorInteractable : Interactable
{
  public float pushForce = 20f;
  public override void Start()
  {
    actionText = GameUIManager.Instance.doorKickText;
    base.Start();
  }

  public override void Interact()
  {
    PushDoor();
  }

  void PushDoor()
  {
    Door door = GetComponent<Door>();
    if (door != null)
    {
      Rigidbody doorRb = door.GetComponent<Rigidbody>();
      HingeJoint doorHinge = door.GetComponent<HingeJoint>();
      if (doorRb != null)
      {
        if (doorHinge != null)
        {
          Destroy(doorHinge);
          // TODO: verify it's not null
          door.GetComponent<SuckableBehaviour>().enabled = true;
        }

        Vector3 forceDirection = Camera.main.transform.forward;

        //doorRb.AddForceAtPosition(forceDirection * pushForce, hit.point, ForceMode.Impulse);
        doorRb.AddForceAtPosition(forceDirection * pushForce, this.transform.position, ForceMode.Impulse);

      }
    }
  }
}
