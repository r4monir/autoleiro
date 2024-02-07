using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnHoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private EventTrigger eventTrigger;
    float timer;
    Action onHoldAction;
    Coroutine coroutine;

    public void OnHold(Action act, float time = 1){
        Button btn = GetComponent<Button>();
        if (!btn.GetComponent<EventTrigger>()) btn.gameObject.AddComponent<EventTrigger>();
        eventTrigger = GetComponent<EventTrigger>();

        onHoldAction = act;
        timer = time;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnHoldPointerDown(); 
        
        eventTrigger.enabled = false; // Desativa o EventTrigger para permitir a rolagem do Scroll View
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnHoldPointerUp(); 
        
        eventTrigger.enabled = true; // Reativa o EventTrigger
    }

    void OnHoldPointerDown () {
        coroutine = StartCoroutine(OnHoldTimer());
    }

    void OnHoldPointerUp () {
        StopCoroutine(coroutine);
    }

    void OnHoldComplete(){
        onHoldAction ();
    }

    IEnumerator OnHoldTimer(){
        yield return new WaitForSeconds(timer);
        OnHoldComplete ();
    }
}