using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Nameless
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Photon Callbacks
        /// <summary>
        /// ���� �÷��̾ ���� ������ �� ȣ��˴ϴ�. ��ó ���� �ε��ؾ� �մϴ�.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"OnPlayerEnteredRoom() {newPlayer.NickName}"); // �÷��̾ ������ ��� ǥ�õ��� ����

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"OnPlayerEnteredRoom IsMasterClient {PhotonNetwork.IsMasterClient}"); // OnPlayerLeftRoom ���� ȣ��˴ϴ�.

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"OnPlayerEnteredRoom() {otherPlayer.NickName}"); // �ٸ� ������ ������ �� ǥ��

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"OnPlayerLeftRoom IsMasterClient {PhotonNetwork.IsMasterClient}"); // OnPlayerLeftRoom ���� ȣ��˴ϴ�.

                LoadArena();
            }
        }

        #endregion

        #region Public Fields

        public static GameManager Instance;

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            // "Player" �������� �ν��Ͻ��� �����ϴ� ���� ������ �����ϴ�. �뿡 ���� �� �ٷ� �ν��Ͻ��� ������ �ʿ䰡 ������,
            // �츮�� ������� �ε� �ߴٴ� ���� �ǹ� �ϴ� GameManager ��ũ��Ʈ Start() ���� �� �� �ֽ��ϴ�.
            // �� �ǹ̴� ����󿡼� �츮�� �뿡 �ִٴ� �ǹ� �Դϴ�.
            Instance = this;

            if(PlayerManager.LocalPlayerInstance == null)
            {
                Debug.Log($"We are Instantiating LocalPlayer from {SceneManagerHelper.ActiveSceneName}");
                PhotonNetwork.Instantiate("PhotonTutorial/Player", new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.Log($"Ignoring scene load for {SceneManagerHelper.ActiveSceneName}");
            }
        }

        private void LoadArena()
        {
            if(!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : ������ �ε��Ϸ��� �ϴµ� ������ Ŭ���̾�Ʈ�� �ƴմϴ�.");
                return;
            }
            Debug.LogFormat($"PhotonNetwork : Loading Level : {PhotonNetwork.CurrentRoom.PlayerCount}");
            PhotonNetwork.LoadLevel($"Room For {PhotonNetwork.CurrentRoom.PlayerCount}");
        }

        #endregion
    }
}
