using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using App.Core;

public class Notification : MonoBehaviour
{
    public enum EnumPopupMsg { NewTurn, Win }

    [Header("PathZoom")]
    [SerializeField] private Path pathZoom;
    [SerializeField] private Button shadow_pathZoom;
    [SerializeField] private Image bg_pathZoomNickname;
    [SerializeField] private TextMeshProUGUI txt_pathZoomNickname;

    [Header("Popup")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Button shadow_popup;
    [SerializeField] private Image img_popup;
    [SerializeField] private TextMeshProUGUI txt_popupMain;
    [SerializeField] private TextMeshProUGUI txt_popupAux;

    [Header("Extra Points")]
    [SerializeField] private GameObject popup_extraPoints;
    [SerializeField] private FieldMoreLess fml_extraPoints;
    [SerializeField] private Button btn_finishExtraPoints;

    const string NEW_TURN_AUX_MSG = "É sua vez!";
    const string WIN_AUX_MSG = "VENCEU!!!";

    void Start(){
        shadow_pathZoom.onClick.AddListener(ButtonShadowPathZoom);
        shadow_popup.onClick.AddListener(ButtonShadowPopup);

        btn_finishExtraPoints.onClick.AddListener(ButtonFinishExtraPoints);
    }

    public void ZoomPathNotification(string desc, string nickname = ""){
        shadow_pathZoom.gameObject.SetActive(true);
        pathZoom.gameObject.SetActive(true);
        
        pathZoom.SetZoomItems(desc);

        if (nickname == "") bg_pathZoomNickname.gameObject.SetActive(false);
        else {
            bg_pathZoomNickname.gameObject.SetActive(true);
            txt_pathZoomNickname.text = nickname;
            
            switch (GameManager.Instance.GameSettings.ExtraPoints){
            case true:
                popup_extraPoints.gameObject.SetActive(true);

                break;
            case false:
                popup_extraPoints.gameObject.SetActive(false);
                break;
            }
        }
    }

    public void ShowPopup(string nickname, EnumPopupMsg type, Color32 color = new Color32()){
        shadow_popup.gameObject.SetActive(true);        
        popup.SetActive(true);

        txt_popupMain.text = nickname;

        switch (type) {
            case EnumPopupMsg.NewTurn:
                txt_popupAux.text = NEW_TURN_AUX_MSG;
                img_popup.color = color;
                break;
            case EnumPopupMsg.Win:
                txt_popupAux.text = WIN_AUX_MSG;
                break;
        }
    }

    void ButtonShadowPathZoom(){
        if (GameManager.Instance.GameSettings.ExtraPoints) return;

        shadow_pathZoom.gameObject.SetActive(false);
        pathZoom.gameObject.SetActive(false);
    }

    void ButtonShadowPopup(){
        shadow_popup.gameObject.SetActive(false);
        popup.SetActive(false);
    }

    void ButtonFinishExtraPoints(){
        int value = fml_extraPoints.Value;
        GameManager.Instance.Boardgame.AddPointsToLastPlayer(value);
        
        fml_extraPoints.ResetValue();

        shadow_pathZoom.gameObject.SetActive(false);
        pathZoom.gameObject.SetActive(false);
    }
}
