using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DialogueManage : MonoBehaviour
{
    public GameObject dialogueBox;

    public TMP_Text nameText;        // ชื่อผู้พูด
    public TMP_Text dialogueText;    // ข้อความพูด

    public GameObject choicesPanel;  // Panel รวมปุ่มตัวเลือก
    public Button choiceButtonPrefab;// Prefab ปุ่มตัวเลือก

    public Button nextButton;        // ปุ่ม Next

    private string[] sentences;
    private int index = 0;
    private PlayerMovement player;
    private PlayerShoot playerShoot; // 🔒 ตัวแปรยิง
    private DialogueTrigger currentTrigger;

    [Header("Sound FX")]
public AudioSource sfxSource;
public AudioClip clickChoiceSFX;   // เสียงกดตัวเลือก
public AudioClip clickNextSFX;     // เสียง Next / กด F



    void Start()
    {
        dialogueBox.SetActive(false);
        choicesPanel.SetActive(false);

        
    if (sfxSource == null)
        sfxSource = GetComponent<AudioSource>();

        // เชื่อมปุ่ม Next
        nextButton.onClick.AddListener(NextSentence);
    }

   public void StartDialogue(string[] lines, PlayerMovement pm, DialogueTrigger trigger)
{
    sentences = lines;
    player = pm;
    currentTrigger = trigger;

    player.SetCanMove(false);
    playerShoot = player.GetComponent<PlayerShoot>();
    if (playerShoot != null)
        playerShoot.canShoot = false;

    index = 0;
    dialogueBox.SetActive(true);
    ShowSentence();
   

}


    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        // ถ้ามีตัวเลือกขึ้น → ห้ามกด F
        if (choicesPanel.activeSelf) return;

        // กด F เพื่อไปต่อ
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            NextSentence();
        }
    }

    void ShowSentence()
    {
        string line = sentences[index];

        // ถ้าเป็นประโยคที่มี Choices
        if (line.StartsWith("CHOICE:"))
        {
            ShowChoices(line);
            return;
        }

        // ซ่อนตัวเลือกถ้าไม่ใช่บรรทัดตัวเลือก
        choicesPanel.SetActive(false);

        // โชว์บทสนทนาแบบ ชื่อ: ข้อความ
        if (line.Contains(":"))
        {
            string[] parts = line.Split(':');
            nameText.text = parts[0].Trim();
            dialogueText.text = parts[1].Trim();
        }
        else
        {
            nameText.text = "";
            dialogueText.text = line;
        }
    }

    void ShowChoices(string line)
    {
        choicesPanel.SetActive(true);

        // ล้างปุ่มเก่า
        foreach (Transform child in choicesPanel.transform)
            Destroy(child.gameObject);

        string choiceLine = line.Replace("CHOICE:", "").Trim();
        string[] options = choiceLine.Split('|');

        foreach (string option in options)
        {
            Button btn = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            btn.GetComponentInChildren<TMP_Text>().text = option.Trim();

            btn.onClick.AddListener(() => {
                 if (sfxSource && clickChoiceSFX)
        sfxSource.PlayOneShot(clickChoiceSFX);
                choicesPanel.SetActive(false);
                NextSentence();
            });
        }
    }

    public void NextSentence()
    {
        if (choicesPanel.activeSelf) return;

        if (sfxSource && clickNextSFX)
        sfxSource.PlayOneShot(clickNextSFX);

        index++;

        if (index >= sentences.Length)
        {
            EndDialogue();
            return;
        }

        ShowSentence();
    }

    void EndDialogue()
{
    dialogueBox.SetActive(false);
    choicesPanel.SetActive(false);

    if (player != null)
    {
        player.SetCanMove(true);

        if (playerShoot != null)
            playerShoot.canShoot = true;
    }

    // เรียกอัพเกรดจาก trigger
    if (currentTrigger != null)
    {
        PlayerShoot ps = player.GetComponent<PlayerShoot>();
        currentTrigger.AfterDialogueUpgrade(ps);
    }
}


    
}
