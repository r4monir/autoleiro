using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using App.Core;

public class NewBoard : MonoBehaviour
{
    [System.Serializable]
    public class Challenge {
        public int id;
        public RectTransform recT;
        public TextMeshProUGUI txt;
        public Button btn;
        public string desc;
        public int lvl;
        public int repeats;
        public bool secret;
    }

    private int currentId;
    private string currentKey;
    [SerializeField] private TMP_InputField inputF_name;
    [SerializeField] private Button btn_back;
    [SerializeField] private Button btn_addChallenge;
    [SerializeField] private Button btn_finish;

    [Header("Challenge")]
    [SerializeField] private ScrollRect scroll_challenge;
    private RectTransform rectT_content;
    private Vector2 rectT_contentPos;
    [SerializeField] private RectTransform prefab_challenge;
    [SerializeField]private List<Challenge> list_challenge;

    [Header("Popup Delet")]
    [SerializeField] private Button btn_delet;
    [SerializeField] private GameObject popup_delet;
    [SerializeField] private Button btn_shadow_delet;
    [SerializeField] private Button btn_delet_yes;
    [SerializeField] private Button btn_delet_no;


    void Awake(){
        btn_back.onClick.AddListener(ButtonFinish);
        btn_addChallenge.onClick.AddListener(AddChallenge);
        btn_finish.onClick.AddListener(ButtonFinish);

        btn_delet.onClick.AddListener(OpenDeletPopup);
        btn_shadow_delet.onClick.AddListener(CloseDeletPopup);
        btn_delet_no.onClick.AddListener(CloseDeletPopup);
        btn_delet_yes.onClick.AddListener(DeletBoard);

        rectT_content = scroll_challenge.content;
        rectT_contentPos = rectT_content.sizeDelta;
    }

    public void RequestBoardData(string key){
        currentKey = key;
        if (PlayerPrefs.HasKey(key)){
            list_challenge = JsonHelper.FromJsonToList<Challenge>(PlayerPrefs.GetString(key));
            LoadBoard();
        }
    }

    public void SetItems(int id, string boardName = null){
        currentId = id;
        
        inputF_name.text = boardName;
    }

    void LoadBoard(){
        for (int i = 0; i < list_challenge.Count; i++){
            GenerateChallenge(i);
        }
    }

    void GenerateChallenge(int id){
        var gO_challenge = Instantiate(prefab_challenge, rectT_content.transform.position, rectT_content.transform.rotation, rectT_content);
        RectTransform recT = gO_challenge.GetComponent<RectTransform>();
        Button btn_challenge = gO_challenge.GetComponent<Button>();
        TextMeshProUGUI txt_challenge = gO_challenge.GetComponentInChildren<TextMeshProUGUI>();

        list_challenge[id].id = id;
        list_challenge[id].recT = recT;
        list_challenge[id].btn = btn_challenge;
        list_challenge[id].txt = txt_challenge;
        txt_challenge.text = list_challenge[id].desc;

        AdjustScrollPosition();

        SetButtonChallenge(id);
        AdjustChallengePosition(recT, id);
    }

    void ButtonChallenge(int id, string desc, int lvl, int repeats, bool secret){
        GameManager.Instance.ChallengeStatus.SetStatus(id, desc, lvl, repeats, secret);

        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.ChallengeStatus);
    }


    public void UpdateChallengeStatus(int id, string new_desc, int new_lvl, int new_repeats, bool new_secret){
        list_challenge[id].desc = new_desc;
        list_challenge[id].lvl = new_lvl;
        list_challenge[id].repeats = new_repeats;
        list_challenge[id].secret = new_secret;

        list_challenge[id].txt.text = new_desc;
    }

    void AdjustScrollPosition(){
        var basePosY = 150 * list_challenge.Count;
        var finalPosY =  rectT_contentPos.y + basePosY;
        scroll_challenge.content.sizeDelta = new Vector2(0, finalPosY);
    }
    
    void AddChallenge(){
        list_challenge.Add(new Challenge());
        int newId = list_challenge.Count - 1;
        GenerateChallenge(newId);
        GameManager.Instance.ChallengeStatus.NewChallenge(newId);

        ModifyChallengesId();

        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.ChallengeStatus);
    }

    void ButtonFinish(){
        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Menu);

        if (list_challenge.Count > 0) {
            SaveBoard();
        }

        ResetScreen();
    }

    void ResetScreen(){
        inputF_name.text = null;
        currentKey = null;

        for (int i = 0; i < rectT_content.childCount; i++) {
            Destroy(rectT_content.GetChild(i).gameObject);
        }

        list_challenge.Clear();
    }

    void AdjustChallengePosition(RectTransform rectT, int id){
        int posX = 0;
        int basePosY = -150;

        rectT.anchoredPosition = new Vector2(posX, basePosY * -(id - list_challenge.Count + 1));
    }

    public void RemoveChallengeFromList(int id){
        Destroy(list_challenge[id].recT.gameObject);
        list_challenge.RemoveAt(id);

        ModifyChallengesId();
        AdjustScrollPosition();
    }

    void ModifyChallengesId(){
        for (int i = 0; i < list_challenge.Count; i++){
            list_challenge[i].id = i;
            AdjustChallengePosition(list_challenge[i].recT, i);
            
            SetButtonChallenge(i);
        }
    }

    void SetButtonChallenge(int id){
        list_challenge[id].btn.onClick.RemoveAllListeners();
        list_challenge[id].btn.onClick.AddListener(() => ButtonChallenge 
            (id, list_challenge[id].desc, list_challenge[id].lvl, list_challenge[id].repeats, list_challenge[id].secret));
    }

    void SaveBoard(){
        var key = currentKey;

        if(key == "" || key == null) key = GameManager.Instance.GenerateRandomKey();

        string json_challenges = JsonHelper.ToJson<Challenge>(list_challenge);
        PlayerPrefs.SetString(key, json_challenges);

        string boardName = inputF_name.text;
        GameManager.Instance.Menu.UpdateBoard(currentId, boardName, key);
    }

    public void OpenDeletPopup(){
        popup_delet.SetActive(true);
    }

    void CloseDeletPopup(){
        popup_delet.SetActive(false);
    }
    
    void DeletBoard(){
        popup_delet.SetActive(false);
        
        if(currentKey != "" && currentKey != null) GameManager.Instance.Menu.RemoveBoard(currentId);

        UIManager.Instance.ChangeScreen(UIManager.EnumScreen.Menu);

        ResetScreen();
    }
}
