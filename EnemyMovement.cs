using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public Animator EnemyAnimator;

    int Hearths = 2;
    private bool Hitted = false;
    private bool Dead = true;
    private float AttackStartTime;

    void Start()
    {
        myRigidBody.velocity = new Vector2(Random.Range(-10, 0)/(float)10, Random.Range(-10, 0) / (float)10) * 3;
    }

    void Update()
    {
        if (Hitted && Time.time - AttackStartTime > 0.45)
        {
            EnemyAnimator.SetBool("Hitted", false);
            Hitted = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name.StartsWith("Player"))
        {
            Movement playerScript = FindObjectOfType<Movement>();

            if(playerScript.isAttcking())
            {

                if(playerScript.hasGoldenSword())
                {
                    Die();
                }
                else
                {
                    Hearths--;

                    if(Hearths == 0)
                    {
                        Die();
                    }
                    else
                    {
                        Hitted = true;
                        EnemyAnimator.SetBool("Hitted", true);
                        AttackStartTime = Time.time;
                    }
                }                
            }
        }
    }

    private void Die()
    {
        EnemyAnimator.SetBool("Dead", true);
        myRigidBody.mass = 1;
        myRigidBody.velocity = new Vector2(0, -3);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}