using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckControll : MonoBehaviour
{
    public bool isSucking = false;
    public SuckZone suckZone;
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isSucking = true;
        }
        else
        {
            isSucking = false;
        }
    }
}
