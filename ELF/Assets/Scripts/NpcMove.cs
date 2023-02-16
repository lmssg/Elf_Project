using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NpcMove : MonoBehaviour
{

    // public Tilemap tileMap;
    private Vector3 startPos;
    private Vector3 EndPos;
    private float direction = -1f;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = new Vector3(8, -4, 0);
        EndPos = new Vector3(-8, 0, 0);
        targetPos = EndPos;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 dirPos = targetPos - transform.position;

        transform.Translate(dirPos.normalized * Time.deltaTime * 1f);

        //float moveX = Time.deltaTime * 1F * direction;

        //transform.position += new Vector3(moveX, 0, 0);
        if (Vector3.Distance(targetPos, transform.position) < 0.1f)
        {
            direction = -direction;
            if (direction == -1)
            {
                targetPos = EndPos;
            }
            else
            {
                targetPos = startPos;
            }
        }


        //  startPos += Time.deltaTime * 1f;
    }
}
