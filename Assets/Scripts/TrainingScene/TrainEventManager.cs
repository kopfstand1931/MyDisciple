using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TrainEventManager : MonoBehaviour
{

    [SerializeField] private GameObject m_EventUI;
    [SerializeField] private TMPro.TextMeshProUGUI m_EventTitle;
    [SerializeField] private TMPro.TextMeshProUGUI m_EventText;
    [SerializeField] private GameObject[] m_EventOptions = new GameObject[2];
    [SerializeField] private GameObject m_EventConfirmButton;

    private bool[] nowOnEvent = new bool[1];  // the event which is now on


    // Start is called before the first frame update
    void Start()
    {
        ResetNowOnEvent();
    }

    // Update is called once per frame
    void Update()
    {
        // Event List) //
        // Event One: Check occurence of event one. <Turn == 6>
        if ((DataController.Instance.gameData.turnElapsed == 6) && (DataController.Instance.gameData.eventOccured[0] == false))
        {
            DataController.Instance.gameData.eventOccured[0] = true;
            DataController.Instance.SaveGameData();

            ResetNowOnEvent();
            nowOnEvent[0] = true;
            TrainingEventOne();
        }
    }


    // Get Event button selection.
    public void TrainingEventSelection(int selected)
    {
        // Disable all option buttons.
        for (int i = 0; i < m_EventOptions.Length; i++)
        {
            m_EventOptions[i].gameObject.SetActive(false);
        }

        // Event List) //
        // Event One:
        if (nowOnEvent[0])
        {
            TrainingEventOneEnd(selected);
        }
    }


    // Close Event UI
    public void TrainingEventConfirm()
    { 
        m_EventConfirmButton.gameObject.SetActive(false);
        m_EventUI.SetActive(false);
    }


    void ResetNowOnEvent()
    {
        for (int i = 0; i < nowOnEvent.Length; i++)
        {
            nowOnEvent[i] = false;
        }
    }


    // Event List) //
    // Event One: Wise Old Hermit
    void TrainingEventOne()
    {
        m_EventUI.SetActive(true);
        m_EventTitle.text = "<b><color=red>�̺�Ʈ �߻�!</color><b>";
        m_EventText.text = " ����� ���� ������ ������.\n\"���� 30�Ⱓ �ܾ��� ������ ����. �ڳװ� ���Ѵٸ� �ϳ� �� ���� �־�. �� �ܾ��� ������ ���� ������ �վ� <b><color=red>��</color></b>�� ������ ���� ������, ���ۿ��� ���� ���� �ֳ�. ������ �ٰ�?\"";

        // Activate option buttons.
        for (int i = 0; i < m_EventOptions.Length; i++)
        {
            m_EventOptions[i].gameObject.SetActive(true);
        }

        m_EventOptions[0].GetComponentInChildren<Text>().text = "�ܾ��� �Դ´�.";
        m_EventOptions[1].GetComponentInChildren<Text>().text = "�ܾ��� ���� �ʴ´�.";
    }


    void TrainingEventOneEnd(int selected)
    {
        if (selected == 1)
        {
            int randomNumber = Random.Range(0, 101);

            // Check the probability ranges and return the corresponding value
            if (randomNumber <= 50)
            {
                // 50% chance for Off +3
                int temp_Off = DataController.Instance.gameData.statOFF;
                DataController.Instance.gameData.statOFF += 3;

                string temp_string = string.Format("�ȿ� ���� ���ĳ���!\n���� ��: {0}\n������: <b><color=red>+3</color></b>\n���� ��: {1}", temp_Off, DataController.Instance.gameData.statOFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }
            else
            {
                // 50% chance for Off -3
                int temp_Off = DataController.Instance.gameData.statOFF;
                DataController.Instance.gameData.statOFF = Mathf.Max(1, DataController.Instance.gameData.statOFF - 3);

                string temp_string = string.Format("���� Ÿ������ �� �ϴ�!\n���� ��: {0}\n������: <b><color=blue>-3</color></b>\n���� ��: {1}", temp_Off, DataController.Instance.gameData.statOFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }
            
        }
        else if (selected == 2)
        {
            m_EventText.text = "����� ������ ������ �����.";
        }

        // Save the Result of the Event and for Reset the Event, Activate Confirm Button.
        DataController.Instance.SaveGameData();
        ResetNowOnEvent();
        m_EventConfirmButton.gameObject.SetActive(true);
    }


    // for string effects
    IEnumerator TypeTextEffect(string text, TMPro.TextMeshProUGUI m_mesh)
    {
        m_mesh.text = string.Empty;
        int currentIndex = 0;

        StringBuilder stringBuilder = new StringBuilder();

        while (currentIndex < text.Length)
        {
            char currentChar = text[currentIndex];

            // Check if the current character is the start of a rich text tag.
            if (currentChar == '<')
            {
                // Loop through the characters until the end of the tag is reached.
                StringBuilder tagBuilder = new StringBuilder();
                while (currentChar != '>')
                {
                    tagBuilder.Append(currentChar);
                    currentIndex++;
                    currentChar = text[currentIndex];
                }

                // Append the end of the tag.
                stringBuilder.Append(tagBuilder);
            }
            else
            {
                // Append the regular character.
                stringBuilder.Append(text[currentIndex]);
                currentIndex++;
            }

            // update the mesh text ui.
            m_mesh.text = stringBuilder.ToString();

            yield return new WaitForSeconds(0.02f);
        }
    }
}
