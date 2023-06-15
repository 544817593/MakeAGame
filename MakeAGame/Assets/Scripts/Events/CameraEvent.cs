using UnityEngine;

namespace Game
{
    // 相机相关事件
    public struct ChangeCameraTargetEvent   // 调整摄像机目标及方位
    {
        public Transform target;
        public Vector3 cameraAngle;
        public float cameraDist;
    }
    
    public struct SetCameraBorderEvent  // 四个边界点，相机视野不能超出：左上、右上、左下、右下
    {
        public Vector3[] viewCorners;
    }
}