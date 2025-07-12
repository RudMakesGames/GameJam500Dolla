using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true, true)]
    [SerializeField] private Color FlashColor = Color.white;
    [SerializeField] private float FlashTime = 0.25f;
    [SerializeField] private AnimationCurve FlashCurve;
    [SerializeField]
    private SpriteRenderer[] spriteRenderers;
    [SerializeField]
    private Material[] materials;
    private Coroutine DamageFlashCoroutine;
    private void Awake()
    {
        spriteRenderers = GetComponents<SpriteRenderer>();
        Init();
    }
    public void CallDamageFlash()
    {
        DamageFlashCoroutine = StartCoroutine(DamageFlasher());
    }
    private void Init()
    {
        materials = new Material[spriteRenderers.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = spriteRenderers[i].material;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount = 0f;
        float ElapsedTime = 0f;

        while (ElapsedTime < FlashTime)
        {
            ElapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, FlashCurve.Evaluate(ElapsedTime), (ElapsedTime / FlashTime));
            setFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    void SetFlashColor()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetColor("_FlashColor", FlashColor);

        }
    }
    void setFlashAmount(float amount)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].SetFloat("_FlashAmount", amount);
        }
    }
}
