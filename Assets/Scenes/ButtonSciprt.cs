using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSciprt : MonoBehaviour
{
    public void Exit(){
        Application.Quit();
    }

    public void Play(){
        SceneManager.LoadSceneAsync(1);
    }
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}


