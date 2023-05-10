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
                    // photonView.IsMine�� true�̸� �� �ν��Ͻ��� ���󰡾� �� �ʿ䰡 �ִٴ� �ǹ�
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
                // �� �÷��̾ �����ϰ� �ֽ��ϴ�: �ٸ� �÷��̾�� �����͸� �����ϴ�
                stream.SendNext(CurrentHp);
            }
            else
            {
                // ��Ʈ��ũ �÷��̾�, ������ ����
                CurrentHp = (float)stream.ReceiveNext();
            }
        }
    }
}
