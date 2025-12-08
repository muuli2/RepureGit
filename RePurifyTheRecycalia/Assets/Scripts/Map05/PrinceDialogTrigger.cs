using UnityEngine;

public class PrinceTriggerDialogue : MonoBehaviour
{
    public DialogueLine2[] dialogueForA;
    public DialogueLine2[] dialogueForB;

    bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();

        if (SelectedCharacter.characterName == "Lumina")
            PrinceDialogueSystem2.Instance.StartDialogue(dialogueForA, pm);
        else
            PrinceDialogueSystem2.Instance.StartDialogue(dialogueForB, pm);
    }
}
