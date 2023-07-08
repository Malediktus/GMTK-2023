using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    private const int minLevel = 1;
    private const int maxLevel = 5;
    private const int minQuality = 0;
    private const int maxQuality = 100;

    // Maybe use ScriptableObjects instead
    public enum ShopItemType
    {
        Weapon,
        Helmet
    }

    [Header("Item Properties")]
    [SerializeField] private ShopItemType itemType = ShopItemType.Weapon;
    [SerializeField] private Sprite image;
    [SerializeField] [Range(minLevel, maxLevel)] private int level = 1;
    [SerializeField] [Range(minQuality, maxQuality)] private int quality = 100;

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

    public int EvaluateCost(int additional)
    {
        switch(itemType)
        {
            case ShopItemType.Weapon:
                return level * 100 + quality / 2 + additional; // Just an example... needs to be balanced
            case ShopItemType.Helmet:
                return level * 100 + quality / 2 + additional; // Same here
            default:
                return 0;
        }
    }

    public Sprite GetImage()
    {
        return image;
    }
}
