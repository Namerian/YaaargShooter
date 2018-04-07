using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class HERO_SPAWN : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] private float ActiveTime = 5;

        // -- CONSTRUCTORS

        void Start()
        {
            StartCoroutine(DisappearCoroutine());
        }

        // -- OPERATIONS

        private IEnumerator DisappearCoroutine()
        {
            yield return new WaitForSeconds(ActiveTime);

            Destroy(gameObject);
        }
    }
}    // end of namespace