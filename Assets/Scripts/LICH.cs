using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class LICH : ENEMY
    {
        // -- ATTRIBUTES

        [Header("Lich")]
        [SerializeField] public float AttackRange = 20;
        [SerializeField] public int AttackDamage = 75;

        [SerializeField] public GameObject FireballPrefab;
        [SerializeField] public Transform StaffTop;

        // -- CONSTRUCTORS

        protected override void Start()
        {
            base.Start();
        }

        // -- OPERATIONS

        protected override bool CheckWantsToAttack()
        {
            Vector3 target_position = Vector3.zero;
            if (ComputeTargetPosition(out target_position))
            {
                return true;
            }

            return false;
        }

        protected override void DoAttack()
        {
            Vector3 target_position = Vector3.zero;
            ComputeTargetPosition(out target_position);

            PROJECTILE projectile = Instantiate(FireballPrefab, StaffTop.position, transform.rotation).GetComponent<PROJECTILE>();
            projectile.transform.forward = (target_position - StaffTop.position);
            projectile.OwnerTag = tag;
        }

        private bool ComputeTargetPosition(out Vector3 target_position)
        {
            target_position = Game.Hero.DetectionTarget.position + Game.Hero.DesiredVelocity * 2;

            if((target_position-DetectionTarget.position).magnitude < AttackRange)
            {
                return true;
            }

            return false;
        }
    }
}    // end of namespace