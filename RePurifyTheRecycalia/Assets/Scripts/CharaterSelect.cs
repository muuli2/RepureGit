using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterSelect : MonoBehaviour
{
    [System.Serializable]
    public class Character
    {
        [Header("ข้อมูลตัวละคร")]
        public string characterName;
        public string description;

        [Header("UI Element")]
        public Button characterButton;
        public Image characterImage;        // Sprite ของตัวละคร
        public CanvasGroup infoPanel;       // Info Panel
        public RectTransform infoPanelRect; // สำหรับเลื่อน
        public Image portrait;             // Portrait ของตัวละคร
        public TMP_Text nameText;
        public TMP_Text descriptionText;

        [HideInInspector]
        public Vector3 panelBasePosition;   // ตำแหน่งปกติของ Panel
    }

    public Character characterLeft;
    public Character characterRight;

    private Character selectedCharacter;

    [Header("Settings")]
    public float fadeDuration = 0.5f;       // เวลาเลื่อน panel
    public float panelOffset = 20f;         // ระยะเลื่อน panel ทุกครั้งที่กด

    void Start()
    {
        // บันทึกตำแหน่งปกติของ Info Panel
        characterLeft.panelBasePosition = characterLeft.infoPanelRect.anchoredPosition;
        characterRight.panelBasePosition = characterRight.infoPanelRect.anchoredPosition;

        // เริ่มจาง
        SetAlpha(characterLeft.characterImage, 0.5f);
        SetAlpha(characterRight.characterImage, 0.5f);

        // ซ่อน Info Panel และ Portrait ทั้งสองฝั่ง
        HideInfo(characterLeft);
        HideInfo(characterRight);

        // Event ของ Button
        characterLeft.characterButton.onClick.AddListener(() => OnCharacterClicked(characterLeft, characterRight));
        characterRight.characterButton.onClick.AddListener(() => OnCharacterClicked(characterRight, characterLeft));
    }

    void SetAlpha(Image img, float alpha)
    {
        if(img == null) return;
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void HideInfo(Character character)
    {
        if(character.infoPanel != null)
        {
            character.infoPanel.alpha = 0;
            character.infoPanel.interactable = false;
            character.infoPanel.blocksRaycasts = false;
        }
        if(character.portrait != null) character.portrait.enabled = false;
    }

    void ShowInfo(Character character)
    {
        if(character.infoPanel != null)
        {
            character.infoPanel.alpha = 1;
            character.infoPanel.interactable = true;
            character.infoPanel.blocksRaycasts = true;
        }
        if(character.portrait != null) character.portrait.enabled = true;

        if(character.nameText != null) character.nameText.text = character.characterName;
        if(character.descriptionText != null) character.descriptionText.text = character.description;
    }

    void OnCharacterClicked(Character characterToShow, Character characterToHide)
    {
        selectedCharacter = characterToShow;

        // ตัวที่เลือกเข้ม ตัวที่ไม่ได้เลือกจาง
        SetAlpha(characterLeft.characterImage, characterLeft == characterToShow ? 1f : 0.5f);
        SetAlpha(characterRight.characterImage, characterRight == characterToShow ? 1f : 0.5f);

        // ซ่อน Info Panel ฝั่งที่ไม่ได้เลือก
        HideInfo(characterToHide);

        // แสดง Info Panel ฝั่งที่เลือก
        ShowInfo(characterToShow);

        // เลื่อน Panel ทุกครั้งที่กด
        float offset = characterToShow == characterLeft ? -panelOffset : panelOffset;
        StartCoroutine(ShakePanel(characterToShow.infoPanelRect, offset, fadeDuration));
    }

    IEnumerator ShakePanel(RectTransform panel, float offset, float duration)
    {
        if(panel == null) yield break;

        Vector3 startPos = panel.anchoredPosition;
        Vector3 targetPos = startPos + new Vector3(offset, 0, 0);
        float t = 0f;

        // เลื่อนไปข้างหน้า
        while(t < 1f)
        {
            t += Time.deltaTime / duration;
            panel.anchoredPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        // เลื่อนกลับ
        t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / duration;
            panel.anchoredPosition = Vector3.Lerp(targetPos, startPos, t);
            yield return null;
        }

        panel.anchoredPosition = startPos;
    }

    public void OnSelectPressed()
{
    if(selectedCharacter == null)
    {
        Debug.Log("กรุณาเลือกตัวละครก่อน!");
        return;
    }

    // บันทึกตัวละครที่เลือก
    SelectedCharacter.characterName = selectedCharacter.characterName;

    // โหลด Scene ด่านถัดไป
    SceneManager.LoadScene("Map01");
}


}
