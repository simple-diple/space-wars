using UnityEngine;

namespace View
{
    public class WeaponView : GameObjectView
    {
        public Transform spawnBulletAnchor;
        public BulletView bulletView;

        public void Fire(float force, int player, int damage)
        {
            var bullet = Instantiate(bulletView, spawnBulletAnchor);
            bullet.body.AddForce(transform.forward * force, ForceMode.Force);
            bullet.body.gameObject.layer = LayerMask.NameToLayer("Player" + player);
        }
    }
}