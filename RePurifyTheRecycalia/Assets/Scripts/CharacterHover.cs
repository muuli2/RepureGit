using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image characterImage;
    public float hoverAlpha = 1f;
    public float normalAlpha = 0.5f;

    // ตัวละครนี้ถูกเลือกหรือไม่
    [HideInInspector] public bool isSelected = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(characterImage != null)
        {
            Color c = characterImage.color;
            c.a = hoverAlpha;
            characterImage.color = c;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(characterImage != null)
        {
            // ถ้าถูกเลือกแล้ว → เข้มค้าง
            float targetAlpha = isSelected ? 1f : normalAlpha;
            Color c = characterImage.color;
            c.a = targetAlpha;
            characterImage.color = c;
        }
    }
}
