using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
  public List<GameObject> trashes;
  public Boolean instantiateTrashes;
  public int numberOfElementToSpawn = 20;
  public float explosionForce = 50f;
  public float explosionRadius = 5f;

  void Start()
  {
    Debug.Log("TRASH GENERATOR START");
    if (instantiateTrashes == true)
    {
      InstantiatingTrashes();
    }
  }

  private void InstantiatingTrashes()
  {
    // TODO: add a verification to be sure it's the correct gameobject (should be a spwner)
    GameObject spawner = gameObject;
    BoxCollider spawnerBoxCollider = spawner.GetComponent<BoxCollider>();
    Bounds bounds = spawnerBoxCollider.bounds;

    for (int i = 0; i < numberOfElementToSpawn; i++)
    {
      InstantiatingTrash(bounds, trashes[UnityEngine.Random.Range(0, trashes.Count)]);
    }
    //Explode();
  }

  private void InstantiatingTrash(Bounds bounds, GameObject trash)
  {
    // Generate a random position within the bounds of spawner
    float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
    float y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
    float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z);
    Vector3 randomPosition = new Vector3(x, y, z);

    Instantiate(trash, randomPosition, Quaternion.identity);
  }

  private void Explode()
  {
    Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

    foreach (Collider hit in colliders)
    {
      Rigidbody rb = hit.GetComponent<Rigidbody>();

      if (rb != null)
      {
        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
      }
    }
  }
}
