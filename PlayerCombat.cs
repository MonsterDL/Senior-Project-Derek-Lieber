using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;//animator from unity
    public Transform attackPoint;//refrence to attack point assest in Unity
    public float attackRange = 0.5f;//Range of the hit box of the attack point
    public LayerMask enemyLayers;//layer to be identifiied with enemies
    public LayerMask bossLayers;//layer to be identifiied with the boss
    public float attackRate = 2f;//the speed of an attack
    float nextAttackTime = 0f;//the time before next attack
    public int playerHealth=100;//starting health
    public static int currentHealth;//health at a single moment


    // Update is called once per frame
    void Start()
    {
        currentHealth = playerHealth;//sets the currentHealth to maxHealth to begin with
    }
    void Update()
    {
        if (Time.time >= nextAttackTime)//Checks if enough time has passed before the player can attack again
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))//left mouse triggers this
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;//begins timer to seperate attacks
            }

        }
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;//subtracts damage from currentHealth, damage comes from the enemy classes
        animator.SetTrigger("Hurt");//causes hurt animation
        if (currentHealth <= 0)//if dead
        {
            Debug.Log("Player has died!");//to keep track of the time of death
            animator.SetBool("IsDead", true);//triggers death animation
        }
    }
    void Attack()
    {
        animator.SetTrigger("Attack");//animation for Attack is triggered
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);//detects enemies in range of attack and damages them
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemySkeleton>().TakeDamage(40);
        }
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, bossLayers);
        foreach (Collider2D boss in hitBoss)
        {
            boss.GetComponent<Enemy_Boss>().TakeDamage(40);
        }
        
    }

    void OnDrawGizmosSelected()//Gives a visualization to the attackPoint to be ajusted as need be
    {
        if (attackPoint == null)//if there isn't an attackPoint, prevents error
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
