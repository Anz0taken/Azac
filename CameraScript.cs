using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Camera myMainCamera;
    public Transform myCameraTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var movementScript = FindObjectOfType<Movement>();
        myCameraTransform.position = new Vector3(movementScript.myRigidBody.position.x, CommonConstants.COMMON_Y_VALUE, myCameraTransform.position.z);
    }
}
