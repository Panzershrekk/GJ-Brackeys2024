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
    public TMP_Text doorKickText;

    public void Start()
    {
        doorKickText.gameObject.SetActive(false);
    }

    public void UpdateScoreText(float score) {
        this.scoreText.text = score.ToString();
        GaugeFill gaugeFill = healthBar.GetComponent<GaugeFill>();

        if (gaugeFill != null) {
            gaugeFill.SetValue((int)score);
        }
    }
}
