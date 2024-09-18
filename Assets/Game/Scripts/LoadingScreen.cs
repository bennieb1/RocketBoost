using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressbar;
    

    public static LoadingScreen Instance;

     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Prevent canvas from being destroyed across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy any duplicate instances
        }


    }

    public async void LoadScene(string sceneName)
    {

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        // Ensure loaderCanvas stays active while loading
        if (_loaderCanvas != null)
        {
            _loaderCanvas.SetActive(true); // Activate loading canvas
        }

        // Wait for scene to load up to 90%
        do
        {
            await Task.Delay(1000); // Reduce delay for faster updates
            _progressbar.fillAmount = scene.progress / 0.9f; // Normalize fill amount to 1.0
        }
        while (scene.progress < 0.9f);

        // Once the scene is ready, activate it
        scene.allowSceneActivation = true;

        // Deactivate the loaderCanvas after the scene is loaded
        if (_loaderCanvas != null)
        {
            _loaderCanvas.SetActive(false); // Simply deactivate it, not destroy
        }
    }

}
