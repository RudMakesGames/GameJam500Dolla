using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject Dialogue;

    [Header("Dialogue UI")]
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;

    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    private AudioClip currentVoiceLine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, AudioClip voiceLine = null)
    {
        isDialogueActive = true;
        Dialogue.SetActive(true);

        currentVoiceLine = voiceLine;

        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }

        DisplayNextDialogueLine();
    }

    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }

    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            PlayVoiceSound();
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    private void PlayVoiceSound()
    {
        if (currentVoiceLine != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(currentVoiceLine);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        Dialogue.SetActive(false);
        currentVoiceLine = null;
    }
}
