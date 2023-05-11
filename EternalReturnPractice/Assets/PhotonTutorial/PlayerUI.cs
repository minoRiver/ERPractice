using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nameless
{
    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields

        [SerializeField]
        private TextMeshProUGUI playerNameText;

        [SerializeField]
        private Slider playerHealthSlider;

        [SerializeField]
        private Vector3 screenOffset = new Vector3(0, 30f, 0);

        private float characterControllerHeight = 0f;
        private Transform targetTransform;
        private Vector3 targetPosition;

        private PlayerManager target;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        }

        private void Update()
        {
            if(playerHealthSlider != null)
            {
                playerHealthSlider.value = target.CurrentHp / target.MaxHp;
            }

            if (target == null)
            {
                Destroy(this.gameObject);
                return;
            }
        }

        private void LateUpdate()
        {

            // #Critical
            // Follow the Target GameObject on screen.
            if (targetTransform != null) 
            {
                targetPosition = targetTransform.position;
                targetPosition.y += characterControllerHeight;
                transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
            }    
        }

        #endregion

        #region Public Methods
        public void SetTarget(PlayerManager t)
        {
            if(t == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
                return;
            }

            target = t;

            targetTransform = target.transform;
            CapsuleCollider controller = target.GetComponent<CapsuleCollider>();

            if(controller != null)
            {
                characterControllerHeight = controller.bounds.size.y;
            }

            if(playerNameText != null)
            {
                playerNameText.text = target.photonView.Owner.NickName;
            }
        }
        #endregion
    }
}
