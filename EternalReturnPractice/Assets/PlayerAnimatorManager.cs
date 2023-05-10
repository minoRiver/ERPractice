using UnityEngine;

namespace Nameless
{
    public class PlayerAnimatorManager : MonoBehaviour
    {
        private Animator animator;

        private AnimatorStateInfo stateInfo;
        private readonly int ID_RunTrigger = Animator.StringToHash("Run");
        private readonly int ID_WaitTrigger = Animator.StringToHash("Wait");
        private readonly int ID_AttackTrigger = Animator.StringToHash("Attack");
        #region MonoBehaviour Callbacks

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update()
        {
            if (!animator) return;
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (h != 0 || v != 0)
            {
                Vector3 dir = new Vector3(h, 0, v);
                transform.rotation = Quaternion.LookRotation(dir);
                animator.SetTrigger(ID_RunTrigger);
            }
            else
            {
                animator.SetTrigger(ID_WaitTrigger);
            }
        }

        #endregion
    }
}


