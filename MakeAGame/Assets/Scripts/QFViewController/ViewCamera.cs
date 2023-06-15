using System;
using QFramework;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class ViewCamera: MonoBehaviour, IController
    {
        private Camera camera;
        private Transform transCamera;
        
        public Transform lookAtTarget;
        public Vector3 cameraAngle;
        public float cameraDist = 12f;
        private float crtCameraDist;

        [Header("是否使用写入的参数设置摄像机，若否，则根据场景开始运行时摄像机本身的参数进行设置")]
        public bool ForceSetValue;
        
        float moveSpeed = 12f; // 相机移动速度
        float borderOffset = 30f; // 鼠标要贴近边缘多少才可以移动相机
        float scrollSpeed = 500; // 缩放速度

        private Vector3[] borderCorners = new Vector3[4];

        // 通过主动调用进行初始化
        public void Init()
        {
            camera = this.GetComponent<Camera>();
            transCamera = transform;
            if (!ForceSetValue)
            {
                cameraAngle = transform.eulerAngles;
                // cameraDist = (lookAtTarget.transform.position - transform.position).magnitude;
            }

            this.RegisterEvent<ChangeCameraTargetEvent>(e => OnTargetChange(e)).UnRegisterWhenGameObjectDestroyed(gameObject);
            this.RegisterEvent<SetCameraBorderEvent>(e => OnSetBorder(e)).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            if(lookAtTarget != null)
                CameraLookatPos(cameraAngle, cameraDist);
        }

        private void Update()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            UpdateCameraDist(-scroll * scrollSpeed * Time.deltaTime);

            var mousePos = Input.mousePosition;
            var screenH = Screen.height;
            var screenW = Screen.width;
            var leftDist = mousePos.x;
            var rightDist = screenW - mousePos.x;
            var downDist = mousePos.y;
            var upDist = screenH - mousePos.y;
            // Debug.Log($"left: {leftDist} right: {rightDist} down: {downDist} up: {upDist}");

            Vector3 movement = Vector3.zero;

            if (leftDist < borderOffset)
            {
                movement.x = (-1) * moveSpeed * Time.deltaTime;
            }
            else if (rightDist < borderOffset)
            {
                movement.x = moveSpeed * Time.deltaTime;
            }
            else if (downDist < borderOffset)
            {
                movement.y = (-1) * moveSpeed * Time.deltaTime;
            }
            else if (upDist < borderOffset)
            {
                movement.y = moveSpeed * Time.deltaTime;
            }

            viewCorners = UpdateViewCorners(crtCameraDist);
            // Debug.Log($"viewCorners: {viewCorners[0]} - {viewCorners[1]} - {viewCorners[2]} - {viewCorners[3]}");
            // Debug.Log($"bordCorners: {borderCorners[0]} - {borderCorners[1]} - {borderCorners[2]} - {borderCorners[3]}");
            if (viewCorners[0].x + movement.x < borderCorners[0].x ||
                viewCorners[0].y + movement.y > borderCorners[0].y ||
                viewCorners[3].x + movement.x > borderCorners[3].x ||
                viewCorners[3].y + movement.y < borderCorners[3].y)
            {
                // Debug.Log("left border!");
                if (leftDist <= rightDist)
                {
                    movement.x = -viewCorners[0].x;
                }
                else
                {
                    movement.x = screenW - viewCorners[1].x;
                }

                if (downDist <= upDist)
                {
                    movement.y = -viewCorners[2].y;
                }
                else
                {
                    movement.y = screenH - viewCorners[0].y;
                }
                
                return;
            }
            
            transform.position += movement;
        }

        private void LateUpdate()
        {
            
        }
        
        
        
        //获取相机视口四个角的坐标
        //参数 distance  视口距离
        private Vector3[] viewCorners = new Vector3[4];
        Vector3[] UpdateViewCorners(float distance)
        {
            Vector3[] viewCorners = new Vector3[4];
            
            //fov为垂直视野  水平fov取决于视口的宽高比  以度为单位
            
            float halfFOV = (camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;//一半fov
            float aspect = camera.aspect;//相机视口宽高比
 
            float height = distance * Mathf.Tan(halfFOV);//distance距离位置，相机视口高度的一半
            float width = height * aspect;//相机视口宽度的一半
 
            //左上
            viewCorners[0] = transCamera.position - (transCamera.right * width);//相机坐标 - 视口宽的一半
            viewCorners[0] += transCamera.up * height;//+视口高的一半
            viewCorners[0] += transCamera.forward * distance;//+视口距离
 
            // 右上
            viewCorners[1] = transCamera.position + (transCamera.right * width);//相机坐标 + 视口宽的一半
            viewCorners[1] += transCamera.up * height;//+视口高的一半
            viewCorners[1] += transCamera.forward * distance;//+视口距离
 
            // 左下
            viewCorners[2] = transCamera.position - (transCamera.right * width);//相机坐标 - 视口宽的一半
            viewCorners[2] -= transCamera.up * height;//-视口高的一半
            viewCorners[2] += transCamera.forward * distance;//+视口距离
 
            // 右下
            viewCorners[3] = transCamera.position + (transCamera.right * width);//相机坐标 + 视口宽的一半
            viewCorners[3] -= transCamera.up * height;//-视口高的一半
            viewCorners[3] += transCamera.forward * distance;//+视口距离

            return viewCorners;
        }
        
        private void CameraLookatPos(Vector3 eulerAngles,  float distanceFromTarget)
        {
            Vector3 lookatPos = lookAtTarget.position;
            Quaternion q = Quaternion.Euler(eulerAngles);
            transform.position = lookatPos - q * Vector3.forward * distanceFromTarget;
            transform.eulerAngles = eulerAngles;
        }

        private void UpdateCameraDist(float amount)
        {
            if(amount == 0) return;

            // 先获取当前注视位置
            Quaternion q = Quaternion.Euler(cameraAngle);
            Vector3 lookatPos = transform.position + q * Vector3.forward * crtCameraDist;
            // 逆推应该在的位置
            // crtCameraDist += amount;
            // transform.position = lookatPos - q * Vector3.forward * crtCameraDist;

            var tmpViewCorners = UpdateViewCorners(crtCameraDist + amount);
            if (IsBorderHit(tmpViewCorners))
            {
                return;
            }
            else
            {
                crtCameraDist += amount;
                transCamera.position = lookatPos - q * Vector3.forward * crtCameraDist;
                ;
            }
        }

        private void OnTargetChange(ChangeCameraTargetEvent e)
        {
            Debug.Log($"camera target change to: {e.target.gameObject.name}");
            lookAtTarget = e.target;
            if(e.cameraAngle != Vector3.zero)
                cameraAngle = e.cameraAngle;
            if(e.cameraDist != 0)
                cameraDist = e.cameraDist;

            crtCameraDist = cameraDist;
            CameraLookatPos(cameraAngle, crtCameraDist);
        }

        bool IsBorderHit(Vector3[] viewCorners)
        {
            return viewCorners[0].x <= borderCorners[0].x || viewCorners[0].y >= borderCorners[0].y ||
                   viewCorners[3].x >= borderCorners[3].x || viewCorners[3].y <= borderCorners[3].y;
        }

        void OnSetBorder(SetCameraBorderEvent e)
        {
            borderCorners = e.viewCorners;
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}