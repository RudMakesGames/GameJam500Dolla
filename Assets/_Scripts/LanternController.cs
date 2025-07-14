using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LanternController : MonoBehaviour
{
    public float CurrentLight;
    public float MaxLight;
    public float LightDeductionAmount;
    public float elapsedTime;
    public Light2D LanternLight;
    private bool HasTriggeredPlatform = false;
    public Slider IntensitySlider;
    public enum LightIntensityState
    {
        LowIntensity,
        MiddleIntensity,
        HighIntensity,
    }
    public LightIntensityState CurrentState;

    [Header("Intensity Thresholds")]
    public float LowThreshold = 25f;
    public float MiddleThreshold = 60f;
    [SerializeField]
    private DamageFlash damageFlash;
    void Start()
    {
        damageFlash = GetComponentInParent<DamageFlash>();
        CurrentLight = MaxLight;
    }
    public void RestoreLight(float light)
    {
        if(CurrentLight < MaxLight)
        {
            CurrentLight += light;
        }
        
    }
    public void DamageLight(float light)
    {
        damageFlash?.CallDamageFlash();
        CurrentLight -= light;
        if (CurrentLight <= 0)
        {
            HandleDeath();
        }    
    }
    public void HandleDeath()
    {
        Destroy(gameObject.GetComponentInParent<Movement>().gameObject);
    }
    public void IncreaseLightConsumption(InputAction.CallbackContext context)
    {
        if (context.performed && LanternLight.intensity < 21f)
        {
            LightDeductionAmount += 0.5f;
            LanternLight.intensity = LanternLight.intensity + 3f;
        }
            
    }
    public void DecreaseLightConsumption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LightDeductionAmount = Mathf.Max(0.1f, LightDeductionAmount - 0.5f);
            if(LanternLight.intensity > 0)
            {
                LanternLight.intensity = LanternLight.intensity - 3f;
                if (LanternLight.intensity <= 0)
                {
                    LanternLight.intensity = 0;
                }
            }
            
            
        }
            
    }
    public void DiminishLightOverPeriodOfTime(float DiminishingAmount)
    {
        CurrentLight -= DiminishingAmount * Time.deltaTime;
        CurrentLight = Mathf.Clamp(CurrentLight, 0, MaxLight);
        if (CurrentLight <= 0)
        {
            HandleDeath();
        }
    }
    void CheckLightIntensityState()
    {
        if (CurrentLight <= LowThreshold)
        {
            CurrentState = LightIntensityState.LowIntensity;
        }
        else if (CurrentLight <= MiddleThreshold)
        {
            CurrentState = LightIntensityState.MiddleIntensity;
        }
        else
        {
            CurrentState = LightIntensityState.HighIntensity;
        }

        switch (CurrentState)
        {
            case LightIntensityState.LowIntensity:
                LanternLight.color = Color.red;
                break;

            case LightIntensityState.MiddleIntensity:
                LanternLight.color = Color.yellow;
                break;

            case LightIntensityState.HighIntensity:
                LanternLight.color = Color.white;
                break;
        }
    }
        void Update()
    {
        IntensitySlider.value = LanternLight.intensity;
        DiminishLightOverPeriodOfTime(LightDeductionAmount);
        CheckLightIntensityState();
    }
   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hidden"))
        {
            collision.GetComponent<SpriteRenderer>().enabled = true;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            MovingPlatform movingPlatform = collision.gameObject.GetComponent<MovingPlatform>();
            if (LanternLight.intensity >= movingPlatform.Threshold)
            {
                Debug.Log("Lantern triggered platform");
                movingPlatform.TriggerPlatformMovement();
                HasTriggeredPlatform = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hidden"))
        {
            collision.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (collision.gameObject.GetComponent<MovingPlatform>() != null)
        {
            HasTriggeredPlatform = false;
        }
    }
}
