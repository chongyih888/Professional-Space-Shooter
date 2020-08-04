using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOVer;

    private void Update()
    {
        //if the r key was preseed
        //restart the current scene
        if(Input.GetKeyDown(KeyCode.R) && _isGameOVer == true)
        {
            SceneManager.LoadScene(1); // Current game scene
        }

        //if the escape key is pressed
        //quit application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOVer = true;
    }
}
