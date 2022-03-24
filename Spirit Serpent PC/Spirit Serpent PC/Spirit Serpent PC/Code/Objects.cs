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


public class backGroundSprite
{
    public Texture2D texture;
    public Vector2 position;
    public Vector2 velocity;
    public backGroundSprite partnerSprite;

    public backGroundSprite(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, backGroundSprite thePartner)
    {
        texture = theTexture;
        position = thePosition;
        velocity = theVelocity;
        partnerSprite = thePartner;
    }

    public void update()
    {
        position += velocity;

        if (position.X + texture.Width < 0)
        {
            position.X = partnerSprite.position.X + partnerSprite.texture.Width + partnerSprite.velocity.X;
        }
    }
}

public struct vitalStats
{
    public Vector2 position;
    public float rotation;

    public vitalStats(gameSprite theSprite)
    {
        position = theSprite.position;
        rotation = theSprite.rotation;
    }
}

public class gameSprite
{
    public Texture2D texture;
    public Vector2 position;
    public Vector2 velocity;
    public Vector2 origin;
    public Vector2 center;
    public float rotation;
    public int collideSize;
    public int pointsWorth;

    public bool isSolid;
    public bool isSquare;

    public bool willDelete;

    public gameSprite(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, float theRotation, int thePointsWorth)
    {
        texture = theTexture;
        position = thePosition;
        velocity = theVelocity;
        rotation = theRotation;
        pointsWorth = thePointsWorth;

        isSolid = true;
        isSquare = false;

        willDelete = false;
        origin = new Vector2(texture.Width / 2, texture.Height / 2);
        center = origin;
        collideSize = Math.Min(texture.Width, texture.Height) / 2;
    }

    public virtual void update()
    {
        center = position - new Vector2(texture.Width / 2, texture.Height / 2);

        if (position.X < -100 || position.X > screenDimensions.dimensions.X + 100 || position.Y < -100 || position.Y > screenDimensions.dimensions.Y + 100)
        {
            willDelete = true;
            score.yourScore += pointsWorth;
        }
    }
}


public class squareSprite : gameSprite
{
    public int collideLeft;
    public int collideRight;
    public int collideBottom;
    public int collideTop;

    public squareSprite(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, float theRotation, int thePointsWorth)
        : base(theTexture, thePosition, theVelocity, theRotation, thePointsWorth)
    {
        isSquare = true;

        collideLeft = (int)position.X;
        collideRight = (int)position.X + texture.Width;
        collideTop = (int)position.Y;
        collideBottom = (int)position.Y + texture.Height;

    }

    public override void update()
    {

        collideLeft = (int)position.X;
        collideRight = (int)position.X + texture.Width;
        collideTop = (int)position.Y;
        collideBottom = (int)position.Y + texture.Height;

        base.update();
    }




}

public class rainbowSprite : gameSprite
{
    public Color color;
    public rainbowSprite(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, float theRotation, int thePointsWorth)
        : base(theTexture, thePosition, theVelocity, theRotation, thePointsWorth)
    {
        color = Color.Lime;
    }

    public override void update()
    {
        base.update();
    }

    public void changeColor()
    {
        switch(screenDimensions.chaos.Next(18))
        {
            case 0:
                color = Color.Yellow;
                break;
            case 1:
                color = Color.Red;
                break;
            case 2:
                color = Color.Lime;
                break;
            case 3:
                color = Color.Blue;
                break;
            case 4:
                color = Color.Cyan;
                break;
            case 5:
                color = Color.Purple;
                break;
        }

    }

    public void setGreen()
    {
        color = Color.Lime;
    }

    public void setFire()
    {
        color = Color.Red;
    }

    public void setColor(Color theColor)
    {
        color = theColor;
    }


}

public class snakeHead : rainbowSprite
{
    public int collideSize2;
    public snakeHead(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, float theRotation) : base (theTexture, thePosition, theVelocity, theRotation, 0)
    {
        collideSize = 1;
        collideSize2 = Math.Min(texture.Width, texture.Height) / 2;
    }

    public override void update()
    {
        MouseState ms = Mouse.GetState();

        if (ms.X < screenDimensions.dimensions.X && ms.X > 0 && ms.Y < screenDimensions.dimensions.Y && ms.Y > 0 && ms.LeftButton == ButtonState.Pressed)
        {
            this.velocity.X = this.position.X - ms.X;
            this.velocity.Y = this.position.Y - ms.Y;

            this.position -= this.velocity * (float)0.2;

            //FUCKING SPINNING IS WHAT
            this.rotation = (float)Math.Atan2(ms.Y - this.position.Y, ms.X - this.position.X);
        }

        base.update();
    }
}


public class bullet : gameSprite
{
    public bullet(gameSprite theParent, int theVelocity, int spread)
        : base(texLib.txd["Bullet"], theParent.position, (theVelocity * -1 * Vector2.Normalize(theParent.velocity)) + new Vector2(screenDimensions.chaos.Next(spread) - spread / 2, screenDimensions.chaos.Next(spread) - spread/2), theParent.rotation, 0)
    { }

    public override void update()
    {
        this.position += this.velocity;

        base.update();
    }

}

public class cursor : gameSprite
{
    public cursor()
        : base(texLib.txd["Cursor"], new Vector2(0, 0), new Vector2(0, 0), 0, 0)
    {
    }

    public override void update()
    {
        MouseState ms = Mouse.GetState();

        this.position.X = ms.X;
        this.position.Y = ms.Y;

        base.update();
    }
}