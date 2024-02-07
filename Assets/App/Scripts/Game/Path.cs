using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using App.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Path : MonoBehaviour
{
    [SerializeField] bool isZoom;
    private RectTransform rectT;
    private int id;
    public string desc;
    string startDesc;

    [SerializeField] TextMeshProUGUI txt;
    [SerializeField] Button btn;
    [SerializeField] Image blocker;
    bool secret;
    public bool walked;
    

    public void SetItems(string new_desc, int new_id = -1, bool new_secret = false){
        if(txt == null) txt = GetComponentInChildren<TextMeshProUGUI>();

        rectT = GetComponent<RectTransform>();

        id = new_id+1;
        startDesc = new_desc;
        desc = startDesc;
        
        SetSecret(new_secret);

        txt.text = desc;

        if(isZoom) return;

        if (id % 2 == 1) btn.image.color = new Color(1,1,1,0.65f);

        btn.onClick.AddListener(ZoomPath);

        AdjustPosition();
    }

    public void ModifyRandomPlayerText(string nickname){
        string newText;
        string selectedPlayer = SelectRandomPlayer();

        if (GameManager.Instance.list_playerItem.Count > 1) {
            while (selectedPlayer == nickname){
                selectedPlayer = SelectRandomPlayer();
            }
        }

        newText = startDesc.Replace("{player}", selectedPlayer);
        desc = newText;
        txt.text = desc;
    }

    string SelectRandomPlayer(){
        var randomPlayer = Random.Range(0, GameManager.Instance.list_playerItem.Count);
        string text = GameManager.Instance.list_playerItem[randomPlayer].nickname;
        return text;
    }

    public void ModifySortText(){
        string newText;
        string pattern = @"\((\d+)\-(\d+)\)";
        string value1 = "";
        string value2 = "";

        Match match = Regex.Match(desc, pattern);

        if (!match.Success) return;

        value1 = match.Groups[1].Value;
        value2 = match.Groups[2].Value;

        int randomDice = Random.Range(int.Parse(value1), int.Parse(value2)+1);

        newText = desc.Replace("{sort("+value1+"-"+value2+")}", randomDice.ToString());
        desc = newText;
        txt.text = desc;
    }

    public void ModifyRandomColorText(){
        string newText;
        string[] listColor = {"Preto", "Branco", "Vermelho", "Azul", "Amarelo", "Verde", "Laranja", "Roxo"};
        int sortColor = Random.Range(0, listColor.Length);

        newText = desc.Replace("{color}", listColor[sortColor]);
        desc = newText;
        txt.text = desc;
    }

    public void SetSecret(bool value){
        secret = value;
        if(secret) return;

        if(blocker != null) Destroy(blocker.gameObject);
        secret = false;
    }

    void AdjustPosition(){
        float width = rectT.rect.width;
        float height = rectT.rect.height;

        float posY = -175;
        float posX = -width-125;


        // CRIACAO DA TRILHA
        int aux = 1;
        for (int i = 0; i < id; i++){
            aux++;
            
            if(aux == 5 || aux == 6 || aux == 10 || aux == 11)
                posY -= height;
            if(aux == 6 || aux == 10 || aux == 11)
                posX += width;
            if(aux < 5)
                posX += width;
            if(aux > 5)
                posX -= width;
            if (aux == 11)
                aux = 1;
        }

        rectT.anchoredPosition = new Vector2(posX, posY);
    }

    public void SetZoomItems(string new_desc){
        SetItems(new_desc);
    }

    void ZoomPath(){
        if(secret) return;
        GameManager.Instance.Notification.ZoomPathNotification(desc);
    }
}
