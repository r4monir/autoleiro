using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppScrollbar : MonoBehaviour
{
    private Scrollbar scrollBar;
    private TextMeshProUGUI txt;
    
    [HideInInspector] private int _value;
    public int Value { get => _value; }
    [SerializeField] private int minValue; 
    [SerializeField] private int startValue; 

    [SerializeField] private int _steps;
    public int Steps { get => _steps; }

    void Awake(){
        scrollBar = GetComponentInChildren<Scrollbar>();
        txt = transform.Find("txt_value").GetComponent<TextMeshProUGUI>();

        scrollBar.onValueChanged.AddListener(delegate { UpdateText(); });
        SetValue(startValue);
    }

    public void SetValue(int value){
        value -= minValue;
        scrollBar.numberOfSteps = _steps;
        scrollBar.value = ((float) value / (scrollBar.numberOfSteps - 1));
        UpdateText();
    }

    void UpdateText(){
        _value = (int) (scrollBar.value * (scrollBar.numberOfSteps - 1)) + minValue;
        txt.text = _value.ToString();
    }
}
