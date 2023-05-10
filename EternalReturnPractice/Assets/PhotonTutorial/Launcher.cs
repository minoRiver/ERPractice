using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Nameless
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// ��� �ִ� �÷��̾� ���Դϴ�. ���� ���� ���� ���ο� �÷��̾ ������ �� �����Ƿ� �� ���� ��������ϴ�.
        /// </summary>
        [Tooltip("��� �ִ� �÷��̾� ���Դϴ�. ���� ���� ���� ���ο� �÷��̾ ������ �� �����Ƿ� �� ���� �����˴ϴ�.")]
        [SerializeField]
        private byte maxPlayerPerRoom = 4;

        #endregion
        #region Private Fields

        /// <summary>
        /// �� Ŭ���̾�Ʈ�� ���� ��ȣ�Դϴ�. ����ڴ� ���� ������ ���� ���� ���е˴ϴ�(�̸� ���� ȹ������ ������ ����).
        /// </summary>
        string gameVersion = "1";

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            // #Critical
            // �̷��� �ϸ� ������ Ŭ���̾�Ʈ���� PhotonNetwork.LoadLevel()�� ����� �� �ְ� ���� �濡 �ִ� ��� Ŭ���̾�Ʈ�� �ڵ����� ������ ����ȭ�� �� �ֽ��ϴ�.
            PhotonNetwork.AutomaticallySyncScene = true;
            // �츮 ������ �÷��̾� ���� ���� ũ�Ⱑ ����Ǵ� ������� ���� �� ���̰� �ε�� ���� �����ϰ� �ִ� ��� �÷��̾�� ���� �� ���Դϴ�. �츮�� ������ �����ϴ�
            // �ſ� ���� ����� �̿��� �� �Դϴ�: PhotonNetwork.AutomaticallySyncScene�� ���� true�� �� masterclient�� PhotonNetwork.LoadLevel()�� ȣ��
            // �� �� �ְ� ��� ����� �÷��̾���� ������ ������ �ڵ������� �ε� �� ���Դϴ�.
        }
        void Start()
        {
            
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// ���� ���μ����� �����մϴ�.
        /// - �̹� ����Ǿ� �ִٸ�, ������ �濡 ������ �õ��մϴ�.
        /// - ���� ������� ���� ���, �� ���ø����̼� �ν��Ͻ��� ������ Ŭ���� ��Ʈ��ũ�� �����մϴ�.
        /// </summary>
        public void Connect()
        {
            // ����Ǿ����� ���θ� Ȯ���ϰ� ����Ǹ� �����ϰ� �׷��� ������ ������ ������ �����մϴ�.
            if(PhotonNetwork.IsConnected)
            {
                // #Critical �� �������� ���� �뿡 �����Ϸ��� �ʼ��Դϴ�. �����ϸ� OnJoinRandomFailed()���� �˸��� �ް� �������� �����մϴ�.
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                // #Critical, ���� ���� ���� �¶��� ������ �����ؾ� �մϴ�.
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            // #Critical: ���� ���� �õ��ϴ� ���� �������� ���� �濡 �����ϴ� ���Դϴ�. �����ϸ� ����, �׷��� ������ OnJoinRandomFailed()�� �ٽ� ȣ��˴ϴ�.
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // #Critical: ������ �濡 �������� ���߽��ϴ�. ���� �������� �ʰų� ��� �� á�� �� �ֽ��ϴ�. �������� ������. �� ���� ����� �帳�ϴ�.
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


