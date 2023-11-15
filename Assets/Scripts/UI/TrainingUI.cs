using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI t_name;
    [SerializeField] private TMPro.TextMeshProUGUI t_statOFF;
    [SerializeField] private TMPro.TextMeshProUGUI t_statDFF;
    [SerializeField] private TMPro.TextMeshProUGUI t_statSPD;
    [SerializeField] private TMPro.TextMeshProUGUI t_modelStyle;
    [SerializeField] private TMPro.TextMeshProUGUI t_modelLevel;
    [SerializeField] private TMPro.TextMeshProUGUI t_turnElapsed;

    [SerializeField] private GameObject trainingEndMessage;

    [SerializeField] private GameObject trainingOff;
    [SerializeField] private GameObject trainingDff;
    [SerializeField] private GameObject trainingSpd;
    [SerializeField] private GameObject trainingMLevel;

    [SerializeField] private GameObject trainingMLevelButton;

    [SerializeField] private TrainingManager m_TrainingManager;

    [SerializeField] private GameObject[] backGrounds = new GameObject[4];

    // Start is called before the first frame update
    void Start()
    {
        DataController.Instance.LoadGameData();
    }

    // Update is called once per frame
    void Update()
    {
        // if turn is 12, show training end message.
        if (DataController.Instance.gameData.turnElapsed >= 12)
        {
            trainingEndMessage.SetActive(true);
        }

        // update background.
        UpdateBackground();

        // check if the player has max level model.
        if (DataController.Instance.gameData.currentModelLevel == 3)
        {
            trainingMLevelButton.SetActive(false);
        }
        else
        {
            trainingMLevelButton.SetActive(true);
        }

        t_turnElapsed.text = DataController.Instance.gameData.turnElapsed.ToString() + "/12";
        t_name.text = "이름: " + DataController.Instance.gameData.name;
        t_statOFF.text = "공: " + DataController.Instance.gameData.statOFF.ToString();
        t_statDFF.text = "방: " + DataController.Instance.gameData.statDFF.ToString();
        t_statSPD.text = "속: " + DataController.Instance.gameData.statSPD.ToString();

        // 모델 스타일이나 난이도는 별도로 구성
        string temp_Style = "";
        string temp_Level = "";
        switch (DataController.Instance.gameData.currentModelStyle)
        {
            case 0:
                temp_Style = "근접";
                break;
            case 1:
                temp_Style = "원거리";
                break;
            case 2:
                temp_Style = "균형";
                break;
        }
        switch (DataController.Instance.gameData.currentModelLevel)
        {
            case 1:
                temp_Level = "하급";
                break;
            case 2:
                temp_Level = "중급";
                break;
            case 3:
                temp_Level = "상급";
                break;
        }

        t_modelStyle.text = "성격: " + temp_Style;
        t_modelLevel.text = "숙련도: " + temp_Level;


        // if esc pressed, go to main
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void OnClickSaveButton()
    {
        DataController.Instance.SaveGameData();
    }


    // Training Turn over
    public void OnClickTrainingEnd()
    {
        DataController.Instance.SaveGameData();
        SceneManager.LoadScene("Title");
    }


    // Offense Training
    public void OnClickTrainingOff()
    {
        trainingOff.SetActive(true);
        m_TrainingManager.TrainingOff();
    }

    public void OnClickTrainingOffConfirm()
    {
        trainingOff.SetActive(false);
    }


    // Deffense Training
    public void OnClickTrainingDff()
    {
        trainingDff.SetActive(true);
        m_TrainingManager.TrainingDff();
    }

    public void OnClickTrainingDffConfirm()
    {
        trainingDff.SetActive(false);
    }


    // Speed Training
    public void OnClickTrainingSpd()
    {
        trainingSpd.SetActive(true);
        m_TrainingManager.TrainingSpd();
    }

    public void OnClickTrainingSpdConfirm()
    {
        trainingSpd.SetActive(false);
    }


    // Model Level Training
    public void OnClickTrainingMLevel()
    {
        trainingMLevel.SetActive(true);
        m_TrainingManager.TrainingMLevel();
    }

    public void OnClickTrainingMLevelConfirm()
    {
        trainingMLevel.SetActive(false);
    }

    public void UpdateBackground()
    {
        int nowTurn = DataController.Instance.gameData.turnElapsed;
        if (nowTurn % 4 == 0)
        {
            backGrounds[0].SetActive(true);
            backGrounds[1].SetActive(false);
            backGrounds[2].SetActive(false);
            backGrounds[3].SetActive(false);
        }
        else if (nowTurn % 4 == 1)
        {
            backGrounds[0].SetActive(false);
            backGrounds[1].SetActive(true);
            backGrounds[2].SetActive(false);
            backGrounds[3].SetActive(false);
        }
        else if (nowTurn % 4 == 2)
        {
            backGrounds[0].SetActive(false);
            backGrounds[1].SetActive(false);
            backGrounds[2].SetActive(true);
            backGrounds[3].SetActive(false);
        }
        else if (nowTurn % 4 == 3)
        {
            backGrounds[0].SetActive(false);
            backGrounds[1].SetActive(false);
            backGrounds[2].SetActive(false);
            backGrounds[3].SetActive(true);
        }
        else
        {
            Debug.Log("Error: UpdateBackground()");
        }
    }
}
