using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FieldMoreLess : MonoBehaviour
{
    enum EnumAddPoint { More, Less }

    private TextMeshProUGUI txt;
    private Button btn_more;
    private Button btn_less;
    [HideInInspector] private int _value;
    public int Value { get => _value; }
    [SerializeField] private int minValue;
    [SerializeField] private int maxValue;
    [SerializeField] private int startValue; 

    void Start()
    {
        txt = transform.Find("txt_value").GetComponent<TextMeshProUGUI>();
        btn_more = transform.Find("btn_more").GetComponent<Button>();
        btn_less = transform.Find("btn_less").GetComponent<Button>();

        btn_more.onClick.AddListener(() => AddValue(EnumAddPoint.More));
        btn_less.onClick.AddListener(() => AddValue(EnumAddPoint.Less));
    }

    void AddValue(EnumAddPoint pointType)
    {
        _value = int.Parse(txt.text);

        switch (pointType){
            case EnumAddPoint.More:
            if (_value >= maxValue) break;
                _value++;
                break;
            case EnumAddPoint.Less:
            if (_value <= minValue) break;
                _value--;
                break;
        }
        
        txt.text = _value.ToString();
    }

    public void ResetValue(){
        _value = startValue;
        txt.text = _value.ToString();
    }
}
