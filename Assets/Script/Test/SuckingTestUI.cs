using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuckingTestUI : MonoBehaviour
{
    public TMP_Text testText;
    public SuckControll suckControll;

    // Update is called once per frame
    void Update()
    {
        testText.text = suckControll.isSucking.ToString();
    }
}
