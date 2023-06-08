using System;
using QFramework;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game
{
    public class ViewCamera: MonoBehaviour, IController
    {
        public Transform lookAtTarget;
        public Vector3 cameraAngle;
        public float cameraDist = 12f;

        [Header("是否使用写入的参数设置摄像机，若否，则根据场景开始运行时摄像机本身的参数进行设置")]
        public bool ForceSetValue;

        // 通过主动调用进行初始化
        public void Init()
        {
            if (!ForceSetValue)
            {
                cameraAngle = transform.eulerAngles;
                // cameraDist = (lookAtTarget.transform.position - transform.position).magnitude;
            }

            this.RegisterEvent<ChangeCameraTargetEvent>(e => OnTargetChange(e)).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            if(lookAtTarget != null)
                CameraLookatPos(cameraAngle, cameraDist);
        }

        private void LateUpdate()
        {
            
        }
        
        private void CameraLookatPos(Vector3 eulerAngles,  float distanceFromTarget)
        {
            Vector3 lookatPos = lookAtTarget.position;
            Quaternion q = Quaternion.Euler(eulerAngles);
            transform.position = lookatPos - q * Vector3.forward * distanceFromTarget;
            transform.eulerAngles = eulerAngles;
        }

        private void OnTargetChange(ChangeCameraTargetEvent e)
        {
            Debug.Log($"camera target change to: {e.target.gameObject.name}");
            lookAtTarget = e.target;
            if(e.cameraAngle != Vector3.zero)
                cameraAngle = e.cameraAngle;
            if(e.cameraDist != 0)
                cameraDist = e.cameraDist;
            
            CameraLookatPos(cameraAngle, cameraDist);
        }

        public IArchitecture GetArchitecture()
        {
            return GameEntry.Interface;
        }
    }
}