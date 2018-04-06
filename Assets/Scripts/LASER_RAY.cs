using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class LASER_RAY : MonoBehaviour
    {
        [SerializeField] private Rigidbody Rigidbody;

        private int Speed = 20;
        private int Damage = 25;

        private Transform Transform;

        public string OwnerTag { get; private set; }
        private Vector3 StartPosition;

        public void Initialize(string ownerTag, Vector3 direction)
        {
            OwnerTag = ownerTag;

            Transform = transform;
            Transform.forward = direction;
            StartPosition = Transform.position;

            Rigidbody.velocity = Speed * direction;
        }

        private void Update()
        {
            if(Vector3.Distance(StartPosition, Transform.position) > 1000)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var character = other.GetComponent<CHARACTER>();
            if (character != null)
            {
                character.OnHit(OwnerTag, Damage);
            }

            if (!other.isTrigger)
            {
                Destroy(gameObject);
            }
        }
    }
}    // end of namespace