using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordMovement : MonoBehaviour
{
    public Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D Collision)
    {
        if (Collision.gameObject.name.StartsWith("Player"))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
