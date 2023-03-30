using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Title Screen");
    }

    public void PlayButton()
    {
        // Stop the title music.
        GameObject.Find("TitleMusic").GetComponent<TitleMusic>().End();
        // Load the level.
        SceneManager.LoadScene("Level1");
    }

    public void ControlsButton()
    {
        SceneManager.LoadScene("Controls Screen");
    }

    public void LoreButton()
    {
        SceneManager.LoadScene("Lore Screen");
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void DeathLoreButton()
    {
        SceneManager.LoadScene("Death Lore");
    }

    public void LostSoulsLoreButton()
    {
        SceneManager.LoadScene("Lost Souls Lore");
    }

    public void AlanTuringLoreButton()
    {
        SceneManager.LoadScene("Alan Turing Lore");
    }
}
