using SFML.Window;

class Scoreboard
{
    // Variables
    SFML.Graphics.RenderWindow _window;
    SFML.Graphics.Text _playerText;
    SFML.Graphics.Text _computerText;
    // TODO: Make the path relative
    SFML.Graphics.Font _font = new("fonts/AncientModernTales.ttf");
    int _playerScore = 0;
    int _computerScore = 0;
    int _fieldTop;

    // Font sizes
    const int scoreTextSize = 40;
    const int titleTextSize = 45;

    // Constructor
    public Scoreboard(SFML.Graphics.RenderWindow window, int fieldTop)
    {
        _window = window;

        _playerText = new("0", _font, scoreTextSize)
        {
            FillColor = SFML.Graphics.Color.White,
            Position = new(10, 5)
        };
        _computerText = new("0", _font, scoreTextSize)
        {
            FillColor = SFML.Graphics.Color.White,
            Position = new(_window.Size.X - 30, 5)
        };

        _fieldTop = fieldTop;
    }

    // Draws the scorecard
    public void Draw()
    {
        // Draw line at top of field
        var line = new SFML.Graphics.Vertex[2];
        line[0] = new SFML.Graphics.Vertex(new(0, _fieldTop), SFML.Graphics.Color.White);
        line[1] = new SFML.Graphics.Vertex(new(_window.Size.X, _fieldTop), SFML.Graphics.Color.White);
        _window.Draw(line, SFML.Graphics.PrimitiveType.Lines);

        // Draw title
        var title = new SFML.Graphics.Text("Pong", _font, titleTextSize)
        {
            FillColor = SFML.Graphics.Color.White
        };
        title.Position = new((_window.Size.X - title.GetLocalBounds().Width) / 2, 0);
        _window.Draw(title);

        // Draw scores
        _playerText.DisplayedString = $"P: {_playerScore}";
        _window.Draw(_playerText);

        _computerText.DisplayedString = $"C: {_computerScore}";
        _computerText.Position = new(_window.Size.X - _computerText.GetLocalBounds().Width - 10, 5);
        _window.Draw(_computerText);
    }

    // Increases player score
    public void AwardPlayer()
    {
        _playerScore++;
    }

    // Increases computer score
    public void AwardComputer()
    {
        _computerScore++;
    }
}