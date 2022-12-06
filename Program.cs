//Galaga spinoff Project
using System;
using Raylib_cs;
using System.Numerics;

// Our code will include eight classes; player, Score, rock, gem, shooter, falling objects, Point, & Game. 

namespace cse210_student_csharp_galaga
{
    // Main Function is to run the window and run the subsquent Class/Methods.
    public class Galaga
    {
        public static void Main()
        {
            int ScreenHeight = 480;
            int ScreenWidth = 800;
            int RectangleSize = 15;
            int MovementSpeed = 4;
            int count = 0;
            int score = 0;
            var Enemies = new List<Movement>();

            Rectangle PlayerRectangle = new Rectangle(ScreenWidth - (RectangleSize * 2), ScreenHeight - (RectangleSize * 2), RectangleSize, RectangleSize);

            Step step = new Step(MovementSpeed, Enemies, ScreenHeight, ScreenWidth, RectangleSize, count, score);
            Player player = new Player(MovementSpeed, PlayerRectangle, RectangleSize, step);


            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                player.Input();
                player.drawPlayer();


                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }
    }
    public class Player
    {
        public Player(int MovementSpeed, Rectangle Player, int size, Step step)
        {
            PlayerRectangle = Player;
            Speed = MovementSpeed;
            Size = size;
            var playerPosition = new Vector2(Player.x, Player.y); 
            steps = step;
        }
        public Rectangle PlayerRectangle;
        int Speed;
        int Size;
        Step steps;
        
        public void Input()
        {
            var playerPosition = new Vector2(PlayerRectangle.x, PlayerRectangle.y);
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D)) 
            {
                if (PlayerRectangle.x <= 800 - Size)
                {
                    PlayerRectangle.x += Speed;
                }
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_A)) 
            {
                if (PlayerRectangle.x >= 0)
                {
                    PlayerRectangle.x -= Speed;
                }
                
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                // Object Reference Not found 
                //steps.CreateObjects(2, -Speed, playerPosition, Size);
            }
        }
        public void drawPlayer()
        {
            Raylib.DrawRectangleRec(PlayerRectangle, Color.WHITE); 
        }
        
    }
    public class Step
    {
        public Step(int MovementSpeed, List<Movement> enemies, int ScreenHeight, int ScreenWidth, int RectangleSize, int count, int score)
        {
            Speed = MovementSpeed;
            Enemies = enemies; 
            Height = ScreenHeight;
            Width = ScreenWidth;
            Size = RectangleSize;
            Count = count;
            Score = score;
        }
        int Speed;
        List<Movement> Enemies;
        int Height;
        int Width;
        int Size;
        int Count;
        int Score;
    }
    
    abstract public class Movement
    {
        public Vector2 Position { get; set; } = new Vector2(0,0);
        public Vector2 Velocity { get; set; } = new Vector2(0,0);

        virtual public void Draw() 
        {
            // Base game object do not have anything to draw.
        }

        public void Move()
        {

        }
    }
    public class ColoredObjects : Movement
    {
        public Color Color { get; set; }
        public ColoredObjects(Color color) { Color = color; }
    }
    public class Enemylist
    {

    }
    //public class ButterflyE : ColoredObjects
    //{
//
    //}
    //public class BeeE : ColoredObjects
    //{
//
    //}
    //public class Bolt : ColoredObjects
    //{
//
    //}
}//