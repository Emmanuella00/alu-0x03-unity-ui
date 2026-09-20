using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // needed for Toggle

public class MainMenu : MonoBehaviour
{
    // Task 12: assign these in the Inspector --
    // trapMat/goalMat are the maze's actual trap/goal materials,
    // colorblindMode is the OptionsMenu > ColorblindMode toggle
    public Material trapMat;
    public Material goalMat;
    public Toggle colorblindMode;

    // Task 9: loads the maze scene when PlayButton is clicked
    public void PlayMaze()
    {
        // Task 12: swap material colors before the maze loads, based on
        // whether Colorblind Mode is checked
        if (colorblindMode != null && colorblindMode.isOn)
        {
            // Orange trap / blue goal -- distinguishable across most
            // types of color vision deficiency, unlike red/green
            trapMat.color = new Color32(255, 112, 0, 1);
            goalMat.color = Color.blue;
        }
        else
        {
            // Original red trap / green goal
            trapMat.color = Color.red;
            goalMat.color = Color.green;
        }

        SceneManager.LoadScene("maze");
    }

    // Task 10: closes the game window when QuitButton is clicked.
    // Application.Quit() does nothing in the Editor Play mode (by design --
    // Unity has no "window" to close there), so the Debug.Log is how you
    // confirm the button is wired correctly while testing in-editor.
    public void QuitMaze()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
