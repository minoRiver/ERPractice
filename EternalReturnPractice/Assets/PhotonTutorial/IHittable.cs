using UnityEngine;

namespace Nameless
{
    public interface IHittable
    {
        public void Hit(int damage, GameObject sender = null);
    }
}
