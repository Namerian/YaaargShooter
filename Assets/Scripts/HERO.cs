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

        protected override void OnUpdate()
        {
            // -
            Vector2 right_stick = new Vector2(Input.GetAxis("R_XAxis_1"), -Input.GetAxis("R_YAxis_1"));
            float forwards_speed = right_stick.y > 0 ? MaximumForwardRunningSpeed : MaximumForwardWalkingSpeed;
            Vector3 forwards_movement = Transform.forward * right_stick.y * forwards_speed;
            Vector3 sideways_movement = Transform.right * right_stick.x * MaximumSidewaysWalkingSpeed;
            Velocity = forwards_movement + sideways_movement;

            TargetRotation *= Quaternion.AngleAxis(Input.GetAxis("L_XAxis_1") * MaximumTurningSpeed * Time.deltaTime, Vector3.up);

            if (Input.GetAxis("TriggersR_1") == 1)
            {
                Attack();
            }

            // -       
            Ray ray = new Ray(CameraPosition.position, CameraPosition.forward);
            Debug.DrawRay(ray.origin, ray.direction);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, LookRayMaxDistance, LayerMask))
            {
                LookPosition = hit.point;
            }
            else
            {
                LookPosition = ray.GetPoint(DefaultLookDistance);
            }
            
            // - 
            HandleCameraCollision(LayerMask);
            CameraPosition.localPosition = new Vector3(0, 0, Mathf.Lerp(CameraPosition.localPosition.z, CurrentCameraDistance, Time.deltaTime * CameraSpeedZ));
        }

        protected override void DoAttack()
        {
            var ray = Instantiate(Resources.Load<GameObject>("Prefabs/LaserRay"), GunMuzzle.position, Quaternion.identity).GetComponent<LASER_RAY>();
            ray.Initialize(tag, LookPosition - GunMuzzle.position);
        }

        private void HandleCameraCollision(LayerMask layer_mask)
        {
            Vector3 origin = CameraPivot.position;
            Vector3 direction = CameraPosition.position - origin;
            RaycastHit hit;

            CurrentCameraDistance = DefaultCameraDistance;

            if(Physics.Raycast(origin, direction, out hit, CurrentCameraDistance, layer_mask))
            {
                Debug.Log("hit");
                float distance_to_hit = Vector3.Distance(origin, hit.point);
                CurrentCameraDistance = -distance_to_hit;
            }
        }
    }
}    // end of namespace