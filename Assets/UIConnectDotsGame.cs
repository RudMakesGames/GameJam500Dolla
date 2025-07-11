using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;

public class UIConnectDotsGame : MonoBehaviour
{
    [Header("Setup")]
    public List<RectTransform> dotOrder; // Set these in the order the player should connect
    public GameObject linePrefab;        // Thin UI image prefab used as a line
    public RectTransform lineParent;     // UI parent container for lines

    private int currentIndex = 0;
    private List<GameObject> drawnLines = new List<GameObject>();
    List<Animator> lineAnimators = new List<Animator>();

    void Start()
    {
        foreach (RectTransform dot in dotOrder)
        {
            Button b = dot.GetComponent<Button>();
            if (b != null)
            {
                b.onClick.AddListener(() => OnDotClicked(dot));
            }
        }
    }

    void OnDotClicked(RectTransform clickedDot)
    {
        if (clickedDot == dotOrder[currentIndex])
        {
            if (currentIndex > 0)
            {
                DrawLine(dotOrder[currentIndex - 1], clickedDot);
            }

            currentIndex++;

            if (currentIndex >= dotOrder.Count)
            {
                OnPuzzleComplete();
            }
        }
    }

    void DrawLine(RectTransform start, RectTransform end)
    {
        GameObject lineObj = Instantiate(linePrefab, lineParent);
        RectTransform lineRect = lineObj.GetComponent<RectTransform>();

        Vector2 startPos, endPos;

        // Convert world positions to local UI positions
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineParent, RectTransformUtility.WorldToScreenPoint(null, start.position), null, out startPos);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineParent, RectTransformUtility.WorldToScreenPoint(null, end.position), null, out endPos);

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        lineRect.sizeDelta = new Vector2(distance, 5f);
        lineRect.anchoredPosition = startPos + direction / 2f;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        lineRect.localRotation = Quaternion.Euler(0, 0, angle);
        Animator animator = lineObj.GetComponent<Animator>();
        if (animator != null)
        {
            lineAnimators.Add(animator);
        }
    }

    void OnPuzzleComplete()
    {
        Debug.Log(" Puzzle Complete!");
        foreach (Animator animator in lineAnimators)
        {
            animator.SetTrigger("Connected"); // Make sure the Animator has a trigger named "Play"
        }
    }

    // Optional: Call this to reset the puzzle
    public void ResetPuzzle()
    {
        currentIndex = 0;
        foreach (var line in drawnLines)
        {
            Destroy(line);
        }
        drawnLines.Clear();
    }
}
