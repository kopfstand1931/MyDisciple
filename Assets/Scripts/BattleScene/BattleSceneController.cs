using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSceneController : MonoBehaviour
{
    [SerializeField] private GameObject winMessage;
    [SerializeField] private GameObject loseMessage;

    [SerializeField] private MartialAgentNPC martialAgentNPC;
    [SerializeField] private MartialAgentPlayer martialAgentPlayer;

    private void OnEnable()
    {
        // Register to listen for the event
        MartialAgentNPC.OnWinMessageRaised += HandleWinMessageRaised;
        MartialAgentPlayer.OnLoseMessageRaised += HandleLoseMessageRaised;
    }

    private void OnDisable()
    {
        // Unregister when the script is disabled or destroyed
        MartialAgentNPC.OnWinMessageRaised -= HandleWinMessageRaised;
        MartialAgentPlayer.OnLoseMessageRaised -= HandleLoseMessageRaised;
    }

    // Method to be called when the event is raised
    private void HandleWinMessageRaised()
    {
        Time.timeScale = 1f;
        winMessage.SetActive(true);
        martialAgentPlayer.StopAgent();
        ClearedStageUpdate();
    }


    private void HandleLoseMessageRaised()
    {
        Time.timeScale = 1f;
        loseMessage.SetActive(true);
        martialAgentNPC.StopAgent();
    }

    private void ClearedStageUpdate()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);
        
        if (sceneName.StartsWith("1"))
        {
            DataController.Instance.gameData.stageClear[0] = true;
        }
        else if (sceneName.StartsWith("2"))
        {
            DataController.Instance.gameData.stageClear[1] = true;
        }
        else if (sceneName.StartsWith("3"))
        {
            DataController.Instance.gameData.stageClear[2] = true;
        }
        else if (sceneName.StartsWith("4"))
        {
            DataController.Instance.gameData.stageClear[3] = true;
        }
        else if (sceneName.StartsWith("5"))
        {
            DataController.Instance.gameData.stageClear[4] = true;
        }
        else if (sceneName.StartsWith("6"))
        {
            DataController.Instance.gameData.stageClear[5] = true;
        }
        else if (sceneName.StartsWith("7"))
        {
            DataController.Instance.gameData.stageClear[6] = true;
        }
        else if (sceneName.StartsWith("8"))
        {
            DataController.Instance.gameData.stageClear[7] = true;
        }
        else if (sceneName.StartsWith("9"))
        {
            DataController.Instance.gameData.stageClear[8] = true;
        }
        else if (sceneName.StartsWith("10"))
        {
            DataController.Instance.gameData.stageClear[9] = true;
        }
        else
        {
            Debug.Log("Stage Number Error");
        }

        DataController.Instance.SaveGameData();
    }
}
