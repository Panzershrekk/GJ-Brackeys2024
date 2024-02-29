using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour {
    public TMP_Text actionText;

    public virtual void Start()
    {
        if (actionText != null)
        {
            actionText.gameObject.SetActive(false);
        }
    }

    public virtual void Interact()
    {
        Debug.LogWarning("Calling parent function");
    }
 }
