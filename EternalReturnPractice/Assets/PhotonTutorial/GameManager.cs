using Photon.Pun;
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

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion
    }
}
