using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeFill : MonoBehaviour
{

    public int maxValue;
    public Image fill;
    private int currentValue;

    // Start is called before the first frame update
    void Start()
    {
        currentValue = 0;
        fill.fillAmount = 0;
    }

    public void Add(int incr) {
        currentValue += incr;

        if (currentValue == maxValue) {
            currentValue = maxValue;
        }

        fill.fillAmount = (float)currentValue / maxValue;
    }
}
