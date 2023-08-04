using UnityEngine;

namespace View
{
    public class BulletView : GameObjectView
    {
        public Rigidbody body;
        public int damage;

        private void Awake()
        {
            Destroy(gameObject, 5);
        }

        private void OnCollisionEnter(Collision collision)
        {
            UnitCollider unitCollider = collision.gameObject.GetComponent<UnitCollider>();
            
            if (unitCollider)
            {
                unitCollider.unit.GetDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}