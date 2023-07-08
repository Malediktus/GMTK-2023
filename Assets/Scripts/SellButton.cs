using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellButton : MonoBehaviour
{
    Button button;

    private void Start() {
        button = GetComponent<Button>();
    }

    private void Update() {
        button.interactable = NPCManager.Instance.HasVisitor();
    }
}
