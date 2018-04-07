using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YaaargShooter
{
    public class GAME : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] public HERO Hero;
        [SerializeField] public Transform EnemyHolder;
        [SerializeField] public HERO_SPAWN HeroSpawn;

        [HideInInspector] public List<ENEMY> EnemiesList = new List<ENEMY>();

        // -- CONSTRUCTORS

        void Start()
        {
            Hero.transform.position = HeroSpawn.transform.position;
            Hero.Game = this;

            for (int enemyIndex = 0; enemyIndex < EnemyHolder.childCount; enemyIndex++)
            {
                ENEMY enemy = EnemyHolder.GetChild(enemyIndex).GetComponent<ENEMY>();

                if (enemy != null)
                {
                    enemy.Game = this;
                    EnemiesList.Add(enemy);
                }
            }
        }

        // -- OPERATIONS

        public void PlayerDied()
        {

        }

        public void EnemyDied(ENEMY enemy)
        {
            EnemiesList.Remove(enemy);

            Hero.Score += enemy.ScoreValue;
        }

        private void Update()
        {

        }
    }
}    // end of namespace