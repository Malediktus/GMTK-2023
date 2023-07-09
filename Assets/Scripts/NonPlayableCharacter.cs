using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter
{
    public List<ShopItem> inventory = new List<ShopItem>();
    public float money;
    public string name;
    public Sprite sprite;
    public bool available;
    public int trust;

    public NonPlayableCharacter(Sprite _sprite, string _name)
    {
        sprite = _sprite;
        name = _name;
        money = Random.Range(150, 300);
        available = true;
        trust = 0;
    }

    public bool EvaluateTrade(ShopItem item, float additional)
    {
        if ((item.EvaluateCost(additional) - (trust / 2)) <= money)
        {
            return true;
        }

        return false;
    }
}
