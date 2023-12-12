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
    [SerializeField] private Image m_EventImage;
    [SerializeField] private GameObject[] m_EventOptions = new GameObject[2];
    [SerializeField] private GameObject m_EventConfirmButton;

    private bool[] nowOnEvent = new bool[2];  // the event which is now on


    // for Sound Effects
    [SerializeField] private SoundEffectPlayer m_soundEffectPlayer;

    // resource for event images
    [SerializeField] private Sprite[] m_eventImages = new Sprite[2];


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
        // Event Two: Check occurence of event two. <Turn == 14>
        else if ((DataController.Instance.gameData.turnElapsed == 14) && (DataController.Instance.gameData.eventOccured[1] == false))
        {
            DataController.Instance.gameData.eventOccured[1] = true;
            DataController.Instance.SaveGameData();

            ResetNowOnEvent();
            nowOnEvent[1] = true;
            TrainingEventTwo();
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
        // Event Two:
        else if (nowOnEvent[1])
        {
            TrainingEventTwoEnd(selected);
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
        m_EventImage.sprite = m_eventImages[0];
        m_EventUI.SetActive(true);
        m_EventTitle.text = "<b><color=red>이벤트 발생!</color><b>";
        m_EventText.text = " 당신은 늙은 도인을 만났다.\n\"나는 30년간 단약을 조제해 왔지. 자네가 원한다면 하나 줄 수도 있어. 이 단약을 먹으면 팔의 혈맥을 뚫어 <b><color=red>힘</color></b>이 증가할 수도 있지만, 부작용이 있을 수도 있네. 어찌할 텐가?\"";

        // Activate option buttons.
        for (int i = 0; i < m_EventOptions.Length; i++)
        {
            m_EventOptions[i].gameObject.SetActive(true);
        }

        m_EventOptions[0].GetComponentInChildren<Text>().text = "단약을 먹는다.";
        m_EventOptions[1].GetComponentInChildren<Text>().text = "단약을 먹지 않는다.";
    }


    void TrainingEventOneEnd(int selected)
    {

        m_soundEffectPlayer.PlaySfx3();

        if (selected == 1)
        {
            int randomNumber = Random.Range(0, 101);

            // Check the probability ranges and return the corresponding value
            if (randomNumber <= 50)
            {
                // 50% chance for Off +3
                int temp_Off = DataController.Instance.gameData.statOFF;
                DataController.Instance.gameData.statOFF += 3;

                string temp_string = string.Format("팔에 힘이 넘쳐난다!\n기존 공: {0}\n변동량: <b><color=red>+3</color></b>\n현재 공: {1}", temp_Off, DataController.Instance.gameData.statOFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }
            else
            {
                // 50% chance for Off -3
                int temp_Off = DataController.Instance.gameData.statOFF;
                DataController.Instance.gameData.statOFF = Mathf.Max(1, DataController.Instance.gameData.statOFF - 3);

                string temp_string = string.Format("팔이 타오르는 듯 하다!\n기존 공: {0}\n변동량: <b><color=blue>-3</color></b>\n현재 공: {1}", temp_Off, DataController.Instance.gameData.statOFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }
            
        }
        else if (selected == 2)
        {
            m_EventText.text = "당신은 동굴을 빠르게 벗어났다.";
        }

        // Save the Result of the Event and for Reset the Event, Activate Confirm Button.
        DataController.Instance.SaveGameData();
        ResetNowOnEvent();
        m_EventConfirmButton.gameObject.SetActive(true);
    }


    // Event Two: 
    void TrainingEventTwo()
    {
        m_EventImage.sprite = m_eventImages[1];
        m_EventUI.SetActive(true);
        m_EventTitle.text = "<b><color=red>이벤트 발생!</color><b>";
        m_EventText.text = " 달빛에 홀린 듯 정처없이 걷던 당신은, 탁 트인 동굴 앞에서 한 두루마리를 발견했다.\n 얼핏 훑어 보기에 <b><color=red>호신술</color></b>에 관련된 <b><color=yellow>비급</color></b>으로 보이지만, 이 비급에 담긴 경지가 자신에게 도움이 될지 확신할 순 없다.";

        // Activate option buttons.
        for (int i = 0; i < m_EventOptions.Length; i++)
        {
            m_EventOptions[i].gameObject.SetActive(true);
        }

        m_EventOptions[0].GetComponentInChildren<Text>().text = "비급을 익혀 본다.";
        m_EventOptions[1].GetComponentInChildren<Text>().text = "무시한다.";
    }


    void TrainingEventTwoEnd(int selected)
    {

        m_soundEffectPlayer.PlaySfx3();

        if (selected == 1)
        {
            int randomNumber = Random.Range(0, 101);

            // Check the probability ranges and return the corresponding value
            if (randomNumber <= 50)
            {
                // 50% chance for Dff +5
                int temp_Dff = DataController.Instance.gameData.statDFF;
                DataController.Instance.gameData.statDFF += 5;

                string temp_string = string.Format("비급대로 수련하자 호신술을 증진할 수 있었다!\n기존 방: {0}\n변동량: <b><color=red>+5</color></b>\n현재 방: {1}", temp_Dff, DataController.Instance.gameData.statDFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }
            else
            {
                // 50% chance for Dff -5
                int temp_Dff = DataController.Instance.gameData.statDFF;
                DataController.Instance.gameData.statDFF = Mathf.Max(1, DataController.Instance.gameData.statDFF - 5);

                string temp_string = string.Format("비급을 이해하지 못한 채로 나쁜 버릇만 늘어나고 말았다...\n기존 방: {0}\n변동량: <b><color=blue>-5</color></b>\n현재 방: {1}", temp_Dff, DataController.Instance.gameData.statDFF);
                StartCoroutine(TypeTextEffect(temp_string, m_EventText));
            }

        }
        else if (selected == 2)
        {
            m_EventText.text = "당신은 비급을 무시하고 떠났다.";
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
