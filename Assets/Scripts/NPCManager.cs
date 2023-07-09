using LuckiusDev.Utils.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using static ShopItem;
using UnityEditor;
using UnityEngine.InputSystem;

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
    [SerializeField] private AudioSource audioSource;

    [Header("Simulation")]
    [SerializeField] private float surviveThreshold = 80;
    [SerializeField] private float surviveChance = 130;
    [SerializeField] private Transform relations;
    [SerializeField] private GameObject relationPrefab;
    [SerializeField] public TMP_Text tooltipText;
    [SerializeField] private DayManager dayManager;

    [SerializeField] public Sprite armor;
    [SerializeField] public Sprite item;
    [SerializeField] public Sprite potion;
    [SerializeField] public Sprite tresureMap;
    [SerializeField] public Sprite weapon;

    public static List<string> greetings = new List<string> {"Good Day","Hello", "Good Afternoon", "Hey, what is new in stock ?"};
    public static List<string> buyDialog = new List<string> { "Thank you", "This is great!", "I love this product!" };
    public static List<string> sellDialog = new List<string> { "You won’t regret it", "Top of my loot!", "You will be back for more" };

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
            var instance = Instantiate(relationPrefab, relations);
            instance.name = character.name;
            instance.GetComponentInChildren<TMP_Text>().text = character.name;

            for (int w = UnityEngine.Random.Range(0, 5); w > 0; w--)
            {
                character.inventory.Add(loot[UnityEngine.Random.Range(0, loot.Count - 1)]);
            }
            characters.Add(character);
        }
        Invoke("SpawnCharacter", UnityEngine.Random.Range(characterSpawnRate.x, characterSpawnRate.y));
        CharacterVisit(characters[UnityEngine.Random.Range(0, characters.Count - 1)]);
        Invoke("RandomCharacterVisit", UnityEngine.Random.Range(randomCharacterVisitRate.x, randomCharacterVisitRate.y));
    }

    private void Update()
    {
        foreach (var character in characters)
        {
            var child = relations.Find(character.name);
            child.GetComponentInChildren<Slider>().value = character.trust;
        }
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
        var instance = Instantiate(relationPrefab, relations);
        instance.name = character.name;
        instance.GetComponentInChildren<TMP_Text>().text = character.name;
        for (int i = UnityEngine.Random.Range(0, 5); i > 0; i--)
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

        audioSource.Play();
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
        Debug.Log(character.inventory.Count);
        GameObject prefabInstance = Instantiate(characterPrefab, canvas);
        prefabInstance.transform.SetAsFirstSibling();
        prefabInstance.GetComponent<Image>().sprite = character.sprite;
        currentVisitorInstance = prefabInstance;
        prefabInstance.GetComponentInChildren<TMP_Text>().text = character.name;
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
        Dialogue.Instance.Tell(greetings[UnityEngine.Random.Range(0, greetings.Count - 1)]);
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
        dayManager.UpdateHours();
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
        character.available = false;

        float cost = 0;

        foreach (var item in character.inventory)
        {
            cost += item.EvaluateCost(0);
        }

        if ((cost) <= surviveThreshold)
        {
            var child = relations.Find(character.name);
            Destroy(child.gameObject);
            characters.Remove(character);
            return; // Die
        }

        float t = UnityEngine.Random.Range(surviveThreshold, cost);
        Debug.Log(t);
        bool survive = t >= surviveChance;
        if (!survive)
        {
            var child = relations.Find(character.name);
            Destroy(child.gameObject);
            characters.Remove(character);
            return; // Die
        }

        Debug.Log("survived");

        List<ShopItem> visitorInventory = new List<ShopItem>(character.inventory);
        foreach (var item in visitorInventory)
        {
            item.quality -= UnityEngine.Random.Range((int)qualityLossRate.x, (int)qualityLossRate.y);
            if (item.quality <= 0)
            {
                character.inventory.Remove(item); // Item breaks
            }
        }

        for (int i = UnityEngine.Random.Range(0, 5); i < 0; i++)
        {
            character.inventory.Add(loot[UnityEngine.Random.Range(0, loot.Count - 1)]);
        }

        character.money += UnityEngine.Random.Range(70, 350);

        character.available = true;
        return;
    }
}
