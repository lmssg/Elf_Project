using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    private Vector3 topLeftPos;
    private Vector3 bottomRigntPos;

    private static Vector3 deltaPos;

    private Vector3 oneFingerStartPos;
    private Vector3 oneFingerEndPos;

    private void Start()
    {
        topLeftPos = GameObject.Find("Range").GetComponent<CameraBoundScript>().CameraClampTopLeftPosition;
        bottomRigntPos = GameObject.Find("Range").GetComponent<CameraBoundScript>().CameraClampBottomRightPosition;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            oneFingerStartPos = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            oneFingerEndPos = Input.mousePosition;
            float distance = Vector3.Distance(oneFingerEndPos, oneFingerStartPos);

            if (distance >= 0.5f)
            {
                float moveX = oneFingerEndPos.x - oneFingerStartPos.x;
                float moveY = oneFingerEndPos.y - oneFingerStartPos.y;
                float worldSizePerPixel = 2 * Camera.main.orthographicSize / (float)Screen.height;
                var deltaPositionX = -moveX * worldSizePerPixel;
                var deltaPositionY = -moveY * worldSizePerPixel;

                Vector3 deltaPosition = Vector3.zero;

                deltaPosition.Set(deltaPositionX, deltaPositionY, 0);

                Camera.main.transform.position += deltaPosition;
                ClampCamera(topLeftPos, bottomRigntPos);
                oneFingerStartPos = oneFingerEndPos;
            }




        }
        else if (Input.GetMouseButtonUp(0))
        {
            oneFingerStartPos = oneFingerEndPos;
        }


        var distance2 = Input.GetAxis("Mouse ScrollWheel");
        HandleMouseScrollWheel(distance2 * 10);


    }

    void HandleMouseScrollWheel(float distance)
    {
        if (0 == distance) return;
        ScaleCamere(distance);
    }
    void ScaleCamere(float scale)
    {
        Camera.main.orthographicSize -= scale * 0.1f;
        if (Camera.main.orthographicSize < 8)
        {
            Camera.main.orthographicSize = 8;
        }
        if (Camera.main.orthographicSize > 12)
        {
            Camera.main.orthographicSize = 12;
        }
    }


    /// <summary>
    /// 防止镜头出界
    /// </summary>
    /// <param name="topLeftPosition"></param>
    /// <param name="bottomRightPosition"></param>
    public static void ClampCamera(Vector3 topLeftPosition, Vector3 bottomRightPosition)
    {
        float worldSizePerPixel = 2 * Camera.main.orthographicSize / (float)Screen.height;
        //clamp camera left and top
        Vector3 leftClampScreenPos = Camera.main.WorldToScreenPoint(topLeftPosition);
        if (leftClampScreenPos.x > 0)
        {
            float deltaFactor = leftClampScreenPos.x * worldSizePerPixel;
            //Vector3 delta = new Vector3(deltaFactor, 0, 0);
            deltaPos.Set(deltaFactor, 0, 0);
            Camera.main.transform.localPosition += deltaPos;
        }

        if (leftClampScreenPos.y < Screen.height)
        {
            float deltaFactor = (Screen.height - leftClampScreenPos.y) * worldSizePerPixel;
            //Vector3 delta = new Vector3(0, -deltaFactor, 0);
            deltaPos.Set(0, -deltaFactor, 0);
            Camera.main.transform.localPosition += deltaPos;
        }
        //clamp camera right and bottom
        Vector3 rightClampScreenPos = Camera.main.WorldToScreenPoint(bottomRightPosition);

        if (rightClampScreenPos.x < Screen.width)
        {
            float deltaFactor = (rightClampScreenPos.x - Screen.width) * worldSizePerPixel;
            //Vector3 delta = new Vector3(deltaFactor, 0, 0);
            deltaPos.Set(deltaFactor, 0, 0);
            Camera.main.transform.localPosition += deltaPos;
        }

        if (rightClampScreenPos.y > 0)
        {
            float deltaFactor = rightClampScreenPos.y * worldSizePerPixel;
            //Vector3 delta = new Vector3(0, deltaFactor, 0);
            deltaPos.Set(0, deltaFactor, 0);
            Camera.main.transform.localPosition += deltaPos;
        }
    }

}
