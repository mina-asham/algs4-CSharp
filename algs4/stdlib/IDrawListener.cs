namespace algs4.stdlib
{
    public interface IDrawListener
    {
        void MouseDown(double x, double y);
        void MouseDragged(double x, double y);
        void MouseUp(double x, double y);
        void KeyPressed(char c);
    }
}