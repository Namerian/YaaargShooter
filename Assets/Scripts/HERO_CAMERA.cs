using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO_CAMERA : MonoBehaviour
    {
        // -- CONSTANTS

        [SerializeField] private HERO Hero;
        [SerializeField] private Transform Pivot;

        [SerializeField] private float SmoothTime = 0.5f;
        [SerializeField] private float MaximumXAngle = 30;
        [SerializeField] private float MinimumXAngle = -30;

        // -- ATTRIBUTES

        public Transform Transform { get; private set; }

        private float OrbitXAngle;


        // -- CONSTRUCTORS

        private void Start()
        {
            Transform = transform;
        }

        // -- OPERATIONS

        //private void Update()
        //{
        //float rotation = Input.GetAxis("L_YAxis_1") * Hero.MaximumTurningSpeed * Time.deltaTime;
        //float new_angle = Vector3.SignedAngle(Pivot.forward, Quaternion.Euler(rotation, 0, 0) * Pivot.forward, Pivot.right);

        //float rotation = Input.GetAxis("L_YAxis_1") * Hero.MaximumTurningSpeed * Time.deltaTime;
        //var current_look_dir = Transform.position - Target.position;
        //var base__direction = current_look_dir;
        //base__direction.y = 0;
        //float current_angle = Vector3.SignedAngle(current_look_dir, base__direction, Target.right);
        //float new_angle = Mathf.Clamp(current_angle + rotation, MaximumUpAngle, MaximumDownAngle);
        //rotation = new_angle - current_angle;
        //Transform.RotateAround(Target.position, Target.right, rotation);

        //var new_rotation = Transform.rotation.eulerAngles;
        //new_rotation.x = Mathf.Clamp(new_rotation.x + Input.GetAxis("L_XAxis_1") * Hero.MaximumTurningSpeed, MaximumDownAngle, MaximumUpAngle);
        //Transform.Rotate(new_rotation.x - Transform.rotation.eulerAngles.x, 0, 0);           
        //}

        //private void LateUpdate()
        //{
        //    Move();
        //    LookAt();
        //}

        private void Update()
        {
            Orbit();
            Follow();
        }

        //private void Move()
        //{
        //temp_target_position = Hero.GetPivot().position;
        //temp_destination = Quaternion.Euler(temp_x_rotation, temp_y_rotation, 0) * -Vector3.forward * DistanceFromTarget;
        //Transform.position = temp_destination + temp_target_position;
        //}

        //private void LookAt()
        //{


        //Vector2 aim_input = new Vector2(Input.GetAxis("L_XAxis_1"), Input.GetAxis("L_YAxis_1"));

        //Quaternion target_y_rotation = Quaternion.AngleAxis(aim_input.x * Hero.MaximumTurningSpeed * Time.deltaTime, Transform.up);
        //Quaternion target_x_rotation = Quaternion.AngleAxis(aim_input.y * )

        //float euler_y_angle = Mathf.SmoothDampAngle(Transform.eulerAngles.y, Pivot.eulerAngles.y, ref current_y_angle_velocity, SmoothTime/*, Hero.MaximumTurningSpeed*/);

        //Transform.rotation = Quaternion.Euler(Transform.eulerAngles.x, euler_y_angle, 0);
        //}

        private void Orbit()
        {
            float left_stick_vertical = -Input.GetAxis("L_YAxis_1");
            OrbitXAngle += left_stick_vertical * Hero.MaximumTurningSpeed * Time.deltaTime;
            OrbitXAngle = Mathf.Clamp(OrbitXAngle, MinimumXAngle, MaximumXAngle);

            Pivot.localRotation = Quaternion.Euler(OrbitXAngle, 0, 0);
        }

        private void Follow()
        {
            Transform.position = Vector3.Lerp(Transform.position, Hero.Transform.position, Time.deltaTime * 6);
            Transform.rotation = Hero.TargetRotation;
        }
    }
}    // end of namespace