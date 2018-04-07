using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class PROJECTILE : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] public Rigidbody Rigidbody;

        [SerializeField] public int Speed = 20;
        [SerializeField] public int Damage = 25;
        [SerializeField] public float LifeTime = 6;

        [SerializeField] public GameObject ImpactPrefab;

        [HideInInspector] protected Transform Transform;
        [HideInInspector] public string OwnerTag;

        // -- CONSTRUCTORS

        public void Start()
        {
            Transform = transform;
            Rigidbody.velocity = Speed * Transform.forward;
        }

        // -- OPERATIONS

        private void OnCollisionEnter(Collision collision)
        {
            var character = collision.collider.GetComponent<CHARACTER>();
            if (character != null)
            {
                character.Hit(OwnerTag, Damage);
            }

            Instantiate(ImpactPrefab, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject);
        }

        private IEnumerator EndOfLifeCoroutine()
        {
            yield return new WaitForSeconds(LifeTime);

            Destroy(gameObject);
        }
    }
}    // end of namespace