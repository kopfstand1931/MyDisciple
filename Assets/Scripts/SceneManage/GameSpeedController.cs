using System.Collections;
using System.Collections.Generic;
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

    private void Start()
    {
        // Add button click listeners
        x2Button.onClick.AddListener(SetSpeedX2);
        x4Button.onClick.AddListener(SetSpeedX4);
        x8Button.onClick.AddListener(SetSpeedX8);

        m_now_x2 = false;
        m_now_x4 = false;
        m_now_x8 = false;
    }

    private void SetSpeedX2()
    {
        if (m_now_x2 == false)
        {
            Time.timeScale = 2f; 
            m_now_x2 = true;
        }
        else
        {
            Time.timeScale = 1f; 
            m_now_x2 = false;
        }
    }

    private void SetSpeedX4()
    {
        if (m_now_x4 == false)
        {
            Time.timeScale = 4f;
            m_now_x4 = true;
        }
        else
        {
            Time.timeScale = 1f;
            m_now_x4 = false;
        }
    }

    private void SetSpeedX8()
    {
        if (m_now_x8 == false)
        {
            Time.timeScale = 8f;
            m_now_x8 = true;
        }
        else
        {
            Time.timeScale = 1f;
            m_now_x8 = false;
        }
    }
}