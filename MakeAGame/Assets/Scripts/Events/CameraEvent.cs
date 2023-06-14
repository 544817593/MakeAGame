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
}