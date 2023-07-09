using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ShopItem")]
public class ShopItem : ScriptableObject
{
    private const int minLevel = 1;
    private const int maxLevel = 5;
    private const int minQuality = 0;
    private const int maxQuality = 100;

    // Maybe use ScriptableObjects instead
    public enum ShopItemType
    {
        Armor,
        Item,
        Potion,
        TresureMap,
        Weapon
    }

    [Header("Item Properties")]
    [SerializeField] public ShopItemType itemType = ShopItemType.Weapon;
    [SerializeField] public Sprite image;
    [SerializeField] [Range(minLevel, maxLevel)] public int level = 1;
    [SerializeField] [Range(minQuality, maxQuality)] public int quality = 100;

    public void SetLevel(int newLevel)
    {
        if (newLevel < minLevel || newLevel > maxLevel)
        {
            Debug.LogWarning("ShopItem.level has been assigned an illigal value!");
        }

        level = newLevel;
    }

    public void SetQuality(int newQuality)
    {
        if (newQuality < minQuality || newQuality > maxQuality)
        {
            Debug.LogWarning("ShopItem.quality has been assigned an illigal value!");
        }

        quality = newQuality;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetQuality()
    {
        return quality;
    }

    public float EvaluateCost(float additional)
    {
        float cost = level * 100 + quality / 2 + additional;

        switch (itemType)
        {
            case ShopItemType.Armor:
                cost += 90;
                break;
            case ShopItemType.Item:
                cost += 50;
                break;
            case ShopItemType.Potion:
                cost += 70;
                break;
            case ShopItemType.TresureMap:
                cost += 30;
                break;
            case ShopItemType.Weapon:
                cost += 800;
                break;
        }

        return cost;
    }

    public Sprite GetImage()
    {
        return image;
    }
}
