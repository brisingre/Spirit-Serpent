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

public class Level
{

    public int procRate;
    public int procCounter;

    public backGroundSprite backGround1;
    public backGroundSprite backGround2;


    public Level()
    {
        procRate = 30;
        procCounter = 0;
       
        backGround1 = new backGroundSprite(texLib.txd["Background"], new Vector2(0, 0), new Vector2(-8, 0), backGround2);
        backGround2 = new backGroundSprite(texLib.txd["Background"], new Vector2(backGround1.texture.Width, 0), new Vector2(-8, 0), backGround1);
        backGround1.partnerSprite = backGround2;
    }


    public virtual void update()
    {
        backGround1.update();
        backGround2.update();

        procCounter++;
        if (procCounter == procRate)
        {
            procCounter = 0;
            enemyProc();
            pickupProc();
        }
    }

    public virtual void enemyProc()
    {
        int[] enemyPercents = new int[4]{25, 60, 13, 2};

        int procNumber = screenDimensions.chaos.Next(100);

        int lowerTarget = 0;
        int upperTarget = 0;
        for (int i = 0; i < enemyPercents.Length; i++)
        {

            lowerTarget = upperTarget;
            upperTarget += enemyPercents[i];

            if (lowerTarget <= procNumber && procNumber < upperTarget)
            {
                switch (i)
                {
                    case 0:

                        break;
                    case 1:
                        objectManager.enemies.Add(new superBird(mainSnake.character, 15));
                        break;
                    case 2:
                        objectManager.enemies.Add(new rainMan(25, 3, 3));
                        break;
                    case 3:
                        objectManager.enemies.Add(new deathEnemy(mainSnake.character, 3, 15, 10, 3));
                        break;
                    default:

                        break;
                }
            }

        }
 
    }

    public virtual void pickupProc()
    {
        int[] pickupPercents = new int[5] { 25, 60, 10, 3, 2 };

        int procNumber = screenDimensions.chaos.Next(100);

        int lowerTarget = 0;
        int upperTarget = 0;
        for (int i = 0; i < pickupPercents.Length; i++)
        {

            lowerTarget = upperTarget;
            upperTarget += pickupPercents[i];

            if (lowerTarget <= procNumber && procNumber < upperTarget)
            {
                switch (i)
                {
                    case 0:

                        break;
                    case 1:
                        objectManager.pickups.Add(new littleCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 2:
                        objectManager.pickups.Add(new bigCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 3:
                        objectManager.pickups.Add(new invinciCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 4:
                        objectManager.pickups.Add(new battleCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    default:

                        break;
                }
            }

        }
    }


}


public class BlocksLevel:Level
{

    public BlocksLevel()
    {
        procRate = 30;
        procCounter = 0;

        backGround1 = new backGroundSprite(texLib.txd["Background"], new Vector2(0, 0), new Vector2(-8, 0), backGround2);
        backGround2 = new backGroundSprite(texLib.txd["Background"], new Vector2(backGround1.texture.Width, 0), new Vector2(-8, 0), backGround1);
        backGround1.partnerSprite = backGround2;
    }

    

    public override void update()
    {
        backGround1.update();
        backGround2.update();

        procCounter++;
        if (procCounter == procRate)
        {
            procCounter = 0;
            enemyProc();
            pickupProc();
        }
    }

    public override void enemyProc()
    {

        int yTarget = screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y / 62) * 62;

        for(int i = 0; i < 40; i++)
        {
            objectManager.enemies.Add(new blockStream(new Vector2(screenDimensions.dimensions.X + ((i + 2) * 62), yTarget)));
        }

    }

    public override void pickupProc()
    {
        int[] pickupPercents = new int[5] { 25, 60, 10, 3, 2 };

        int procNumber = screenDimensions.chaos.Next(100);

        int lowerTarget = 0;
        int upperTarget = 0;
        for (int i = 0; i < pickupPercents.Length; i++)
        {

            lowerTarget = upperTarget;
            upperTarget += pickupPercents[i];

            if (lowerTarget <= procNumber && procNumber < upperTarget)
            {
                switch (i)
                {
                    case 0:

                        break;
                    case 1:
                        objectManager.pickups.Add(new littleCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 2:
                        objectManager.pickups.Add(new bigCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 3:
                        objectManager.pickups.Add(new invinciCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    case 4:
                        objectManager.pickups.Add(new battleCoin(screenDimensions.chaos.Next((int)screenDimensions.dimensions.Y), 15));
                        break;
                    default:

                        break;
                }
            }

        }
    }


}

