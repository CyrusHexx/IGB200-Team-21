using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void TryAgainButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
