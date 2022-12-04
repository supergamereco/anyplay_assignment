using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAndFloating : MonoBehaviour
{
    [SerializeField] private float m_FadeSpeed = 0.3f;
    [SerializeField] private float m_StartPositionY = 0.3f;
    [SerializeField] private float m_FloatingRange;
    [SerializeField] private float m_FloatingSpeed;
    [SerializeField] private float m_RotateSpeed = 0.3f;
    private Color m_Color;
    private bool m_Fading = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Color = GetComponent<SpriteRenderer>().color;
        transform.position = new Vector3(transform.position.x, m_StartPositionY, transform.position.z);
    }

    private void FixedUpdate()
    {
        Floating();
        if (transform.position.y >= (m_StartPositionY + m_FloatingRange) * (1 / 2))
        {
            if (!m_Fading)
            {
                StartCoroutine("Fade");
            }
        }
    }
    /// <summary>
    /// Kom movement
    /// </summary>
    private void Floating()
    {
        //Rotate
        transform.Rotate(0, 0, m_RotateSpeed * Time.deltaTime, Space.Self);
        if (transform.rotation.z >= 0.1f || transform.rotation.z <= -0.1f)
        {
            m_RotateSpeed = m_RotateSpeed * -1;
        }

        //Float
        if (transform.position.y < m_StartPositionY + m_FloatingRange)
        {
            transform.Translate(new Vector3(0, m_FloatingSpeed, 0) * Time.deltaTime);
        }
        else if(transform.position.y > m_StartPositionY + m_FloatingRange)
        {
            transform.position = new Vector3(transform.position.x, m_StartPositionY, transform.position.z);
            m_Fading = false;
        }
    }
    /// <summary>
    /// Fade kom sprite
    /// </summary>
    /// <param name="color"></param>
    /// <param name="_fadeSpeed"></param>
    private IEnumerator Fade()
    {
        m_Fading = true;
        for (float f = 0.05f; f <= 1; f += m_FadeSpeed)
        {
            Color c = m_Color;
            c.a = f;
            GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
