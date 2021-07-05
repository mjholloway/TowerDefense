using UnityEngine;
using TowerDefense.Combat;

namespace TowerDefense.Deck.Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "Cards/Create New Card")]
    public class Card : ScriptableObject
    {
        public int attackValue = -1;
        public int defendValue = -1;
        public bool targets;

        public void PlayCard(CardActions cardActions)
        {
            if (attackValue >= 1)
            {
                cardActions.BasicAttack(attackValue);
            }
        }
    }
}