using Photon.Pun;
using UnityEngine;

namespace Nameless
{
    public class PlayerAnimatorManager : MonoBehaviourPun, IPunObservable
    {
        private Animator animator;
        private Rigidbody rig;

        private readonly int ID_Run = Animator.StringToHash("Run");
        private readonly int ID_Wait = Animator.StringToHash("Wait");

        private bool isAttack = false;
        private readonly int ID_Attack = Animator.StringToHash("Attack");

        #region MonoBehaviour Callbacks

        public void AttackEnd()
        {
            isAttack = false;
            animator.SetBool(ID_Attack, isAttack);
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            rig = GetComponent<Rigidbody>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }

        private void Update()
        {
            // 인스턴스가 client 어플리케이션에서 제어되고 있다면 photonView.IsMine은 true일 것이다
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) return;

            if (!animator) return;
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 moveDir = new Vector3(h, 0, v).normalized;
            Vector3 velocity = Vector3.zero;

            if ((h != 0 || v != 0) && isAttack == false)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);

                animator.SetTrigger(ID_Run);
                velocity = moveDir * 3f;
            }
            else if(moveDir == Vector3.zero && isAttack == false)
            {
                animator.SetTrigger(ID_Wait);
            }

            if (Input.GetMouseButtonDown(0) && isAttack == false)
            {
                isAttack = true;
                animator.SetBool(ID_Attack, isAttack);
                rig.velocity = Vector3.zero;
            }

            rig.velocity = velocity;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isAttack);
            }
            else
            {
                isAttack = (bool)stream.ReceiveNext();
            }
        }

        #endregion
    }
}


