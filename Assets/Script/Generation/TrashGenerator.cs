using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashGenerator : MonoBehaviour
{
  public List<GameObject> trashes;
  public Boolean instantiateTrashes;

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

    foreach (GameObject trash in trashes)
    {
      InstantiatingTrash(bounds, trash);
    }
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
}
