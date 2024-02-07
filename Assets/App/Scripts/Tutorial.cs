using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<GameObject> list_tutorial;
    [SerializeField] private string tutorialKey;
    [SerializeField] private Button btn_continue;
    private int counter;

    void Start()
    {
        btn_continue.onClick.AddListener(ButtonContinue);

        RequestData();
    }

    void RequestData(){
        if(PlayerPrefs.HasKey(tutorialKey)) {
            Destroy(gameObject);
            return;
        }

        gameObject.SetActive(true);
        ButtonContinue();
    }

    void ButtonContinue(){
        if(counter != 0) list_tutorial[counter-1].gameObject.SetActive(false);

        if(counter < list_tutorial.Count) list_tutorial[counter].gameObject.SetActive(true);
        else {
            Destroy(gameObject);
            SaveData();
        }

        counter++;
    }

    void SaveData(){
        PlayerPrefs.SetInt(tutorialKey, 1);
    }
}
