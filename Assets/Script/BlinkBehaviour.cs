using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BlinkBehaviour : MonoBehaviour
{
    public Light spotlight;
    // Start is called before the first frame update
    void Start()
    {
        spotlight.DOIntensity(20, 1).SetLoops(-1).SetEase(Ease.Flash, 20, 10);
    }
}
