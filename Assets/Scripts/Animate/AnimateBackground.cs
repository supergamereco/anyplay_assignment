using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateBackground : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_Stars;
    [SerializeField] private Transform m_Moon;
    [SerializeField] private Transform m_Cloud1;
    [SerializeField] private Transform m_Cloud2;
    [SerializeField] private Transform m_Kom1;
    [SerializeField] private Transform m_Kom2;
    [SerializeField] private Transform m_Kom3;
    [SerializeField] private Transform m_Kom4;
    [SerializeField] private Transform m_Kom5;
    [SerializeField] private Transform m_Kom6;
    [SerializeField] private Transform m_Wave1;
    [SerializeField] private Transform m_Wave2;

    //Animate Moon
    [SerializeField] private float m_MoonFloatingRange;
    [SerializeField] private float m_MoonFloatingSpeed;
    private float m_MoonStartPositionY;
    //Animate Cloud
    [SerializeField] private float m_Cloud1FloatingRange;
    [SerializeField] private float m_Cloud1FloatingSpeed;
    [SerializeField] private float m_Cloud2FloatingRange;
    [SerializeField] private float m_Cloud2FloatingSpeed;
    private float m_Cloud1StartPositionX;
    private float m_Cloud2StartPositionX;
    //Animate Wave
    [SerializeField] private float m_WaveScrollSpeed;
    private float m_WaveStartPositionX = -19.9f;
    //Fade Stars
    [SerializeField] private float m_FadeSpeed = 0.3f;
    private bool m_FadeIn = false;
    private bool m_FadeOut = true;
    
    // Start is called before the first frame update
    private void Start()
    {
        m_MoonStartPositionY = m_Moon.transform.position.y;
        m_Cloud1StartPositionX = m_Cloud1.transform.position.x;
        m_Cloud2StartPositionX = m_Cloud2.transform.position.x;
    }

    private void FixedUpdate()
    {
        MoonFloating();
        CloudFloating();
        WaveScrolling();
        FadeStars();
    }
    /// <summary>
    /// Wave movement
    /// </summary>
    private void WaveScrolling()
    {
        if(m_Wave1.transform.position.x > 19.9)
        {
            m_Wave1.transform.position = new Vector3(m_WaveStartPositionX, 0, 0);
        }
        if (m_Wave2.transform.position.x > 19.9)
        {
            m_Wave2.transform.position = new Vector3(m_WaveStartPositionX, 0, 0);
        }
        m_Wave1.transform.Translate(new Vector3(m_WaveScrollSpeed, 0, 0) * Time.deltaTime);
        m_Wave2.transform.Translate(new Vector3(m_WaveScrollSpeed, 0, 0) * Time.deltaTime);
    }
    /// <summary>
    /// Moon movement
    /// </summary>
    private void MoonFloating()
    {
        if (m_Moon.transform.position.y > m_MoonStartPositionY + m_MoonFloatingRange)
        {
            m_MoonFloatingSpeed = m_MoonFloatingSpeed * -1;
        }
        if (m_Moon.transform.position.y < m_MoonStartPositionY - m_MoonFloatingRange)
        {
            m_MoonFloatingSpeed = m_MoonFloatingSpeed * -1;
        }
        m_Moon.transform.Translate(new Vector3(0, m_MoonFloatingSpeed, 0) * Time.deltaTime);
    }
    /// <summary>
    /// Cloud movement
    /// </summary>
    /// <param name="_cloud"></param>
    /// <param name="speed"></param>
    /// <param name="range"></param>
    private void CloudFloating()
    {
        if (m_Cloud1.transform.position.x > m_Cloud1StartPositionX + m_Cloud1FloatingRange)
        {
            m_Cloud1FloatingSpeed = m_Cloud1FloatingSpeed * -1;
        }
        if (m_Cloud1.transform.position.x < m_Cloud1StartPositionX - m_Cloud1FloatingRange)
        {
            m_Cloud1FloatingSpeed = m_Cloud1FloatingSpeed * -1;
        }
        if (m_Cloud2.transform.position.x > m_Cloud2StartPositionX + m_Cloud2FloatingRange)
        {
            m_Cloud2FloatingSpeed = m_Cloud2FloatingSpeed * -1;
        }
        if (m_Cloud2.transform.position.x < m_Cloud2StartPositionX - m_Cloud2FloatingRange)
        {
            m_Cloud2FloatingSpeed = m_Cloud2FloatingSpeed * -1;
        }
        m_Cloud1.transform.Translate(new Vector3(m_Cloud1FloatingSpeed, 0, 0) * Time.deltaTime);
        m_Cloud2.transform.Translate(new Vector3(m_Cloud2FloatingSpeed, 0, 0) * Time.deltaTime);
    }
    /// <summary>
    /// Fade stars sprite
    /// </summary>
    private void FadeStars()
    {
        if (m_FadeOut)
        {
            if (m_Stars.color.a > 0)
            {
                Color c = m_Stars.color;
                c.a = c.a - m_FadeSpeed * Time.deltaTime;
                m_Stars.color = c;
            }
            else
            {
                m_FadeIn = true;
                m_FadeOut = false;
            }
        }
        if (m_FadeIn)
        {
            if (m_Stars.color.a < 1)
            {
                Color c = m_Stars.color;
                c.a = c.a + m_FadeSpeed * Time.deltaTime;
                m_Stars.color = c;
            }
            else
            {
                m_FadeOut = true;
                m_FadeIn = false;
            }
        }
    }
}
