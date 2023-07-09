using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

// This script is probatly unnecessary but I could find a better way rn
public class ToggleButton : MonoBehaviour
{
    [SerializeField] private Sprite toggleOnSprite;
    [SerializeField] private Sprite toggleOffSprite;
    [SerializeField] private UnityEvent toggleOnEvent;
    [SerializeField] private UnityEvent toggleOffEvent;
    private Button button;
    private Image image;
    private bool state;

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(OnToggle);
        toggleOffEvent?.Invoke();
        image.sprite = toggleOffSprite;
        state = false;
    }

    private void OnToggle()
    {
        if (state)
        {
            ToggleOff();
        }
        else
        {
            ToggleOn();
        }
    }

    public void ToggleOn()
    {
        toggleOnEvent?.Invoke();
        image.sprite = toggleOnSprite;
        state = true;
    }

    public void ToggleOff()
    {
        toggleOffEvent?.Invoke();
        image.sprite = toggleOffSprite;
        state = false;
    }
}
