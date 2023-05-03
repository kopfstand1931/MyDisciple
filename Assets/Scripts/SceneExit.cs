using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.MLAgents;

public class SceneExit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the current scene's name is "Title"
            if (SceneManager.GetActiveScene().name == "Title")
            {
                // Quit the application
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
            }
            Academy.Instance.Dispose();
            SceneManager.LoadScene("Title");
        }
    }
}
