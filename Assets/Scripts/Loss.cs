using UnityEngine;
using UnityEngine.SceneManagement;

public class Loss : MonoBehaviour
{
    public void TryAgainButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
