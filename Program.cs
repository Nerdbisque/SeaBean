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
            int Lives = 3;
            var Enemies = new List<Movement>();
            var Rand = new Random();
            bool playGame = true;
            int whichPosition = 1;

            Rectangle PlayerRectangle = new Rectangle(ScreenWidth / 2, ScreenHeight - 100, RectangleSize, RectangleSize);

            Step step = new Step(MovementSpeed, Enemies, ScreenHeight, ScreenWidth, RectangleSize, count, score, 
            whichPosition, Lives, Rand);
            Player player = new Player(MovementSpeed, PlayerRectangle, RectangleSize, step, Rand);

            Raylib.InitWindow(ScreenWidth, ScreenHeight, "GALAGA");
            Raylib.SetTargetFPS(60);

            while (!Raylib.WindowShouldClose())
            {
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.BLACK);

                step.newScore();
                step.newLives();
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
        public Player(int MovementSpeed, Rectangle Player, int size, Step step, Random rand)
        {
            PlayerRectangle = Player;
            Speed = MovementSpeed;
            Size = size; 
            steps = step;
            Rand = rand;
        }
        public Rectangle PlayerRectangle;
        int Speed;
        int Size;
        Step steps;
        Random Rand;
        
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
                steps.CreateEnemies(2, -10, boltPosition, Size, Rand);
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
        public Step(int MovementSpeed, List<Movement> enemies, int ScreenHeight, int ScreenWidth,
        int RectangleSize, int count, int score, int WhichPosition, int lives, Random Rand)
        {
            Speed = MovementSpeed;
            Enemies = enemies; 
            Height = ScreenHeight;
            Width = ScreenWidth;
            Size = RectangleSize;
            Count = count;
            Count2 = count;
            Score = score;
            Lives = lives;
            whichXPosition = WhichPosition;
            whichYPosition = WhichPosition;
            rand = Rand;

        }
        int Speed;
        List<Movement> Enemies;
        int Height;
        int Width;
        int Size;
        int Count;
        int Count2;
        int Score;
        int Lives;
        int whichXPosition;
        int whichYPosition;
        Random rand;

        public void Instance(Rectangle PlayerRectangle)
        {
            int setVelocity = 0;
            var position = new Vector2(0, 0);
            int whichType = rand.Next(2);
            var objectsToRemove = new List<Movement>();
            if (whichXPosition < 9)
            {
                CreateEnemies(whichType, setVelocity, position, Size, rand);
            }
            if (whichXPosition >= 9)
            {
                Count2 = 0;
                foreach (var obj in Enemies) 
                {
                    if(obj is BeeE)
                    {
                        Count2 += 1;
                        Console.WriteLine(Count2);
                    }
                    else if (obj is ButterflyE)
                    {
                        Count2 += 1;
                        Console.WriteLine(Count2);
                    }
                }
                if (Count2 == 0)
                {
                    whichXPosition = 1;
                }
            }

            // Make Background Pixels
            if (Count == 5)
            {
                CreateEnemies(3, setVelocity, position, Size, rand);
                Count = 0;
            }
            else
            {
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
            // Check for Collisions
            foreach (var obj in Enemies)
            {
                if(obj is BeeE)
                {
                    BeeE beeE = (BeeE)obj;
                    if (Raylib.CheckCollisionRecs(beeE.eachRectangle(), PlayerRectangle)) 
                    {
                        subLives(1);
                        objectsToRemove.Add(obj);
                    }
                    foreach (var col in Enemies)
                    {
                        if (col is Bolt)
                        {
                            Bolt bolt = (Bolt)col;
                            if (Raylib.CheckCollisionRecs(bolt.eachboltRectangle(), beeE.eachRectangle()))
                            {
                                addScore(50);
                                objectsToRemove.Add(obj);
                                objectsToRemove.Add(col);
                            }
                        }
                    }   
                }
                else if (obj is ButterflyE)
                {
                    
                    ButterflyE butterflyE = (ButterflyE)obj;
                    if (Raylib.CheckCollisionRecs(butterflyE.eachRectangle(), PlayerRectangle)) 
                    {
                        subLives(1);
                        objectsToRemove.Add(obj);
                    }  
                    foreach (var col in Enemies)
                    {
                        if (col is Bolt)
                        {
                            Bolt bolt = (Bolt)col;
                            if (Raylib.CheckCollisionRecs(bolt.eachboltRectangle(), butterflyE.eachRectangle()))
                            {
                                addScore(80);
                                objectsToRemove.Add(obj);
                                objectsToRemove.Add(col);
                            }
                        }
                    }   
                }
            }
            foreach (var obj in Enemies)
            {
                if (obj.Position.Y > Height + 30)
                {
                    objectsToRemove.Add(obj);
                }
                if (obj.Position.Y < -30)
                {
                    objectsToRemove.Add(obj);
                }
            }
            Enemies = Enemies.Except(objectsToRemove).ToList();
        }
        public void addScore(int value)
        {
            Score += value;
        }
        // method for decreasing player's score 
        public void subLives(int value)
        {
            Lives -= value;
        }
        // Method for displaying score 
        public void newScore()
        {
            int returnScore = Score;
            Raylib.DrawText("Score: " + returnScore , 12, 12, 20, Color.WHITE);
        }
        public void newLives()
        {
            int returnLives = Lives;
            Raylib.DrawText("Lives: " + returnLives , 12, 750, 20, Color.WHITE);
        }

        public void CreateEnemies(int whichType, int setVelocity, Vector2 position, int Size, Random Rand)
        {
            switch (whichType) 
                {
                case 0:
                    var butter = new ButterflyE(Color.GREEN, Size);
                    butter.Position = findSpawnPosition();
                    butter.Velocity = new Vector2(1, 0);
                    butter.originalPosition = new Vector2(butter.Position.X, butter.Position.Y);
                    Enemies.Add(butter);
                    break;
                case 1:
                    var bee = new BeeE(Color.BLUE, Size);
                    bee.Position =  findSpawnPosition();
                    bee.Velocity = new Vector2(1, 0);
                    bee.originalPosition = new Vector2(bee.Position.X, bee.Position.Y);
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
                case 3:
                    int randomX = Rand.Next(0, Width - Size);

                    var pixel = new BPixels(GenerateColor(), Size);
                    pixel.Position = new Vector2(randomX, 0 - Size);
                    pixel.Velocity = new Vector2(0, 6);
                    Enemies.Add(pixel);
                    break;
                }
        }
        public Vector2 findSpawnPosition()
        {
            Vector2 firstPosition = new Vector2(64, 200);
            Vector2 spawnPosition = new Vector2(0, 0);

            float xPosition = firstPosition.X * (whichXPosition);
            float yPosition = firstPosition.Y;
            spawnPosition = new Vector2(xPosition, yPosition);

            whichXPosition += 1;
            Console.WriteLine(whichXPosition);
            return(spawnPosition);
        }
        private Color GenerateColor()
        {
            var Random = new Random();
            Color[] Colors = {Color.SKYBLUE, Color.BROWN, Color.BEIGE,Color.DARKPURPLE, Color.VIOLET, Color.PURPLE, Color.DARKBLUE, Color.BLUE, 
                        Color.BLACK, Color.DARKGREEN, Color.LIME, Color.GREEN, Color.MAROON, Color.RED, Color.PINK, Color.ORANGE, Color.GOLD, Color.YELLOW,
                        Color.DARKGRAY, Color.GRAY, Color.LIGHTGRAY, Color.BLANK, Color.MAGENTA, Color.RAYWHITE, Color.DARKBROWN, Color.WHITE}; 
            var randomColorNumber = Random.Next(0, Colors.Length);
            Color randomColor = Colors[randomColorNumber];
            return randomColor;
        }
    }
    
    abstract public class Movement
    {
        public Vector2 Position { get; set; } = new Vector2(0,0);
        public Vector2 Velocity { get; set; } = new Vector2(0,0);
        public Vector2 originalPosition { get; set; } = new Vector2(0,0);
        virtual public void Draw() 
        {
            // Base game object do not have anything to draw.
        }

        public void Move()
        { //Randomized falling movement and synchronized side to side.
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
        public Rectangle eachRectangle()
        {
            Rectangle ownedRectangle = new Rectangle((int)Position.X, (int)Position.Y, Size, Size);
            return ownedRectangle;
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
        public Rectangle eachRectangle()
        {
            Rectangle ownedRectangle = new Rectangle((int)Position.X, (int)Position.Y, Size, Size);
            return ownedRectangle;
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
    public class BPixels : ColoredObjects 
    {
        public int Size { get; set; }
        public BPixels(Color color, int size): base(color) 
        {
            Size = size;
        }
        override public void Draw() {
            Raylib.DrawRectangle((int)Position.X, (int)Position.Y, Size/7, Size/7, Color);
        }
    }
}