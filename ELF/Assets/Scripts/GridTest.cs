using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Tilemap tl = gameObject.transform.Find("Road").GetComponent<Tilemap>();

        initGrid(tl);
    }

    void initGrid(Tilemap tilemap)
    {
        BoundsInt mapBounds = tilemap.cellBounds;

        int[] phyPoint = new int[mapBounds.size.x * mapBounds.size.y];

        for (int x = mapBounds.xMin; x < mapBounds.xMax; x++)
        {

            for (int y = mapBounds.yMin; y < mapBounds.yMax; y++)
            {

                int index = x - mapBounds.xMin + (y - mapBounds.yMin) * mapBounds.size.x;

                Vector3Int cellPos = new Vector3Int(x, y, 0);

                Tile tile = tilemap.GetTile<Tile>(cellPos);
                if (tile != null)
                {
                    if (tile.name == "FloorBricksToGrassCorner_2")
                    {
                        phyPoint[index] = 0;
                    }
                    else
                    {
                        phyPoint[index] = 1;
                    }

                }
                else
                {
                    phyPoint[index] = 1;
                }


            }

        }


     
    }


    // Update is called once per frame
    void Update()
    {

    }
}
