using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SuckableBehaviour : MonoBehaviour
{
    public float scoreValue = 10;
    public float resistanceTime;
    public float resistanceTimeVarianceInSeconds;
    public float proximityThresold = 1.5f;
    public float proximityFactor = 0.25f;
    public Rigidbody rb;
    public Collider col;
    public float endSequence = 0.2f;

    private float _trueResistanceTime;
    private SuckZone _suckZone;
    //If enough time has passed, it is validated
    private bool _isCompletelySucked = false;
    private float _currentResitanceTime;
    private Vector3 _baseScale;
    private Tweener _shakeTween = null;
    private bool _isInTriggerZone;

    public void Start()
    {
        _trueResistanceTime = resistanceTime + Random.Range(-resistanceTimeVarianceInSeconds, resistanceTimeVarianceInSeconds);
        _baseScale = transform.localScale;
    }

    private void Update()
    {
        if (_isCompletelySucked == false)
        {
            bool isSucked = false;
            if (_isInTriggerZone && _suckZone != null)
            {
                isSucked = _suckZone.suckControll.isSucking;
            }

            if (isSucked)
            {
                bool lineOfSigh = false;
                Ray ray = new Ray(_suckZone.suckOrigin.position, this.transform.position - _suckZone.suckOrigin.position);

                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.transform.GetComponent<SuckableBehaviour>() == null)
                    {
                        lineOfSigh = true;
                    }
                }
                if (lineOfSigh == false)
                {
                    if (_shakeTween == null)
                    {
                        _shakeTween = transform.DOShakeScale(0.1f, 0.05f, 1, 10, true, ShakeRandomnessMode.Harmonic).OnComplete(() =>
                        {
                            transform.localScale = _baseScale;
                            _shakeTween = null;
                        });
                    }
                    float distance = Mathf.Abs(Vector3.Distance(transform.position, _suckZone.suckOrigin.transform.position));
                    float proximityMultiplier = 1;
                    if (distance > proximityThresold)
                    {
                        proximityMultiplier /= distance * proximityFactor;
                    }
                    _currentResitanceTime += Time.deltaTime * proximityMultiplier;
                    if (_currentResitanceTime > _trueResistanceTime)
                    {
                        _isCompletelySucked = true;
                        MoveTowardDestination();
                    }
                }
            }
            else
            {
                _currentResitanceTime = 0;
            }
        }
    }

    private void MoveTowardDestination()
    {
        //rb.isKinematic = true;
        col.enabled = false;
        transform.DOScale(0.1f, endSequence);
        transform.DOShakeRotation(endSequence, 90, 10, 90, false, ShakeRandomnessMode.Harmonic);
        transform.DOMove(_suckZone.suckOrigin.position, endSequence, false).onComplete = OnComplete;
    }

    public void SetInTriggerZone(bool inZone)
    {
        _isInTriggerZone = inZone;
    }

    public void SetSuckZone(SuckZone suckZone)
    {
        _suckZone = suckZone;
    }

    private void OnComplete()
    {
        GameManager.Instance.AddScore(this.scoreValue);
        _suckZone.RemoveColliderFromList(col);
        Destroy(this.gameObject);
    }
}
