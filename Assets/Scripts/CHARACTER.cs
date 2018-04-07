using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public abstract class CHARACTER : MonoBehaviour
    {
        // -- ATTRIBUTES

        [Header("Character")]
        [SerializeField] public Animator Animator;
        [SerializeField] public Transform DetectionTarget;

        [SerializeField] public float MaxForwardSpeed;
        [SerializeField] public float MaxSidewaysSpeed;
        [SerializeField] public float MaxBackwardSpeed;
        [SerializeField] public float MaxTurningSpeed;

        [SerializeField] public int InitialHealth;
        [SerializeField] public float AttackDelay;
        [SerializeField] public float AttackDuration;

        [SerializeField] public bool HasForwardAnim;
        [SerializeField] public bool HasSidewaysAnim;
        [SerializeField] public bool HasDeathAnim;
        [SerializeField] public bool HasAttackAnim;
        [SerializeField] public bool HasHitAnim;


        [HideInInspector] public GAME Game;
        [HideInInspector] public float Health;
        [HideInInspector] public LayerMask LayerMask;

        /// <summary>
        /// The velocity of the character in world space.
        /// </summary>
        [HideInInspector] public Vector3 DesiredVelocity;
        /// <summary>
        /// The direction the character wants to face.
        /// </summary>
        [HideInInspector] public Quaternion DesiredRotation;

        [HideInInspector] public E_CHARACTER_STATE CurrentState = E_CHARACTER_STATE.IDLE;
        [HideInInspector] public bool CanAttack = true;
        [HideInInspector] public bool WantsToAttack;
        [HideInInspector] public bool IsAttacking;

        // -- CONSTRUCTORS

        protected virtual void Start()
        {
            Health = InitialHealth;
            LayerMask = ~gameObject.layer;

            DesiredRotation = transform.rotation;
        }

        // -- INQUIRIES

        public bool IsDead()
        {
            return Health <= 0;
        }

        // -- OPERATIONS

        public void Hit(string attacker_tag, int damage)
        {
            if (attacker_tag == tag)
            {
                return;
            }
            else if(CurrentState == E_CHARACTER_STATE.DEAD)
            {
                return;
            }

            Health -= damage;

            if (Health <= 0)
            {
                if (HasDeathAnim)
                {
                    Animator.SetTrigger("Death");
                }

                OnDeath();

                //StartCoroutine(DeathCoroutine());
            }
            else
            {
                if (HasHitAnim)
                {
                    Animator.SetTrigger("Hit");
                }

                OnHit();
            }
        }

        protected abstract void OnUpdate(float deltaTime);
        protected abstract void DoAttack();
        protected abstract void OnDeath();
        protected abstract void OnHit();

        private void Update()
        {
            OnUpdate(Time.deltaTime);

            if (CanAttack && WantsToAttack)
            {
                DoAttack();

                CanAttack = false;
                WantsToAttack = false;

                if (HasAttackAnim)
                {
                    Animator.SetTrigger("Attack");
                    Animator.SetBool("Attacking", true);
                    IsAttacking = true;
                }

                StartCoroutine(AttackDelayCoroutine());
                StartCoroutine(AttackingCoroutine());
            }
            else
            {
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
            if (!IsAttacking)
            {
                transform.Translate(transform.InverseTransformDirection(DesiredVelocity * deltaTime));
            }
        }

        private void Turn()
        {
            if (!IsAttacking)
            {
                transform.rotation = DesiredRotation;
            }
        }

        private void HandleAnimations()
        {
            Vector3 velocity_projected_forward = Vector3.Project(DesiredVelocity, transform.forward);
            Vector3 velocity_projected_sideways = Vector3.Project(DesiredVelocity, transform.right);

            float animator_forward = 0;
            float animator_sideways = 0;

            if (Vector3.Dot(transform.forward, velocity_projected_forward) > 0)
            {
                animator_forward = Mathf.Clamp(velocity_projected_forward.magnitude / MaxForwardSpeed, 0, 1);
            }
            else
            {
                animator_forward = Mathf.Clamp(-(velocity_projected_forward.magnitude / MaxBackwardSpeed), -1, 0);
            }

            if (Vector3.Dot(transform.right, velocity_projected_sideways) > 0)
            {
                animator_sideways = Mathf.Clamp(velocity_projected_sideways.magnitude / MaxSidewaysSpeed, 0, 1);
            }
            else
            {
                animator_sideways = Mathf.Clamp(-(velocity_projected_sideways.magnitude / MaxSidewaysSpeed), -1, 0);
            }

            if (HasForwardAnim)
            {
                Animator.SetFloat("Forward", animator_forward);
            }

            if (HasSidewaysAnim)
            {
                Animator.SetFloat("Sideways", animator_sideways);
            }
        }

        //private IEnumerator DeathCoroutine()
        //{
        //    while (!Animator.GetCurrentAnimatorStateInfo(0).IsName("Exit"))
        //    {
        //        yield return null;
        //    }

        //    OnDeath();
        //}

        private IEnumerator AttackDelayCoroutine()
        {
            yield return new WaitForSeconds(AttackDelay);

            CanAttack = true;
        }

        private IEnumerator AttackingCoroutine()
        {
            yield return new WaitForSeconds(AttackDuration);

            if (HasAttackAnim)
            {
                Animator.SetBool("Attacking", false);
            }
            IsAttacking = false;
        }
    }
}    // end of namespace