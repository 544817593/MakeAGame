using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    private static CameraManager instance; // 镜头管理器实例

    // 镜头管理器Getter
    public static CameraManager Instance { get { return instance; } }

    private void Awake()
    {

        // 确保单例
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // 把自身赋予GM
        GameManager.Instance.camMan = this;
    }

    float camMoveSpeed = 25f; // 相机移动速度
    float borderOffset = 10f; // 鼠标要贴近边缘多少才可以移动相机

    // 相机移动上下限
    float minBorderLimitX = -30f;
    float maxBorderLimitX = 30f;
    float minBorderLimitZ = -30f;
    float maxBorderLimitZ = 0f;

    // 相机缩放上下限
    float minScrollLimit = 30f;
    float maxScrollLimit = 70f;

    float scrollSpeed = 3000f; // 缩放速度

    public bool cameraLock = false; // 相机锁定，不可移动

    // 相机控制
    void Update()
    {

        Vector3 pos = transform.position;

        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - borderOffset)
        {
            pos.z += camMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= borderOffset)
        {
            pos.z -= camMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - borderOffset)
        {
            pos.x += camMoveSpeed * Time.deltaTime;
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= borderOffset)
        {
            pos.x -= camMoveSpeed * Time.deltaTime;
        }

        // 如果相机锁激活，相机不可缩放
        if (!cameraLock)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            pos.y -= scroll * scrollSpeed * Time.deltaTime;
        }
        else
        {
            pos.y = 30f;
        }


        // 最远最近缩放和移动区间
        pos.x = Mathf.Clamp(pos.x, minBorderLimitX, maxBorderLimitX);
        pos.y = Mathf.Clamp(pos.y, minScrollLimit, maxScrollLimit);
        pos.z = Mathf.Clamp(pos.z, minBorderLimitZ, maxBorderLimitZ);

        transform.position = pos;
    }

}
