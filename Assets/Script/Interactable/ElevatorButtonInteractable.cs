using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButtonInteractable : Interactable
{
    public ElevatorSequence elevatorSequence;

    private bool _sequenceOnGoing = false;
    public override void Interact()
    {
        if (GameManager.Instance.IsGameStarted == true && GameManager.Instance.IsRoundFinished == true)
        {
            GameManager.Instance.ResetRound();
        }
        if (GameManager.Instance.IsGameStarted == true && _sequenceOnGoing == false && GameManager.Instance.IsRoundStarted == false)
        {
            GameManager.Instance.fPSController.canMove = false;
            _sequenceOnGoing = true;
            elevatorSequence.afterOneSecond += GameManager.Instance.CleanAll;
            elevatorSequence.afterTwoSecond += GameManager.Instance.GenerateRound;
            elevatorSequence.onComplete = CompleteSequence;
            elevatorSequence.StartElevatorSequence();
        }
    }

    private void CompleteSequence()
    {
        GameManager.Instance.fPSController.canMove = true;
        _sequenceOnGoing = false;
    }
}
