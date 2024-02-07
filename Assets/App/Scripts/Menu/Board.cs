using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using App.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    private RectTransform rectT;
    TextMeshProUGUI txt;
    [SerializeField] private Button btn_game;
    [SerializeField] private Button btn_select; // Botão transparente que cobre o botão
    [SerializeField] private Image img_select;
    [SerializeField] private int id;
    [SerializeField] private string boardName;
    [SerializeField] private string json;
    [SerializeField] private Image img_icon;
    [SerializeField] private Button btn_add;
    [SerializeField] private Button btn_edit;
    
    private bool _isDefault;
    public bool IsDefault { get => _isDefault; }

    private bool _isSelect;
    public bool IsSelect { get => _isSelect; set => _isSelect = value; }


    void Awake(){
        rectT = GetComponent<RectTransform>();
        txt = GetComponentInChildren<TextMeshProUGUI>();

        btn_game.onClick.AddListener(SelectBoard);
        btn_select.onClick = btn_game.onClick;
        btn_add.onClick.AddListener(AddNewBoard);
        btn_edit.onClick.AddListener(EditBoard);

        btn_game.GetComponent<OnHoldButton>().OnHold(OnHoldSelectBoard);
    }

    public void SetDefault(Color color, Sprite icon){
        _isDefault = true;
        Destroy(btn_edit.gameObject);

        img_icon.gameObject.SetActive(true);
        btn_game.image.color = color;
        img_icon.sprite = icon;
    }

    public void SetItems(int new_id, bool empty, string new_boardName = null, string new_json = null){
        ModifyId(new_id);

        if(empty) return;

        boardName = new_boardName;
        json = new_json;

        txt.text = boardName;

        ActivePlayer();
    }

    void AddNewBoard(){
        GameManager.Instance.NewBoard.SetItems(id);
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.NewBoard);
    }

    void EditBoard(){
        GameManager.Instance.NewBoard.SetItems(id, boardName);
        GameManager.Instance.NewBoard.RequestBoardData(json);
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.NewBoard);
    }

    void ActivePlayer(){
        if(btn_add)
            Destroy(btn_add.gameObject);
    }

    public void AdjustPosition(){
        float height = rectT.rect.height;
        float space = 0;

        float posY = -space;
        float posX = 0;

        if(_isDefault) posY = -((height+space) * id) - space;
        else posY = -((height+space) * (id + GameManager.Instance.Menu.DefaultListAmount)) - space;

        rectT.anchoredPosition = new Vector2(posX, posY);
    }

    void SelectBoard(){
        if(GameManager.Instance.Menu.SelectCounter > 0) {
            MultiSelectBoard();
            return;
        }

        GameManager.Instance.AddNextGame(json, _isDefault);
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.ScreenPlayers);
    }

    void OnHoldSelectBoard(){
        MultiSelectBoard();
    }

    public void MultiSelectBoard(){
        if(!_isSelect) GameManager.Instance.Menu.AddSelect(1);
        else GameManager.Instance.Menu.AddSelect(-1);

        _isSelect = !_isSelect;

        img_select.enabled = _isSelect;
        btn_select.gameObject.SetActive(_isSelect);

        GameManager.Instance.Menu.CheckMultiGames();
    }

    public void ModifyId(int new_id){
        id = new_id;
        AdjustPosition();
    }

    public void RemoveBoard(){
        Destroy(rectT.gameObject);
    }
}
