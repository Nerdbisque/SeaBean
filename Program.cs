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
            int ScreenHeight = 800;
            int ScreenWidth = 600;
            int RectangleSize = 25;
            int MovementSpeed = 5;
            int count = 0;
            int score = 0;
            var Enemies = new List<Movement>();
            bool playGame = true;
            int whichPosition = 0;

            Rectangle PlayerRectangle = new Rectangle(ScreenWidth / 2, ScreenHeight - 100, RectangleSize, RectangleSize);

            Step step = new Step(MovementSpeed, Enemies, ScreenHeight, ScreenWidth, RectangleSize, count, score, whichPosition);
            Player player = new Player(MovementSpeed, PlayerRectangle, RectangleSize, step);

            Raylib.InitWindow(ScreenWidth, ScreenHeight, "GALAGA");
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);
                if (!playGame)
                {
                    player.Input();
                    step.Instance(player.PlayerRectangle);
                }
                if (playGame)
                {
                    playGame = player.startGame(playGame);
                    Raylib.DrawText("Press Space To Begin", 200, 375, 20, Color.WHITE);
                }
                
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
            steps = step;
        }
        public Rectangle PlayerRectangle;
        int Speed;
        int Size;
        Step steps;
        
        public void Input()
        {
            var playerPosition = new Vector2(PlayerRectangle.x, PlayerRectangle.y);
            var boltPosition = new Vector2(PlayerRectangle.x + (Size/2) - (Size/8) , PlayerRectangle.y);
            if (Raylib.IsKeyDown(KeyboardKey.KEY_D)) 
            {
                if (PlayerRectangle.x <= 550 - Size)
                {
                    PlayerRectangle.x += Speed;
                }
            }

            if (Raylib.IsKeyDown(KeyboardKey.KEY_A)) 
            {
                if (PlayerRectangle.x >= 50)
                {
                    PlayerRectangle.x -= Speed;
                }
                
            }
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                // Object Reference Not found
                steps.CreateEnemies(2, -Speed, boltPosition, Size);
            }
        }
        public bool startGame(bool playGame)
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                playGame = false;
            }
            return playGame;
        }
        public void drawPlayer()
        {
            Raylib.DrawRectangleRec(PlayerRectangle, Color.WHITE); 
        }
        
    }
    public class Step
    {
        public Step(int MovementSpeed, List<Movement> enemies, int ScreenHeight, int ScreenWidth, int RectangleSize, int count, int score, int WhichPosition)
        {
            Speed = MovementSpeed;
            Enemies = enemies; 
            Height = ScreenHeight;
            Width = ScreenWidth;
            Size = RectangleSize;
            Count = count;
            Score = score;
            whichPosition = WhichPosition;
        }
        int Speed;
        List<Movement> Enemies;
        int Height;
        int Width;
        int Size;
        int Count;
        int Score;
        int whichPosition;

        public void Instance(Rectangle PlayerRectangle)
        {
            int setVelocity = 0;
            var position = new Vector2(0, 0);
            var objectsToRemove = new List<Movement>();

            if (Count <= 7)
            {
                CreateEnemies(0, setVelocity, position, Size);
                CreateEnemies(0, setVelocity, position, Size);
                Count += 2;
            }
            if (Count <= 7)
            {
                CreateEnemies(1, setVelocity, position, Size);
                Count += 1;
            }


            foreach (var obj in Enemies) 
            {
                obj.Draw();
            }

            foreach (var obj in Enemies) 
            {
                obj.Move();
            }
            //// Check for Collisions
            //foreach (var obj in Enemies)
            //{
            //    if(obj is Rock)
            //    {
            //        Rock rock = (Rock)obj;
            //        if (Raylib.CheckCollisionRecs(rock.eachRectangle(), PlayerRectangle)) 
            //        {
            //            subScore(1);
            //            objectsToRemove.Add(obj);
            //        }
            //        foreach (var col in objects)
            //        {
            //            if (col is Bolt)
            //            {
            //                Bolt bolt = (Bolt)col;
            //                if (Raylib.CheckCollisionRecs(bolt.eachboltRectangle(), rock.eachRectangle()))
            //                {
            //                    addScore(2);
            //                    objectsToRemove.Add(obj);
            //                    objectsToRemove.Add(col);
            //                }
            //            }
            //        }   
            //    }
            //    else if (obj is Gem)
            //    {
            //        
            //        Gem gem = (Gem)obj;
            //        if (Raylib.CheckCollisionCircleRec(gem.Position, gem.Radius, PlayerRectangle)) 
            //        {
            //            addScore(1);
            //            objectsToRemove.Add(obj);
            //        }  
            //        foreach (var col in objects)
            //        {
            //            if (col is Bolt)
            //            {
            //                Bolt bolt = (Bolt)col;
            //                if (Raylib.CheckCollisionCircleRec(gem.Position, gem.Radius, bolt.eachboltRectangle()))
            //                {
            //                    subScore(2);
            //                    objectsToRemove.Add(obj);
            //                    objectsToRemove.Add(col);
            //                }
            //            }
            //        }   
            //    }
            //}
            foreach (var obj in Enemies)
            {
                if (obj.Position.Y > Height + 30)
                {
                    objectsToRemove.Add(obj);
                }
                if (obj.Position.Y <  -30)
                {
                    objectsToRemove.Add(obj);
                }
            }
            Enemies = Enemies.Except(objectsToRemove).ToList();
        }

        public void CreateEnemies(int whichType, int setVelocity, Vector2 position, int Size)
        {
            switch (whichType) 
                {
                case 0:
                    var butter = new ButterflyE(Color.GREEN, Size);
                    butter.Position = findSpawnPosition();
                    butter.Velocity = new Vector2(0, 0);
                    Enemies.Add(butter);
                    break;
                case 1:
                    var bee = new BeeE(Color.BLUE, Size);
                    bee.Position =  findSpawnPosition();
                    bee.Velocity = new Vector2(0, 0);
                    Enemies.Add(bee);
                    break;
                case 2:
                    int boltCount = 0;
                    foreach (var obj in Enemies)
                    {
                        if (obj is Bolt)
                        {
                            boltCount += 1;
                        }
                    }
                    if (boltCount < 3)
                    {
                        var bolt = new Bolt(Color.RED, Size);
                        bolt.Position = position;
                        bolt.Velocity = new Vector2(0, setVelocity);
                        Enemies.Add(bolt);
                    }
                    break;
                }
        }
        public Vector2 findSpawnPosition()
        {
            Vector2 firstPosition = new Vector2(64, 200);
            Vector2 spawnPosition = new Vector2(0, 0);
            if (whichPosition <= 10)
            {
                float xPosition = firstPosition.X * (whichPosition + 1);
                float yPosition = firstPosition.Y;
                spawnPosition = new Vector2(xPosition, yPosition);
            }
            whichPosition += 1;
            return(spawnPosition);
        }
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
        { //Randomized falling movement and synchronized side to side.
            Vector2 FirstPosition = Position;
            
            Vector2 NewPosition = Position;
            NewPosition.Y += Velocity.Y;
            Position = NewPosition;
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
    public class ButterflyE : ColoredObjects
    {
        public int Size { get; set; }
        public ButterflyE(Color color, int size): base(color) 
        {
            Size = size;
        }
        override public void Draw() {
            Raylib.DrawRectangle((int)Position.X, (int)Position.Y, Size, Size, Color);
        }
    }
    public class BeeE : ColoredObjects
    {
        public int Size { get; set; }
        public BeeE(Color color, int size): base(color) 
        {
            Size = size;
        }
        override public void Draw() {
            Raylib.DrawRectangle((int)Position.X, (int)Position.Y, Size, Size, Color);
        }
    }
    public class Bolt : ColoredObjects
    {
        public int Size { get; set; }
        public Bolt(Color color, int size): base(color) 
        {
            Size = size;
        }
        override public void Draw() {
            Raylib.DrawRectangle((int)Position.X, (int)Position.Y, Size/3, (Size+(Size/3))/2, Color);
        }
        public Rectangle eachboltRectangle()
        {
            Rectangle ownedboltRectangle = new Rectangle((int)Position.X, (int)Position.Y, Size/3, (Size+(Size/3))/2);
            return ownedboltRectangle;
        }
    }
}