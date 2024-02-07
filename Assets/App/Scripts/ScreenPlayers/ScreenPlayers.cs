using System.Collections;
using System.Collections.Generic;
using App.Core;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPlayers : MonoBehaviour
{
    [SerializeField] private Button btn_back;
    [SerializeField] private RectTransform point_menuPlayer;
    [SerializeField] private RectTransform prefab_menuPlayer;
    [SerializeField]private List<MenuPlayer> list_menuPlayer;
    // [SerializeField]private List<GameManager.PlayerItem> list_playerItem;
    [SerializeField] private Button btn_play;

    public int menuPlayerDistanceY = 200;

    const string NEW_PLAYER_PLAYERS = "NEW_PLAYER_PLAYERS";

    void Start(){
        btn_back.onClick.AddListener(() => UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Menu));
        btn_play.onClick.AddListener(StartGame);
        
        RequestData();
    }

    void StartGame(){
        SaveData();

        GameManager.Instance.Boardgame.StartGame();
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Game);
    }

    void RequestData()
    {
        if (PlayerPrefs.HasKey(NEW_PLAYER_PLAYERS))
        {
            GameManager.PlayerItem[] arr_menuPlayerItems;
            arr_menuPlayerItems = JsonHelper.FromJson<GameManager.PlayerItem>(PlayerPrefs.GetString(NEW_PLAYER_PLAYERS));
            AddNewPlayers(arr_menuPlayerItems);
        }
        else
            DefaultPlayerList();

    }

    void DefaultPlayerList()
    {
        AddNewPlayer();
        list_menuPlayer[0].OpenPlayer();
    }

    public void AddNewPlayer(string name = null)
    {
        if(list_menuPlayer.Count >= GameManager.Instance.MaxPlayers) return;

        var gO_menuPlayer = Instantiate(prefab_menuPlayer, point_menuPlayer.transform.position, point_menuPlayer.transform.rotation, point_menuPlayer);
        var menuPlayer = gO_menuPlayer.GetComponent<MenuPlayer>();

        list_menuPlayer.Add(menuPlayer);

        menuPlayer.ModifyId(list_menuPlayer.Count - 1);

        if (name != null) menuPlayer.SetItems(name);
    }

    public void AddNewPlayers(GameManager.PlayerItem[] arr_item)
    {
        for (int i = 0; i < arr_item.Length; i++)
            AddNewPlayer(arr_item[i].nickname);

        AddNewPlayer();
    }

    public void RemovePlayerFromList(int id)
    {
            
        list_menuPlayer.RemoveAt(id);

        if(list_menuPlayer.Count + 1 == GameManager.Instance.MaxPlayers
        && list_menuPlayer[list_menuPlayer.Count - 1].Live)
            AddNewPlayer();

        ModifyPlayerVariableValues();

    }

    void ModifyPlayerVariableValues()
    {
        for (int i = 0; i < list_menuPlayer.Count; i++)
        {
            list_menuPlayer[i].ModifyId(i);
        }
    }

    void SaveData()
    {
        int length;
        length = list_menuPlayer.Count - 1;
        if(list_menuPlayer.Count == GameManager.Instance.MaxPlayers) 
            length += 1; //Correção para quando tiver todos players

        List<GameManager.PlayerItem> items = new List<GameManager.PlayerItem>();
        for (int i = 0; i < length; i++)
        {
            string name = list_menuPlayer[i].inputF_name.text;
            Color32 color = GameManager.Instance.list_color[i];

            items.Add(new GameManager.PlayerItem(name, color));
        }
        // list_playerItem = items;
        GameManager.Instance.list_playerItem = items;

        string json_players = JsonHelper.ToJson<GameManager.PlayerItem>(items);
        PlayerPrefs.SetString(NEW_PLAYER_PLAYERS, json_players);
    }
}
