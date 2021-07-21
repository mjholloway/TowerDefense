using TowerDefense.Combat;
using UnityEngine;

namespace TowerDefense.Deck
{
    public class SpitCard : MonoBehaviour, IPlayable
    {
        public bool targets { get; set; } = true;

        public void PlayCard()
        {
            GetComponentInParent<CardActions>().BasicAttack(-5);
        }
    }
}
