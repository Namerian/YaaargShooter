using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class GOLEM : ENEMY
    {
        // -- ATTRIBUTES

        [Header("Golem")]
        [SerializeField] public float AttackRange = 2;
        [SerializeField] public int AttackDamage = 100;
        [SerializeField] public float GolemHeight = 3;

        // -- CONSTRUCTORS

        protected override void Start()
        {
            base.Start();
        }

        // -- OPERATIONS

        protected override bool CheckWantsToAttack()
        {
            Vector3 center = transform.position + ((AttackRange * 0.5f) * transform.forward) + (transform.up * (GolemHeight * 0.5f));
            Vector3 half_extents = new Vector3(GolemHeight * 0.5f, GolemHeight * 0.5f, AttackRange * 0.5f);
            RaycastHit[] box_cast_hits = Physics.BoxCastAll(center, half_extents, transform.forward, transform.rotation, AttackRange, LayerMask);

            foreach (var hit in box_cast_hits)
            {
                if (hit.collider.tag == "Player")
                {
                    return true;
                }
            }

            return false;
        }

        protected override void DoAttack()
        {
            Game.Hero.Hit(tag, AttackDamage);
        }
    }
}    // end of namespace