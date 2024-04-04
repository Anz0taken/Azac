using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandler : MonoBehaviour
{
    public const float TERRAIN_SIZE = (float)0.705;
    public const int TERRAIN_Z = -2;

    public GameObject Terrain_left;
    public GameObject Terrain_mid;
    public GameObject Terrain_right;
    public GameObject Terrain_left_under;
    public GameObject Terrain_mid_under;
    public GameObject Terrain_right_under;

    public GameObject GoldenSword;

    private List<GameObject> terrainSegments = new List<GameObject>();

    private float BlockSpawnPointX = 0;
    private float BlockSpawnPointY = (float)-4.7592;

    Movement playerMovement;

    void Start()
    {
        playerMovement = FindObjectOfType<Movement>();

        CreateTerrainBlock(10);

        CreateComplexTerrain(10);
    }

    void Update()
    {
        if (playerMovement.transform.position.x > BlockSpawnPointX - 10)
        {
            CreateComplexTerrain(10);

            terrainSegments.RemoveAll(segment =>
            {
                if (segment.transform.position.x < playerMovement.transform.position.x - 10)
                {
                    Destroy(segment);
                    return true;
                }
                return false;
            });
        }
    }

    void CreateComplexTerrain(int NumberOfChunks)
    {
        for(int i = 0; i < NumberOfChunks; i++)
        {
            BlockSpawnPointX += 2 * Random.Range(1, 3);
            AdjustTerrainYOffset();

            CreateTerrainBlock(Random.Range(2, 5));
        }
    }

    void CreateTerrainBlock(int NumberOfBlocks)
    {
        InstantiateTerrain(Terrain_left, Terrain_left_under);

        int IndexSpawnWord = Random.Range(0, NumberOfBlocks);

        for (int i = 0; i < IndexSpawnWord; i++)
        {
            InstantiateTerrain(Terrain_mid, Terrain_mid_under);
        }

        if(Random.Range(0, 10) == 7)
        {
            Instantiate(GoldenSword, new Vector3(BlockSpawnPointX, BlockSpawnPointY + 1, TERRAIN_Z), Quaternion.identity);
            terrainSegments.Add(GoldenSword);
        }

        for (int i = IndexSpawnWord; i < NumberOfBlocks; i++)
        {
            InstantiateTerrain(Terrain_mid, Terrain_mid_under);
        }

        InstantiateTerrain(Terrain_right, Terrain_right_under);
    }


    void InstantiateTerrain(GameObject prefab, GameObject under)
    {
        GameObject terrain = Instantiate(prefab, new Vector3(BlockSpawnPointX, BlockSpawnPointY, TERRAIN_Z), Quaternion.identity);
        terrainSegments.Add(terrain);

        Renderer renderer = terrain.GetComponent<Renderer>();
        float terrainSize = renderer.bounds.size.x;

        float TempY = BlockSpawnPointY;

        while (TempY > (float)-4.7592)
        {
            TempY -= terrainSize;
            GameObject underTerrain = Instantiate(under, new Vector3(BlockSpawnPointX, TempY, TERRAIN_Z), Quaternion.identity);
            terrainSegments.Add(underTerrain);
        }
        
        BlockSpawnPointX += terrainSize;
    }

    void AdjustTerrainYOffset()
    {
        if(Random.Range(0, 2) == 1)
        {
            float OldValue = BlockSpawnPointY;
            BlockSpawnPointY += Random.Range(0, 2);

            if (BlockSpawnPointY > 1.5)
            {
                BlockSpawnPointY = OldValue;
            }
        }
        else
        {
            BlockSpawnPointY += (Random.Range(0, 4) - 2);

            if (BlockSpawnPointY < (float)-4.7592)
            {
                BlockSpawnPointY = (float)-4.7592;
            }
        }
    }


    bool ShouldSpawnJumpChunk()
    {
        return Random.Range(0, 2) == 1;
    }
}
