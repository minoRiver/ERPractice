using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Nameless
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        #region Private Constants

        const string playerNamePrefKey = "PlayerName";

        #endregion

        #region MonoBehaviour Callbacks

        private void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField inputField = GetComponent<TMP_InputField>();
            if(inputField != null )
            {
                if(PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    defaultName = PlayerPrefs.GetString(playerNamePrefKey); 
                    inputField.text = defaultName;
                }
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// �÷��̾��� �̸��� �����ϰ� ���� ������ ���� PlayerPrefs�� �����մϴ�.
        /// </summary>
        /// <param name="value">�÷��̾��� �̸�</param>
        public void SetPlayerName(string value)
        {
            // �߿�
            if(string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;

            PlayerPrefs.SetString(playerNamePrefKey, value);
        }

        #endregion
    }
}
