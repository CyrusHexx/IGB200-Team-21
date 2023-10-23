using UnityEngine;

public class WireManager : MonoBehaviour
{
    public GameObject fuseBoxGamePanel;
    public GameObject finishNodeObject;
    public GameObject GhostOne;
    public GameObject GhostTwo;

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

            // Restart the power.
            GameManager.instance.resetLoad();

            // Increase ghost speeds.
            GhostOne.GetComponent<Ghost>().ghostNavMesh.speed = 10f;
            GhostTwo.GetComponent<Ghost>().ghostNavMesh.speed = 10f;

            // Reactivate the ghosts.
            GhostOne.SetActive(true);
            GhostTwo.SetActive(true);

            // Hide the fuse box game panel after winning.
            if (fuseBoxGamePanel)
                fuseBoxGamePanel.SetActive(false);

            enabled = false;  // Disable the WireManager after completing the puzzle.
        }
    }



    bool IsPuzzleComplete()
    {
        // Check if the Finish node's isOn property is true
        FuseBoxGameState finishNodeCollision = finishNodeObject.GetComponent<FuseBoxGameState>();
        return finishNodeCollision && finishNodeCollision.isOn;
    }
}
