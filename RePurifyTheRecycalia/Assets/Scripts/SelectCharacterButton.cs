using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCharacterButton : MonoBehaviour
{
    public string characterName; // เช่น "Knight" หรือ "Mage"

    public void OnClickSelect()
    {
        SelectedCharacter.characterName = characterName;
        SceneManager.LoadScene("Map01");
    }
}
