using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    //[RequireComponent(typeof(CapsuleCollider))]
    public abstract class CHARACTER : MonoBehaviour
    {
        // -- CONSTANTS

        [Header("Character")]
        [SerializeField] private Rigidbody Rigidbody;
        [SerializeField] private Animator Animator;

        private const float DeadZone = 0.01f;

        public abstract float MaximumForwardWalkingSpeed { get; }
        public abstract float MaximumForwardRunningSpeed { get; }
        public abstract float MaximumSidewaysWalkingSpeed { get; }
        public abstract float MaximumTurningSpeed { get; }

        public abstract int InitialHealth { get; }
        public abstract float AttackDelay { get; }

        // -- ATTRIBUTES

        public Transform Transform { get; private set; }

        private E_CHARACTER_STATE State;
        public float Health { get; private set; }

        /// <summary>
        /// The velocity of the character in world space.
        /// </summary>
        protected Vector3 Velocity { get; set; }
        /// <summary>
        /// The direction the character wants to face.
        /// </summary>
        protected Quaternion TargetRotation { get; set; }

        private float AttackDelayTimer;
        protected bool WantsToAttack { get; set; }

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

        public void Hit(string attacker_tag, int damage)
        {
            if (attacker_tag == tag)
            {
                return;
            }

            Health -= damage;

            if (Health <= 0)
            {
                // DIE
            }
        }

        protected abstract void OnUpdate(float deltaTime);
        protected abstract void DoAttack();

        private void Update()
        {
            if (AttackDelayTimer > 0)
            {
                AttackDelayTimer = Mathf.Clamp(AttackDelayTimer - Time.deltaTime, 0, float.MaxValue);
            }

            OnUpdate(Time.deltaTime);

            if(AttackDelayTimer == 0 && WantsToAttack)
            {
                DoAttack();
                AttackDelayTimer = AttackDelay;
                WantsToAttack = false;
            }

            Turn();

            HandleAnimations();
        }

        private void FixedUpdate()
        {
            Move(Time.fixedDeltaTime);
        }

        private void Move(float deltaTime)
        {
            Transform.Translate(Transform.InverseTransformDirection(Velocity * deltaTime));
        }

        private void Turn()
        {
            
            Transform.rotation = TargetRotation;
        }

        public void HandleAnimations()
        {
            Vector3 velocity_projected_forward = Vector3.Project(Velocity, Transform.forward);
            Vector3 velocity_projected_sideways = Vector3.Project(Velocity, Transform.right);

            float animator_forward = 0;
            float animator_sideways = 0;

            if(Vector3.Dot(Transform.forward, velocity_projected_forward) > 0)
            {
                animator_forward = Mathf.Clamp(velocity_projected_forward.magnitude / MaximumForwardRunningSpeed, 0, 1);
            }
            else
            {
                animator_forward = Mathf.Clamp(-(velocity_projected_forward.magnitude / MaximumForwardWalkingSpeed), -1, 0);
            }

            if(Vector3.Dot(Transform.right, velocity_projected_sideways) > 0)
            {
                animator_sideways = Mathf.Clamp(velocity_projected_sideways.magnitude / MaximumSidewaysWalkingSpeed, 0, 1);
            }
            else
            {
                animator_sideways = Mathf.Clamp(-(velocity_projected_sideways.magnitude / MaximumSidewaysWalkingSpeed), -1, 0);
            }

            Animator.SetFloat("Forward", animator_forward);
            Animator.SetFloat("Sideways", animator_sideways);
        }
    }
}    // end of namespace