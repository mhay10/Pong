using SFML.System;

class Game
{
    const int WIN_WIDTH = 640;
    const int WIN_HEIGHT = 480;

    static unsafe void Main()
    {
        // Initialize window
        var window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode(WIN_WIDTH, WIN_HEIGHT), "Pong",
            SFML.Window.Styles.Titlebar | SFML.Window.Styles.Close);
        window.KeyPressed += Window_KeyPressed;
        window.SetFramerateLimit(60);

        // Set game borders
        var borderTop = 60;
        var borderBottom = WIN_HEIGHT;
        var borderLeft = 0;
        var borderRight = WIN_WIDTH;

        // Create sprites
        var player = new Paddle(window, SFML.Graphics.Color.White, 15, (WIN_HEIGHT + Paddle.HEIGHT) / 2);
        var computer = new Paddle(window, SFML.Graphics.Color.White, WIN_WIDTH - (15 + Paddle.WIDTH), (WIN_HEIGHT + Paddle.HEIGHT) / 2);
        var ball = new Ball(window, SFML.Graphics.Color.White, (WIN_WIDTH - Ball.WIDTH) / 2, (WIN_HEIGHT + Paddle.HEIGHT) / 2);
        var scoreboard = new Scoreboard(window, borderTop);

        // Player constants
        const int PLAYER_SPEED = 5;

        // Computer constants
        const int COMPUTER_SPEED = 4;

        // Ball constants 
        const int baseVel = 5;
        int xvel = 0, yvel = 0;

        // Setup field
        ResetField(ball, player, computer, &xvel, &yvel, baseVel, true);

        // Main loop
        while (window.IsOpen)
        {
            window.DispatchEvents();

            // Player movement
            bool up = SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Up);
            bool down = SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Down);

            if (up && down) ; // Do nothing if both are pressed
            else if (up)
                player.MoveUp(PLAYER_SPEED, borderTop);
            else if (down)
                player.MoveDown(PLAYER_SPEED, borderBottom - Paddle.HEIGHT);

            // Computer movement
            if (computer.GetPos().Y + ((Paddle.HEIGHT - Ball.HEIGHT) / 2) < ball.GetPos().Y)
                computer.MoveDown(COMPUTER_SPEED, borderBottom - Paddle.HEIGHT);
            else if (computer.GetPos().Y + ((Paddle.HEIGHT - Ball.HEIGHT) / 2) > ball.GetPos().Y)
                computer.MoveUp(COMPUTER_SPEED, borderTop);

            // Ball movement
            ball.Move(xvel, yvel);

            // Bouce off top and bottom
            if (ball.GetPos().Y <= borderTop || ball.GetPos().Y >= borderBottom - Ball.HEIGHT)
                yvel *= -1;

            // Bounce back if hit edge
            bool playerTopEdge = ball.GetRect().Contains(player.GetPos().X + Paddle.WIDTH, player.GetPos().Y) && yvel > 0;
            bool playerBottomEdge = ball.GetRect().Contains(player.GetPos().X + Paddle.WIDTH, player.GetPos().Y + Paddle.HEIGHT) && yvel < 0;

            bool computerTopEdge = ball.GetRect().Contains(computer.GetPos().X, computer.GetPos().Y) && yvel > 0;
            bool computerBottomEdge = ball.GetRect().Contains(computer.GetPos().X, computer.GetPos().Y + Paddle.HEIGHT) && yvel < 0;

            // Bounce off paddles
            bool playerCollision = ball.GetRect().Intersects(player.GetRect());
            bool computerCollision = ball.GetRect().Intersects(computer.GetRect());

            // Corner collision
            if (playerTopEdge || playerBottomEdge)
            {
                ball.Reset(player.GetPos().X + Paddle.WIDTH, ball.GetPos().Y);
                yvel *= -1;
            }
            else if (computerTopEdge || computerBottomEdge)
            {
                ball.Reset(computer.GetPos().X - Ball.WIDTH, ball.GetPos().Y);
                yvel *= -1;
            }

            // Regular collision
            if (playerCollision || computerCollision)
                xvel *= -1;

            // Increase score if ball hits sides
            if (ball.GetPos().X <= borderLeft)
            {
                scoreboard.AwardComputer();
                ResetField(ball, player, computer, &xvel, &yvel, baseVel);
            }
            else if (ball.GetPos().X >= borderRight - Ball.WIDTH)
            {
                scoreboard.AwardPlayer();
                ResetField(ball, player, computer, &xvel, &yvel, baseVel);
            }

            // Update screen
            window.Clear();

            player.Draw();
            computer.Draw();
            ball.Draw();
            scoreboard.Draw();

            window.Display();
        }
    }

    // Reset field to initial state
    static unsafe void ResetField(Ball ball, Paddle player, Paddle computer,
        int* xvel, int* yvel, int baseVel, bool ignoreWait = false)
    {
        // Recenter ball
        ball.Reset((WIN_WIDTH - Ball.WIDTH) / 2, (WIN_HEIGHT + Paddle.HEIGHT) / 2);

        // Recenter paddles
        player.Reset(player.GetPos().X, (WIN_HEIGHT + Paddle.HEIGHT) / 2);

        // Randomize ball direction
        var rand = new System.Random();
        *xvel = baseVel * (rand.NextDouble() < 0.5 ? -1 : 1);
        *yvel = baseVel * (rand.NextDouble() < 0.5 ? -1 : 1);

        // Wait for spacebar
        while (!ignoreWait && !SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Space))
            if (SFML.Window.Keyboard.IsKeyPressed(SFML.Window.Keyboard.Key.Q))
                System.Environment.Exit(0);
    }

    // Close window when Q is pressed
    static void Window_KeyPressed(object? sender, SFML.Window.KeyEventArgs e)
    {
        if (e.Code == SFML.Window.Keyboard.Key.Q)
        {
            var window = (SFML.Graphics.RenderWindow)sender;
            window.Close();
        }
    }
}