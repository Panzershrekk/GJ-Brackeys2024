using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SuckControll : MonoBehaviour
{
    public bool isSucking = false;
    public SuckZone suckZone;
    public Transform sucker;
    private Tweener _shakeTween = null;
    private Vector3 _suckerBaseScale;

    public void Start()
    {
        _suckerBaseScale = sucker.localScale;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isSucking = true;
            if (_shakeTween == null)
            {
                _shakeTween = sucker.DOShakeScale(0.1f, 0.05f, 1, 10, true, ShakeRandomnessMode.Harmonic).OnComplete(() =>
                {
                    sucker.localScale = _suckerBaseScale;
                    _shakeTween = null;
                });
            }
        }
        else
        {
            isSucking = false;
        }
    }
}
