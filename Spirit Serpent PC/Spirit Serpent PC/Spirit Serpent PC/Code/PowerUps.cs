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



public class powerUp : gameSprite
{
    public int powerUpNumber;
    public powerUp(Texture2D theTexture, Vector2 thePosition, Vector2 theVelocity, float theRotation)
        : base(theTexture, thePosition, theVelocity, theRotation, 0)
    {

    }

    public override void update()
    {
        base.update();
    }
}

public class littleCoin : powerUp
{
    public littleCoin(int yPos, int speed)
        : base(texLib.txd["Littlecoin"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Littlecoin"].Width / 2, yPos), new Vector2(-1 * speed, 0), 0)
    {
        powerUpNumber = 1;
    }

    public override void update()
    {
        position += velocity;
        base.update();
    }
}

public class bigCoin : powerUp
{
    public bigCoin(int yPos, int speed)
        : base(texLib.txd["Bigcoin"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Littlecoin"].Width / 2, yPos), new Vector2(-1 * speed, 0), 0)
    {
        powerUpNumber = 2;
    }

    public override void update()
    {
        position += velocity;
        base.update();
    }
}

public class invinciCoin : powerUp
{
    public invinciCoin(int yPos, int speed)
        : base(texLib.txd["Invincicoin"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Littlecoin"].Width / 2, yPos), new Vector2(-1 * speed, 0), 0)
    {
        powerUpNumber = 3;
    }

    public override void update()
    {
        position += velocity;
        base.update();
    }
}

public class battleCoin : powerUp
{
    public battleCoin(int yPos, int speed)
        : base(texLib.txd["Battlecoin"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Littlecoin"].Width / 2, yPos), new Vector2(-1 * speed, 0), 0)
    {
        powerUpNumber = 4;
    }

    public override void update()
    {
        position += velocity;
        base.update();
    }
}