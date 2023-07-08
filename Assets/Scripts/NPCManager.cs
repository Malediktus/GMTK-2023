using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private int numCharactersAtStart = 5;
    [SerializeField] private Vector2 characterSpawnRate = new Vector2(300, 600);
    [SerializeField] private Vector2 characterVisitRate = new Vector2(60, 180);
    [SerializeField] private UnityEvent<NonPlayableCharacter> onVisitEvent = new UnityEvent<NonPlayableCharacter>();
    [SerializeField] private List<Sprite> characterSprites;
    private List<NonPlayableCharacter> characters;

    private void Start()
    {
        for (int i = 0; i < numCharactersAtStart; i++)
        {
            characters.Add(new NonPlayableCharacter(characterSprites[Random.Range(0, characterSprites.Count - 1)]));
        }
        Invoke("SpawnCharacter", Random.Range(characterSpawnRate.x, characterSpawnRate.y));
        Invoke("CharacterVisit", Random.Range(characterVisitRate.x, characterVisitRate.y));
    }

    private void SpawnCharacter()
    {
        characters.Add(new NonPlayableCharacter(characterSprites[Random.Range(0, characterSprites.Count - 1)]));
        Invoke("SpawnCharacter", Random.Range(characterSpawnRate.x, characterSpawnRate.y));
    }

    private void CharacterVisit()
    {
        onVisitEvent?.Invoke(characters[Random.Range(0, characters.Count - 1)]);
        Invoke("CharacterVisit", Random.Range(characterVisitRate.x, characterVisitRate.y));
    }
}
