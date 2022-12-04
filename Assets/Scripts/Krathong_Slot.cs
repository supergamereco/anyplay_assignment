using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Krathong_Slot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_TextPrice;
    [SerializeField] private Image m_Image;
    [SerializeField] private Image m_PriceImage;
    public int m_Id;
    public int m_Price;
    public string m_PriceType;
    public Action<int> OnSelected;


    // Start is called before the first frame update
    void Start()
    {
        Button _button = GetComponent<Button>();
        _button.onClick.AddListener(OnSelectButton);
        UpdateSlots();
    }
    /// <summary>
    /// Update Krathong Slot Data
    /// </summary>
    /// <param name="id"></param>
    /// <param name="price_type"></param>
    /// <param name="price"></param>
    /// <param name="onSelectedCallBack"></param>
    public void SetUp(int id, string price_type, int price, Action<int> onSelectedCallBack)
    {
        m_Id = id;
        m_Price = price;
        m_PriceType = price_type;
        OnSelected = onSelectedCallBack;
    }
    /// <summary>
    /// Update Krathong Selection Data
    /// </summary>
    private void UpdateSlots()
    {
        //Set Image
        m_Image.sprite = Resources.Load<Sprite>($"Sprites/krathong{m_Id}");
        //Set Price Type
        if (m_PriceType == "coins")
        {
            m_PriceImage.sprite = Resources.Load<Sprite>($"UI/ccoin_icon");
        }
        else
        {
            m_PriceImage.sprite = Resources.Load<Sprite>($"UI/ecoin_icon");
        }
        //Set Price
        if(m_Price <= 0)
        {
            m_TextPrice.text = "FREE";
        }
        else
        {
            m_TextPrice.text = m_Price.ToString();
        }
    }
    /// <summary>
    /// On Select Krathong Callback
    /// </summary>
    private void OnSelectButton()
    {
        OnSelected?.Invoke(m_Id);
    }
}
