using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public GameObject[,] BgTrees;

    struct BackgroundMovement
    {
        public float Speed;
        public float OffSet;
        public float ImageDifference;
    }

    BackgroundMovement[] BackgroundInfos;

    void Start()
    {
        InitializeMatrixs();
        InitializeBackgroundInfo();
    }

    void Update()
    {
        UpdateBackground();
    }

    private void InitializeMatrixs()
    {
        BgTrees = new GameObject[4, 2];

        for (int i = 0; i < 4; i++)
            for (int j = 0; j < 2; j++)
            {
                BgTrees[i, j] = GameObject.FindGameObjectWithTag("BgTrees_" + (j + 1) + "_" + (i + 1));
            }
    }

    private void InitializeBackgroundInfo()
    {
        BackgroundInfos = new BackgroundMovement[4];
        BackgroundInfos[0].ImageDifference = (float)21.981;
        BackgroundInfos[3].ImageDifference = 19;
        BackgroundInfos[1].ImageDifference = (float)20.98;
        BackgroundInfos[2].ImageDifference = (float)21.98;

        for (int i = 0; i < 4; i++)
        {
            BackgroundInfos[i].OffSet = 0;
            BackgroundInfos[i].Speed = -(float)(i + 1);
        }
    }

    private void UpdateBackground()
    {
        Movement movementScript = FindObjectOfType<Movement>();

        for (int i = 0; i < BackgroundInfos.Length; i++)
        {
            BackgroundInfos[i].OffSet += BackgroundInfos[i].Speed * Time.deltaTime;

            for (int j = 0; j < BgTrees.GetLength(1); j++)
            {
                BgTrees[i, j].transform.position = new Vector3(
                    movementScript.myRigidBody.position.x + BackgroundInfos[i].OffSet + BackgroundInfos[i].ImageDifference * j,
                    CommonConstants.COMMON_Y_VALUE,
                    BgTrees[i, j].transform.position.z
                );
            }

            CheckAndResetPosition(i);
        }
    }

    private void CheckAndResetPosition(int index)
    {
        Transform myCameraTransform = FindAnyObjectByType<CameraScript>().myCameraTransform;
        float cameraX = myCameraTransform.position.x;

        Renderer leftRenderer = BgTrees[index, 0].GetComponent<Renderer>();

        float leftImageWidth = leftRenderer.bounds.size.x;

        if (cameraX - BgTrees[index, 0].transform.position.x > leftImageWidth)
        {
            BgTrees[index, 0].transform.position = BgTrees[index, 1].transform.position;
            BgTrees[index, 1].transform.position = BgTrees[index, 1].transform.position + new Vector3(leftImageWidth, 0f, 0f);
            BackgroundInfos[index].OffSet = 0;
        }
    }
}
