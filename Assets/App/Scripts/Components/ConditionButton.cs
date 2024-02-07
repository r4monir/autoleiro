using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConditionButton : MonoBehaviour
{
    private Button _btn;
    public Button Btn { get => _btn; }

    [HideInInspector] private bool _active;
    public bool Active { get => _active; }

    private Image check;


    private void Awake() {
        _btn = GetComponent<Button>();
        check = transform.Find("Checkmark").GetComponent<Image>();

        _btn.onClick.AddListener(delegate { SetValue(!_active); });
    }

    public void SetValue(bool secret){
        _active = secret;

        check.gameObject.SetActive(_active);

        if(_active) _btn.image.color = Color.green;
        else _btn.image.color = Color.red;
    }
}
