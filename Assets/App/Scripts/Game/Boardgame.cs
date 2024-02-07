using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using App.Core;

public class Boardgame : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private ScrollRect scroll_paths;
    private RectTransform rectT_content;
    private float rectT_content_startPosY;

    [System.Serializable]
    public class PathItem
    {
        public string desc;
        public int lvl;
        public int repeats;
        public bool secret;

        public PathItem(string new_desc, int new_lvl, int new_repeats, bool new_secret)
        {
            desc = new_desc;
            lvl = new_lvl;
            repeats = new_repeats;
            secret = new_secret;
        }
    }

    [Header("Path")]
    [SerializeField] private RectTransform prefab_path;
    [SerializeField] private List<PathItem> list_pathItem;
    [SerializeField] private List<Path> list_path;

    [Header("Players")]
    [SerializeField] private RectTransform prefab_player;
    [SerializeField] private List<GameManager.PlayerItem> list_playerItem;
    [SerializeField] private List<Player> list_player;

    [Header("Game")]
    int currentPlayer;
    int lastPlayer;

    void Start()
    {
        rectT_content = scroll_paths.content;
        rectT_content_startPosY = scroll_paths.content.sizeDelta.y;

        prefab_path = (Resources.Load("Prefabs/Path") as GameObject).GetComponent<RectTransform>();
        prefab_player = (Resources.Load("Prefabs/Player") as GameObject).GetComponent<RectTransform>();
    }

    public void StartGame()
    {
        list_playerItem = GameManager.Instance.list_playerItem;

        string json;

        List<PathItem> list_pathItems = new List<PathItem>();
        List<PathItem> list_extraPathItems = new List<PathItem>();
        List<GameManager.NextGames> list_nextGames = GameManager.Instance.list_nextGames;
        for (int i = 0; i < list_nextGames.Count; i++)
        {
            if (list_nextGames[i].isDefault)
            {
                json = (Resources.Load("Json/" + list_nextGames[i].game) as Object).ToString();
                list_extraPathItems = JsonHelper.FromJsonToList<PathItem>(json);
            }
            else
            {
                json = list_nextGames[i].game;
                List<NewBoard.Challenge> list_challenge = JsonHelper.FromJsonToList<NewBoard.Challenge>(PlayerPrefs.GetString(json));
                for (int i2 = 0; i2 < list_challenge.Count; i2++)
                {
                    list_extraPathItems.Add(new PathItem(list_challenge[i2].desc, list_challenge[i2].lvl, list_challenge[i2].repeats, list_challenge[i2].secret));
                }
            }
            list_pathItems.AddRange(list_extraPathItems);
        }

        CreatePaths(list_pathItems);

        CreatePlayers();

        GameManager.Instance.Hud.NewTurn(list_player[currentPlayer].Nickname, list_player[currentPlayer].Color);
        GameManager.Instance.Notification.ShowPopup(list_player[currentPlayer].Nickname, Notification.EnumPopupMsg.NewTurn, list_player[currentPlayer].Color);
        GameManager.Instance.ResetNextGames();
        GameManager.Instance.Menu.DeselectAllGames();

    }

    void CreatePaths(List<PathItem> list_path)
    {
        CreatePathList(list_path);

        AdjustScrollPosition(list_pathItem.Count);

        for (int i = 0; i < list_pathItem.Count; i++)
            GeneratePath(i, list_pathItem[i].desc, list_pathItem[i].secret);

        GeneratePath(list_pathItem.Count, "CHEGADA");
    }

    void CreatePathList(List<PathItem> list_path)
    {
        for (int i = 1; i < GameManager.Instance.maxLvl + 1; i++)
        {
            List<PathItem> list_pathItems = new List<PathItem>();
            for (int i2 = 0; i2 < list_path.Count; i2++)
            {
                if (list_path[i2].lvl == i)
                {
                    for (int i3 = 0; i3 < list_path[i2].repeats; i3++)
                    {
                        list_pathItems.Add(list_path[i2]);
                    }
                }
            }
            list_pathItems.Shuffle();
            list_pathItem.AddRange(list_pathItems);
        }

        if (GameManager.Instance.GameSettings.SkipLevels) list_pathItem.Shuffle();
    }

    void AdjustScrollPosition(int length)
    {
        int aux = length / 5;
        var basePosY = 500 * (aux - 1);
        var finalPosY = rectT_content_startPosY + basePosY;
        scroll_paths.content.sizeDelta = new Vector2(0, finalPosY);
        scroll_paths.content.anchoredPosition = new Vector2(0, -basePosY / 2);
    }

    void GeneratePath(int id, string desc, bool secret = false)
    {
        var gO_path = Instantiate(prefab_path, rectT_content.transform.position, rectT_content.transform.rotation, rectT_content);
        var path = gO_path.GetComponent<Path>();

        path.SetItems(desc, id, secret);
        list_path.Add(path);
    }

    void CreatePlayers()
    {
        list_playerItem.Shuffle();
        for (int i = 0; i < list_playerItem.Count; i++)
        {
            GeneratePlayer(i, list_playerItem[i].nickname, list_playerItem[i].color);
        }

    }

    void GeneratePlayer(int id, string nickname, Color32 color)
    {
        var gO_player = Instantiate(prefab_player, rectT_content.transform.position, rectT_content.transform.rotation, rectT_content);
        var player = gO_player.GetComponent<Player>();

        player.SetItems(id, nickname, color);

        list_player.Add(player);
    }

    public void DiceThrown(int result)
    {
        StartCoroutine(CoroutineDiceThrown(result));
    }

    IEnumerator CoroutineDiceThrown(int result)
    {
        bool passTurn = result == 0; // Caso result = 0 passTurn = true;
        int finalResult = list_player[currentPlayer].Points + result;
        int pathLength = list_pathItem.Count + 1;

        switch (passTurn)
        {
            case false:
                if (finalResult < pathLength)
                {
                    list_player[currentPlayer].AddPoints(result);
                }
                else
                {
                    PlayerWin(pathLength);
                    yield break;
                }

                yield return new WaitForSeconds(1);

                switch (GameManager.Instance.GameSettings.NotRepeatPath)
                {
                    case true:
                        if (list_path[finalResult - 1].walked)
                        {
                            StartCoroutine(CoroutineDiceThrown(1));
                            yield break;
                        }
                        break;
                }

                list_path[finalResult - 1].ModifyRandomPlayerText(list_player[currentPlayer].Nickname);
                list_path[finalResult - 1].ModifySortText();
                list_path[finalResult - 1].ModifyRandomColorText();
                list_path[finalResult - 1].SetSecret(false);
                list_path[finalResult - 1].walked = true;
                GameManager.Instance.Notification.ZoomPathNotification(list_path[finalResult - 1].desc, list_player[currentPlayer].Nickname);
                break;
        }

        ChangePlayer();

        GameManager.Instance.Hud.NewTurn(list_player[currentPlayer].Nickname, list_player[currentPlayer].Color);
        GameManager.Instance.Notification.ShowPopup(list_player[currentPlayer].Nickname, Notification.EnumPopupMsg.NewTurn, list_player[currentPlayer].Color);

        GameManager.Instance.Hud.ButtonIsInteractive(true);
    }

    void PlayerWin(int pathLength)
    {
        list_player[currentPlayer].SetPoints(pathLength);
        GameManager.Instance.Notification.ShowPopup(list_player[currentPlayer].Nickname, Notification.EnumPopupMsg.Win);

        // Continue Game
        list_player.RemoveAt(currentPlayer);

        ChangePlayer();

        if (list_player.Count == 0)
        { // No possible continue
            GameManager.Instance.Hud.OpenFinishPopup(true);
        }
        else
        {
            GameManager.Instance.Hud.OpenFinishPopup();
            GameManager.Instance.Hud.NewTurn(list_player[currentPlayer].Nickname, list_player[currentPlayer].Color);
        }
    }

    void ChangePlayer()
    {
        lastPlayer = currentPlayer;
        if (currentPlayer < list_player.Count - 1)
            currentPlayer++;
        else
            currentPlayer = 0;
    }

    public void AddPointsToLastPlayer(int value)
    {
        list_player[lastPlayer].AddPoints(value);
    }

    public void FinishGame()
    {
        ResetGame();

        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Menu);
    }

    void ResetGame()
    {
        for (int i = 1; i < rectT_content.childCount; i++)
        {
            Destroy(rectT_content.GetChild(i).gameObject);
        }
        for (int i = 0; i < list_player.Count; i++)
        {
            list_player[i].SetPoints(0);
        }

        list_pathItem.Clear();
        list_path.Clear();
        list_playerItem.Clear();
        list_player.Clear();
    }
}
