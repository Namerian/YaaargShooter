using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YaaargShooter
{
    public class HERO_HUD : MonoBehaviour
    {
        // -- ATTRIBUTES

        [SerializeField] public HERO Hero;
        [SerializeField] public Text ScoreText;
        [SerializeField] public Text CurrentHealthText;
        [SerializeField] public Text InitialHealthText;

        // -- CONSTRUCTORS

        void Start()
        {

        }

        // -- OPERATIONS

        void Update()
        {
            ScoreText.text = Hero.Score.ToString();
            CurrentHealthText.text = Hero.Health.ToString();
            InitialHealthText.text = Hero.InitialHealth.ToString();
        }
    }
}