using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Character Prefabs")]
    public GameObject MalePrefab;
    public GameObject MagePrefab;

    [Header("Spawn Point")]
    public Transform PlayerParent;  // <-- ประกาศตรงนี้

    void Start()
    {
        string selected = SelectedCharacter.characterName;

        GameObject toSpawn = null;
        if(selected == "Lucias")
            toSpawn = MalePrefab;
        else if(selected == "Lumina")
            toSpawn = MagePrefab;

        if(toSpawn != null)
            Instantiate(toSpawn, PlayerParent.position, Quaternion.identity, PlayerParent);
    }
}
