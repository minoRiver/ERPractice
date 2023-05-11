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

        [Tooltip("����ڰ� �̸��� �Է��ϰ� �����Ͽ� �÷����� �� �ִ� Ui �г�")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("����ڿ��� ������ ���� ������ �˸��� UI ���̺�")]
        [SerializeField]
        private GameObject progressLabel;

        #endregion

        #region Private Fields

        /// <summary>
        /// �� Ŭ���̾�Ʈ�� ���� ��ȣ�Դϴ�. ����ڴ� ���� ������ ���� ���� ���е˴ϴ�(�̸� ���� ȹ������ ������ ����).
        /// </summary>
        private string gameVersion = "1";

        /// <summary>
        /// ���� ���μ����� �����մϴ�. ������ �񵿱���̸� Photon�� ���� �ݹ鿡 ����ϱ� �����Դϴ�,
        /// Photon�� �ݹ��� ���� �� ������ ������ �����ϱ� ���� �̸� �����ؾ� �մϴ�.
        /// �Ϲ������� OnConnectedToMaster() �ݹ鿡 ���˴ϴ�.
        /// </summary>
        private bool isConnecting;

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

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
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
            // ���ӿ��� ���ƿ��� ����Ǿ��ٴ� �ݹ��� �ް� �ǹǷ� �׶� ������ �ؾ� ���� �˾ƾ� �ϹǷ� �濡 �����Ϸ��� ������ �����մϴ�.
            isConnecting = true;
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);

            // ����Ǿ����� ���θ� Ȯ���ϰ� ����Ǹ� �����ϰ� �׷��� ������ ������ ������ �����մϴ�.
            if (PhotonNetwork.IsConnected)
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

            // �濡 �����Ϸ��� �õ����� �ʴ´ٸ� �ƹ� �۾��� ���� �ʽ��ϴ�.
            // �� ��� isConnecting�� ������ ���� �Ϲ������� ������ �Ұų� �������� ���̸�, �� ������ �ε�Ǹ� OnConnectedToMaster�� ȣ��˴ϴ�.
            // �ƹ��͵� ���� �ʽ��ϴ�.
            if (isConnecting)
            {
                // #Critical: ���� ���� �õ��ϴ� ���� �������� ���� �濡 �����ϴ� ���Դϴ�. �����ϸ� ����, �׷��� ������ OnJoinRandomFailed()�� �ٽ� ȣ��˴ϴ�.
                PhotonNetwork.JoinRandomRoom();
            }
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

            // #Critical : ù ��° �÷��̾��� ��쿡�� �ε��ϰ�, �׷��� ���� ��� �ν��Ͻ� ���� ����ȭ�ϱ� ���� `PhotonNetwork.AutomaticallySyncScene`�� �����մϴ�.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room For 1' ");

                // #Critical
                // Load the Room Level
                PhotonNetwork.LoadLevel("Room For 1");
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        #endregion
    }
}


