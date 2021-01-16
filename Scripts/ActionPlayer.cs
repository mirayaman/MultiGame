using UnityEngine;

[RequireComponent(typeof(PlayerMove))]
public class ActionPlayer : MonoBehaviour
{
    private PlayerMove motorPlayer = null;

    private void Start()
    {
        motorPlayer = GetComponent<PlayerMove>();
        LoadSettings();
    }


    private void FixedUpdate()
    {
        motorPlayer.Tick();
    }

    private void Update()
    {
        
    }

    private void LoadSettings() 
    {
        float getingFloat = PlayerPrefs.GetFloat("sensetivity", 50.0f);
        motorPlayer.sensetivity = getingFloat;

        string getingKey = PlayerPrefs.GetString("keyJump", "Space");
        motorPlayer.keyJump = FindKey(getingKey);
    }

    private KeyCode FindKey(string name = "None")
    {
        foreach(KeyCode soughtKey in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (soughtKey.ToString() == name) return soughtKey;
        }

        return KeyCode.None;
    }

}
