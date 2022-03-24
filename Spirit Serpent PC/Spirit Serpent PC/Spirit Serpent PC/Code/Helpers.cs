using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

public static class screenDimensions
{
    public static Vector2 dimensions;
    public static Random chaos = new Random();
}

public static class texLib
{
    public static Dictionary<string, Texture2D> txd = new Dictionary<string, Texture2D>();
    public static Dictionary<string, SoundEffect> snd = new Dictionary<string,SoundEffect>();
    public static Dictionary<string, SpriteFont> fnd = new Dictionary<string,SpriteFont>();

    public static void addTextures(ContentManager Content)
    {
        txd.Add("Head", Content.Load<Texture2D>("player_head"));
        txd.Add("Middle", Content.Load<Texture2D>("player_tail_middle"));
        txd.Add("Tail", Content.Load<Texture2D>("player_tail_end"));
        txd.Add("Cloud", Content.Load<Texture2D>("enemy_raincloud"));
        txd.Add("Raindrop", Content.Load<Texture2D>("enemy_raindrop"));
        txd.Add("Death", Content.Load<Texture2D>("Death"));
        txd.Add("Murderball", Content.Load<Texture2D>("bullet_blood"));
        txd.Add("Hatefish", Content.Load<Texture2D>("bullet_fish"));
        txd.Add("Bird", Content.Load<Texture2D>("wind"));
        txd.Add("Littlecoin", Content.Load<Texture2D>("coin"));
        txd.Add("Bigcoin", Content.Load<Texture2D>("super_coin"));
        txd.Add("Invincicoin", Content.Load<Texture2D>("coin_invincible"));
        txd.Add("Battlecoin", Content.Load<Texture2D>("coin_bullets"));
        txd.Add("Background", Content.Load<Texture2D>("Background"));
        txd.Add("Bullet", Content.Load<Texture2D>("bullet_fire"));
        txd.Add("Cursor", Content.Load<Texture2D>("cursor"));
        txd.Add("Test", Content.Load<Texture2D>("PhoneGameThumb"));

        snd.Add("Mainmusic", Content.Load<SoundEffect>("Output12"));

        fnd.Add("Segoui", Content.Load<SpriteFont>("SegoeUI"));
    }

}

public static class objectManager
{
    public static List<gameSprite> enemies = new List<gameSprite>();
    public static List<powerUp> pickups = new List<powerUp>();
    public static List<gameSprite> bullets = new List<gameSprite>();

    public static void clear()
    {
        enemies.Clear();
        pickups.Clear();
    }
}

public static class mainSnake
{
    public static snakeHead character;
    public static List<gameSprite> tailSegments = new List<gameSprite>();
    public static List<vitalStats> tailArray = new List<vitalStats>();

    public static void init()
    {
        character = new snakeHead(texLib.txd["Head"], new Vector2(screenDimensions.dimensions.X / 2, screenDimensions.dimensions.Y / 2), new Vector2(0, 0), 0);

        for (int i = 0; i < 4; i++)
        {
            tailSegments.Add(new rainbowSprite(texLib.txd["Middle"], character.position * (float)i, new Vector2(0, 0), 0, 0));
        }

        tailSegments.Add(new rainbowSprite(texLib.txd["Tail"], character.position * 5, new Vector2(0, 0), 0, 0));

    }

    public static void updateSnake()
    {
        character.update();

        updateTailArray(120, new vitalStats(character));

        updateTailSegments(3);
    }

    public static void updateTailArray(int cullNumber, vitalStats newNumber)
    {
        tailArray.Add(newNumber);

        if (tailArray.Count > cullNumber)
        {
            tailArray.RemoveAt(0);
        }
    }

    public static void updateTailSegments(int frameLag)
    {
        for (int i = 0; i < tailSegments.Count; i++)
        {
            if (frameLag * (i + 1) < tailArray.Count)
            {
                tailSegments[i].position.X = tailArray[tailArray.Count - (frameLag * (i + 1)) - 1].position.X;
                tailSegments[i].position.Y = tailArray[tailArray.Count - (frameLag * (i + 1)) - 1].position.Y;
                tailSegments[i].rotation = tailArray[tailArray.Count - (frameLag * (i + 1)) - 1].rotation;
            }
        }
    }


        
}

public static class score
{
    public static int highScore = 0;
    public static int yourScore = 0;
    public static int deaths = 0;
    public static SpriteFont scoreFont = texLib.fnd["Segoui"];
    public static Vector2 scorePos = new Vector2(0, 0);
    public static string scoreString = "Score goes here." ;

    public static void update()
    {
        if (yourScore > highScore)
        {
            highScore = yourScore;
        }
        scoreString = yourScore + " / " + highScore + " (" + deaths + " deaths.)";
    }
}