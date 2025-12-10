using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class PrinceDialogueSystem2 : MonoBehaviour
{
    public static PrinceDialogueSystem2 Instance;

    [Header("Main Panel")]
    public GameObject dialoguePanel;

    [Header("Prince Side")]
    public GameObject princePanel;
    public Image princePortrait;
    public TMP_Text princeNameText;

    [Header("Player Side")]
    public GameObject playerPanel;
    public Image playerPortrait;
    public TMP_Text playerNameText;

    [Header("Dialogue")]
    public TMP_Text dialogueText;
    public Button nextButton;

    private DialogueLine2[] lines;
    private int index = 0;
    private bool active = false;

    private PlayerMovement pm;
    private PlayerShoot ps;
    public FadeController fadeController;
    public CameraPan cameraPan;



    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
        nextButton.onClick.AddListener(NextLine);
    }


    public void StartDialogue(DialogueLine2[] dialogueLines, PlayerMovement player)
    {
        lines = dialogueLines;
        index = 0;
        active = true;

        pm = player;
        pm.SetCanMove(false);

        ps = player.GetComponent<PlayerShoot>();
        if (ps != null) ps.canShoot = false;

        dialoguePanel.SetActive(true);
        ShowLine();
    }


    void Update()
    {
        if (!active) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
            NextLine();
    }


    void ShowLine()
    {
        DialogueLine2 line = lines[index];

        dialogueText.text = line.text;

       bool playerSpeaking = line.speaker == Speaker.Player;

        // Prince speaking
        princePanel.SetActive(true);
        playerPanel.SetActive(true);


if (playerSpeaking)
{
    playerPanel.GetComponent<CanvasGroup>().alpha = 1f;
    princePanel.GetComponent<CanvasGroup>().alpha = 0f;

    playerNameText.text = line.speakerName;
    playerPortrait.sprite = line.portrait;
}
else
{
    playerPanel.GetComponent<CanvasGroup>().alpha = 0f;
    princePanel.GetComponent<CanvasGroup>().alpha = 1f;

    princeNameText.text = line.speakerName;
    princePortrait.sprite = line.portrait;
}

    }


    public void NextLine()
    {
        index++;

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
    }


   void EndDialogue()
{
    active = false;

    // คืนการควบคุมผู้เล่นก่อน
    pm.SetCanMove(true);
    if (ps != null) ps.canShoot = true;

    // ⭐ เริ่มเอฟเฟกต์ก่อน (อย่าปิด dialoguePanel ตอนนี้)
    StartCoroutine(DoEndingEffects());

    //    SceneManager.LoadScene("End");
    
}

private IEnumerator DoEndingEffects()
{
    // เฟดดำ 1.5 วิ
    yield return StartCoroutine(fadeController.FadeInBlack());

    // แพนกล้องขึ้น 2 วิ
    if (cameraPan != null)
        yield return StartCoroutine(cameraPan.PanUp());

         SceneManager.LoadScene("End");
}


}
