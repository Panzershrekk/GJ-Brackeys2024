using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckParticleManager : MonoBehaviour {

    public ParticleSystem SuckParticleSystem;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0)) {
            SuckParticleSystem.Emit(1);
        } else {
            SuckParticleSystem.Stop();
        }
    }
}
