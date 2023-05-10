using Photon.Pun;
using UnityEngine;

namespace Nameless
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IHittable, IPunObservable
    {
        [Tooltip("The Current Health of our player")]
        public float MaxHp { get; set; } = 1000;
        public float CurrentHp { get; set; }

        private void Awake()
        {
            
        }

        private void Start()
        {
            CameraWork cameraWork = gameObject.GetComponent<CameraWork>();
            if(cameraWork != null)
            {
                if(photonView.IsMine)
                {
                    // photonView.IsMine이 true이면 이 인스턴스를 따라가야 할 필요가 있다는 의미
                    cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }
        }

        public override void OnEnable()
        {
           CurrentHp = MaxHp;
        }

        public void Hit(int damage, GameObject sender = null)
        {
            if(photonView.IsMine)
            {
                CurrentHp -= damage;
                if (CurrentHp < 0f)
                {
                    CurrentHp = 0f;
                    GameManager.Instance.LeaveRoom();
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if(stream.IsWriting)
            {
                // 이 플레이어를 소유하고 있습니다: 다른 플레이어에게 데이터를 보냅니다
                stream.SendNext(CurrentHp);
            }
            else
            {
                // 네트워크 플레이어, 데이터 수신
                CurrentHp = (float)stream.ReceiveNext();
            }
        }
    }
}
