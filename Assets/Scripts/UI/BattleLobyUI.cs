using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleLobyUI : MonoBehaviour
{
    // for UI text Update
    [SerializeField] private TMPro.TextMeshProUGUI b_name;
    [SerializeField] private TMPro.TextMeshProUGUI b_statOFF;
    [SerializeField] private TMPro.TextMeshProUGUI b_statDFF;
    [SerializeField] private TMPro.TextMeshProUGUI b_statSPD;
    [SerializeField] private TMPro.TextMeshProUGUI b_modelStyle;
    [SerializeField] private TMPro.TextMeshProUGUI b_modelLevel;

    // for Stage Selection
    [SerializeField] private Image[] stageButtons = new Image[3];


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DataController.Instance.LoadGameData();

        // Update UI texts
        b_name.text = "�̸�: " + DataController.Instance.gameData.name;
        b_statOFF.text = "��: " + DataController.Instance.gameData.statOFF.ToString();
        b_statDFF.text = "��: " + DataController.Instance.gameData.statDFF.ToString();
        b_statSPD.text = "��: " + DataController.Instance.gameData.statSPD.ToString();

        // �� ��Ÿ���̳� ���̵��� ������ ����
        string temp_Style = "";
        string temp_Level = "";
        switch (DataController.Instance.gameData.currentModelStyle)
        {
            case 0:
                temp_Style = "����";
                break;
            case 1:
                temp_Style = "���Ÿ�";
                break;
            case 2:
                temp_Style = "����";
                break;
        }
        switch (DataController.Instance.gameData.currentModelLevel)
        {
            case 1:
                temp_Level = "�ϱ�";
                break;
            case 2:
                temp_Level = "�߱�";
                break;
            case 3:
                temp_Level = "���";
                break;
        }

        b_modelStyle.text = "����: " + temp_Style;
        b_modelLevel.text = "���õ�: " + temp_Level;

        UpdateStageClearUI();

        // if key enter pressed, load title scene
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Title");
    }


    public void UpdateStageClearUI()
    {
        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (DataController.Instance.gameData.stageClear[i])
                stageButtons[i].color = new Color(0.8f, 0.145f, 0.173f, 1f);
            else
                stageButtons[i].color = Color.white;
        }
    }

    public void OnClickStageButton(int stageLevel)
    {
        /*
        DataController.Instance.gameData.currentStage = stage;
        DataController.Instance.SaveGameData();
        */

        // Check the starting stage level,
        // and check the model's style and level.
        // Then, load the stage scene.
        // for example, "1Stage" + "M" + "1".

        string stageName = "";

        stageName += stageLevel.ToString() + "Stage";

        if (DataController.Instance.gameData.currentModelStyle == 0)
            stageName += "M";
        else if (DataController.Instance.gameData.currentModelStyle == 1)
            stageName += "R";
        else if (DataController.Instance.gameData.currentModelStyle == 2)
            stageName += "B";

        if (DataController.Instance.gameData.currentModelLevel == 1)
            stageName += "1";
        else if (DataController.Instance.gameData.currentModelLevel == 2)
            stageName += "2";
        else if (DataController.Instance.gameData.currentModelLevel == 3)
            stageName += "3";

        SceneManager.LoadScene(stageName);
    }
}
