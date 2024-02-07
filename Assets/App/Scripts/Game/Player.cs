using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Player : MonoBehaviour
{
    private RectTransform rectT;
    private Image img;
    private int id;
    private string _nickname;
    public string Nickname { get => _nickname; }
    private int _points;
    public int Points { get => _points; }

    private Color32 _color;
    public Color32 Color { get => _color; }

    public void SetItems(int new_id, string new_nickname, Color32 new_color){
        rectT = GetComponent<RectTransform>();
        img = GetComponentInChildren<Image>();

        id = new_id;
        _nickname = new_nickname;
        _color = new_color;

        img.color = _color;

        AdjustPosition();
    }

    void AdjustPosition(){
        float width = 250;
        float height = 250;

        float posY = -10;
        float auxPosX = id * rectT.rect.width;
        float posX = -width-235+auxPosX;


        // CRIACAO DA TRILHA
        int aux = 1;
        for (int i = 0; i < _points; i++){
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
        
        rectT.DOAnchorPos(new Vector2(posX, posY), .5f);
    }

    public void AddPoints(int result){
        _points += result;
        if(_points < 0) _points = 0;
        
        AdjustPosition();
    }

    public void SetPoints(int result){
        _points = result;
        
        AdjustPosition();
    }
}
