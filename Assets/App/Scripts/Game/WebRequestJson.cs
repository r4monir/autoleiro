using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestJson : MonoBehaviour
{
    [SerializeField] private string[] Games;

    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;
        for (int i = 0; i < Games.Length; i++){
            StartCoroutine(RequestJSON(Games[i]));
        }
    }

    IEnumerator RequestJSON(string game){
        string json;
        UnityWebRequest request = UnityWebRequest.Get($"https://r4monir.github.io/autoleiro_games/{game}.json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) {
            Debug.Log(request.error);
            json = null;
        }
        else {
            json = request.downloadHandler.text;
            SaveStringAsJSON(json, game);
        }
    }

    private void SaveStringAsJSON(string json, string game)
    {
        string filePath = Application.dataPath + "/App/Resources/Json/"+game+".json";
        File.WriteAllText(filePath, json);
    }
}
