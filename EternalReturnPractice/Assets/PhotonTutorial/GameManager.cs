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
            Instance = this;
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
