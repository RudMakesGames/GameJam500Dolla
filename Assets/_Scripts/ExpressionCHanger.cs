using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExpressionCHanger : MonoBehaviour
{
    private void Start()
    {
        intensity = GetComponent<Slider>();
    }
    Slider intensity;
    [SerializeField] Sprite expressionSad, expressionDepressed, expressionNeutral;
    [SerializeField] UnityEngine.UI.Image Expression;
    public void ChangeExpression()
    {
        if (intensity.value > 7)
        {
            Expression.sprite = expressionNeutral;
        }
        else if (intensity.value < 7 && intensity.value > 3)
        {
            Expression.sprite = expressionSad;
        }
        else
            Expression.sprite = expressionDepressed;

    }
}
