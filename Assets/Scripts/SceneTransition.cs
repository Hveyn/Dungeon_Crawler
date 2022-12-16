using System;
using UnityEngine;
using UnityEngine.SceneManagement ;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Text LoadingPercentage;
    public Image LoadingProgressBar;
    
    private static SceneTransition _instance;
    private static bool _shouldPlayOpeningAnimation = false;
    
    private Animator _componentAnimator;
    private AsyncOperation _loadSceneOperation;
    private int _loadPercent;

    private float _ratioLoadSceneToGenScripts = 0.8f;
    public static void SwitchToScene(int indexScene)
    {
        _instance._componentAnimator.SetTrigger("SceneClosing");

        _instance._ratioLoadSceneToGenScripts = indexScene == 0 ? 1 : 0.8f;

        _instance._loadSceneOperation = SceneManager.LoadSceneAsync(indexScene);
        _instance._loadSceneOperation.allowSceneActivation = false;
        
        _instance.LoadingProgressBar.fillAmount = 0;
    }

    private void Start()
    {
        _instance = this;
        _componentAnimator = GetComponent<Animator>();

        if (_shouldPlayOpeningAnimation)
        {
            _componentAnimator.SetTrigger("SceneOpening");
            _shouldPlayOpeningAnimation = false;
            LoadingProgressBar.fillAmount = 1;
        }
    } 

    private void Update()
    {
        if (_loadSceneOperation != null)
        {
            _loadPercent = Mathf.RoundToInt(_loadSceneOperation.progress * _ratioLoadSceneToGenScripts*100);
            LoadingPercentage.text = _loadPercent + "%";
            LoadingProgressBar.fillAmount = 
                Mathf.Lerp(LoadingProgressBar.fillAmount, 
                            _loadSceneOperation.progress*_ratioLoadSceneToGenScripts,
                                Time.deltaTime * 5);
        }
    }

    public void OnAnimationOver()
    {
        _loadSceneOperation.allowSceneActivation = true;
    }

    public static void AddLoadingProgress()
    {

        _instance.LoadingPercentage.text = Mathf.RoundToInt(_instance._loadPercent+5) + "%";
        _instance.LoadingProgressBar.fillAmount = 
            Mathf.Lerp(_instance.LoadingProgressBar.fillAmount,
                        _instance.LoadingProgressBar.fillAmount+0.05f,
                            Time.deltaTime * 5);
    }
    
    public static void SceneLoaded()
    {
        _shouldPlayOpeningAnimation = true;
    }
}
