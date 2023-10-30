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
        CheckPuzzleCompletion();
    }

    void CheckPuzzleCompletion()
    {
        if (IsPuzzleComplete())
        {
            Debug.Log("Puzzle completed!");

            GameManager.instance.resetLoad();

            GhostOne.GetComponent<Ghost>().ghostNavMesh.speed = 10f;
            GhostTwo.GetComponent<Ghost>().ghostNavMesh.speed = 10f;

            GhostOne.SetActive(true);
            GhostTwo.SetActive(true);

            if (fuseBoxGamePanel)
                fuseBoxGamePanel.SetActive(false);

            enabled = false;  // Disable the WireManager after completing the puzzle.
        }
    }



    bool IsPuzzleComplete()
    {
        FuseBoxGameState finishNodeCollision = finishNodeObject.GetComponent<FuseBoxGameState>();
        return finishNodeCollision && finishNodeCollision.isOn;
    }
}
