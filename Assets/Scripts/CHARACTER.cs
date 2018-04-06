using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    //[RequireComponent(typeof(CapsuleCollider))]
    public abstract class CHARACTER : MonoBehaviour
    {
        // -- CONSTANTS

        private const float DeadZone = 0.01f;

        public abstract float MaximumForwardWalkingSpeed { get; }
        public abstract float MaximumForwardRunningSpeed { get; }
        public abstract float MaximumSidewaysWalkingSpeed { get; }
        public abstract float MaximumTurningSpeed { get; }

        public abstract int InitialHealth { get; }
        public abstract float AttackDelay { get; }

        // -- ATTRIBUTES

        [Header("Character")]
        [SerializeField] private Rigidbody Rigidbody;

        public Transform Transform { get; private set; }

        private E_CHARACTER_STATE State;
        public float Health { get; private set; }

        public Vector3 Velocity { get; protected set; }
        public Quaternion TargetRotation { get; protected set; }

        private float AttackDelayTimer;

        // -- CONSTRUCTORS

        protected virtual void Start()
        {
            Transform = transform;

            State = E_CHARACTER_STATE.IDLE;
            Health = InitialHealth;

            TargetRotation = Transform.rotation;
        }

        // -- INQUIRIES

        // -- OPERATIONS

        protected abstract void OnUpdate();

        private void Update()
        {
            if (AttackDelayTimer > 0)
            {
                AttackDelayTimer = Mathf.Clamp(AttackDelayTimer - Time.deltaTime, 0, float.MaxValue);
            }

            OnUpdate();

            Turn();
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        private void Move(float deltaTime)
        {
            //Debug.Log(Velocity);
            //Rigidbody.velocity = Velocity;
            Transform.Translate(Transform.InverseTransformDirection(Velocity * deltaTime));
        }

        private void Turn()
        {
            Transform.rotation = TargetRotation;
        }

        public void OnHit(string attacker_tag, int damage)
        {
            if(attacker_tag == tag)
            {
                return;
            }

            Health -= damage;

            if(Health <= 0)
            {
                // DIE
            }
        }

        protected void Attack()
        {
            if (AttackDelayTimer == 0)
            {
                DoAttack();
                AttackDelayTimer = AttackDelay;
            }
        }

        protected abstract void DoAttack();
    }
}    // end of namespace