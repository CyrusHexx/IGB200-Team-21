using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] tutorialPages; // Array of all tutorial panels (pages)
    private int currentPageIndex = 0;

    private void Start()
    {
        ShowPage(currentPageIndex); // Show the first page initially
    }

    public void NextPage()
    {
        if (currentPageIndex < tutorialPages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
        }
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    private void ShowPage(int pageIndex)
    {
        // First deactivate all pages
        foreach (GameObject page in tutorialPages)
        {
            page.SetActive(false);
        }

        // Now activate the desired page
        tutorialPages[pageIndex].SetActive(true);
    }

    public void CloseTutorial()
    {
        gameObject.SetActive(false); // Hide the main tutorial panel
        currentPageIndex = 0; // Reset page index for next time
    }
}
