using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Nameless
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// 방당 최대 플레이어 수입니다. 방이 가득 차면 새로운 플레이어가 참여할 수 없으므로 새 방이 만들어집니다.
        /// </summary>
        [Tooltip("방당 최대 플레이어 수입니다. 방이 가득 차면 새로운 플레이어가 참여할 수 없으므로 새 방이 생성됩니다.")]
        [SerializeField]
        private byte maxPlayerPerRoom = 4;

        #endregion
        #region Private Fields

        /// <summary>
        /// 이 클라이언트의 버전 번호입니다. 사용자는 게임 버전에 따라 서로 구분됩니다(이를 통해 획기적인 변경이 가능).
        /// </summary>
        string gameVersion = "1";

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // #Critical
            // 이렇게 하면 마스터 클라이언트에서 PhotonNetwork.LoadLevel()을 사용할 수 있고 같은 방에 있는 모든 클라이언트가 자동으로 레벨을 동기화할 수 있습니다.
            PhotonNetwork.AutomaticallySyncScene = true;
            // 우리 게임은 플레이어 수에 따라 크기가 변경되는 경기장을 갖게 될 것이고 로드된 씬은 연결하고 있는 모든 플레이어에서 동일 한 것입니다. 우리는 포톤이 제공하는
            // 매우 편리한 기능을 이용할 것 입니다: PhotonNetwork.AutomaticallySyncScene이 값이 true일 때 masterclient는 PhotonNetwork.LoadLevel()을 호출
            // 할 수 있고 모든 연결된 플레이어들은 동일한 레벨을 자동적으로 로드 할 것입니다.
        }
        void Start()
        {
            
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// 연결 프로세스를 시작합니다.
        /// - 이미 연결되어 있다면, 무작위 방에 가입을 시도합니다.
        /// - 아직 연결되지 않은 경우, 이 애플리케이션 인스턴스를 포토톤 클라우드 네트워크에 연결합니다.
        /// </summary>
        public void Connect()
        {
            // 연결되었는지 여부를 확인하고 연결되면 가입하고 그렇지 않으면 서버에 연결을 시작합니다.
            if(PhotonNetwork.IsConnected)
            {
                // #Critical 이 시점에서 랜덤 룸에 참가하려면 필수입니다. 실패하면 OnJoinRandomFailed()에서 알림을 받고 랜덤룸을 생성합니다.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, 가장 먼저 포톤 온라인 서버에 연결해야 합니다.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: 가장 먼저 시도하는 것은 잠재적인 기존 방에 가입하는 것입니다. 존재하면 좋고, 그렇지 않으면 OnJoinRandomFailed()로 다시 호출됩니다.
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // #Critical: 임의의 방에 참여하지 못했습니다. 방이 존재하지 않거나 모두 꽉 찼을 수 있습니다. 걱정하지 마세요. 새 방을 만들어 드립니다.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion
    }
}


