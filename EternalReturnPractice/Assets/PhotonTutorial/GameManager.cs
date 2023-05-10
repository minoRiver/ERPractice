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
        /// 로컬 플레이어가 방을 나갔을 때 호출됩니다. 런처 씬을 로드해야 합니다.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"OnPlayerEnteredRoom() {newPlayer.NickName}"); // 플레이어가 접속한 경우 표시되지 않음

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"OnPlayerEnteredRoom IsMasterClient {PhotonNetwork.IsMasterClient}"); // OnPlayerLeftRoom 전에 호출됩니다.

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"OnPlayerEnteredRoom() {otherPlayer.NickName}"); // 다른 연결이 끊어질 때 표시

            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log($"OnPlayerLeftRoom IsMasterClient {PhotonNetwork.IsMasterClient}"); // OnPlayerLeftRoom 전에 호출됩니다.

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
            // "Player" 프리팹의 인스턴스를 생성하는 것은 정말로 쉽습니다. 룸에 들어갔을 때 바로 인스턴스를 생성할 필요가 있으며,
            // 우리가 경기장을 로드 했다는 것을 의미 하는 GameManager 스크립트 Start() 에서 할 수 있습니다.
            // 이 의미는 설계상에서 우리가 룸에 있다는 의미 입니다.
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
                Debug.LogError("PhotonNetwork : 레벨을 로드하려고 하는데 마스터 클라이언트가 아닙니다.");
                return;
            }
            Debug.LogFormat($"PhotonNetwork : Loading Level : {PhotonNetwork.CurrentRoom.PlayerCount}");
            PhotonNetwork.LoadLevel($"Room For {PhotonNetwork.CurrentRoom.PlayerCount}");
        }

        #endregion
    }
}
