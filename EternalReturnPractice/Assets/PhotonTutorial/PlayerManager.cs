using Photon.Pun;
using UnityEngine;

namespace Nameless
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IHittable
    {
        [Tooltip("The Current Health of our player")]
        public float MaxHp { get; set; } = 1000;
        public float CurrentHp { get; set; }

        private void Awake()
        {
            
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
    }
}
