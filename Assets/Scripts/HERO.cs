using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO : CHARACTER
    {
        // -- CONSTANTS

        [Header("Hero")]
        //[SerializeField] private HERO_CAMERA HeroCamera;
        [SerializeField] private Transform GunMuzzle;
        [SerializeField] private Transform CameraPivot;
        [SerializeField] private Transform CameraPosition;

        [SerializeField] private float DefaultCameraDistance = -4;
        [SerializeField] private float CameraSpeedZ = 15;

        [SerializeField] private float DefaultLookDistance = 20;
        [SerializeField] private float LookRayMaxDistance = 100;

        public override float MaximumForwardWalkingSpeed { get { return 3; } }
        public override float MaximumForwardRunningSpeed { get { return 6; } }
        public override float MaximumSidewaysWalkingSpeed { get { return 3; } }
        public override float MaximumTurningSpeed { get { return 180; } }

        public override int InitialHealth { get { return 100; } }
        public override float AttackDelay { get { return 0.5f; } }

        private LayerMask LayerMask;

        // -- ATTRIBUTES

        private float CurrentCameraDistance;
        private Vector3 LookPosition;
        private bool RightTriggerPressed;


        // -- CONSTRUCTORS

        protected override void Start()
        {
            base.Start();

            LayerMask = ~gameObject.layer;
        }

        // -- INQUIRIES

        public Transform GetPivot()
        {
            return CameraPivot;
        }

        // -- OPERATIONS

        protected override void OnUpdate(float deltaTime)
        {
            // -
            HandleInput(deltaTime);

            // -
            ComputeLookPoint();

            // - 
            HandleCameraCollision();
            CameraPosition.localPosition = new Vector3(0, 0, Mathf.Lerp(CameraPosition.localPosition.z, CurrentCameraDistance, deltaTime * CameraSpeedZ));
        }

        protected override void DoAttack()
        {
            var ray = Instantiate(Resources.Load<GameObject>("Prefabs/LaserRay"), GunMuzzle.position, Quaternion.identity).GetComponent<LASER_RAY>();
            ray.Initialize(tag, LookPosition - GunMuzzle.position);
        }

        private void HandleInput(float deltaTime)
        {
            Vector2 right_stick = new Vector2(Input.GetAxis("R_XAxis_1"), -Input.GetAxis("R_YAxis_1"));
            float forwards_speed = right_stick.y > 0 ? MaximumForwardRunningSpeed : MaximumForwardWalkingSpeed;
            Vector3 forwards_movement = Transform.forward * right_stick.y * forwards_speed;
            Vector3 sideways_movement = Transform.right * right_stick.x * MaximumSidewaysWalkingSpeed;
            Velocity = forwards_movement + sideways_movement;

            TargetRotation *= Quaternion.AngleAxis(Input.GetAxis("L_XAxis_1") * MaximumTurningSpeed * deltaTime, Vector3.up);

            float right_trigger_input = Input.GetAxis("TriggersR_1");
            if (!RightTriggerPressed && right_trigger_input == 1)
            {
                WantsToAttack = true;
                RightTriggerPressed = true;
            }
            else if (right_trigger_input == 0)
            {
                RightTriggerPressed = false;
            }
        }

        private void ComputeLookPoint()
        {
            Vector3 origin = CameraPosition.position;
            Vector3 direction = CameraPosition.forward;
            Ray ray = new Ray(origin, direction);
            RaycastHit hit;

            //Debug.DrawRay(ray.origin, ray.direction);

            if (Physics.Raycast(ray, out hit, LookRayMaxDistance, LayerMask))
            {
                LookPosition = hit.point;
            }
            else
            {
                LookPosition = ray.GetPoint(DefaultLookDistance);
            }

            Debug.DrawLine(CameraPosition.position, LookPosition, Color.blue);
        }

        private void HandleCameraCollision()
        {
            Vector3 origin = CameraPivot.position;
            Vector3 direction = CameraPosition.position - origin;
            RaycastHit hit;

            CurrentCameraDistance = DefaultCameraDistance;

            if (Physics.Raycast(origin, direction, out hit, Mathf.Abs(CurrentCameraDistance), LayerMask))
            {
                Debug.Log("hit");
                float distance_to_hit = Vector3.Distance(origin, hit.point);
                CurrentCameraDistance = -distance_to_hit;
            }
        }
    }
}    // end of namespace