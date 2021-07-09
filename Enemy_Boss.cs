using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Boss : Enemy
{
    public float speed;//set speed
    public float chaseDistance;//distance from player to chase
    public float stopDistance;//distance to player before stopping
    public GameObject target;//target refers to player
    public int maxHealth = 400;//starting health
    public Transform attackPoint;//point of attack where the player can be hit from
    public float attackRange = 1f;//how far it reaches
    public LayerMask playerLayer;//layer for character
    public float attackRate = 2f;//rate that the attacks are spaced out
    float nextAttackTime = 0f;//time between attacks
    float attackDelay = 2f;//starting delay of intial attack
    float prepareAttack = 0f;
    private float targetDistance;//how far the character is
    public Animator animator;
    void Start()
    {
        animator.GetComponent<Animator>();

    }



    void Update()
    {
        targetDistance = Vector2.Distance(transform.position, target.transform.position);
        if (targetDistance < chaseDistance && targetDistance > stopDistance)//if player is in distance
            ChasePlayer();
        else
            StopChasePlayer();

    }

    private void ChasePlayer()//closes into player
    {
        if (prepareAttack > 0)
            prepareAttack = 0f;
        if (transform.position.x < target.transform.position.x)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        animator.SetBool("IsWalking", true);//triggers walking animation
    }
    private void StopChasePlayer()//stops the enemy from chasing
    {
        animator.SetBool("IsWalking", false);
        prepareAttack = Time.time + 2f / attackRate;
        int playerHealth = PlayerCombat.currentHealth;//tracks player's health
        if (Time.time >= nextAttackTime && prepareAttack > attackDelay)
        {
            if (targetDistance <= attackRange && playerHealth > 0)//will only attack if in range and the character is alive
            {
                animator.SetTrigger("Attack");//attack animation trigger
                nextAttackTime = Time.time + 2f / attackRate;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        maxHealth -= damage;
        animator.SetTrigger("Hurt");
        if (maxHealth <= 0)
        {
            Debug.Log("Enemy died!");//tracks deaths
            animator.SetBool("isDead", true);//death animation trigger
            GetComponent<Collider2D>().enabled = false;//Disable enemy
            this.enabled = false;
        }
    }
    void AttackPlayer()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);//checks if player is hit in range
        foreach (Collider2D player in hitPlayer)
        {
            player.GetComponent<PlayerCombat>().TakeDamage(25);
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}