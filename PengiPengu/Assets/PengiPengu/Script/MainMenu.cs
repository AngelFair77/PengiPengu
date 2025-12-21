using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Basla()
    {
        SceneManager.LoadScene("IcePlace");
    }

    public void Help()
    {
        SceneManager.LoadScene("Help");

    }

    public void Exit()
    {
        Application.Quit();
    }
    
    
}
