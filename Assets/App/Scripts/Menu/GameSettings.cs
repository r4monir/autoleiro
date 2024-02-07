using System.Collections;
using System.Collections.Generic;
using App.Core;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [System.Serializable]
    public class SettingsItems {
        public int maxDice;
        public bool extraPoints;
        public bool notRepeatPath;
        public bool skipLevels;
    }

    [SerializeField] private AppScrollbar scrollBar_sidesDice;
    [SerializeField] private ConditionButton cBtn_extraPoints;
    [SerializeField] private ConditionButton cBtn_notRepeatPath;
    [SerializeField] private ConditionButton cBtn_skipLevels;

    [Header("Base")]
    [SerializeField] private Button btn_back;
    [SerializeField] private Button btn_finish;

    [Header("Settings")]
    private int _maxDice;
    public int MaxDice { get => _maxDice; }
    private bool _extraPoints;
    public bool ExtraPoints { get => _extraPoints; }
    private bool _notRepeatPath;
    public bool NotRepeatPath { get => _notRepeatPath; }
    private bool _skipLevels;
    public bool SkipLevels { get => _skipLevels; }

    const string keyData = "GAMESETTINGS";

    void Start()
    {
        btn_finish.onClick.AddListener(ButtonFinish);
        btn_back.onClick = btn_finish.onClick;     

        _maxDice = scrollBar_sidesDice.Value;

        RequestData();
    }

    void RequestData(){
        if(!PlayerPrefs.HasKey(keyData)) return;
        string json = PlayerPrefs.GetString(keyData);

        SettingsItems settingsItems = JsonUtility.FromJson<SettingsItems>(json);
        scrollBar_sidesDice.SetValue(settingsItems.maxDice);
        cBtn_extraPoints.SetValue(settingsItems.extraPoints);
        cBtn_notRepeatPath.SetValue(settingsItems.notRepeatPath);
        cBtn_skipLevels.SetValue(settingsItems.skipLevels);

        SetSaveValues();
    }

    void SetSaveValues(){
        _maxDice = scrollBar_sidesDice.Value;
        _extraPoints = cBtn_extraPoints.Active;
        _notRepeatPath  = cBtn_notRepeatPath.Active;
        _skipLevels  = cBtn_skipLevels.Active;
    }

    void ButtonFinish(){
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Menu);

        SetSaveValues();

        SaveData();
    }

    void SaveData(){
        SettingsItems settingsItems = new SettingsItems();
        settingsItems.maxDice = _maxDice;
        settingsItems.extraPoints = _extraPoints;
        settingsItems.notRepeatPath = _notRepeatPath;
        settingsItems.skipLevels = _skipLevels;
        string json = JsonUtility.ToJson(settingsItems);

        PlayerPrefs.SetString(keyData, json);
    }
}
