using UnityEngine;

public enum Speaker { Prince, Player }

[System.Serializable]
public class DialogueLine2
{
    public Speaker speaker; 
    public string speakerName;
    [TextArea] public string text;
    public Sprite portrait;
}
