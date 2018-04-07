using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO : CHARACTER
    {
        // -- ATTRIBUTES

        [Header("Hero")]
        [SerializeField] private Transform GunMuzzle;
        [SerializeField] private Transform CameraPivot;
        [SerializeField] private Transform CameraPosition;

        [SerializeField] private float DefaultCameraDistance = -4;
        [SerializeField] private float CameraSpeedZ = 15;

        [SerializeField] private float DefaultLookDistance = 20;
        [SerializeField] private float LookRayMaxDistance = 100;

        [SerializeField] private GameObject ProjectilePrefab;

        private float CurrentCameraDistance;
        private Vector3 LookPosition;
        [HideInInspector] public int Score;

        // -- CONSTRUCTORS

        protected override void Start()
        {
            base.Start();

            CurrentState = E_CHARACTER_STATE.ATTACKING;
        }

        // -- OPERATIONS

        protected override void OnUpdate(float deltaTime)
        {
            if (CurrentState == E_CHARACTER_STATE.ATTACKING)
            {
                HandleInput(deltaTime);

                ComputeLookPoint();

                HandleCameraCollision();
                CameraPosition.localPosition = new Vector3(0, 0, Mathf.Lerp(CameraPosition.localPosition.z, CurrentCameraDistance, deltaTime * CameraSpeedZ));
            }
        }

        protected override void DoAttack()
        {
            PROJECTILE projectile = Instantiate(ProjectilePrefab, GunMuzzle.position, GunMuzzle.rotation).GetComponent<PROJECTILE>();
            projectile.transform.forward = (LookPosition - GunMuzzle.position);
            projectile.OwnerTag = tag;
        }

        protected override void OnHit()
        {

        }

        protected override void OnDeath()
        {
            CurrentState = E_CHARACTER_STATE.DEAD;

            Game.PlayerDied();
        }

        private void HandleInput(float deltaTime)
        {
            Vector2 right_stick = new Vector2(Input.GetAxis("R_XAxis_1"), -Input.GetAxis("R_YAxis_1"));
            float forwards_speed = right_stick.y > 0 ? MaxForwardSpeed : MaxBackwardSpeed;
            Vector3 forwards_movement = transform.forward * right_stick.y * forwards_speed;
            Vector3 sideways_movement = transform.right * right_stick.x * MaxSidewaysSpeed;
            DesiredVelocity = forwards_movement + sideways_movement;

            DesiredRotation *= Quaternion.AngleAxis(Input.GetAxis("L_XAxis_1") * MaxTurningSpeed * deltaTime, Vector3.up);

            float right_trigger_input = Input.GetAxis("TriggersR_1");
            if (right_trigger_input == 1)
            {
                WantsToAttack = true;
            }
        }

        private void ComputeLookPoint()
        {
            Vector3 origin = CameraPosition.position;
            Vector3 direction = CameraPosition.forward;
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, LookRayMaxDistance, LayerMask))
            {
                LookPosition = hit.point;
            }
            else
            {
                LookPosition = ray.GetPoint(DefaultLookDistance);
            }
        }

        private void HandleCameraCollision()
        {
            Vector3 origin = CameraPivot.position;
            Vector3 direction = CameraPosition.position - origin;
            RaycastHit hit;

            CurrentCameraDistance = DefaultCameraDistance;

            if (Physics.Raycast(origin, direction, out hit, Mathf.Abs(CurrentCameraDistance), LayerMask))
            {
                float distance_to_hit = Vector3.Distance(origin, hit.point);
                CurrentCameraDistance = -distance_to_hit;
            }
        }
    }
}    // end of namespace