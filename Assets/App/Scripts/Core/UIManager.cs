using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace App.Core
{
    public class UIManager : BaseManager<UIManager>
    {

        public enum EnumScreen { Menu, ScreenPlayers, Game, NewBoard, ChallengeStatus, GameSettings }

        [SerializeField] private RectTransform menu;
        [SerializeField] private RectTransform screenPlayers;
        [SerializeField] private RectTransform game;
        [SerializeField] private RectTransform newBoard;
        [SerializeField] private RectTransform challengeStatus;
        [SerializeField] private RectTransform gameSettings;

        private void Awake()
        {
            StartAllScreens();
            ChangeScreen(EnumScreen.Menu);
        }

        void StartAllScreens(){
            menu.gameObject.SetActive(true);
            screenPlayers.gameObject.SetActive(true);
            game.gameObject.SetActive(true);
            newBoard.gameObject.SetActive(true);
            challengeStatus.gameObject.SetActive(true);
            gameSettings.gameObject.SetActive(true);
        }

        public void ChangeScreen(EnumScreen newScreen)
        {
            menu.gameObject.SetActive(false);
            screenPlayers.gameObject.SetActive(false);
            game.gameObject.SetActive(false);
            newBoard.gameObject.SetActive(false);
            challengeStatus.gameObject.SetActive(false);
            gameSettings.gameObject.SetActive(false);

            switch (newScreen)
            {
                case EnumScreen.Menu:
                    menu.gameObject.SetActive(true);
                    break;
                case EnumScreen.ScreenPlayers:
                    screenPlayers.gameObject.SetActive(true);
                    break;
                case EnumScreen.Game:
                    game.gameObject.SetActive(true);
                    break;
                case EnumScreen.NewBoard:
                    newBoard.gameObject.SetActive(true);
                    break;
                case EnumScreen.ChallengeStatus:
                    challengeStatus.gameObject.SetActive(true);
                    break;
                case EnumScreen.GameSettings:
                    gameSettings.gameObject.SetActive(true);
                    break;
            }
        }
    }
}