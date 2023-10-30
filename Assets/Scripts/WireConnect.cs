using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using System;


public class WireConnect : MonoBehaviour
{
    public Button[] wiresLeft;
    public LineRenderer[] wireLineRenderers;
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

        Shuffle(wiresLeft, wiresRight);

        for (int i = 0; i < wiresLeft.Length; i++)
        {
            wireLineRenderers[i].positionCount = 2;
            wireLineRenderers[i].SetPosition(0, wiresLeft[i].transform.position);
            wireLineRenderers[i].SetPosition(1, wiresLeft[i].transform.position);
        }

    }

    private void Shuffle(Button[] wireButtons, Image[] wireImages)
    {
        wireButtons = wireButtons.OrderBy(x => UnityEngine.Random.value).ToArray();
        wireImages = wireImages.OrderBy(x => UnityEngine.Random.value).ToArray();

        for (int i = 0; i < wireButtons.Length; i++)
        {
            wireButtons[i].transform.position = initialPositionsLeft[i];
            wireImages[i].transform.position = initialPositionsRight[i];
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

        int index = Array.IndexOf(wiresLeft, draggedWire.GetComponent<Button>());
        if (index != -1)
        {
            wireLineRenderers[index].SetPosition(1, Input.mousePosition);
        }
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
        int index = Array.IndexOf(wiresLeft, draggedWire.GetComponent<Button>()); 

        for (int i = 0; i < wiresRight.Length; i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(wiresRight[i].rectTransform, Input.mousePosition))
            {
                if (ColorsApproximatelyEqual(draggedWire.color, wiresRight[i].color))
                {
                    draggedWire.transform.position = wiresRight[i].transform.position;
                    draggedWire.raycastTarget = false;

                    if (index != -1)
                    {
                        wireLineRenderers[index].SetPosition(1, wiresRight[i].transform.position);
                    }

                    isMatched = true;
                    correctlyConnectedCount++;
                    CheckWinCondition();

                    break;
                }
            }
        }

        if (!isMatched)
        {
            draggedWire.transform.position = initialPosition;

            // Reset the LineRenderer's end position to the initial position
            if (index != -1)
            {
                wireLineRenderers[index].SetPosition(1, wiresLeft[index].transform.position);
            }
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
    }

    public void ShowGame()
    {
        Shuffle(wiresLeft, wiresRight);
        ResetWireRaycasts();
        this.gameObject.SetActive(true);
        correctlyConnectedCount = 0;

        EventSystem.current.SetSelectedGameObject(null);
    }


    private void ResetWireRaycasts()
    {
        foreach (var wire in wiresLeft)
        {
            wire.GetComponent<Image>().raycastTarget = true;
        }
    }
}