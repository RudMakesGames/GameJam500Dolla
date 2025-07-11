using UnityEngine;
using UnityEngine.UI;

public class DotIndex : MonoBehaviour
{
    public int dotIndex;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => ConnectDots());
    }

    public void ConnectDots()
    {
        
    }
}
