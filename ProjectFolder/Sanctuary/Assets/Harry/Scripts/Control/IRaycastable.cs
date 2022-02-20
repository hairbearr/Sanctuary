namespace Sanctuary.Harry.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerController callingController);
    }

}