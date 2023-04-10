using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    public string sceneToLoad;
    public void LoadNextScene()
    {
        sceneToLoad = PlayerPrefs.GetString("Next Scene", sceneToLoad);
        LoadScene(sceneToLoad);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
