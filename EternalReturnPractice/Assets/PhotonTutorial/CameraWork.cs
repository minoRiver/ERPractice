using System;
using UnityEngine;

namespace Nameless
{
    public class CameraWork : MonoBehaviour
    {
        #region Private Fields
        [Tooltip("The distance in the local x-z plane to the target")]
        [SerializeField]
        private float distance = 7.0f;

        [Tooltip("The height we want the camera to be above the target")]
        [SerializeField]
        private float height = 3.0f;

        [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
        [SerializeField]
        private Vector3 centerOffset = Vector3.zero;

        [Tooltip("Set this as false if a component of a prefab being instanciated by Photon Network, and manually call OnStartFollowing() when and if needed.")]
        [SerializeField]
        private bool followOnStart = false;

        [Tooltip("The Smoothing for the camera to follow the target")]
        [SerializeField]
        private float smoothSpeed = 0.125f;

 
        Transform cameraTransform;

        // 타겟을 분실하거나 카메라가 전환된 경우 다시 연결할 수 있도록 내부적으로 플래그를 유지합니다.
        bool isFollowing;

        Vector3 cameraOffset = Vector3.zero;

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            if(followOnStart)
            {
                OnStartFollowing();
            }
        }

        private void LateUpdate()
        {
            // 트랜스폼 타깃은 레벨 로드 시 소멸되지 않을 수 있습니다,
            // 따라서 새 씬을 로드할 때마다 메인 카메라가 달라지는 코너 케이스를 처리하고, 그런 일이 발생하면 다시 연결해야 합니다.
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            // 오직 팔로우만 명시적으로 선언됩니다.
            if (isFollowing)
            {
                Follow();
            }
        }

        #endregion

        #region Public Methods

        public void OnStartFollowing()
        {
            cameraTransform = Camera.main.transform;
            isFollowing = true;

            // 아무것도 부드럽게 처리하지 않고 바로 오른쪽 카메라 샷으로 이동합니다.
            Cut();
        }

        #endregion

        #region Private Methods
        private void Follow()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = transform.position + cameraOffset;
            cameraTransform.LookAt(transform.position + centerOffset);
        }

        private void Cut()
        {
            cameraOffset.z = -distance;
            cameraOffset.y = height;

            cameraTransform.position = this.transform.position + this.transform.TransformVector(cameraOffset);

            cameraTransform.LookAt(this.transform.position + centerOffset);
        }

        #endregion
    }
}

