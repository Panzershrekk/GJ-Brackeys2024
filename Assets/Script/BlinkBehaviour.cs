using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BlinkBehaviour : MonoBehaviour
{
    public Light spotlight;

    public int randomMax = 1000;

    private int[] randomRange =  Enumerable.Range(0, 100).ToArray();

    void FixedUpdate() {

        int randomNumber = Random.Range(0, randomMax);

        // if (DOTween.IsTweening(spotlight)) {
        //     spotlight.transform.parent.gameObject.GetComponent<Renderer>().materials[0].DisableKeyword("_EMISSION");
        // } else {
        //      spotlight.transform.parent.gameObject.GetComponent<Renderer>().materials[0].EnableKeyword("_EMISSION");
        // }
        if (!DOTween.IsTweening(spotlight) && randomRange.Contains(randomNumber)) {
            
            spotlight.transform.parent.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.black, "_EmissionColor", 1).SetEase(Ease.InBounce, 100, 1);
            spotlight.DOIntensity(0, 1).SetEase(Ease.Flash, 100, 3).OnComplete(() => spotlight.transform.parent.gameObject.GetComponent<MeshRenderer>().material.DOColor(Color.white, "_EmissionColor", 0));
            ;
        }
    }
}
