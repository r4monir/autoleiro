using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using App.Core;

public class ChallengeStatus : MonoBehaviour
{
    private int currentId;
    [SerializeField] private TMP_InputField inputF_desc;
    [SerializeField] private AppScrollbar scrollBar_lvl;
    [SerializeField] private AppScrollbar scrollBar_repeat;
    [SerializeField] private ConditionButton cBtn_secret;

    [Header("Base")]
    [SerializeField] private Button btn_back;
    [SerializeField] private Button btn_finish;
    [SerializeField] private Button btn_delet;
    [SerializeField] private Button btn_ask;
    [SerializeField] private Button btn_popup_ask;

    private void Start() {
        btn_finish.onClick.AddListener(ButtonFinish);
        btn_back.onClick = btn_finish.onClick;
        btn_delet.onClick.AddListener(ButtonDelet);
        btn_ask.onClick.AddListener(ButtonAsk);
        btn_popup_ask.onClick = btn_ask.onClick;

        GameManager.Instance.maxLvl = scrollBar_lvl.Steps;
    }

    public void SetStatus(int id, string desc = null, int lvl = 1, int repeats = 1, bool secret = false){
        currentId = id;
        inputF_desc.text = desc;

        scrollBar_lvl.SetValue(lvl);
        scrollBar_repeat.SetValue(repeats);
    }

    void ResetStatus(){
        SetStatus(0);
    }

    public void NewChallenge(int id){
        currentId = id;
        inputF_desc.text = null;
        scrollBar_lvl.SetValue(1);
        scrollBar_repeat.SetValue(1);
    }

    void ButtonFinish(){
        GameManager.Instance.NewBoard.UpdateChallengeStatus
            (currentId, inputF_desc.text, scrollBar_lvl.Value, scrollBar_repeat.Value, cBtn_secret.Active);
        
        ResetStatus();
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.NewBoard);

        btn_popup_ask.gameObject.SetActive(false);
    }

    void ButtonDelet(){
        GameManager.Instance.NewBoard.RemoveChallengeFromList(currentId);

        ResetStatus();
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.NewBoard);

        btn_popup_ask.gameObject.SetActive(false);
    }

    void ButtonAsk(){
        btn_popup_ask.gameObject.SetActive(!btn_popup_ask.isActiveAndEnabled);
    }
}
