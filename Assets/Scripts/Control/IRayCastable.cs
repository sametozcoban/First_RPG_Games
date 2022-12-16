namespace RPG.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool handleRaycast(Player_Controller callingController);
    }
}
