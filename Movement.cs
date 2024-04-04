using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public Rigidbody2D myRigidBody;
    public Animator PlayerAnimator;

    bool InJump = false;
    bool Attacking = false;
    bool Dead = false;
    private float AttackStartTime;
    int GolderSwordHits = 0;
    string NameAttackAnimation = "Attacking";

    int Score = 0;
    ScoreHandler ScoreHandle;

    void Start()
    {
        myRigidBody.velocity = new Vector2(1, 0) * 3;
        ScoreHandle = FindObjectOfType<ScoreHandler>();
    }

    void Update()
    {
        if(!Dead && Attacking && Time.time - AttackStartTime > 0.5)
        {
            Attacking = false;
            PlayerAnimator.SetBool("AttackingG", Attacking);
            PlayerAnimator.SetBool("Attacking",  Attacking);

            if (InJump)
                PlayerAnimator.SetBool("InJump", InJump);
        }

        if(myRigidBody.transform.position.y < - 10)
        {
            Dead = true;
            PlayAgainHandler endScreen = FindObjectOfType<PlayAgainHandler>();
            endScreen.ShowPlayAgain();
            myRigidBody.velocity = new Vector2(0, 0);
        }

        ScoreHandle.setScore(Score);
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if(Collision.gameObject.name.Equals("Terrain_mid(Clone)") || Collision.gameObject.name.Equals("Terrain_left(Clone)") || Collision.gameObject.name.Equals("Terrain_right(Clone)") && InJump)
        {
            Score++;
            InJump = false;
            PlayerAnimator.SetBool("InJump", InJump);
        }
        else if(Collision.gameObject.name.StartsWith("Enemy"))
        {
            if(Attacking == false)
            {
                Dead = true;
                PlayerAnimator.SetBool("Dead", true);
                myRigidBody.velocity = new Vector2(0, 0);

                PlayAgainHandler endScreen = FindObjectOfType<PlayAgainHandler>();
                endScreen.ShowPlayAgain();
            }
            else
            {
                Score += 3;
                if (GolderSwordHits > 0)
                {
                    GolderSwordHits--;

                    Score += 3;

                    if (GolderSwordHits == 0)
                        NameAttackAnimation = "Attacking";
                }
            }
        }
        else if (Collision.gameObject.name.StartsWith("Golden"))
        {
            Score += 2;
            GolderSwordHits = 2;
            NameAttackAnimation = "AttackingG";
        }
    }

    public void Jump(InputAction.CallbackContext value)
    {
        if(!Dead && value.started && !InJump)
        {
            myRigidBody.velocity = new Vector2(1, 3) * 3;
            InJump = true;
            PlayerAnimator.SetBool("InJump", InJump);
        }
        else if(value.canceled)
        {

        }
    }

    public void Attack(InputAction.CallbackContext value)
    {
        if (!Dead && value.started && !Attacking)
        {
            Attacking = true;
            PlayerAnimator.SetBool(NameAttackAnimation, Attacking);
            PlayerAnimator.SetBool("InJump", false);
            AttackStartTime = Time.time;
        }
        else if (value.canceled)
        {

        }
    }

    public void Roll(InputAction.CallbackContext value) //Play Again
    {
        if (value.started)
        {
            PlayAgainHandler endScreen = FindObjectOfType<PlayAgainHandler>();
            endScreen.Dismiss();

            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
        else if (value.canceled)
        {

        }
    }

    public bool isAlive()
    {
        return !Dead;
    }

    public bool isAttcking()
    {
        return Attacking;
    }

    public bool hasGoldenSword()
    {
        return GolderSwordHits > 0;
    }
}
