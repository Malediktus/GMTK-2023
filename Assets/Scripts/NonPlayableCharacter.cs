using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter
{
    public List<ShopItem> inventory;
    public string name;
    private Sprite sprite;

    public NonPlayableCharacter(Sprite _sprite, string _name)
    {
        sprite = _sprite;
        name = _name;
    }
}
