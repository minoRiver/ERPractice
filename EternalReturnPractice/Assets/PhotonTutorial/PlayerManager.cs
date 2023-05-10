using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nameless
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IHittable, IPunObservable
    {
        [SerializeField]
        private GameObject playerUiPrefab;

        public float MoveSpeed = 5;

        public float MaxHp { get; set; } = 1000;
        public float CurrentHp { get; set; }

        [Tooltip("로컬 플레이어 인스턴스입니다. 로컬 플레이어가 씬에 표시되는지 확인하려면 이 값을 사용합니다.")]
        public static GameObject LocalPlayerInstance;

        private void Awake()
        {
            // #중요
            // GameManager.cs에서 사용: 레벨이 동기화될 때 인스턴스화를 방지하기 위해 localPlayer 인스턴스를 추적합니다.
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = gameObject;
            }

            // #Critical
            // 로드 시 파괴하지 않음으로 플래그를 지정하여 인스턴스가 레벨 동기화에서 살아남아 레벨 로드 시 원활한 경험을 제공합니다.
            DontDestroyOnLoad(gameObject);
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

            if(playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

            SceneManager.sceneLoaded += (scene, loadingMode) => { OnLevelWasLoaded(scene.buildIndex); };
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

        private void OnLevelWasLoaded(int level)
        {
            // 투기장 밖에 있는지 확인하고, 투기장 밖에 있다면 투기장 중앙에 있는 안전 지대에 스폰합니다.
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }
}
