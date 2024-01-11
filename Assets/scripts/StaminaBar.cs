using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;
    [SerializeField] public int maxStamina, currentStamina;
    //[SerializeField] public TagEasyAI agent;

    public static StaminaBar instance;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = maxStamina;
    }

    public void useStamina(int amount)
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;
        }
    }

    public void gainStamina(int amount)
    {
        if (currentStamina + amount <= maxStamina)
        {
            currentStamina += amount;
            staminaBar.value = currentStamina;
        }
    }
}
