using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckZone : MonoBehaviour
{
    public Transform suckOrigin;
    public SuckControll suckControll;

    private List<Collider> _colliders = new List<Collider>();
    public List<Collider> GetColliders() { return _colliders; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SuckableBehaviour>() != null && !_colliders.Contains(other))
        {
            _colliders.Add(other);
            SuckableBehaviour suckableBehaviour = other.GetComponent<SuckableBehaviour>();
            suckableBehaviour.SetInTriggerZone(true);
            suckableBehaviour.SetSuckZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SuckableBehaviour>() != null && _colliders.Contains(other))
        {
            SuckableBehaviour suckableBehaviour = other.GetComponent<SuckableBehaviour>();
            suckableBehaviour.SetInTriggerZone(false);
            _colliders.Remove(other);
        }
    }

    public void RemoveColliderFromList(Collider collider)
    {
        _colliders.Remove(collider);
    }
}
