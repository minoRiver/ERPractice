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
        /// 플레이어의 이름을 설정하고 향후 세션을 위해 PlayerPrefs에 저장합니다.
        /// </summary>
        /// <param name="value">플레이어의 이름</param>
        public void SetPlayerName(string value)
        {
            // 중요
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
