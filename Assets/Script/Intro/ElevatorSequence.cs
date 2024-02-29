using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSequence : MonoBehaviour
{
    public float timeInElevator = 3f;
    public Animator elevatorAnimator;
    public Action afterOneSecond;
    public Action afterTwoSecond;
    public Action onComplete;

    public void StartElevatorSequence()
    {
        StartCoroutine(ElevatorSequenceCoroutine());
    }

    private IEnumerator ElevatorSequenceCoroutine()
    {
        elevatorAnimator.Play("DoorClosing");
        yield return new WaitForSeconds(1);
        afterOneSecond?.Invoke();
        yield return new WaitForSeconds(1);
        afterTwoSecond?.Invoke();
        yield return new WaitForSeconds(timeInElevator);
        elevatorAnimator.Play("DoorOpening");
        onComplete?.Invoke();
        onComplete = null;
        afterOneSecond = null;
        afterTwoSecond = null;
    }
}
