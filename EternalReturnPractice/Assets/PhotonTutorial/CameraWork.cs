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

        // Ÿ���� �н��ϰų� ī�޶� ��ȯ�� ��� �ٽ� ������ �� �ֵ��� ���������� �÷��׸� �����մϴ�.
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
            // Ʈ������ Ÿ���� ���� �ε� �� �Ҹ���� ���� �� �ֽ��ϴ�,
            // ���� �� ���� �ε��� ������ ���� ī�޶� �޶����� �ڳ� ���̽��� ó���ϰ�, �׷� ���� �߻��ϸ� �ٽ� �����ؾ� �մϴ�.
            if (cameraTransform == null && isFollowing)
            {
                OnStartFollowing();
            }

            // ���� �ȷο츸 ��������� ����˴ϴ�.
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

            // �ƹ��͵� �ε巴�� ó������ �ʰ� �ٷ� ������ ī�޶� ������ �̵��մϴ�.
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

