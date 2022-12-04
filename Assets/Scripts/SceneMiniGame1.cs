using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SceneMiniGame1 : MonoBehaviour
{

    [SerializeField] private GameObject m_KrathongPanel;
    [SerializeField] private GameObject m_ConfirmPanel;
    [SerializeField] private GameObject m_ErrorPanel;
    [SerializeField] private GameObject m_KrathongSlots;
    [SerializeField] private GameObject m_KrathongParents;
    [SerializeField] private GameObject m_KrathongButton;
    [SerializeField] private Button m_BackButton;
    [SerializeField] private Button m_LaunchkButton;
    [SerializeField] private Button m_ConfirmButton;
    [SerializeField] private Button m_CancleButton;
    [SerializeField] private Button m_ErrorConfirmButton;
    [SerializeField] private Button m_CloseButton;
    [SerializeField] private Image m_SelectedImage;
    [SerializeField] private GameObject m_PriceTypeIcon;
    [SerializeField] private TextMeshProUGUI m_CoinsText;
    [SerializeField] private TextMeshProUGUI m_CashText;
    [SerializeField] private TextMeshProUGUI m_Wish;
    [SerializeField] private TextMeshProUGUI m_Price;
    [SerializeField] private TextMeshProUGUI m_ErrorMessage;
    private List<GameObject> m_Krathongs = new List<GameObject>();
    private KrathongData[] m_KrathongData = new KrathongData[7];
    private KrathongData m_CurrentkrathongData = new KrathongData();
    private UserData m_UserData = new UserData();
    public GameObject m_Slot;
    public GameObject m_Krathong;
    private int m_KrathongCount;

    //Struct that store Krathong data
    struct CurrentKrathongData
    {
        public int id;
        public string userid;
        public int krathong_id;
        public string wish;
        public string time;
        public string fbname;
    }
    CurrentKrathongData[] m_CurrentKrathongData;

    // Start is called before the first frame update
    private void Start()
    {
        m_KrathongButton.GetComponent<Button>().onClick.AddListener(ActiveSelectionPanelButton);
        m_BackButton.onClick.AddListener(OnBackButton);
        m_LaunchkButton.onClick.AddListener(OnLaunchButton);
        m_ConfirmButton.onClick.AddListener(OnConfirm);
        m_CancleButton.onClick.AddListener(OnCancle);
        m_CloseButton.onClick.AddListener(OnClose);
        m_ErrorConfirmButton.onClick.AddListener(OnCancle);
        StartCoroutine("GetUserData");
        StartCoroutine("GetKrathongData");
        StartCoroutine("GetCurrentKrathongs");
    }
    /// <summary>
    /// Create Krathong slots
    /// </summary>
    private void CreateSlot()
    {
        for (int i = 0; i < m_KrathongData.Length; i++)
        {
            GameObject _slot = Instantiate(m_Slot, new Vector2(0, 0), Quaternion.identity);
            _slot.transform.localScale = new Vector3(2,2,2);
            _slot.GetComponent<Krathong_Slot>().SetUp(Int32.Parse(m_KrathongData[i].krathong_id), m_KrathongData[i].price_type, m_KrathongData[i].price, OnSelectKrathong);
            _slot.transform.SetParent(m_KrathongSlots.transform);
        }
    }
    /// <summary>
    /// Create Krathong object
    /// </summary>
    private void CreateKrathong(int id)
    {
        GameObject _krathong = Instantiate(m_Krathong, new Vector2(0, 0), Quaternion.identity);
        _krathong.transform.localScale = new Vector3(2, 2, 2);
        _krathong.GetComponent<Krathong>().SetUp(id, m_CurrentKrathongData[id].krathong_id, m_CurrentKrathongData[id].userid, m_CurrentKrathongData[id].wish, m_CurrentKrathongData[id].fbname, OnResetKrathong);
        _krathong.transform.SetParent(m_KrathongParents.transform);
        m_KrathongCount++;
        m_Krathongs.Add(_krathong);
    }
    /// <summary>
    /// Open Krathong selection panel
    /// </summary>
    private void ActiveSelectionPanelButton()
    {
        m_KrathongButton.SetActive(false);
        m_KrathongPanel.SetActive(true);
    }
    /// <summary>
    /// Close Krathong panel
    /// </summary>
    private void OnBackButton()
    {
        m_KrathongButton.SetActive(true);
        m_KrathongPanel.SetActive(false);
    }
    /// <summary>
    /// Launch Krathong
    /// </summary>
    private void OnLaunchButton()
    {
        int _respond_code;
        //Check result
        if(1 >= m_Wish.text.Length)
        {
            _respond_code = 401;
        }
        else if (m_CurrentkrathongData.max_floating <= m_UserData.krathongCount[Int32.Parse(m_CurrentkrathongData.krathong_id) - 1])
        {
            _respond_code = 403;
        }
        else if (m_CurrentkrathongData.price_type == "coins")
        {
            if(m_CurrentkrathongData.price > m_UserData.coins)
            {
                _respond_code = 402;
            }
            else
            {
                _respond_code = 200;
            }
        }
        else if (m_CurrentkrathongData.price_type == "cash")
        {
            if (m_CurrentkrathongData.price > m_UserData.cash)
            {
                _respond_code = 402;
            }
            else
            {
                _respond_code = 200;
            }
        }
        else
        {
            _respond_code = 200;
        }
        //Result Action
        switch (_respond_code)
        {
            case 200:
                if (m_CurrentkrathongData.price_type == "coins")
                {
                    Sprite _sprite = Resources.Load<Sprite>($"UI/ccoin_icon");
                    m_PriceTypeIcon.GetComponent<Image>().sprite = _sprite;
                }
                else
                {
                    Sprite _sprite = Resources.Load<Sprite>($"UI/ecoin_icon");
                    m_PriceTypeIcon.GetComponent<Image>().sprite = _sprite;
                }
                if(m_CurrentkrathongData.price > 0)
                {
                    if(m_CurrentkrathongData.price >= 1000)
                    {
                        m_Price.text = $"{(m_CurrentkrathongData.price/1000).ToString()}K";
                    }
                    else
                    {
                        m_Price.text = m_CurrentkrathongData.price.ToString();
                    }
                    m_PriceTypeIcon.SetActive(true);
                }
                else
                {
                    m_Price.text = "FREE";
                    m_PriceTypeIcon.SetActive(false);
                }
                m_ConfirmPanel.gameObject.SetActive(true);
                break;
            case 401:
                m_ErrorPanel.gameObject.SetActive(true);
                m_ErrorMessage.text = $"กรุณาใส่คำอธิษฐาน";
                break;
            case 402:
                m_ErrorPanel.gameObject.SetActive(true);
                m_ErrorMessage.text = $"ค่ายใช้จ่ายในการลอยกระทงไม่เพียงพอ";
                break;
            case 403:
                m_ErrorPanel.gameObject.SetActive(true);
                m_ErrorMessage.text = $"ลอยกระทงนี้ไปแล้ว สามารถลอยกระทงนี้ได้เพียง {m_CurrentkrathongData.max_floating} ครั้ง";
                break;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    private void OnConfirm()
    {
        StartCoroutine("PostKrathong");
        m_ConfirmPanel.gameObject.SetActive(false);
        m_KrathongPanel.SetActive(false);
        m_KrathongButton.SetActive(true);
    }
    /// <summary>
    /// Cancle launch krathong
    /// </summary>
    private void OnCancle()
    {
        m_ConfirmPanel.gameObject.SetActive(false);
        m_ErrorPanel.gameObject.SetActive(false);
    }
    /// <summary>
    /// Update User data
    /// </summary>
    private void OnClose()
    {
        StartCoroutine("GetUserData");
        m_KrathongButton.SetActive(true);
        m_KrathongPanel.SetActive(false);
    }
    /// <summary>
    /// Reset Krathong information
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isRepeat"></param>
    private void OnResetKrathong(int id)
    {
        StartCoroutine("GetCurrentKrathongs");
        m_Krathongs[id].GetComponent<Krathong>().m_KrathongId = m_CurrentKrathongData[id].krathong_id;
    }
    /// <summary>
    /// Select Krathong
    /// </summary>
    /// <param name="krathongId"></param>
    private void OnSelectKrathong(int krathongId)
    {
        m_CurrentkrathongData = m_KrathongData[krathongId - 1];
        //Set Sprite
        m_SelectedImage.sprite = Resources.Load<Sprite>($"Sprites/krathong{krathongId}");
    }
    /// <summary>
    /// Update user data
    /// </summary>
    private void UpdateUserData()
    {
        m_CoinsText.text = m_UserData.coins.ToString();
        m_CashText.text = m_UserData.cash.ToString();
    }
    /// <summary>
    /// Download user data
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetUserData()
    {
        WWWForm _form = new WWWForm();
        _form.AddField("userid", "100837165998706");
        using (UnityWebRequest request = UnityWebRequest.Post("http://103.91.190.179/test_krathong/account_data/", _form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                m_UserData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                UpdateUserData();
            }
        }
    }
    /// <summary>
    /// download krathong data
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetKrathongData()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://103.91.190.179/test_krathong/krathong_list/"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                m_KrathongData = JsonConvert.DeserializeObject<KrathongData[]>(request.downloadHandler.text);
                m_CurrentkrathongData = m_KrathongData[0];
                CreateSlot();
            }
        }
    }
    /// <summary>
    /// Upload selected Krathong data and user data
    /// </summary>
    /// <returns></returns>
    private IEnumerator PostKrathong()
    {
        WWWForm _form = new WWWForm();
        _form.AddField("userid", m_UserData.userid);
        _form.AddField("krathong_id", m_CurrentkrathongData.krathong_id);
        _form.AddField("wish", m_Wish.text);
        using (UnityWebRequest request = UnityWebRequest.Post("http://103.91.190.179/test_krathong/krathong_submit/", _form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                m_UserData.krathongCount[Int32.Parse(m_CurrentkrathongData.krathong_id) - 1]++;
                if (m_CurrentkrathongData.price_type == "coins")
                {
                    m_UserData.coins = m_UserData.coins - m_CurrentkrathongData.price;
                    UpdateUserData();
                }
                else
                {
                    m_UserData.cash = m_UserData.cash - m_CurrentkrathongData.price;
                    UpdateUserData();
                }
            }
        }
    }
    /// <summary>
    /// Download current Krathong data
    /// </summary>
    /// <returns></returns>
    private IEnumerator GetCurrentKrathongs()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://103.91.190.179/test_krathong/krathong_logs/"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.responseCode);
                m_CurrentKrathongData = JsonConvert.DeserializeObject<CurrentKrathongData[]>(request.downloadHandler.text);
                if (m_Krathongs.Count < m_CurrentKrathongData.Length)
                {
                    for (int i = 0; i < m_CurrentKrathongData.Length; i++)
                    {
                        CreateKrathong(m_KrathongCount);
                    }
                }
            }
        }
    }
}
