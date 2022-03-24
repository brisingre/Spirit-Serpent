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


public class deathEnemy : gameSprite
{
    gameSprite target;
    int shotSpeed;
    int fishSpeed;
    int shotCounter;
    int fishCounter;
    public int shotVelocity;
    int moveSpeed;

    public deathEnemy(gameSprite theTarget, int theMoveSpeed, int fishRate, int ballRate, int theShotVelocity)
        : base(texLib.txd["Death"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Death"].Width / 2, theTarget.position.Y), new Vector2(theMoveSpeed * -1, 0), 0, 10000)
    {
        target = theTarget;
        moveSpeed = theMoveSpeed;
        shotCounter = 0;
        fishCounter = 0;
        shotSpeed = ballRate;
        fishSpeed = fishRate;
        shotVelocity = theShotVelocity;

    }

    public override void update()
    {
        this.velocity = new Vector2(this.position.X - target.position.X, this.position.Y - target.position.Y);
        this.velocity = Vector2.Normalize(this.velocity);
        this.velocity *= moveSpeed;
        this.velocity.X = moveSpeed;

        this.position -= this.velocity;

        shotCounter++;

        if (shotCounter >= shotSpeed)
        {
            shotCounter = 0;
            objectManager.enemies.Add(new murderBall(this, new Vector2(this.velocity.X + 1, this.velocity.Y + 1)));
            objectManager.enemies.Add(new murderBall(this, new Vector2(this.velocity.X - 1, this.velocity.Y + 1)));
            objectManager.enemies.Add(new murderBall(this, new Vector2(this.velocity.X + 1, this.velocity.Y - 1)));
            objectManager.enemies.Add(new murderBall(this, new Vector2(this.velocity.X - 1, this.velocity.Y - 1)));
            objectManager.enemies.Add(new murderBall(this, new Vector2(this.velocity.X, this.velocity.Y)));
        }

        fishCounter++;

        if (fishCounter >= fishSpeed)
        {
            fishCounter = 0;
            objectManager.enemies.Add(new hateFish(this, new Vector2(this.velocity.X * -1, 1)));
            objectManager.enemies.Add(new hateFish(this, new Vector2(this.velocity.X * -1, -1)));
        }

        base.update();
    }
}

public class murderBall : gameSprite
{
    public murderBall(deathEnemy theDeath, Vector2 theVelocity)
        : base(texLib.txd["Murderball"], theDeath.position, theVelocity * theDeath.shotVelocity, 0, 0)
    { }

    public override void update()
    {
        this.position += this.velocity;

        base.update();
    }

}

public class hateFish : gameSprite
{
    public hateFish(deathEnemy theDeath, Vector2 theVelocity)
        : base(texLib.txd["Hatefish"], theDeath.position, theVelocity * theDeath.shotVelocity, 0, 0)
    { }

    public override void update()
    {
        this.position += this.velocity;

        base.update();
    }

}

public class rainMan : gameSprite
{
    int shootSpeed;
    int shootCounter;
    public int fallSpeed;
    public rainMan(int shootingRate, int moveSpeed, int fallRate)
        : base(texLib.txd["Cloud"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Cloud"].Width / 2, texLib.txd["Cloud"].Height / 2), new Vector2(moveSpeed * -1, 0), 0, 500)
    {
        shootSpeed = shootingRate;
        shootCounter = 0;
        fallSpeed = fallRate;
    }

    public override void update()
    {
        position += velocity;

        shootCounter++;
        if (shootCounter >= shootSpeed)
        {
            shootCounter = 0;

            objectManager.enemies.Add(new rainChild(this));
        }



        base.update();
    }
}

public class rainChild : gameSprite
{

    public rainChild(rainMan theMan)
        : base(texLib.txd["Raindrop"], theMan.position, new Vector2(0, theMan.fallSpeed), 0, 0)
    {

    }

    public override void update()
    {
        this.position += this.velocity;

        base.update();
    }




}

public class superBird : gameSprite
{
    gameSprite target;
    int moveSpeed;
    public superBird(gameSprite theTarget, int theMoveSpeed)
        : base(texLib.txd["Bird"], new Vector2(screenDimensions.dimensions.X + texLib.txd["Bird"].Width / 2, theTarget.position.Y), new Vector2(theMoveSpeed * -1, 0), 0, 30)
    {
        moveSpeed = theMoveSpeed;
        target = theTarget;
    }

    public override void update()
    {
        this.velocity = new Vector2(this.position.X - target.position.X, this.position.Y - target.position.Y);
        this.velocity = Vector2.Normalize(this.velocity);
        this.velocity *= moveSpeed;
        this.velocity.X = moveSpeed;

        this.position -= this.velocity;

        base.update();
    }
}

public class blockStream : squareSprite
{
    public blockStream(Vector2 thePosition)
        : base(texLib.txd["Test"], thePosition, new Vector2(-32, 0), 0, 10)
    {

    }

    public override void update()
    {
        this.position += this.velocity;
    }

}