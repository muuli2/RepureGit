using UnityEngine;
using TMPro;
using UnityEngine.EventSystems; // ต้องใช้สำหรับ PointerEnter / Exit

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;          // Text ของปุ่ม
    public Color hoverColor = Color.yellow;  // สีเมื่อชี้
    private Color defaultColor;    // สีเดิม

    void Start()
    {
        if (text == null)
        {
            text = GetComponentInChildren<TMP_Text>(); // หา Text อัตโนมัติ
        }
        defaultColor = text.color;
    }

    // เมื่อชี้เมาส์
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = hoverColor;
    }

    // เมื่อเอาเมาส์ออก
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColor;
    }
}
