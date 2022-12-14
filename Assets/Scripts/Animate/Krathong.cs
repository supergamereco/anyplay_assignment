using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Krathong : MonoBehaviour
{
    [SerializeField] private float m_StartPositionX = 0;
    [SerializeField] private float m_ResetPositionX = -6.0f;
    [SerializeField] private float m_FloatingSpeed;
    [SerializeField] private TextMeshPro m_FBNameText;
    [SerializeField] private TextMeshPro m_WishText;
    private float m_ResetPositionY;
    public string m_FBName;
    public string m_Wish;
    public string m_UserId;
    public int m_KrathongId;
    public Transform m_Krathong;
    public SpriteRenderer m_KrathongImage;
    public int m_Id;
    public Action<int> OnReset;
    public float m_RotateSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        RandomYPos();
        transform.position = new Vector3(m_StartPositionX * m_Id, m_ResetPositionY, 0);
        m_RotateSpeed = UnityEngine.Random.Range(1, 2.5f);
    }
    /// <summary>
    /// Setup Krathong Data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="krathongid"></param>
    /// <param name="userid"></param>
    /// <param name="wish"></param>
    /// <param name="fbname"></param>
    /// <param name="onResetCallBack"></param>
    public void SetUp(int id, int krathongid, string userid, string wish, string fbname, Action<int> onResetCallBack)
    {
        //Set Data
        m_Id = id;
        m_KrathongId = krathongid;
        m_FBName = fbname;
        m_Wish = wish;
        m_UserId = userid;
        OnReset = onResetCallBack;
        m_FBNameText.text = fbname;
        m_WishText.text = wish;
        //Set Sprite
        m_KrathongImage.sprite = Resources.Load<Sprite>($"Sprites/krathong{m_KrathongId}");
    }

    void FixedUpdate()
    {
        Floating();
    }
    /// <summary>
    /// Krathong Movement
    /// </summary>
    private void Floating()
    {
        //Rotate
        m_Krathong.transform.Rotate(0, 0, m_RotateSpeed * Time.deltaTime, Space.Self);
        if (m_Krathong.transform.rotation.z >= 0.1f || m_Krathong.transform.rotation.z <= -0.1f)
        {
            m_RotateSpeed = m_RotateSpeed * -1;
        }

        //Float

        if (transform.position.x < 12.25f)
        {
            transform.Translate(new Vector3(m_FloatingSpeed, 0, 0) * Time.deltaTime);
            transform.position = transform.position + new Vector3(m_FloatingSpeed, 0, 0) * Time.deltaTime;
        }
        else
        {
            RandomYPos();
            ResetKrathong();
        }
    }
    /// <summary>
    /// Random Y position
    /// </summary>
    private void RandomYPos()
    {
        int _randomYPosition = UnityEngine.Random.Range(0, 3);
        if (_randomYPosition == 0)
        {
            m_ResetPositionY = -0.3f;
        }
        else if (_randomYPosition == 1)
        {
            m_ResetPositionY = -1.5f;
        }
        else
        {
            m_ResetPositionY = -2.9f;
        }
    }
    /// <summary>
    /// Reset Krathong Data
    /// </summary>
    private void ResetKrathong()
    {
        //Set Sprite
        m_KrathongImage.sprite = Resources.Load<Sprite>($"Sprites/krathong{m_KrathongId}");
        transform.position = new Vector3(m_ResetPositionX * 8, m_ResetPositionY, transform.position.z);
        OnResetKrathong();
    }
    /// <summary>
    /// Reset Krathong Data Callback
    /// </summary>
    private void OnResetKrathong()
    {
        OnReset?.Invoke(m_Id);
    }
}
