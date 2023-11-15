using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NurturingUI : MonoBehaviour
{

    [SerializeField] private GameObject nameUI;
    [SerializeField] private TMP_InputField inputName;

    [SerializeField] private GameObject statUI;

    [SerializeField] private GameObject modelUI;

    [SerializeField] private GameObject completionUI;

    // Start is called before the first frame update
    void Start()
    {
        DataController.Instance.LoadGameData();
        inputName.text = DataController.Instance.gameData.name;

        // Initialize Stats
        DataController.Instance.gameData.statOFF = 1;
        DataController.Instance.gameData.statDFF = 1;
        DataController.Instance.gameData.statSPD = 1;
        DataController.Instance.gameData.currentModelStyle = 0;
        DataController.Instance.gameData.currentModelLevel = 1;
        DataController.Instance.SaveGameData();
    }

    public void NameUpdate()
    {
        string name = inputName.text;
        DataController.Instance.gameData.name = name;
        DataController.Instance.SaveGameData();

        nameUI.SetActive(false);
        statUI.SetActive(true);
    }

    public void StatUpdate(int selected)
    {
        switch (selected)
        {
            case 0:
                DataController.Instance.gameData.statOFF += 1;
                break;
            case 1:
                DataController.Instance.gameData.statDFF += 1;
                break;
            case 2:
                DataController.Instance.gameData.statSPD += 1;
                break;
        }

        DataController.Instance.SaveGameData();

        statUI.SetActive(false);
        modelUI.SetActive(true);
    }

    public void ModelUpdate(int selected)
    {
        switch (selected)
        {
            case 0:
                DataController.Instance.gameData.currentModelStyle = 0;
                break;
            case 1:
                DataController.Instance.gameData.currentModelStyle = 1;
                break;
            case 2:
                DataController.Instance.gameData.currentModelStyle = 2;
                break;
        }

        DataController.Instance.gameData.currentModelLevel = 1;
        DataController.Instance.SaveGameData();

        modelUI.SetActive(false);
        completionUI.SetActive(true);
    }

    public void Completion()
    { 
        SceneManager.LoadScene("Training");
    }
}
