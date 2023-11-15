using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class TrainingManager : MonoBehaviour
{

    // for fake loading

    [SerializeField] private Image fillOff;
    [SerializeField] private Image fillDff;
    [SerializeField] private Image fillSpd;
    [SerializeField] private Image fillMLevel;

    private float time_loading = 3;
    private float time_current;
    private float time_start;
    private bool isEnded = true;

    private Image currentFillUp;


    // confirm buttons

    [SerializeField] private GameObject trainingConfirmOff;
    [SerializeField] private GameObject trainingConfirmDff;
    [SerializeField] private GameObject trainingConfirmSpd;
    [SerializeField] private GameObject trainingConfirmMLevel;


    // Text Fields

    [SerializeField] private TMPro.TextMeshProUGUI titleOff;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionOff;
    [SerializeField] private TMPro.TextMeshProUGUI titleDff;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionDff;
    [SerializeField] private TMPro.TextMeshProUGUI titleSpd;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionSpd;
    [SerializeField] private TMPro.TextMeshProUGUI titleMLevel;
    [SerializeField] private TMPro.TextMeshProUGUI descriptionMLevel;


    void Start()
    {
        // fake loading reset
        Reset_Loading(currentFillUp);
    }

    void Update()
    {
        // fake loading update
        if (isEnded)
        {
            return;
        }
        Check_Loading(currentFillUp);
    }


    // All 3 Training have a probability of increasing the amount of stats.

    public void TrainingOff()
    {
        trainingConfirmOff.SetActive(false);
        titleOff.text = "���ݷ� �Ʒ� ������...";
        descriptionOff.text = "";

        currentFillUp = fillOff;
        Reset_Loading(currentFillUp);

        StartCoroutine(CoroutineUpdateOff());

    }

    public void TrainingDff()
    {
        trainingConfirmDff.SetActive(false);
        titleDff.text = "���� �Ʒ� ������...";
        descriptionDff.text = "";

        currentFillUp = fillDff;
        Reset_Loading(currentFillUp);

        StartCoroutine(CoroutineUpdateDff());

    }

    public void TrainingSpd()
    {
        trainingConfirmSpd.SetActive(false);
        titleSpd.text = "���ǵ� �Ʒ� ������...";
        descriptionSpd.text = "";

        currentFillUp = fillSpd;
        Reset_Loading(currentFillUp);

        StartCoroutine(CoroutineUpdateSpd());

    }

    public void TrainingMLevel()
    {
        trainingConfirmMLevel.SetActive(false);
        titleMLevel.text = "���� ������...";
        descriptionMLevel.text = "";

        currentFillUp = fillMLevel;
        Reset_Loading(currentFillUp);

        StartCoroutine(CoroutineUpdateMLevel());
    }

    private int GetRandomValue()
    {
        // Generate a random value between 0 and 100
        int randomNumber = Random.Range(0, 101);

        // Check the probability ranges and return the corresponding value
        if (randomNumber <= 60)
        {
            return 1; // 60% chance for +1
        }
        else if (randomNumber <= 90)
        {
            return 2; // 30% chance for +2
        }
        else
        {
            return 3; // 10% chance for +3
        }
    }


    // update coroutines
    IEnumerator CoroutineUpdateOff()
    {
        int sec = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            sec++;

            if (sec >= 3)
            {
                int randomValue = GetRandomValue();
                int temp_Off = DataController.Instance.gameData.statOFF;
                DataController.Instance.gameData.statOFF += randomValue;
                DataController.Instance.gameData.turnElapsed += 1;
                DataController.Instance.SaveGameData();

                titleOff.text = "���ݷ� �Ʒ� �Ϸ�!";
                string temp_string = string.Format("���� ��: {0}\n������: <b><color=red>+{1}</color></b>\n���� ��: {2}", temp_Off, randomValue, DataController.Instance.gameData.statOFF);
                StartCoroutine(TypeTextEffect(temp_string, descriptionOff));
                trainingConfirmOff.SetActive(true);
                break;
            }
        }
    }


    IEnumerator CoroutineUpdateDff()
    {
        int sec = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            sec++;

            if (sec >= 3)
            {
                int randomValue = GetRandomValue();
                int temp_Dff = DataController.Instance.gameData.statDFF;
                DataController.Instance.gameData.statDFF += randomValue;
                DataController.Instance.gameData.turnElapsed += 1;
                DataController.Instance.SaveGameData();

                titleDff.text = "���� �Ʒ� �Ϸ�!";
                string temp_string = string.Format("���� ��: {0}\n������: <b><color=red>+{1}</color></b>\n���� ��: {2}", temp_Dff, randomValue, DataController.Instance.gameData.statDFF);
                StartCoroutine(TypeTextEffect(temp_string, descriptionDff));
                trainingConfirmDff.SetActive(true);
                break;
            }
        }
    }


    IEnumerator CoroutineUpdateSpd()
    {
        int sec = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            sec++;

            if (sec >= 3)
            {
                int randomValue = GetRandomValue();
                int temp_Spd = DataController.Instance.gameData.statSPD;
                DataController.Instance.gameData.statSPD += randomValue;
                DataController.Instance.gameData.turnElapsed += 1;
                DataController.Instance.SaveGameData();

                titleSpd.text = "���ǵ� �Ʒ� �Ϸ�!";
                string temp_string = string.Format("���� ��: {0}\n������: <b><color=red>+{1}</color></b>\n���� ��: {2}", temp_Spd, randomValue, DataController.Instance.gameData.statSPD);
                StartCoroutine(TypeTextEffect(temp_string, descriptionSpd));
                trainingConfirmSpd.SetActive(true);
                break;
            }
        }
    }


    IEnumerator CoroutineUpdateMLevel()
    {
        int sec = 0;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            sec++;

            if (sec >= 3)
            {
                string temp_string = "";

                DataController.Instance.gameData.currentEXP += 1;
                DataController.Instance.gameData.turnElapsed += 1;

                if (DataController.Instance.gameData.currentEXP >= 2 && DataController.Instance.gameData.currentModelLevel == 1)
                {
                    DataController.Instance.gameData.currentEXP = 0;
                    DataController.Instance.gameData.currentModelLevel += 1;
                    DataController.Instance.SaveGameData();

                    titleMLevel.text = "���õ� ���� ���!";
                    temp_string = string.Format("�����մϴ�!\n���� ����ġ�� �ʿ�ġ�� �����Ͽ�, ���õ���\n<b><color=red>'�ϱ�'</color></b>���� <b><color=red>'�߱�'</color></b>���� ����Ͽ����ϴ�!");
                    StartCoroutine(TypeTextEffect(temp_string, descriptionMLevel));
                    trainingConfirmMLevel.SetActive(true);
                    break;
                }
                else if (DataController.Instance.gameData.currentEXP >= 4 && DataController.Instance.gameData.currentModelLevel == 2)
                {
                    DataController.Instance.gameData.currentEXP = 0;
                    DataController.Instance.gameData.currentModelLevel += 1;
                    DataController.Instance.SaveGameData();

                    titleMLevel.text = "���õ� <b><color=red>�ִ�ġ ����!</color></b>";
                    temp_string = string.Format("�����մϴ�!\n���� ����ġ�� �ʿ�ġ�� �����Ͽ�, ���õ���\n<b><color=red>'�߱�'</color></b>���� <b><color=red>'���'</color></b>���� ����Ͽ����ϴ�!");
                    StartCoroutine(TypeTextEffect(temp_string, descriptionMLevel));
                    trainingConfirmMLevel.SetActive(true);
                    break;
                }
                else
                {
                    DataController.Instance.SaveGameData();

                    titleMLevel.text = "���� ���� �Ϸ�!";
                    int temp_needed;
                    if (DataController.Instance.gameData.currentModelLevel == 1)
                    {
                        temp_needed = 2;
                    }
                    else if (DataController.Instance.gameData.currentModelLevel == 2)
                    {
                        temp_needed = 4;
                    }
                    else
                    {
                        temp_needed = 0;
                    }
                    temp_string = string.Format("���� ����ġ +1!\n���� ���� ����ġ: {0}\n���� ��������\n�ʿ� ���� ����ġ: <b><color=red>{1}</color></b>", DataController.Instance.gameData.currentEXP, temp_needed); 
                    StartCoroutine(TypeTextEffect(temp_string, descriptionMLevel));
                    trainingConfirmMLevel.SetActive(true);
                    break;
                }
            }
        }
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

            yield return new WaitForSeconds(0.05f);
        }
    }


    // for fake loading functions
    private void Check_Loading(Image image_fill)
    {
        time_current = Time.time - time_start;
        if (time_current < time_loading)
        {
            Set_FillAmount(time_current / time_loading, image_fill);
        }
        else if (!isEnded)
        {
            End_Loading(image_fill);
        }
    }

    private void End_Loading(Image image_fill)
    {
        Set_FillAmount(1, image_fill);
        isEnded = true;
    }

    private void Reset_Loading(Image image_fill)
    {
        time_current = time_loading;
        time_start = Time.time;
        Set_FillAmount(0, image_fill);
        isEnded = false;
    }
    private void Set_FillAmount(float _value, Image image_fill)
    {
        image_fill.fillAmount = _value;
    }
}
