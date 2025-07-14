using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LanternController : MonoBehaviour
{
    public float MaxIntensity = 21f;
    public float LightDeductionAmount = 0.5f;
    public Light2D LanternLight;
    public Slider IntensitySlider;

    private bool HasTriggeredPlatform = false;
    private DamageFlash damageFlash;

    public AudioSource Heartbeat;
    public float VolumeInc;
    public enum LightIntensityState { Low, Middle, High }
    public LightIntensityState CurrentState;

    [Header("Thresholds")]
    public float LowThreshold = 5f;
    public float MiddleThreshold = 12f;

    [Header("Vignette Settings")]
    public Volume globalVolume;
    private Vignette vignette;

    void Start()
    {
        damageFlash = GetComponentInParent<DamageFlash>();

        if (globalVolume != null && globalVolume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }
        else
        {
            Debug.LogWarning("Vignette not found on volume!");
        }

        LanternLight.intensity = MaxIntensity;
    }

    public void RestoreLight(float Amount)
    {
        if(LanternLight.intensity < MaxIntensity)
        LanternLight.intensity += Amount;
    }
    public void IncreaseLightConsumption(InputAction.CallbackContext context)
    {
        if (context.performed && LanternLight.intensity < MaxIntensity && !CutsceneManager.instance?.isCutsceneActive == true)
        {
            
            LanternLight.intensity = Mathf.Min(MaxIntensity, LanternLight.intensity + 3f);
        }
    }

    public void DecreaseLightConsumption(InputAction.CallbackContext context)
    {
        if (context.performed && !CutsceneManager.instance?.isCutsceneActive == true)
        {
            
            LanternLight.intensity = Mathf.Max(0f, LanternLight.intensity - 3f);
        }
    }

    public void DamageLight(float damage)
    {
        damageFlash?.CallDamageFlash();
        LanternLight.intensity -= damage;
        if (LanternLight.intensity <= 0)
        {
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        Destroy(gameObject.GetComponentInParent<Movement>().gameObject);
    }

    void Update()
    {
        if (!CutsceneManager.instance?.isCutsceneActive == true)
        {
            LanternLight.intensity -= LightDeductionAmount * Time.deltaTime;
            LanternLight.intensity = Mathf.Clamp(LanternLight.intensity, 0, MaxIntensity);
        }

        IntensitySlider.value = LanternLight.intensity;
        UpdateLightState();
    }

    void UpdateLightState()
    {
        if (LanternLight.intensity <= LowThreshold)
            CurrentState = LightIntensityState.Low;
        else if (LanternLight.intensity <= MiddleThreshold)
            CurrentState = LightIntensityState.Middle;
        else
            CurrentState = LightIntensityState.High;

        // Visual feedback through lantern color
        switch (CurrentState)
        {
            case LightIntensityState.Low:
                Heartbeat.volume = VolumeInc;
                Heartbeat.Play();
                if (vignette != null) vignette.intensity.Override(0.5f);
                break;
            case LightIntensityState.Middle:
                Heartbeat.volume = 0.3f;
                Heartbeat.Play();
                if (vignette != null) vignette.intensity.Override(0.325f);
                break;
            case LightIntensityState.High:
                if(Heartbeat.isPlaying)
                {
                    Heartbeat.Stop();
                }
                if (vignette != null) vignette.intensity.Override(0.05f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidden"))
            collision.GetComponent<SpriteRenderer>().enabled = true;

        if (collision.GetComponent<MovingPlatform>() is MovingPlatform movingPlatform)
        {
            if (LanternLight.intensity >= movingPlatform.Threshold && !HasTriggeredPlatform)
            {
                movingPlatform.TriggerPlatformMovement();
                HasTriggeredPlatform = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Hidden"))
            collision.GetComponent<SpriteRenderer>().enabled = false;

        if (collision.GetComponent<MovingPlatform>() != null)
            HasTriggeredPlatform = false;
    }


}
