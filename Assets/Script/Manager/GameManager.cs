using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// THIS CLASS US SINGLETON PATTERN
//

public class GameManager : MonoBehaviour
{
    #region Singleton pattern

    /*
    ** Singleton pattern
    */

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


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

    public bool IsGameStarted = false;
    public bool IsGameOver = false;

    public float score = 0;
   
    public void StartGame()
    {
        IsGameStarted = true;
    }

    public void FinishGame()
    {
        IsGameOver = true;
    }

    public void AddScore(float score) {
        this.score += score;
        GameUIManager.Instance.UpdateScoreText(this.score);
    }
}
