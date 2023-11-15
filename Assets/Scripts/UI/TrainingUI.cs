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
        t_name.text = "�̸�: " + DataController.Instance.gameData.name;
        t_statOFF.text = "��: " + DataController.Instance.gameData.statOFF.ToString();
        t_statDFF.text = "��: " + DataController.Instance.gameData.statDFF.ToString();
        t_statSPD.text = "��: " + DataController.Instance.gameData.statSPD.ToString();
        // ���߿� enum ����� �����Ϸ� �ٲ���.
        t_modelStyle.text = "����: " + DataController.Instance.gameData.currentModelStyle.ToString();
        t_modelLevel.text = "���õ�: " + DataController.Instance.gameData.currentModelLevel.ToString();
    }
}
