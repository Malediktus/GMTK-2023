using LuckiusDev.Utils.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static Cinemachine.DocumentationSortingAttribute;
using static ShopItem;
using UnityEngine.TextCore.Text;

public class NPCManager : Singleton<NPCManager>
{
    [SerializeField] private int numCharactersAtStart = 5;
    [SerializeField] private Vector2 characterSpawnRate = new Vector2(300, 600);
    [SerializeField] private Vector2 randomCharacterVisitRate = new Vector2(300, 600);
    [SerializeField] private Vector2 qualityLossRate = new Vector2(5, 30);
    [SerializeField] private UnityEvent<NonPlayableCharacter> onVisitEvent = new UnityEvent<NonPlayableCharacter>();
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private List<string> characterNames;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject buyButton;
    [SerializeField] private List<ShopItem> loot;
    [Header("Simulation")]
    [SerializeField] private float surviveThreshold = 80;
    [SerializeField] private float surviveChance = 130;

    private List<NonPlayableCharacter> characters = new List<NonPlayableCharacter>();
    private NonPlayableCharacter currentVisitor;
    private GameObject currentVisitorInstance;

    public bool HasVisitor() {
        return currentVisitor != null;
    }

    public NonPlayableCharacter GetCurrentVisitor() {
        return currentVisitor;
    }

    private void Start()
    {
        currentVisitor = null;
        currentVisitorInstance = null;
        for (int i = 0; i < numCharactersAtStart; i++)
        {
            string name = characterNames[UnityEngine.Random.Range(0, characterNames.Count - 1)];
            characterNames.Remove(name); // Dont reuse names
            var character = new NonPlayableCharacter(characterSprites[UnityEngine.Random.Range(0, characterSprites.Count - 1)], name);
            for (int w = UnityEngine.Random.Range(0, 5); w < 0; w++)
            {
                character.inventory.Add(loot[UnityEngine.Random.Range(0, loot.Count - 1)]);
            }
            characters.Add(character);
        }
        Invoke("SpawnCharacter", UnityEngine.Random.Range(characterSpawnRate.x, characterSpawnRate.y));
        CharacterVisit(characters[UnityEngine.Random.Range(0, characters.Count - 1)]);
        Invoke("RandomCharacterVisit", UnityEngine.Random.Range(randomCharacterVisitRate.x, randomCharacterVisitRate.y));
    }

    private void SpawnCharacter()
    {
        if (characters.Count >= numCharactersAtStart)
        {
            return;
        }

        string name = characterNames[UnityEngine.Random.Range(0, characterNames.Count - 1)];
        characterNames.Remove(name); // Dont reuse names
        var character = new NonPlayableCharacter(characterSprites[UnityEngine.Random.Range(0, characterSprites.Count - 1)], name);
        for (int i = UnityEngine.Random.Range(0, 5); i < 0; i++)
        {
            character.inventory.Add(loot[UnityEngine.Random.Range(0, loot.Count - 1)]);
        }
        characters.Add(character);
        Invoke("SpawnCharacter", UnityEngine.Random.Range(characterSpawnRate.x, characterSpawnRate.y));
    }

    private void RandomCharacterVisit()
    {
        if (currentVisitor != null) // Already a visitor
        {
            Invoke("RandomCharacterVisit", UnityEngine.Random.Range(randomCharacterVisitRate.x, randomCharacterVisitRate.y));
            return;
        }

        var randomChar = characters[UnityEngine.Random.Range(0, characters.Count - 1)];
        int i = 10;
        while (!randomChar.available && i > 0)
        {
            randomChar = characters[UnityEngine.Random.Range(0, characters.Count - 1)];
            i++;
        }
        if (!randomChar.available)
        {
            Invoke("RandomCharacterVisit", UnityEngine.Random.Range(randomCharacterVisitRate.x, randomCharacterVisitRate.y));
            return;
        }
        CharacterVisit(randomChar);

        Invoke("RandomCharacterVisit", UnityEngine.Random.Range(randomCharacterVisitRate.x, randomCharacterVisitRate.y));
    }

    private void CharacterVisit(NonPlayableCharacter character)
    {
        currentVisitor = character;
        GameObject prefabInstance = Instantiate(characterPrefab, canvas);
        prefabInstance.transform.SetAsFirstSibling();
        prefabInstance.GetComponent<Image>().sprite = character.sprite;
        currentVisitorInstance = prefabInstance;
        if (character.inventory.Count <= 0)
        {
            buyButton.SetActive(false);
        }
        else
        {
            buyButton.SetActive(true);
        }
        ShuffleList<ShopItem>(character.inventory);
        onVisitEvent?.Invoke(character);
        Dialogue.Instance.Tell(new List<string> { "A new visitor arrived!", "You can buy stuff from him or sell him stuff." });
    }

    public void OnEndDialog()
    {
        buyButton.SetActive(true);
        if (Instance.currentVisitor == null)
        {
            return;
        }

        Simulate();
        Destroy(Instance.currentVisitorInstance);
        Instance.currentVisitor = null;
        Instance.currentVisitorInstance = null;
        Dialogue.Instance.Stop();
    }

    static void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            System.Random random = new System.Random();
            int j = random.Next(i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    private void Simulate()
    {
        Debug.Log("Started simulation");
        NonPlayableCharacter character = currentVisitor;
        currentVisitor.available = false;

        float bestWeaponCost = 0;
        float bestHelmetCost = 0;

        foreach (var item in currentVisitor.inventory)
        {
            switch (item.itemType)
            {
                case ShopItemType.Weapon:
                    if (item.EvaluateCost(0) > bestWeaponCost)
                    {
                        bestWeaponCost = item.EvaluateCost(0);
                    }
                    break;

                case ShopItemType.Helmet:
                    if (item.EvaluateCost(0) > bestHelmetCost)
                    {
                        bestHelmetCost = item.EvaluateCost(0);
                    }
                    break;

                default:
                    break;
            }
        }

        Debug.Log(bestWeaponCost + bestHelmetCost);
        if ((bestWeaponCost + bestHelmetCost) <= surviveThreshold)
        {
            characters.Remove(character);
            return; // Die
        }

        float t = UnityEngine.Random.Range(surviveThreshold, bestWeaponCost + bestHelmetCost);
        Debug.Log(t);
        bool survive = t >= surviveChance;
        if (!survive)
        {
            characters.Remove(character);
            return; // Die
        }

        Debug.Log("survived");

        foreach (var item in currentVisitor.inventory)
        {
            item.quality -= UnityEngine.Random.Range((int)qualityLossRate.x, (int)qualityLossRate.y);
            if (item.quality <= 0)
            {
                currentVisitor.inventory.Remove(item); // Item breaks
            }
        }

        for (int i = UnityEngine.Random.Range(0, 5); i < 0; i++)
        {
            currentVisitor.inventory.Add(loot[UnityEngine.Random.Range(0, loot.Count - 1)]);
        }

        currentVisitor.available = true;
        return;
    }
}
