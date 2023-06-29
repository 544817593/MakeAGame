using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
/// <summary>
/// Debug 绘制camera的 视口、视锥 和 FOV
/// </summary>
public class DrawLineCameraViewHelper: MonoBehaviour
{
    public float _farDistance = 12;//远视口距离
    public float _nearDistance = 3;//近视口距离
 
    private Camera _camera;
    private Transform _camTrans;
 
 
    void Start()
    {
        
        
   
    }
 
    /// <summary>
    /// 绘制图形
    /// </summary>
    void OnDrawGizmos()
    {
        if (_camera == null)
        {
            _camera = this.GetComponent<Camera>();
            _camTrans = _camera.transform;
        }
        OnDrawFarView();
        OnDrawNearView();
        OnDrawFOV();
        OnDrawConeOfCameraVision();
 
    }
 
    /// <summary>
    /// 选中的时候显示绘制出图形
    /// </summary>
    void OnDrawGizmosSelected() {
        //Debug.Log("OnDrawGizmosSelected");
        //if (_camera == null)
        //{
        //    _camera = this.GetComponent<Camera>();
        //    _camTrans = _camera.transform;
        //}
        //OnDrawFarView();
        //OnDrawNearView();
        //OnDrawFOV();
        //OnDrawConeOfCameraVision();
    }
 
    /// <summary>
    /// 绘制较远的视口
    /// </summary>
    void OnDrawFarView()
    {
        Vector3[] corners = GetCorners(_farDistance);
 
        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.yellow); // UpperLeft -> UpperRight
        Debug.DrawLine(corners[1], corners[3], Color.yellow); // UpperRight -> LowerRight
        Debug.DrawLine(corners[3], corners[2], Color.yellow); // LowerRight -> LowerLeft
        Debug.DrawLine(corners[2], corners[0], Color.yellow); // LowerLeft -> UpperLeft
 
 
        //中心线
        Vector3 vecStart = _camTrans.transform.position;
        Vector3 vecEnd = vecStart;
        vecEnd += _camTrans.forward * _farDistance;
        Debug.DrawLine(vecStart, vecEnd, Color.red);
    }
 
    /// <summary>
    /// 绘制较近的视口
    /// </summary>
    void OnDrawNearView()
    {
        Vector3[] corners = GetCorners(_nearDistance);
 
        // for debugging
        Debug.DrawLine(corners[0], corners[1], Color.red);//左上-右上
        Debug.DrawLine(corners[1], corners[3], Color.red);//右上-右下
        Debug.DrawLine(corners[3], corners[2], Color.red);//右下-左下
        Debug.DrawLine(corners[2], corners[0], Color.red);//左下-左上
    }
 
    /// <summary>
    /// 绘制 camera 的 FOV
    /// </summary>
    void OnDrawFOV()
    {
        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//一半fov
        float halfHeight = _farDistance * Mathf.Tan(halfFOV);//distance距离位置，相机视口高度的一半
 
        //起点
        Vector3 vecStart = _camTrans.position;
 
        //上中
        Vector3 vecUpCenter = vecStart;
        vecUpCenter.y -= halfHeight;
        vecUpCenter.z += _farDistance;
 
        //下中
        Vector3 vecBottomCenter = vecStart;
        vecBottomCenter.y += halfHeight;
        vecBottomCenter.z += _farDistance;
 
        Debug.DrawLine(vecStart, vecUpCenter, Color.blue);
        Debug.DrawLine(vecStart, vecBottomCenter, Color.blue);
    }
 
    /// <summary>
    /// 绘制 camera 的视锥 边沿
    /// </summary>
    void OnDrawConeOfCameraVision() {
        Vector3[] corners = GetCorners(_farDistance);
 
        // for debugging
        Debug.DrawLine(_camTrans.position, corners[1], Color.green); // UpperLeft -> UpperRight
        Debug.DrawLine(_camTrans.position, corners[3], Color.green); // UpperRight -> LowerRight
        Debug.DrawLine(_camTrans.position, corners[2], Color.green); // LowerRight -> LowerLeft
        Debug.DrawLine(_camTrans.position, corners[0], Color.green); // LowerLeft -> UpperLeft
    }
 
 
    //获取相机视口四个角的坐标
    //参数 distance  视口距离
    Vector3[] GetCorners(float distance)
    {
        Vector3[] corners = new Vector3[4];
 
        //fov为垂直视野  水平fov取决于视口的宽高比  以度为单位
 
 
        float halfFOV = (_camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//一半fov
        float aspect = _camera.aspect;//相机视口宽高比
 
        float height = distance * Mathf.Tan(halfFOV);//distance距离位置，相机视口高度的一半
        float width = height * aspect;//相机视口宽度的一半
 
        //左上
        corners[0] = _camTrans.position - (_camTrans.right * width);//相机坐标 - 视口宽的一半
        corners[0] += _camTrans.up * height;//+视口高的一半
        corners[0] += _camTrans.forward * distance;//+视口距离
 
        // 右上
        corners[1] = _camTrans.position + (_camTrans.right * width);//相机坐标 + 视口宽的一半
        corners[1] += _camTrans.up * height;//+视口高的一半
        corners[1] += _camTrans.forward * distance;//+视口距离
 
        // 左下
        corners[2] = _camTrans.position - (_camTrans.right * width);//相机坐标 - 视口宽的一半
        corners[2] -= _camTrans.up * height;//-视口高的一半
        corners[2] += _camTrans.forward * distance;//+视口距离
 
        // 右下
        corners[3] = _camTrans.position + (_camTrans.right * width);//相机坐标 + 视口宽的一半
        corners[3] -= _camTrans.up * height;//-视口高的一半
        corners[3] += _camTrans.forward * distance;//+视口距离
 
        return corners;
    }
}