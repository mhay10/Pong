class Paddle
{
    // Properties
    public const int WIDTH = 15;
    public const int HEIGHT = 60;

    // Variables
    SFML.Graphics.RectangleShape _shape;
    SFML.Graphics.Color _color;
    SFML.Graphics.RenderWindow _window;
    SFML.System.Vector2f _pos;

    // Constructor
    public Paddle(SFML.Graphics.RenderWindow window, SFML.Graphics.Color color, float x, float y)
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

    // Draws the paddle
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

    // Moves the paddle up
    public void MoveUp(int vel, int borderTop)
    {
        if (_pos.Y > borderTop)
            _pos.Y -= vel;
        else
            _pos.Y = borderTop;

        _shape.Position = _pos;
    }

    // Moves the paddle down
    public void MoveDown(int vel, int borderBottom)
    {
        if (_pos.Y < borderBottom)
            _pos.Y += vel;
        else
            _pos.Y = borderBottom;

        _shape.Position = _pos;
    }

    // Resets the paddle to middle
    public void Reset(float x, float y)
    {
        _pos.X = x;
        _pos.Y = y;

        _shape.Position = _pos;
    }
}