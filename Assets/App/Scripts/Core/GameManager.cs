using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace App.Core {
    public class GameManager : BaseManager<GameManager> {

        [Header("Helper")]
        public bool playerPrefsDeleteAll;

        [System.Serializable]
        public class PlayerItem {

            public string nickname;
            public Color32 color;

            public PlayerItem(string new_nickname, Color32 new_color) {
                nickname = new_nickname;
                color = new_color;
            }
        }

        [System.Serializable]
        public class NextGames {

            public string game;
            public bool isDefault;

            public NextGames(string new_game, bool new_isDefault) {
                game = new_game;
                isDefault = new_isDefault;
            }
        }
        
        private Menu _menu;
        public Menu Menu { get => _menu; }
        private GameSettings _gameSettings;
        public GameSettings GameSettings { get => _gameSettings; }

        private ScreenPlayers _screenPlayers;
        public ScreenPlayers ScreenPlayers { get => _screenPlayers; }

        private NewBoard _newBoard;
        public NewBoard NewBoard { get => _newBoard; }
        private ChallengeStatus _challengeStatus;
        public ChallengeStatus ChallengeStatus { get => _challengeStatus; }

        private Boardgame _boardgame;
        public Boardgame Boardgame { get => _boardgame; }

        private Hud _hud;
        public Hud Hud { get => _hud; }
        private Notification _notification;
        public Notification Notification { get => _notification; }

        public List<Color32> list_color;
        [HideInInspector] public List<PlayerItem> list_playerItem;

        private int _maxPlayers;
        public int MaxPlayers { get => _maxPlayers; }

        [SerializeField]private List<NextGames> _list_nextGames;
        public List<NextGames> list_nextGames { get => _list_nextGames; }

        [HideInInspector] public int maxLvl;

        private void Awake () {
            if(playerPrefsDeleteAll) PlayerPrefs.DeleteAll();

            this._menu = FindObjectOfType<Menu>();
            this._gameSettings = FindObjectOfType<GameSettings>();
            this._screenPlayers = FindObjectOfType<ScreenPlayers>();
            this._newBoard = FindObjectOfType<NewBoard>();
            this._challengeStatus = FindObjectOfType<ChallengeStatus>();

            this._boardgame = FindObjectOfType<Boardgame>();

            this._notification = FindObjectOfType<Notification>();
            this._hud = FindObjectOfType<Hud>();

            _maxPlayers = list_color.Count;
        }

        public void AddNextGame(string game, bool isDefault = false){
            _list_nextGames.Add(new NextGames(game, isDefault));
        }

        public void ResetNextGames(){
            _list_nextGames.Clear();
        }

        public string GenerateRandomKey(){
            string characters = "abcdefghijklmnopkrstuvxwyz0123456789";
            int keyLength = 10;
            string key = null;
            for (int i = 0; i < keyLength; i++)
                key += characters[UnityEngine.Random.Range(0, characters.Length)];
            return key;
        }
    }
}