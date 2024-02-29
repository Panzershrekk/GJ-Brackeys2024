using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeFill : MonoBehaviour
{
    public Image fill;

    // Start is called before the first frame update
    void Start()
    {
        fill.fillAmount = 0;
    }

    public void SetValue(int value) {
        int currentValue = value;

        if (currentValue == GameManager.Instance.quota) {
            currentValue = GameManager.Instance.quota;
        }

        fill.fillAmount = (float)currentValue / GameManager.Instance.quota;
    }
}
