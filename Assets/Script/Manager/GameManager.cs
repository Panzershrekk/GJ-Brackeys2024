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
    public bool IsRoundStarted = false;
    public bool IsRoundFinished = false;
    public bool IsGameOver = false;


    public int quota = 150;
    public FPSController fPSController;
    public BuildingGenerationBis buildingGenerationBis;

    public float score = 0;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        IsGameStarted = true;
    }
    public void StartRound()
    {
        IsRoundStarted = true;
    }

    public void GenerateRound()
    {
        buildingGenerationBis.Generate();
        StartRound();
    }

    public void EndCurrentRound()
    {
        IsRoundFinished = true;
    }

    public void ResetRound()
    {
        IsRoundStarted = false;
        IsRoundFinished = false;
        this.score = 0;
        GameUIManager.Instance.UpdateScoreText(this.score);
    }

    public void FinishGame()
    {
        IsGameOver = true;
    }

    public void CleanAll()
    {
        SuckableBehaviour[] suckableBehaviours = FindObjectsByType<SuckableBehaviour>(FindObjectsSortMode.None);

        foreach (SuckableBehaviour suckableBehaviour in suckableBehaviours)
        {
            Destroy(suckableBehaviour.gameObject);
        }

        RoomConfiguration[] roomConfigs = FindObjectsByType<RoomConfiguration>(FindObjectsSortMode.None);

        foreach (RoomConfiguration roomConfiguration in roomConfigs)
        {
            Destroy(roomConfiguration.gameObject);
        }
        buildingGenerationBis.roomConfigurations.Clear();
    }

    public void AddScore(float score)
    {
        this.score += score;
        GameUIManager.Instance.UpdateScoreText(this.score);
        if (this.score >= quota)
        {
            EndCurrentRound();
        }
    }
}
