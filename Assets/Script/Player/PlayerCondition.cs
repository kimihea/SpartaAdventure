using Unity.VisualScripting;
using UnityEngine;
using System;
using System.Collections;
public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    public Action<float> staminaLose;
    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }
    public float staminaLosing;
    public bool staminaOring;
    public float noHungerHealthDecay;
    public event Action onTakeDamage;
    void Awake()
    {
        staminaLosing = 0;
        staminaLose += UsingSkill;
    }
    void Update()
    {
        stamina.Add(stamina.passiveValue * Time.deltaTime);
        if (health.curValue <= 0f)
        {
            Die();
        }
        if(!staminaOring)
            staminaLose?.Invoke(staminaLosing * Time.deltaTime);

        if (stamina.curValue > stamina.maxValue / 2f)
        {
            staminaOring = false;
        }
    }
    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void StaminaPlus(float amount)
    {
        stamina.Add(amount);
    }
    public void Die()
    {
        //playerDie!
    }
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
    public void UseSkill(float amount)
    {
        staminaLosing += amount;
    }
    public void UsingSkill(float amount)
    {
        if (stamina.curValue - amount <= 0f)
        {
            staminaOring = true;
            return;
        }
        stamina.Subtract((float)amount);
    }
}