using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    #region Singleton pattern

    /*
    ** Singleton pattern
    */

    private static SceneLoader _instance;

    public static SceneLoader Instance { get { return _instance; } }

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

    public AnimationClip FadeInClip;
    public AnimationClip FadeOutClip;
    public Animator Transition;
    public bool PlayFadeOnStart = false;

    public void Start()
    {
        if (PlayFadeOnStart == true)
        {
            PlayFadeInOnStartFunction();
        }
    }

    public void OnStartClick(int index)
    {
        StartCoroutine(LoadLevel(index));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Transition.Play(FadeOutClip.name);
        //AudioManager.Instance.Play("Click");
        yield return new WaitForSeconds(FadeInClip.length);
        SceneManager.LoadScene(levelIndex);
    }

    public void PlayFadeInOnStartFunction()
    {
        Transition.Play(FadeInClip.name);
    }

}