using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI t_name;
    [SerializeField] private TMPro.TextMeshProUGUI t_statOFF;
    [SerializeField] private TMPro.TextMeshProUGUI t_statDFF;
    [SerializeField] private TMPro.TextMeshProUGUI t_statSPD;
    [SerializeField] private TMPro.TextMeshProUGUI t_modelStyle;
    [SerializeField] private TMPro.TextMeshProUGUI t_modelLevel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DataController.Instance.LoadGameData();
        t_name.text = "이름: " + DataController.Instance.gameData.name;
        t_statOFF.text = "공: " + DataController.Instance.gameData.statOFF.ToString();
        t_statDFF.text = "방: " + DataController.Instance.gameData.statDFF.ToString();
        t_statSPD.text = "속: " + DataController.Instance.gameData.statSPD.ToString();
        // 나중에 enum 사용해 상중하로 바꾸자.
        t_modelStyle.text = "성격: " + DataController.Instance.gameData.currentModelStyle.ToString();
        t_modelLevel.text = "숙련도: " + DataController.Instance.gameData.currentModelLevel.ToString();
    }
}
