using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    // [SerializeField] private GameObject[] stageButtons;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DataController.Instance.LoadGameData();

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

        // if key enter pressed, load title scene
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Title");
    }

    public void OnClickStageButton(int stageLevel)
    {
        /*
        DataController.Instance.gameData.currentStage = stage;
        DataController.Instance.SaveGameData();
        */

        if (stageLevel == 0)
            SceneManager.LoadScene("Duel2MartialSvS");
    }
}
