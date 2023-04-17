using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string SceneName;
    public void LoadingNewScene()
    {
        SceneManager.LoadScene(SceneName);
    }

}
