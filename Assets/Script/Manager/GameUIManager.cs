using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUIManager : MonoBehaviour
{
    #region Singleton pattern

    /*
    ** Singleton pattern
    */

    private static GameUIManager _instance;
    public static GameUIManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public TMP_Text scoreText;
    public Component healthBar;

    public void UpdateScoreText(float score, float trashScore) {
        this.scoreText.text = score.ToString();
        GaugeFill gaugeFill = healthBar.GetComponent<GaugeFill>();

        if (gaugeFill != null) {
            gaugeFill.Add((int)trashScore);
        }
    }
}
