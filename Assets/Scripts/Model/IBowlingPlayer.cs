public interface IBowlingPlayer
{
    string Name { get; }
    Turn[] Turns { get; set; }
    void Throw(int pinsThrown);
    void Attach(IPlayerObserver  observer);
}