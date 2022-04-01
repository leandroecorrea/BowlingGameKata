public interface IBowlingPlayer
{
    string Name { get; }
    Turn[] Turns { get; }
    void Throw(int pinsThrown);
    void Attach(IPlayerObserver  observer);

    Turn CurrentTurn();
    Turn PreviousTurn();
    bool IsLastTurn();
}