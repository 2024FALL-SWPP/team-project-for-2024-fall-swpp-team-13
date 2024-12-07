using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    [SerializeField] float rechargeRate = 7.0f;
    private Slider staminaSlider;
    private Coroutine recharging;
    private StageManager stageManager;

    void Awake()
    {
        staminaSlider = GetComponent<Slider>();
        stageManager = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetMaxStamina();
    }

    private void SetMaxStamina()
    {
        stageManager.stamina = stageManager.maxStamina;
        staminaSlider.maxValue = stageManager.maxStamina;
        staminaSlider.value = stageManager.maxStamina;
    }

    public void RunStamina(float staminaCost)
    {
        stageManager.stamina = (stageManager.stamina >= staminaCost) ? stageManager.stamina - staminaCost : 0.0f;
        staminaSlider.value = stageManager.stamina;

        // Recharging stamina coroutine happens uniquely
        if (recharging != null) StopCoroutine(recharging);
        recharging = StartCoroutine(RechargeStamina());
    }

    public bool CanUsePowerup(float staminaCost)
    {
        return stageManager.stamina >= staminaCost;
    }

    public bool isEmpty()
    {
        return stageManager.stamina <= 0.0f;
    }

    IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1);
        while (stageManager.stamina < stageManager.maxStamina)
        {
            // Recharge Stamina by 'rechargeRate' every second
            stageManager.stamina += rechargeRate / 50.0f;
            if (stageManager.stamina >= stageManager.maxStamina) stageManager.stamina = stageManager.maxStamina;
            staminaSlider.value = stageManager.stamina;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
