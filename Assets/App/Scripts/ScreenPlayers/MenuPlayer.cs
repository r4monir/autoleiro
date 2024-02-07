using System.Collections;
using System.Collections.Generic;
using App.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayer : MonoBehaviour
{
    private int id;
    [SerializeField] private RectTransform rectT;
    [SerializeField] private Image img_playerColor;
    [SerializeField] private Button btn_close;
    public TMP_InputField inputF_name;
    [SerializeField] private Button btn_add;

    private bool _live;
    public bool Live { get => _live; }

    void Awake(){
        btn_close.onClick.AddListener(RemovePlayer);
        btn_add.onClick.AddListener(OpenPlayer);
    }

    public void SetItems(string name){
        inputF_name.text = name;
       
        ActivePlayer();
    }

    public void OpenPlayer()
    {
        ActivePlayer();

        GameManager.Instance.ScreenPlayers.AddNewPlayer();
    }

    void ActivePlayer(){
        Destroy(btn_add.gameObject);
        btn_close.gameObject.SetActive(true);
        _live = true;
    }

    public void ModifyId(int new_id){
        id = new_id;
        AdjustPlayerPosition();
        SetColor();
    }

    void AdjustPlayerPosition()
    {
        int playerPosX;

        if (id % 2 == 0)
            playerPosX = -275;
        else
            playerPosX = 250;

        rectT.anchoredPosition = new Vector2(playerPosX, -(int)(id/2) * GameManager.Instance.ScreenPlayers.menuPlayerDistanceY);
    }

    void RemovePlayer()
    {
        GameManager.Instance.ScreenPlayers.RemovePlayerFromList(id);

        Destroy(rectT.gameObject);
    }

    void SetColor(){
        img_playerColor.color = GameManager.Instance.list_color[id];
    }
}
