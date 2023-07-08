using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private int numCharactersAtStart = 5;
    [SerializeField] private Vector2 characterSpawnRate = new Vector2(300, 600);
    [SerializeField] private UnityEvent<NonPlayableCharacter> onVisitEvent = new UnityEvent<NonPlayableCharacter>();
    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private List<string> characterNames;
    [SerializeField] private Transform canvas;
    [SerializeField] private GameObject characterPrefab;
    private List<NonPlayableCharacter> characters = new List<NonPlayableCharacter>();
    private NonPlayableCharacter currentVisitor;
    private GameObject currentVisitorInstance;

    private void Start()
    {
        currentVisitor = null;
        currentVisitorInstance = null;
        for (int i = 0; i < numCharactersAtStart; i++)
        {
            string name = characterNames[Random.Range(0, characterNames.Count - 1)];
            characterNames.Remove(name); // Dont reuse names
            characters.Add(new NonPlayableCharacter(characterSprites[Random.Range(0, characterSprites.Count - 1)], name));
        }
        Invoke("SpawnCharacter", Random.Range(characterSpawnRate.x, characterSpawnRate.y));
        CharacterVisit(characters[Random.Range(0, characters.Count - 1)]);
    }

    private void SpawnCharacter()
    {
        string name = characterNames[Random.Range(0, characterNames.Count - 1)];
        characterNames.Remove(name); // Dont reuse names
        characters.Add(new NonPlayableCharacter(characterSprites[Random.Range(0, characterSprites.Count - 1)], name));
        Invoke("SpawnCharacter", Random.Range(characterSpawnRate.x, characterSpawnRate.y));
    }

    private void CharacterVisit(NonPlayableCharacter character)
    {
        currentVisitor = character;
        GameObject prefabInstance = Instantiate(characterPrefab, canvas);
        prefabInstance.transform.SetAsFirstSibling();
        currentVisitorInstance = prefabInstance;
        onVisitEvent?.Invoke(character);
        Dialogue.Instance.Tell(new List<string> { "A new visitor arrived!", "You can buy stuff from him or sell him stuff." }); // TODO: Fix
    }

    public void OnEndDialog()
    {
        if (currentVisitor == null)
        {
            return;
        }
        Destroy(currentVisitorInstance);
        currentVisitor = null;
        currentVisitorInstance = null;
    }
}
