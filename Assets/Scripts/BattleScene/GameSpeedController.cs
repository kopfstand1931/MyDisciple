using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    public Button x2Button;
    public Button x4Button;
    public Button x8Button;

    private bool m_now_x2;
    private bool m_now_x4;
    private bool m_now_x8;

    private MartialAgentNPC m_agentNPC;
    private MartialAgentPlayer m_agentPlayer;
    private DecisionRequester m_requesterNPC;
    private DecisionRequester m_requesterPlayer;

    private int defaultDecisionPeriod = 2;

    private void Start()
    {
        // Add button click listeners
        x2Button.onClick.AddListener(SetSpeedX2);
        x4Button.onClick.AddListener(SetSpeedX4);
        x8Button.onClick.AddListener(SetSpeedX8);

        m_now_x2 = false;
        m_now_x4 = false;
        m_now_x8 = false;

        defaultDecisionPeriod = 2;

        // find Agents
        m_agentNPC = FindFirstObjectByType<MartialAgentNPC>();
        m_agentPlayer = FindFirstObjectByType<MartialAgentPlayer>();

        m_requesterNPC = m_agentNPC.gameObject.GetComponent<DecisionRequester>();
        m_requesterPlayer = m_agentPlayer.gameObject.GetComponent<DecisionRequester>();

        
    }

    private void SetSpeedX2()
    {
        if (m_now_x2 == false)
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod * 2;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod * 2;

            Time.timeScale = 2f;

            m_now_x2 = true;
            m_now_x4 = false;
            m_now_x8 = false;

        }
        else
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod;

            Time.timeScale = 1f;

            m_now_x2 = false;

        }
    }

    private void SetSpeedX4()
    {
        if (m_now_x4 == false)
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod * 4;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod * 4;

            Time.timeScale = 4f;

            m_now_x2 = false;
            m_now_x4 = true;
            m_now_x8 = false;

        }
        else
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod;

            Time.timeScale = 1f;

            m_now_x4 = false;

        }
    }

    private void SetSpeedX8()
    {
        if (m_now_x8 == false)
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod * 8;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod * 8;

            Time.timeScale = 8f;

            m_now_x2 = false;
            m_now_x4 = false;
            m_now_x8 = true;

        }
        else
        {
            m_requesterNPC.DecisionPeriod = defaultDecisionPeriod;
            m_requesterPlayer.DecisionPeriod = defaultDecisionPeriod;

            Time.timeScale = 1f;

            m_now_x8 = false;

        }
    }

    private void OnDestroy()
    {
        // Remove button click listeners
        x2Button.onClick.RemoveListener(SetSpeedX2);
        x4Button.onClick.RemoveListener(SetSpeedX4);
        x8Button.onClick.RemoveListener(SetSpeedX8);

        // Set TimeScale to 1
        Time.timeScale = 1f;
    }
}