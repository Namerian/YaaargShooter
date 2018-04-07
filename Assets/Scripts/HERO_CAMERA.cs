﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO_CAMERA : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] private HERO Hero;
        [SerializeField] private Transform Pivot;

        [SerializeField] private float MaximumXAngle = 30;
        [SerializeField] private float MinimumXAngle = -30;

        public Transform Transform { get; private set; }

        private float OrbitXAngle;

        // -- CONSTRUCTORS

        private void Start()
        {
            Transform = transform;
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
            Transform.position = Vector3.Lerp(Transform.position, Hero.transform.position, Time.deltaTime * 6);
            Transform.rotation = Hero.transform.rotation;
        }
    }
}    // end of namespace