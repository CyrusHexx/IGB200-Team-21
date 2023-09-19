using UnityEngine;
using UnityEngine.SceneManagement;

public class Loss : MonoBehaviour
{
    public void TryAgainButton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
