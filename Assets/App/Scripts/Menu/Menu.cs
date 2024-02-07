using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using App.Core;

public class Menu : MonoBehaviour
{
    [System.Serializable]
    public class BoardItem {
        [HideInInspector] public Board board;
        [HideInInspector] public int id;
        [TextArea] public string boardName;
        public string json;
        public Color color;
        public Sprite icon;
    }
    
    [SerializeField] private ScrollRect scroll_boards;
    private RectTransform rectT_content;
    private Vector2 rectT_contentPos;
    [SerializeField] private RectTransform prefab_board;
    [SerializeField] private List<BoardItem> list_defaultBoardItem;
    [SerializeField]private List<BoardItem> list_boardItem;
    private int _defaultListAmount;
    public int DefaultListAmount { get => _defaultListAmount; }

    const string NEW_BOARDS = "NEW_BOARDS";

    private int _selectCounter;
    public int SelectCounter { get => _selectCounter; }

    [Header("Base")]
    [SerializeField] private Button btn_gear;
    [SerializeField] private Button btn_finish;

    void Start(){
        btn_gear.onClick.AddListener(ButtonGear);
        btn_finish.onClick.AddListener(ButtonFinish);
        
        rectT_content = scroll_boards.content;
        rectT_contentPos = rectT_content.sizeDelta;
        _defaultListAmount = list_defaultBoardItem.Count;

        CreateBoards();
    }

    void CreateBoards(){
        for (int i = 0; i < list_defaultBoardItem.Count; i++){
            list_defaultBoardItem[i].id = i;
            GenerateBoard(i, false, list_defaultBoardItem[i].boardName, list_defaultBoardItem[i].json, 
                true, list_defaultBoardItem[i].color, list_defaultBoardItem[i].icon);
        }
        
        RequestData();

        GenerateDefaultEmptyBoard();
    }

    void RequestData(){
        if (PlayerPrefs.HasKey(NEW_BOARDS)){
            list_boardItem = JsonHelper.FromJsonToList<BoardItem>(PlayerPrefs.GetString(NEW_BOARDS));
            for (int i = 0; i < list_boardItem.Count - 1; i++){
                list_boardItem[i].id = i;
                GenerateBoard(i, false, list_boardItem[i].boardName, list_boardItem[i].json);
            }
            list_boardItem.RemoveAt(list_boardItem.Count - 1);
        }
    }

    void GenerateDefaultEmptyBoard(){
        list_boardItem.Add(new BoardItem());
        GenerateBoard(list_boardItem.Count - 1);
    }

    public void GenerateBoard(int id, bool empty = true, string boardName = null, string json = null, 
        bool isDefaultList = false, Color color = default(Color), Sprite icon = null){

        RectTransform gO_board = Instantiate(prefab_board, rectT_content.transform.position, rectT_content.transform.rotation, rectT_content);
        var board = gO_board.GetComponent<Board>();

        if(isDefaultList) {
            list_defaultBoardItem[id].board = board;
            list_defaultBoardItem[id].board.SetDefault(color, icon);
        }
        else list_boardItem[id].board = board;

        board.SetItems(id, empty, boardName, json);

        AdjustScrollPosition();
    }

    
    void AdjustScrollPosition(){
        var extraSpace = 300 + 65;
        var basePosY = extraSpace * (list_defaultBoardItem.Count + list_boardItem.Count);
        var finalPosY =  rectT_contentPos.y + extraSpace + basePosY;
        scroll_boards.content.sizeDelta = new Vector2(0, finalPosY);
    }

    public void UpdateBoard(int id, string boardName, string json){
        bool isNewBoard = false;

        list_boardItem[id].id = id;
        list_boardItem[id].boardName = boardName;
        list_boardItem[id].board.SetItems(id, false, boardName, json);
        list_boardItem[id].json = json;

        if(list_boardItem[id].id == list_boardItem.Count - 1) isNewBoard = true;
        if(isNewBoard) GenerateDefaultEmptyBoard();

        SaveBoards();
    }

    void SaveBoards(){
        string json_boards = JsonHelper.ToJson<BoardItem>(list_boardItem);
        PlayerPrefs.SetString(NEW_BOARDS, json_boards);
    }

    
    public void RemoveBoard(int id){
        list_boardItem[id].board.RemoveBoard();
        PlayerPrefs.DeleteKey(list_boardItem[id].json);
        list_boardItem.RemoveAt(id);
        SaveBoards();

        ModifyBoardsId();
        AdjustScrollPosition();
    }

    void ModifyBoardsId(){
        for (int i = 0; i < list_boardItem.Count; i++){
            list_boardItem[i].board.ModifyId(i);
        }
    }

    void ButtonGear(){
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.GameSettings);
    }

    public void AddSelect(int add){
        _selectCounter += add;
    }

    public void CheckMultiGames(){
        if(_selectCounter > 0) btn_finish.gameObject.SetActive(true);
        else btn_finish.gameObject.SetActive(false);
    }

    public void DeselectAllGames(){
        for (int i = 0; i < list_defaultBoardItem.Count; i++)
        {
            if(list_defaultBoardItem[i].board.IsSelect) 
                list_defaultBoardItem[i].board.MultiSelectBoard();
        }
        for (int i = 0; i < list_boardItem.Count; i++)
        {
            if(list_boardItem[i].board.IsSelect) 
                list_boardItem[i].board.MultiSelectBoard();
        }

        _selectCounter = 0;
    }

    void ButtonFinish(){
        for (int i = 0; i < list_defaultBoardItem.Count; i++)
        {
            if(list_defaultBoardItem[i].board.IsSelect)
                GameManager.Instance.AddNextGame(list_defaultBoardItem[i].json, list_defaultBoardItem[i].board.IsDefault);
        }
        for (int i = 0; i < list_boardItem.Count; i++)
        {
            if(list_boardItem[i].board.IsSelect)
                GameManager.Instance.AddNextGame(list_boardItem[i].json, list_boardItem[i].board.IsDefault);
        }
        // GameManager.Instance.NextGameJson(json, isDefault);
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.ScreenPlayers);
    }
}
