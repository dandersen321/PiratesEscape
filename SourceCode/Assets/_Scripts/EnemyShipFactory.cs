using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipFactory
{

    public static Ship createFrigate(GameObject shipObj)
    {
        ShipTile[,] basicDesign = ShipTile.generateEmpty(6, 6);

        basicDesign[0, 1].comp = ShipComponent.westCannon;
        basicDesign[0, 2].comp = ShipComponent.sail;
        basicDesign[0, 3].comp = ShipComponent.sail;
        basicDesign[0, 4].comp = ShipComponent.eastCannon;

        basicDesign[1, 0].comp = ShipComponent.westCannon;
        basicDesign[1, 1].comp = ShipComponent.westCannon;
        basicDesign[1, 2].comp = ShipComponent.hull;
        basicDesign[1, 3].comp = ShipComponent.hull;
        basicDesign[1, 4].comp = ShipComponent.eastCannon;
        basicDesign[1, 5].comp = ShipComponent.eastCannon;

        basicDesign[2, 0].comp = ShipComponent.westCannon;
        basicDesign[2, 1].comp = ShipComponent.westCannon;
        basicDesign[2, 2].comp = ShipComponent.hull;
        basicDesign[2, 3].comp = ShipComponent.hull;
        basicDesign[2, 4].comp = ShipComponent.eastCannon;
        basicDesign[2, 5].comp = ShipComponent.eastCannon;

        basicDesign[3, 0].comp = ShipComponent.westCannon;
        basicDesign[3, 1].comp = ShipComponent.westCannon;
        basicDesign[3, 2].comp = ShipComponent.hull;
        basicDesign[3, 3].comp = ShipComponent.hull;
        basicDesign[3, 4].comp = ShipComponent.eastCannon;
        basicDesign[3, 5].comp = ShipComponent.eastCannon;

        basicDesign[4, 2].comp = ShipComponent.sail;
        basicDesign[4, 3].comp = ShipComponent.sail;

        basicDesign[5, 2].comp = ShipComponent.northCannon;
        basicDesign[5, 3].comp = ShipComponent.northCannon;


        return createShip(basicDesign, shipObj);
    }

    public static Ship createFighter(GameObject shipObj)
    {
        ShipTile[,] basicDesign = ShipTile.generateEmpty(3,3);

        basicDesign[0, 0].comp = ShipComponent.hull;
        basicDesign[1, 0].comp = ShipComponent.sail;
        basicDesign[2, 0].comp = ShipComponent.hull;

        basicDesign[0, 1].comp = ShipComponent.hull;
        basicDesign[1, 1].comp = ShipComponent.sail;
        basicDesign[2, 1].comp = ShipComponent.northCannon;

        basicDesign[0, 2].comp = ShipComponent.hull;
        basicDesign[1, 2].comp = ShipComponent.sail;
        basicDesign[2, 2].comp = ShipComponent.hull;

        return createShip(basicDesign, shipObj);
    }

    public static Ship createCruiser(GameObject shipObj)
    {
        ShipTile[,] basicDesign = ShipTile.generateEmpty(20, 40);

        basicDesign[7, 19].comp = ShipComponent.southCannon;
        basicDesign[7, 20].comp = ShipComponent.southCannon;
        basicDesign[7, 21].comp = ShipComponent.southCannon;

        basicDesign[8, 18].comp = ShipComponent.westCannon;
        basicDesign[8, 19].comp = ShipComponent.sail;
        basicDesign[8, 20].comp = ShipComponent.sail;
        basicDesign[8, 21].comp = ShipComponent.sail;
        basicDesign[8, 22].comp = ShipComponent.eastCannon;

        basicDesign[9, 18].comp = ShipComponent.westCannon;
        basicDesign[9, 19].comp = ShipComponent.hull;
        basicDesign[9, 20].comp = ShipComponent.hull;
        basicDesign[9, 21].comp = ShipComponent.hull;
        basicDesign[9, 22].comp = ShipComponent.eastCannon;

        basicDesign[10, 18].comp = ShipComponent.westCannon;
        basicDesign[10, 19].comp = ShipComponent.cargo;
        basicDesign[10, 20].comp = ShipComponent.cargo;
        basicDesign[10, 21].comp = ShipComponent.cargo;
        basicDesign[10, 22].comp = ShipComponent.eastCannon;

        basicDesign[11, 18].comp = ShipComponent.westCannon;
        basicDesign[11, 19].comp = ShipComponent.cargo;
        basicDesign[11, 20].comp = ShipComponent.cargo;
        basicDesign[11, 21].comp = ShipComponent.cargo;
        basicDesign[11, 22].comp = ShipComponent.eastCannon;

        basicDesign[12, 18].comp = ShipComponent.westCannon;
        basicDesign[12, 19].comp = ShipComponent.hull;
        basicDesign[12, 20].comp = ShipComponent.hull;
        basicDesign[12, 21].comp = ShipComponent.hull;
        basicDesign[12, 22].comp = ShipComponent.eastCannon;

        basicDesign[13, 19].comp = ShipComponent.northCannon;
        basicDesign[13, 20].comp = ShipComponent.northCannon;
        basicDesign[13, 21].comp = ShipComponent.northCannon;

        basicDesign[14, 20].comp = ShipComponent.northCannon;

        return createShip(basicDesign, shipObj);
    }

    public static Ship createAnySizeShip(GameObject shipObj)
    {
        ShipTile[,] basicDesign = ShipTile.generateEmpty(Random.Range(5, 25),  Random.Range(5, 15));
        //ShipTile[,] basicDesign = ShipTile.generateEmpty(50, 50);
        for (int r = 0; r < basicDesign.GetLength(0); ++r)
        {
            for (int c = 0; c < basicDesign.GetLength(1); ++c)
            {
                int randNum = Random.Range(0, 10);
                ShipComponent randComp;
                if(randNum == 0)
                {
                    randComp = ShipComponent.northCannon;
                }
                else if(randNum==1)
                {
                    randComp = ShipComponent.eastCannon;
                }
                else if (randNum == 2)
                {
                    randComp = ShipComponent.southCannon;
                }
                else if (randNum == 3)
                {
                    randComp = ShipComponent.westCannon;
                }
                else if (randNum == 4)
                {
                    randComp = ShipComponent.cargo;
                }
                else if (randNum == 5)
                {
                    randComp = ShipComponent.sail;
                }
                else
                {
                    randComp = ShipComponent.hull;
                }
                basicDesign[r, c].comp = randComp;

            }
        }
        return createShip(basicDesign, shipObj);
    }

    public static Ship createShip(ShipTile[,] tiles, GameObject shipObj)
    {
        Ship ship = shipObj.GetComponent<Ship>();
        ship.init(false, tiles);
        return ship;
    }

    public static Ship createRandomShip(GameObject shipObj)
    {
        //float range = UnityEngine.Random.Range(0, 5);
        float range = 5;
        if (range >= 3)
        {
            return createAnySizeShip(shipObj);
        }
        if(range >= 2)
        {
            return createCruiser(shipObj);
        }
        else if(range >= 1)
        {
            return createFrigate(shipObj);
        }
        else
        {
            return createFighter(shipObj);
        }
    }
	
}
