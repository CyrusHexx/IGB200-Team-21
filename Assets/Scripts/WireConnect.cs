using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;


public class WireConnect : MonoBehaviour
{
    public Button[] wiresLeft;
    public Image[] wiresRight;

    private Vector3[] initialPositionsLeft;
    private Vector3[] initialPositionsRight;


    private Image draggedWire;
    private Vector3 initialPosition;
    private int gamesTriggered = 0;

    private int correctlyConnectedCount = 0;

    public event Action OnGameCompleted;


    void Start()
    {
        HideGame();

        initialPositionsLeft = wiresLeft.Select(wire => wire.transform.position).ToArray();
        initialPositionsRight = wiresRight.Select(wire => wire.transform.position).ToArray();

        ShuffleAndResetPositions(); // Shuffle and reset positions when the game starts.
    }

    private void ShuffleAndResetPositions()
    {
        Shuffle(wiresLeft);
        Shuffle(wiresRight);

        for (int i = 0; i < wiresLeft.Length; i++)
        {
            wiresLeft[i].transform.position = initialPositionsLeft[i];
            wiresRight[i].transform.position = initialPositionsRight[i];
        }
    }

    private void Shuffle<T>(T[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            T tmp = array[i];
            array[i] = array[r];
            array[r] = tmp;
        }
    }


    public void BeginDragSimple()
    {
        draggedWire = EventSystem.current.currentSelectedGameObject.GetComponent<Image>();
        initialPosition = draggedWire.transform.position;
    }

    public void DragSimple()
    {
        if (draggedWire == null) return;
        draggedWire.transform.position = Input.mousePosition;
    }

    private bool ColorsApproximatelyEqual(Color a, Color b, float tolerance = 0.05f)
    {
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance &&
               Mathf.Abs(a.a - b.a) < tolerance;
    }

    public void EndDragSimple()
    {
        if (draggedWire == null) return;

        bool isMatched = false;

        for (int i = 0; i < wiresRight.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(wiresRight[i].rectTransform, Input.mousePosition))
            {
                if (ColorsApproximatelyEqual(draggedWire.color, wiresRight[i].color))
                {
                    draggedWire.transform.position = wiresRight[i].transform.position;
                    draggedWire.raycastTarget = false;

                    isMatched = true;
                    correctlyConnectedCount++; // Increment the counter
                    CheckWinCondition();

                    break;
                }
            }
        }

        if (!isMatched)
        {
            draggedWire.transform.position = initialPosition;
        }

        draggedWire = null;
    }


    private void CheckWinCondition()
    {
        if (correctlyConnectedCount == wiresLeft.Length)
        {
            Debug.Log("Game completed event being invoked!");
            OnGameCompleted?.Invoke();
            HideGame();
            Debug.Log("Checking win condition!");
        }
    }

    public void HideGame()
    {
        this.gameObject.SetActive(false);
        ShuffleAndResetPositions(); // Shuffle and reset positions when the game ends.
    }

    public void ShowGame()
    {
        this.gameObject.SetActive(true);
        correctlyConnectedCount = 0; // Reset the counter when the game starts or restarts

        ShuffleAndResetPositions(); // Shuffle and reset positions when the game starts.
    }
}