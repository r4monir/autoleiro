using System.Collections;
using System.Collections.Generic;
using App.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Hud : MonoBehaviour
{
    [SerializeField] private Button btn_rollCoin;
    [SerializeField] private Button btn_pass;

    [Header("Dice")]
    [SerializeField] private Button btn_rollDice;
    [SerializeField] private Image img_dice;
    private Sprite[] sprite_diceSides;

    [Header("Coin")]
    [SerializeField] private Button btn_coin;
    [SerializeField] private Image img_coin;
    private Sprite[] sprite_coinSides;

    [Header("Turn")]
    [SerializeField] private TextMeshProUGUI txt_nickname;
    [SerializeField] private Image img_player;

    [Header("Finish Game")]
    [SerializeField] private Button btn_finish;
    [SerializeField] private GameObject popup_finish;
    [SerializeField] private Button btn_shadow_finish;
    [SerializeField] private Button btn_finish_yes;
    [SerializeField] private Button btn_finish_no;

    void Awake(){
        sprite_diceSides = Resources.LoadAll<Sprite>("Sprites/Dice/");
        btn_rollDice.onClick.AddListener(RollDice);

        sprite_coinSides = Resources.LoadAll<Sprite>("Sprites/Coin/");
        btn_coin.onClick.AddListener(RollCoin);

        btn_pass.onClick.AddListener(PassTurn);

        //Finish Game
        btn_finish.onClick.AddListener(delegate { OpenFinishPopup(false); });
        btn_finish_yes.onClick.AddListener(FinishGame);
    }

    private void Start() {
        img_player.rectTransform.parent.DOPunchScale(new Vector3(.1f, .1f, .1f), .5f, 1).SetDelay(10)
            .OnComplete(() => img_player.rectTransform.parent.DORestart());
    }

    private void RollDice(){
        StartCoroutine(RollingDice());
    }

    private IEnumerator RollingDice()
    {
        ButtonIsInteractive(false);

        int randomDiceSide = 0;
        int result = 0;
        int maxDice = GameManager.Instance.GameSettings.MaxDice;

        for (int i = 0; i <= maxDice * 4; i++)
        {
            randomDiceSide = Random.Range(0, maxDice);

            img_dice.sprite = sprite_diceSides[randomDiceSide];

            yield return new WaitForSeconds(0.05f);
        }

        result = randomDiceSide + 1;

        img_dice.rectTransform.DOPunchScale(new Vector3(.2f, .2f, .2f), .5f, 2);

        GameManager.Instance.Boardgame.DiceThrown(result);
    }

    private void RollCoin(){
        StartCoroutine(RollingCoin());
    }

    private IEnumerator RollingCoin()
    {
        btn_rollCoin.interactable = false;
        
        img_coin.gameObject.SetActive(true);

        int randomCoinSide = 0;

        for (int i = 0; i <= 9; i++)
        {
            randomCoinSide = Random.Range(0, sprite_coinSides.Length);

            img_coin.rectTransform.DOScale(new Vector3(1, img_coin.rectTransform.localScale.y * -1, 1), 0.20f);

            yield return new WaitForSeconds(0.20f);

            img_coin.sprite = sprite_coinSides[randomCoinSide];
        }
        
        img_coin.rectTransform.DOPunchScale(new Vector3(.5f, .5f, .5f), .5f, 2);

        yield return new WaitForSeconds(2.5f);

        img_coin.gameObject.SetActive(false);

        btn_rollCoin.interactable = true;
    }

    public void ButtonIsInteractive(bool value){
        btn_rollDice.interactable = value;
        btn_finish.interactable = value;
        btn_pass.interactable = value;
    }

    public void NewTurn(string nickname, Color32 color){
        txt_nickname.text = nickname;
        img_player.color = color;
    }

    void PassTurn(){
        StartCoroutine(CoroutinePassTurn());
    }

    IEnumerator CoroutinePassTurn(){
        ButtonIsInteractive(false);

        GameManager.Instance.Boardgame.DiceThrown(0);

        yield return new WaitForSeconds(1f);

        ButtonIsInteractive(true);
    }

    public void OpenFinishPopup(bool isEnd = false){
        popup_finish.SetActive(true);

        if(isEnd){
            btn_shadow_finish.onClick.AddListener(FinishGame);
            btn_finish_no.onClick.AddListener(FinishGame);
        } else {
            btn_shadow_finish.onClick.AddListener(CloseFinishPopup);
            btn_finish_no.onClick.AddListener(CloseFinishPopup);
        }
    }

    void CloseFinishPopup(){
        popup_finish.SetActive(false);

        btn_shadow_finish.onClick.RemoveAllListeners();
        btn_finish_no.onClick.RemoveAllListeners();
    }

    void FinishGame(){
        CloseFinishPopup();

        img_dice.sprite = sprite_diceSides[0];

        GameManager.Instance.Boardgame.FinishGame();
    }
}
