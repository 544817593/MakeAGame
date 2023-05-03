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

        private void Start()
        {
            cameraAngle = transform.eulerAngles;
            // cameraDist = (lookAtTarget.transform.position - transform.position).magnitude;

            this.RegisterEvent<ChangeCameraTargetEvent>(e => OnTargetChange(e));
            
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

    public struct ChangeCameraTargetEvent
    {
        public Transform target;
        public Vector3 cameraAngle;
        public float cameraDist;
    }
}