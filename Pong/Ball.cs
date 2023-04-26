using SFML.Window;

internal class Ball
{
    // Properties
    public const int WIDTH = 15;

    public const int HEIGHT = 15;

    // Variables
    SFML.Graphics.RectangleShape _shape;
    SFML.Graphics.Color _color;
    SFML.Graphics.RenderWindow _window;
    SFML.System.Vector2f _pos;

    // Constructor
    public Ball(SFML.Graphics.RenderWindow window, SFML.Graphics.Color color, float x, float y)
    {
        _window = window;
        _color = color;
        _pos = new(x, y);
        _shape = new(new SFML.System.Vector2f(WIDTH, HEIGHT))
        {
            FillColor = _color,
            Position = _pos
        };
    }

    // Draws the ball
    public void Draw()
    {
        _window.Draw(_shape);
    }

    // Returns current position
    public SFML.System.Vector2f GetPos()
    {
        return _pos;
    }

    // Returns rect
    public SFML.Graphics.FloatRect GetRect()
    {
        return _shape.GetGlobalBounds();
    }

    // Resets the ball to middle
    public void Reset(float x, float y)
    {
        _pos.X = x;
        _pos.Y = y;

        _shape.Position = _pos;
    }

    // Moves the ball
    public void Move(int xvel, int yvel)
    {
        _pos.X += xvel;
        _pos.Y += yvel;

        _shape.Position = _pos;
    }

}