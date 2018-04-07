using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO_CAMERA : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] public HERO Hero;
        [SerializeField] public Transform Pivot;

        [SerializeField] public float MaximumXAngle = 30;
        [SerializeField] public float MinimumXAngle = -30;

        private float OrbitXAngle;

        // -- CONSTRUCTORS

        private void Start()
        {
        }

        // -- OPERATIONS

        private void Update()
        {
            Orbit();
            Follow();
        }

        private void Orbit()
        {
            float left_stick_vertical = Input.GetAxis("L_YAxis_1");
            OrbitXAngle += left_stick_vertical * Hero.MaxTurningSpeed * Time.deltaTime;
            OrbitXAngle = Mathf.Clamp(OrbitXAngle, MinimumXAngle, MaximumXAngle);

            Pivot.localRotation = Quaternion.Euler(OrbitXAngle, 0, 0);
        }

        private void Follow()
        {
            transform.position = Vector3.Lerp(transform.position, Hero.transform.position, Time.deltaTime * 6);
            transform.rotation = Hero.transform.rotation;
        }
    }
}    // end of namespace