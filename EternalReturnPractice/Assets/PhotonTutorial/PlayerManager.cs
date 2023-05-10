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

        [Tooltip("���� �÷��̾� �ν��Ͻ��Դϴ�. ���� �÷��̾ ���� ǥ�õǴ��� Ȯ���Ϸ��� �� ���� ����մϴ�.")]
        public static GameObject LocalPlayerInstance;

        private void Awake()
        {
            // #�߿�
            // GameManager.cs���� ���: ������ ����ȭ�� �� �ν��Ͻ�ȭ�� �����ϱ� ���� localPlayer �ν��Ͻ��� �����մϴ�.
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = gameObject;
            }

            // #Critical
            // �ε� �� �ı����� �������� �÷��׸� �����Ͽ� �ν��Ͻ��� ���� ����ȭ���� ��Ƴ��� ���� �ε� �� ��Ȱ�� ������ �����մϴ�.
            DontDestroyOnLoad(gameObject);
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
                // �� �÷��̾ �����ϰ� �ֽ��ϴ�: �ٸ� �÷��̾�� �����͸� �����ϴ�
                stream.SendNext(CurrentHp);
            }
            else
            {
                // ��Ʈ��ũ �÷��̾�, ������ ����
                CurrentHp = (float)stream.ReceiveNext();
            }
        }

        private void OnLevelWasLoaded(int level)
        {
            // ������ �ۿ� �ִ��� Ȯ���ϰ�, ������ �ۿ� �ִٸ� ������ �߾ӿ� �ִ� ���� ���뿡 �����մϴ�.
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }
    }
}
