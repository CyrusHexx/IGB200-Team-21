using UnityEngine;

public class WireManager : MonoBehaviour
{
    public GameObject fuseBoxGamePanel; // Drag your FuseBoxGame panel here in the inspector
    public GameObject finishNodeObject; // Public reference for easy assignment in the Inspector

    void Start()
    {
        if (!finishNodeObject)
        {
            Debug.LogError("Finish node not assigned in the inspector.");
        }
    }

    void Update()
    {
        // Continuously check if the puzzle is completed
        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        if (IsPuzzleComplete())
        {
            Debug.Log("Puzzle completed!");

            // Hide the fuse box game panel after winning.
            if (fuseBoxGamePanel)
                fuseBoxGamePanel.SetActive(false);

            // Restart power immediately after completing the puzzle.
            PowerFuseBox powerfusebox = FindObjectOfType<PowerFuseBox>();
            if (powerfusebox)
            {
                powerfusebox.powerRestart();
                GameManager.instance.resetLoad();
            }

            enabled = false;
        }
    }


    bool IsPuzzleComplete()
    {
        // Check if the Finish node's isOn property is true
        FuseBoxGameState finishNodeCollision = finishNodeObject.GetComponent<FuseBoxGameState>();
        return finishNodeCollision && finishNodeCollision.isOn;
    }
}
