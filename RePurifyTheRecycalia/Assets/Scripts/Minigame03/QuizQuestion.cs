using UnityEngine;

[System.Serializable]
public class QuizQuestion
{
    public Sprite picture;
    public string question;
    public string[] answers;
    public int correctIndex;
}
