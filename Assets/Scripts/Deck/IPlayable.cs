namespace TowerDefense.Deck
{
    public interface IPlayable
    {
        bool targets { get; set; }

        void PlayCard();
    }
}
