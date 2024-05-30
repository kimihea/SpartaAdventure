using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    public bool attacking;
    public float attackDistance;
    public float useStamina;
    [Header("Resource Gathering")]
    public bool doesGatherResources;

    [Header("COmbat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera camera;
    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }

    }
    void OnCanAttack()
    {
        attacking = false;
    }
}
