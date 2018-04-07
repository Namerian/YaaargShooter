using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public abstract class ENEMY : CHARACTER
    {
        // -- ATTRIBUTES

        [Header("Enemy")]
        [SerializeField] public float ViewDistance = 50;
        [SerializeField] public float FieldOfView = 90;
        [SerializeField] public int ScoreValue = 2;

        // -- CONSTRUCTORS

        protected override void Start()
        {
            base.Start();
        }

        // -- OPERATIONS

        protected abstract bool CheckWantsToAttack();

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState == E_CHARACTER_STATE.IDLE)
            {
                if (CheckHeroInView())
                {
                    CurrentState = E_CHARACTER_STATE.ATTACKING;
                }
            }
            else if (CurrentState == E_CHARACTER_STATE.ATTACKING)
            {
                if (CheckWantsToAttack())
                {
                    WantsToAttack = true;
                    DesiredVelocity = Vector3.zero;
                }
                else
                {
                    FollowHero();
                }
            }
        }

        protected override void OnHit()
        {
            if(CurrentState == E_CHARACTER_STATE.IDLE)
            {
                CurrentState = E_CHARACTER_STATE.ATTACKING;
            }
        }

        protected override void OnDeath()
        {
            CurrentState = E_CHARACTER_STATE.DEAD;
            DesiredVelocity = Vector3.zero;

            Game.EnemyDied(this);
        }

        private bool CheckHeroInView()
        {
            Vector3 hero_position = Game.Hero.DetectionTarget.position;
            Vector3 direction_to_hero = hero_position - DetectionTarget.position;
            float angle_to_hero = Vector3.Angle(transform.forward, direction_to_hero);

            if (direction_to_hero.magnitude <= ViewDistance && angle_to_hero <= FieldOfView * 0.5f)
            {
                RaycastHit[] hits = Physics.RaycastAll(DetectionTarget.position, direction_to_hero, ViewDistance, LayerMask);

                if (hits.Length > 0 && hits[0].collider.tag == "Player")
                {
                    return true;
                }
            }

            return false;
        }

        private void FollowHero()
        {
            Vector3 hero_position = Game.Hero.transform.position;
            hero_position.y = transform.position.y;
            Vector3 direction_to_hero = hero_position - transform.position;

            Vector3 new_desired_velocity = Vector3.Project(direction_to_hero, transform.forward);
            new_desired_velocity = Vector3.ClampMagnitude(new_desired_velocity, MaxForwardSpeed);
            DesiredVelocity = new_desired_velocity;

            float rotation_to_hero = Vector3.SignedAngle(transform.forward, direction_to_hero, Vector3.up);
            rotation_to_hero = Mathf.Clamp(rotation_to_hero, -MaxTurningSpeed, MaxTurningSpeed);
            DesiredRotation *= Quaternion.Euler(0, rotation_to_hero, 0);
        }
    }
}    // end of namespace