using LuckiusDev.Utils.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class NPCManager : Singleton<NPCManager>
{
    [SerializeField] private int numCharactersAtStart = 5;
    [SerializeField] private Vector2 characterSpawnRate = new Vector2(300, 600);
    [SerializeField] private UnityEvent<NonPlayableCharacter> onVisitEvent = new UnityEvent<NonPlayableCharacter>();
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private List<string> characterNames;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject buyButton;
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
            characters.Add(new NonPlayableCharacter(characterSprites[UnityEngine.Random.Range(0, characterSprites.Count - 1)], name));
        }
        Invoke("SpawnCharacter", UnityEngine.Random.Range(characterSpawnRate.x, characterSpawnRate.y));
        CharacterVisit(characters[UnityEngine.Random.Range(0, characters.Count - 1)]);
    }

    private void SpawnCharacter()
    {
        string name = characterNames[UnityEngine.Random.Range(0, characterNames.Count - 1)];
        characterNames.Remove(name); // Dont reuse names
        characters.Add(new NonPlayableCharacter(characterSprites[UnityEngine.Random.Range(0, characterSprites.Count - 1)], name));
        Invoke("SpawnCharacter", UnityEngine.Random.Range(characterSpawnRate.x, characterSpawnRate.y));
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
        Destroy(Instance.currentVisitorInstance);
        Instance.currentVisitor = null;
        Instance.currentVisitorInstance = null;
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
}
