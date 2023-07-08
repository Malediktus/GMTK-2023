using LuckiusDev.Utils.Types;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : Singleton<Dialogue>
{
    [SerializeField] TextMeshProUGUI dialogueTextBox;
    [SerializeField] float textSpeed = .2f; // How much wait before getting the next character in the current line
    [SerializeField] float waitDelay = .5f; // How much wait before going to the next line

    private List<string> lines = new List<string>();
    private int index = 0;

    private void Start() {
        dialogueTextBox.text = string.Empty;
        dialogueTextBox.enabled = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            Tell(new List<string>{ "this is a test line, hello world !!", "multiple lines of text yahoo!" });
        }

        // Automatically goes to the next line when the current line is completed
        if (lines.Count > 0) {
            if (dialogueTextBox.text == lines[index]) {
                StartCoroutine(NextLine());
            }
        }
        if (index >= lines.Count -1 && dialogueTextBox.text == lines[index])
        {
            StartCoroutine(ClearLine());
        }
    }

    /// <summary>
    /// Start a new dialogue with the lines given as a list.
    /// </summary>
    /// <param name="lines">The lines of text you want to be displayed.</param>
    public void Tell(List<string> lines) {
        this.lines = lines;
        StartDialogue();
    }

    /// <summary>
    /// Start a new dialogue with a single line of text. Will probably be the one to use the most.
    /// </summary>
    /// <param name="line">The unique line of dialogue you want to pass.</param>
    public void Tell(string line) {
        lines = new List<string>();
        lines.Add(line);
        StartDialogue();
    }

    void StartDialogue() {
        StopAllCoroutines();

        dialogueTextBox.enabled = true;
        dialogueTextBox.text = string.Empty;
        
        index = 0;
        
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() {
        foreach (char character in lines[index].ToCharArray()) {
            dialogueTextBox.text += character;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    IEnumerator NextLine() {
        yield return new WaitForSeconds(waitDelay);
        if (index < lines.Count - 1) {
            index++;
            dialogueTextBox.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator ClearLine() {
        yield return new WaitForSeconds(2.5f);
        dialogueTextBox.text = string.Empty;
    }
}
