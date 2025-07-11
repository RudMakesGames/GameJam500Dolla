using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
    public AudioClip VoiceLine;
    public int index;
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue, VoiceLine);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<DialogueIndex>(out var player))
        {
            if (player.currentDialogueIndex < index)
            {
                player.currentDialogueIndex++;
                StartCoroutine(WakeUp());
            }
        }
    }

    IEnumerator WakeUp()
    {
        yield return new WaitForSeconds(0.75f);
        TriggerDialogue();
        yield return new WaitForSeconds(6);
        DialogueManager.Instance.EndDialogue();
    }
}
