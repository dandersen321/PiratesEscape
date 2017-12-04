using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Ship playerShip;
    private Canvas shipCreationCanvas;
    private ShipCreation shipCreation;

    public GameObject shipPrefab;
    public GameObject objective;

    private bool gameStarted;

    private List<Ship> enemyShips;

    private float timeUntilNextEnemyShip = 0f;
    private float enemyShipInterval = 7f;
    private float enemyShipMinDistance = 150f;
    private float enemyShipMaxDistance = 200f;
    private int shipsCreated;

    private float timeUntilNextRock = 0f;
    private float rockInterval = 6f;
    private float rockMinDistance = 150f;
    private float rockMaxDistance = 200f;
    private int rocksCreated;
    //private float enemyShipMinDistance = 0f;
    //private float enemyShipMaxDistance = 0f;

    public Text statsText;
    public Text messageText;
    //public GameObject messagePanel;
    public Canvas messageCanvas;
    public Button messageButton;

    public static float playerGold = 9999;
    public static float levelGold = 0;
    //private static float shipCost = 0;

    public Dictionary<int, Level> levels;
    public int currentLevel;

    private bool levelCompletedSuccessfully;

    public GameObject rock1;
    public GameObject rock2;
    public GameObject rock3;
    public GameObject rock4;
    public GameObject rock5;
    public GameObject rock6;
    public GameObject rock7;
    public GameObject rock8;
    public GameObject rock9;
    public GameObject rock10;

    // Use this for initialization
    void Start () {
        shipCreationCanvas = GameObject.Find("ShipCreationCanvas").GetComponent<Canvas>();
        shipCreation = shipCreationCanvas.GetComponent<ShipCreation>();

        messageButton.onClick.AddListener(() => messageButtonClicked());
        messageCanvas.enabled = false;

        gameStarted = false;

        GameObject playerShipObj = GameObject.FindGameObjectWithTag("Player");
        playerShip = playerShipObj.GetComponent<Ship>();

        enemyShips = new List<Ship>();

        //statsText = GameObject.Find("StatsText").GetComponent<Text>();
        //messageText = GameObject.Find("MessageText").GetComponent<Text>();
        levels = new Dictionary<int, Level>();
        levels[-1] = new Level(1, 0, 100, 6f, 1f);
        levels[1] = new Level(1, 2, 500, 5f, 1f);
        levels[2] = new Level(2, 5, 500, 4f, 1.2f);
        levels[3] = new Level(3, 7, 1000, 4f, 1.3f);
        levels[4] = new Level(4, 15, 1500, 3f, 1.5f);
        levels[5] = new Level(5, 20, 2000, 3f, 1.7f);
        currentLevel = 1;

        startLevel();
    }

    float randomFloat(float minValue, float maxValue, bool allowNegative=true)
    {
        float value = Random.Range(minValue, maxValue);
        //Debug.Log(Random.Range(0f, 2f) < 1f);
        if(allowNegative && Random.Range(0f, 2f) < 0.5f)
        {
            return -value;
        }

        return value;
    }

    //public float calculateCost(ShipTile[,] tiles)
    //{
    //    float tmpGold = playerGold;
    //    foreach (var cube in tiles)
    //    {
    //        if(tiles.)
    //        tmpGold -= Ship.pricePerComp[cube.comp];
    //    }
    //    return tmpGold;
    //}

    

    void spawnEnemyShip()
    {
        timeUntilNextEnemyShip = Time.time + enemyShipInterval;
        GameObject shipObject = GameObject.Instantiate(shipPrefab);
        Ship randomEnemyShip = EnemyShipFactory.createRandomShip(shipObject);
        //var ship = EnemyShipFactory.createFrigate(shipObject);
        //shipObject.transform.position = new Vector3(25, 0, 25);



        //shipObject.transform.position = new Vector3(playerShip.transform.position.x + Random.Range(enemyShipMinDistance, enemyShipMaxDistance), 0,
        //                                            playerShip.transform.position.z + Random.Range(enemyShipMinDistance, enemyShipMaxDistance));

        //Vector3 direction = new Vector3();
        //if (Random.Range(0f, 2f) < 1f)
        //{
        //    direction = new Vector3(Random.Range(-1f, -0.5f), 0, Random.Range(-1f, 1f));
        //}

        Vector3 direction = new Vector3(randomFloat(0.5f, 1f), 0, randomFloat(0.5f, 1f));

        //if (Random.Range(0f, 2f) < 1.5f)
        if(true)
        {
            if(currentLevel == 1)
                direction = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(0.75f, 1f));
            else
                direction = new Vector3(Random.Range(-0.35f, 0.35f), 0, Random.Range(0.75f, 1f));
        }
        //Vector3 direction = Vector3.forward;
        float distance = Random.Range(enemyShipMinDistance, enemyShipMaxDistance) + Mathf.Max(Mathf.Sqrt(randomEnemyShip.shipCubes.Count));
        Vector3 newPostion = playerShip.transform.position + direction * distance;


        //shipObject.transform.position = new Vector3(playerShip.transform.position.x + randomFloat(enemyShipMinDistance, enemyShipMaxDistance), 0,
        //                                            playerShip.transform.position.y + randomFloat(enemyShipMinDistance, enemyShipMaxDistance));
        shipObject.transform.position = newPostion;
        shipObject.transform.LookAt(playerShip.transform.position);
        //Debug.Log(playerShip.transform.position.ToString() + "->" +  shipObject.transform.position.ToString() + "->" + distance);

        float interval = enemyShipInterval;
        if (shipsCreated < 10)
            interval /= 2;
        timeUntilNextEnemyShip = Time.time + interval;

        shipsCreated += 1;
        //enemyShips.Add(null);
    }

    void spawnRock()
    {
        timeUntilNextRock = Time.time + rockInterval;

        int randInt = Random.Range(1, 11);
        GameObject rockPrefab;
        if(randInt ==1)
        {
            rockPrefab = rock1;
        }
        else if (randInt == 2)
        {
            rockPrefab = rock2;
        }
        else if (randInt == 3)
        {
            rockPrefab = rock3;
        }
        else if (randInt == 4)
        {
            rockPrefab = rock4;
        }
        else if (randInt == 5)
        {
            rockPrefab = rock5;
        }
        else if (randInt == 6)
        {
            rockPrefab = rock6;
        }
        else if (randInt == 7)
        {
            rockPrefab = rock7;
        }
        else if (randInt == 8)
        {
            rockPrefab = rock8;
        }
        else if (randInt == 9)
        {
            rockPrefab = rock9;
        }
        else
        {
            rockPrefab = rock10;
        }

        GameObject rockObject = GameObject.Instantiate(rockPrefab);
        rockObject.transform.localScale = new Vector3(Random.Range(0.3f, 0.7f), Random.Range(0.3f, 0.5f), Random.Range(0.3f, 0.7f));

        Vector3 direction = new Vector3(randomFloat(0.5f, 1f), 0, randomFloat(0.5f, 1f));

        if (Random.Range(0f, 2f) < 1.5f)
        {
            direction = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(0.75f, 1f));
        }
        //Vector3 direction = Vector3.forward;
        float distance = Random.Range(rockMinDistance, rockMaxDistance);
        Vector3 newPostion = playerShip.transform.position + direction * distance;
        while (Vector3.Distance(objective.transform.position, newPostion) < 100)
        {
            distance += 25f;
            newPostion = playerShip.transform.position + direction * distance;
        }


        //shipObject.transform.position = new Vector3(playerShip.transform.position.x + randomFloat(rockMinDistance, rockMaxDistance), 0,
        //                                            playerShip.transform.position.y + randomFloat(rockMinDistance, rockMaxDistance));
        rockObject.transform.position = newPostion;
        
        Debug.Log(playerShip.transform.position.ToString() + "->" + rockObject.transform.position.ToString() + "->" + distance);

        float interval = rockInterval;
        if (rocksCreated < 5)
            interval /= 2;
        timeUntilNextRock = Time.time + interval;

    }
	
	// Update is called once per frame
	void Update ()
    {
        if(playerShip.alive)
        {
            if (Time.time > timeUntilNextEnemyShip)
            {
                spawnEnemyShip();
            }

            if (Time.time > timeUntilNextRock)
            {
                spawnRock();
            }

            updatePlayerStats();
        }

        //if(enemyShips.Count > 0)
        //{
        //    Debug.Log(enemyShips[0].alive);
        //}
		
	}

    public void startLevel()
    {
        //if(this.levelCompletedSuccessfully)
        //{
        //    playerGold;
        //}
        //if (currentLevel == 1)
        //    playerGold = 5000;
        playerShip.resetPlayerCubes();
        Level curLevel = levels[currentLevel];
        objective.transform.position = new Vector3(0, 0, curLevel.distanceToTarget);
        Debug.Log(objective.transform.position);
        GameObject.Find("DescriptionText").GetComponent<Text>().text = string.Format("<b>Mission {0}:</b> Escape with least {1} cargo. You will get 200g per cargo and 5g per ship part destroyed.  Ultimate goal is to make the most money possible.\n", currentLevel, curLevel.numberOfCargoNeeded) +
                                                                        string.Format("<b>Cost:</b> Guns cost {0}g & {1}weight, cargo {2}g & {3}weight, sail {4}g & {5}weight, hull {6}g & {7}weight.\n", 
                                                                        Ship.pricePerComp[ShipComponent.northCannon], Ship.weightPerComp[ShipComponent.northCannon],
                                                                        Ship.pricePerComp[ShipComponent.cargo], Ship.weightPerComp[ShipComponent.cargo],
                                                                        Ship.pricePerComp[ShipComponent.sail], Ship.weightPerComp[ShipComponent.sail],
                                                                        Ship.pricePerComp[ShipComponent.hull], Ship.weightPerComp[ShipComponent.hull]) +
                                                                        "<b>Controls:</b> Use WASD to move and Arrow Keys to fire.\n" +
                                                                        "<b>Detail:</b> You are a pirate trying to escape the royal navy after a day of plundering.  Reach your secret cove before you are overwhelmed and lost in the misty waters.  Shooting enemy ships will slow them down.  Design your own ship but be careful; the more weight you have the worse your speed. Use guns to defend yourself, sails to move faster (reduce weight), hull to create cheap defense, and cargo to make money. Note: technically your ship components don't need to be attached to each other (they bind together through the power of friendship).";
        //GameObject.Find("DescriptionText").GetComponent<Text>().resizeTextForBestFit = true;
        shipCreationCanvas.enabled = true;
        shipCreation.beginMenu();
        objective.GetComponent<Renderer>().enabled = false;
        levelCompletedSuccessfully = false;
        
        //startLevel(currentLevel);
    }

    public void beginLevelPlay(ShipTile[,] shipTiles)
    {
        enemyShipInterval = levels[currentLevel].enemyInterval;
        foreach(var ship in GameObject.FindGameObjectsWithTag("Ship"))
        {
            if (ship == playerShip)
                continue;
            Destroy(ship);
        }
        foreach(var rock in GameObject.FindGameObjectsWithTag("Rock"))
        {
            Destroy(rock);
            //Debug.Log("destroyed rock");
        }

        int cargoCount = 0;
        foreach(var tile in shipTiles)
        {
            if (tile.comp == ShipComponent.cargo)
                cargoCount += 1;
        }
        if(cargoCount < levels[currentLevel].numberOfCargoNeeded)
        {
            showDialog(string.Format("You need to add at least {0} more cargo for this level.", levels[currentLevel].numberOfCargoNeeded - cargoCount), "Ok");
            return;
        }
        //float shipCost =0;
        //for (int r = 0; r < shipTiles.GetLength(0); ++r)
        //{
        //    for(int c =0; c <shipTiles.GetLength(1); ++c)
        //        if(Ship.pricePerComp.ContainsKey(shipTiles[r, c].comp))
        //            shipCost += Ship.pricePerComp[shipTiles[r,c].comp];
        //}

        if (playerGold < 0)
        {
            showDialog(string.Format("You need {0} more gold before you can start.", Mathf.Abs(playerGold)), "Ok");
            return;
        }
        createPlayerShip(shipTiles);
        
        //playerGold -= shipCost;
        shipCreationCanvas.enabled = false;
        objective.GetComponent<Renderer>().enabled = true;
        gameStarted = true;
        shipsCreated = 0; 
        rocksCreated = 0;
    }

    public void updatePlayerStats()
    {
        //int cargoChests = 0;
        //foreach(var cube in playerShip.shipCubes)
        //{
        //    if (cube.comp == ShipComponent.cargo)
        //        cargoChests += 1;
        //}
        float gold = playerGold;
        //if (tiles != null)
        //    gold = calculateCost(shipTiles);
        float cargoNeeded = 5;
        if (levels != null && levels.ContainsKey(currentLevel))
            cargoNeeded = levels[currentLevel].numberOfCargoNeeded;
        float distance = Vector3.Distance(playerShip.transform.position, objective.transform.position);
        statsText.text = string.Format("Cargo: {0}/{1}\nWeight: {2}/100\nSpeed: {3}/10\nDistance: {4}\nGold: {5}\nLevel: {6}/5", 
                                       playerShip.countCargo(), cargoNeeded, playerShip.weight, (playerShip.speedModifier * 10).ToString("0.00"), distance.ToString("0"), gold+levelGold, currentLevel);
    }

    public void win()
    {
        messageText.text = "You win!";
        currentLevel += 1;
        playerShip.alive = false;
        int cargoChests = 0;
        foreach (var cube in playerShip.shipCubes)
        {
            if (cube.comp == ShipComponent.cargo)
                cargoChests += 1;
        }
        float goldEarned = cargoChests * 200 + levelGold;
        playerGold += goldEarned;
        
        float shipCost = 0;
        foreach (var cube in playerShip.shipCubes)
        {
            shipCost += Ship.pricePerComp[cube.comp];
        }
        if (!levels.ContainsKey(currentLevel))
        {
            showDialog("You beat the game! Congrats! Your score is " + (playerGold + shipCost).ToString() + ".", "");
            currentLevel = 1;
        }
        else
        {   
            levelGold = 0;
            showDialog("You completed the mission!  You earned " + goldEarned.ToString() + "g!" , "Next Mission");
            
        }

        updatePlayerStats();



    }

    public void die()
    {
        //messageText.enabled = true;
        //messageText.text = "You lose!";
        playerShip.alive = false;

        levelGold = 0;

        float shipCost = 0;
        foreach (var cube in playerShip.shipCubes)
        {
            shipCost += Ship.pricePerComp[cube.comp];
        }
        updatePlayerStats();

        showDialog("You lost too much cargo! Your score is " + (shipCost + playerGold).ToString() + ".", "Restart Level");
    }

    // yes this is terrible code but oh well
    private void messageButtonClicked()
    {  
        startLevel();
        messageCanvas.enabled = false;
        shipCreationCanvas.enabled = true;
    }

    public void createPlayerShip(ShipTile[,] shipTiles)
    {
        //ship = Ship.createShip(shipTiles);
        //ship = new Ship(shipTiles);
        //GameObject shipObject = GameObject.Instantiate(shipPrefab);
        //GameObject playerShipObj = GameObject.FindGameObjectWithTag("Player");
        //playerShip.resetPlayerCubes();
        playerShip.gameObject.transform.position = new Vector3(0, 0, 0);
        playerShip.gameObject.transform.LookAt(objective.transform);
        playerShip.init(true, shipTiles);
        //UnitController unit = newUnitObj.GetComponent<UnitController>();
        //ship.Init(shipTiles);
    }

    //public void startLevel(int levelNum)
    //{
    //    Level level = levels[levelNum];
    //    //GameObject shipObject = GameObject.Instantiate(shipPrefab);
        
    //}

    public void showDialog(string message, string messageButtonText)
    {
        shipCreationCanvas.enabled = false;
        messageCanvas.enabled = true;
        messageText.text = message;
        messageButton.GetComponentInChildren<Text>().text = messageButtonText;
        if(messageButtonText == "")
        {
            messageButton.enabled = false;
        }
        else
        {
            messageButton.enabled = true;
        }
    }


    public class Level
    {
        public int levelNum;
        public int numberOfCargoNeeded;
        public int distanceToTarget;
        public float enemyInterval;
        public float enemySpeedIncrease;

        public Level(int levelNum, int numberOfCargoNeeded, int distanceToTarget, float enemyInterval, float speedIncrease)
        {
            this.levelNum = levelNum;
            this.numberOfCargoNeeded = numberOfCargoNeeded;
            this.distanceToTarget = distanceToTarget;
            this.enemyInterval = enemyInterval;
            this.enemySpeedIncrease = speedIncrease;
        }
    }
}
