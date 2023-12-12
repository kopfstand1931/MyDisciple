using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
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
    [SerializeField] private Image[] stageButtons = new Image[10];
    [SerializeField] private GameObject worldOneStages;
    [SerializeField] private GameObject worldTwoStages;

    // for Stage BackGround
    [SerializeField] private GameObject worldBG;
    [SerializeField] private Sprite[] resourceWorldImages = new Sprite[2];

    // for stage clear reward message
    [SerializeField] private GameObject worldOneClearRewardMessage;
    [SerializeField] private GameObject worldTwoClearRewardMessage;

    // World Switching Buttons
    [SerializeField] private GameObject worldSwitchingButton;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DataController.Instance.LoadGameData();

        // Update UI texts
        b_name.text = "이름: " + DataController.Instance.gameData.name;
        b_statOFF.text = "공: " + DataController.Instance.gameData.statOFF.ToString();
        b_statDFF.text = "방: " + DataController.Instance.gameData.statDFF.ToString();
        b_statSPD.text = "속: " + DataController.Instance.gameData.statSPD.ToString();

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

        b_modelStyle.text = "성격: " + temp_Style;
        b_modelLevel.text = "숙련도: " + temp_Level;

        UpdateStageClearUI();
        CheckStageReward();
        CheckWorldSwitching();

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

        Debug.Log(stageName);
        SceneManager.LoadScene(stageName);
    }


    public void CheckStageReward()
    {
        // Check the stage reward
        // if the stage is cleared and the reward is not given, give the reward.
        // if the stage is not cleared, do nothing.
        // if the stage is cleared and the reward is given, do nothing.
        DataController.Instance.LoadGameData();

        // for world 1: from stage 1 to 5 is cleared and not rewarded
        if (DataController.Instance.gameData.stageClear.Take(5).All(x => x) && !DataController.Instance.gameData.isRewarded[0])
        {
            DataController.Instance.gameData.isRewarded[0] = true;
            DataController.Instance.gameData.turnLimit += 6;

            worldOneClearRewardMessage.SetActive(true);
        }

        DataController.Instance.SaveGameData();
    }


    public void OnClearMessageConfirm()
    {
        worldOneClearRewardMessage.SetActive(false);
        worldTwoClearRewardMessage.SetActive(false);

        DataController.Instance.SaveGameData();
        SceneManager.LoadScene("Training");
    }


    public void CheckWorldSwitching()
    {
        if (DataController.Instance.gameData.isRewarded[0])
        {
            worldSwitchingButton.SetActive(true);
        }
        else
        {
            worldSwitchingButton.SetActive(false);
        }
    }


    public void OnClickWorldSwitch(int i)
    {
        if (i == 1)
        {
            worldOneStages.SetActive(true);
            worldTwoStages.SetActive(false);
            worldBG.GetComponent<SpriteRenderer>().sprite = resourceWorldImages[0];
        }
        else if (i == 2)
        {
            worldOneStages.SetActive(false);
            worldTwoStages.SetActive(true);
            worldBG.GetComponent<SpriteRenderer>().sprite = resourceWorldImages[1];
        }
    }
}
