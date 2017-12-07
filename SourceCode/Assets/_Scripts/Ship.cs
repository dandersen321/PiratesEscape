using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCube
{
    public GameObject obj;
    public ShipComponent comp;
    public int row;
    public int col;

    public ShipCube(GameObject obj, ShipComponent comp, int row, int col)
    {
        this.obj = obj;
        this.comp = comp;
        this.row = row;
        this.col = col;
    }
}

// this should be an abstract class but oh well
public class Ship : MonoBehaviour {

    //private float cubeWidth = 10f;
    private float cubeLength = 1f;
    private float cubeHeight = 1f;
    private float baseHeight = 3f;

    public GameObject northCannonPrefab;
    public GameObject eastCannonPrefab;
    public GameObject southCannonPrefab;
    public GameObject westCannonPrefab;
    public GameObject cargoPrefab;
    public GameObject hullPrefab;
    public GameObject sailPrefab;
    public GameObject cannonBallPrefab;
    public GameObject explodeCubePrefab;
    public GameObject explodeShipPrefab;
    public GameObject crashShipPrefab;

    public AudioClip fireSound;
    public AudioClip hitSound;

    public List<ShipCube> shipCubes;

    public static Dictionary<ShipComponent, float> pricePerComp = new Dictionary<ShipComponent, float>()
    {
        {ShipComponent.northCannon, 200f },
        {ShipComponent.eastCannon,  200f },
        {ShipComponent.southCannon, 200f },
        {ShipComponent.westCannon,  200f },
        {ShipComponent.cargo,       100f },
        {ShipComponent.sail,        50f },
        {ShipComponent.hull,        10f }
    };

    public static Dictionary<ShipComponent, float> weightPerComp = new Dictionary<ShipComponent, float>()
    {
        {ShipComponent.northCannon, 5 },
        {ShipComponent.eastCannon,  5 },
        {ShipComponent.southCannon, 5 },
        {ShipComponent.westCannon,  5 },
        {ShipComponent.cargo,       10 },
        {ShipComponent.sail,        -5 },
        {ShipComponent.hull,        1 }
    };


    private float cannonCooldown = 0.5f;
    //private float cannonCooldown = 0f;

    public bool alive = false;
    public bool isPlayer;
    public float startinCubeCount;
    public float weight;
    public float baseSpeed = 5000000f;
    private float baseRotateSpeed = 1f;
    public float movementSpeed;
    public float speedModifier;
    private float rotateSpeed;
    private int healthPerCube;
    private int damagePerHit =1;
    public bool finishedSpawing = false;
    private ShipTile[,] shipTiles = null;

    private static float maxWeight = 100f;

    private GameController gameController;

    private GameObject objectiveArrow;
    private GameObject objective;

    private Dictionary<ShipComponent, float> nextFireTime;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        objectiveArrow = GameObject.FindGameObjectWithTag("ObjectiveArrow");
        objective = GameObject.FindGameObjectWithTag("Objective");
    }
	
	// Update is called once per frame
	void Update ()
    {
        //if(!finishedSpawing)
        //    StartCoroutine(initEnemy(shipTiles));

        if (!gameController.playerShip.alive || !alive)
            return;

        move();	
	}

    private IEnumerator initEnemy(ShipTile[,] shipTiles)
    //private void initEnemy(ShipTile[,] shipTiles)
    {
        while(shipTiles == null)
            yield return new WaitForSeconds(0.1f);
        this.isPlayer = false;
        this.shipTiles = shipTiles;

        this.transform.position = new Vector3(0, -250, -250);

        Debug.Log("Starting" + shipTiles.ToString());
        for (int r = 0; r < shipTiles.GetLength(0); ++r)
        {
            for (int c = 0; c < shipTiles.GetLength(1); ++c)
            {
                //Debug.Log("Starting2");
                if (shipTiles[r, c].comp != ShipComponent.empty)
                {
                    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //cube.transform.position = new Vector3(cubeLength*r - ShipCreation.shipMaxHeight/2*cubeLength, 0, cubeLength*c-ShipCreation.shipMaxWidth/2*cubeLength);
                    //cube.transform.localScale = new Vector3(cubeLength, cubeHeight, cubeLength);
                    //cube.GetComponent<Renderer>().material.color = Color.gray;
                    bool alreadyMade = false;
                    foreach(var cube in shipCubes)
                    {
                        if(cube.row == r && cube.col == c)
                        {
                            alreadyMade = true;
                        }
                    }
                    //Debug.Log("Starting3 " + alreadyMade.ToString());
                    if (alreadyMade)
                    {
                        continue;
                    }
                    GameObject tile = null;
                    //Debug.Log("creating cube! " + r.ToString() + "-" + c.ToString());
                    tile = createTile(shipTiles[r, c].comp, r, c, shipTiles.GetLength(0), shipTiles.GetLength(1));
                    
                    
                    shipCubes.Add(new ShipCube(tile, shipTiles[r, c].comp, r, c));

                    if(c%10==0)
                    {
                        yield return new WaitForSeconds(0.01f);
                        //yield return new WaitForSeconds(20f);
                    }

                    //if(shipTiles[r, c].comp == ShipComponent.cargo)
                    //{

                    //}
                }

            }
            //yield return null;
            
        }

        

        Vector3 direction = new Vector3(randomFloat(0.5f, 1f), 0, randomFloat(0.5f, 1f));

        //if (Random.Range(0f, 2f) < 1.5f)
        if (true)
        {
            if (gameController.currentLevel == 1)
                direction = new Vector3(Random.Range(-0.25f, 0.25f), 0, Random.Range(0.75f, 1f));
            else
                direction = new Vector3(Random.Range(-0.35f, 0.35f), 0, Random.Range(0.75f, 1f));
        }
        //Vector3 direction = Vector3.forward;
        float distance = Random.Range(gameController.enemyShipMinDistance, gameController.enemyShipMaxDistance) + Mathf.Max(Mathf.Sqrt(shipCubes.Count));
        //float distance = 75;
        //float distance = 50;
        Vector3 newPostion = gameController.playerShip.transform.position + direction * distance;
        newPostion = new Vector3(newPostion.x, 0, newPostion.z);


        //shipObject.transform.position = new Vector3(playerShip.transform.position.x + randomFloat(enemyShipMinDistance, enemyShipMaxDistance), 0,
        //                                            playerShip.transform.position.y + randomFloat(enemyShipMinDistance, enemyShipMaxDistance));
        transform.position = newPostion;
        //transform.position = new Vector3(0, 0, 50);
        //transform.position = gameController.playerShip.transform.position;
        transform.LookAt(gameController.playerShip.transform.position);

        this.alive = true;
        this.finishedSpawing = true;
        startinCubeCount = shipCubes.Count;
        baseSpeed = 15f;
        baseRotateSpeed = 15f;
        updateStats();
        Debug.Log("Enemey ship position: " + transform.position.ToString());
        Debug.Log("Enemy Ship inited!");
    }

    float randomFloat(float minValue, float maxValue, bool allowNegative = true)
    {
        float value = Random.Range(minValue, maxValue);
        //Debug.Log(Random.Range(0f, 2f) < 1f);
        if (allowNegative && Random.Range(0f, 2f) < 0.5f)
        {
            return -value;
        }

        return value;
    }

    public void init(bool isPlayer, ShipTile[,] shipTiles, bool alive=true)
    {
        shipCubes = new List<ShipCube>();
        nextFireTime = new Dictionary<ShipComponent, float>()
        {
            { ShipComponent.northCannon, 0f },
            { ShipComponent.eastCannon, 0f },
            { ShipComponent.southCannon, 0f },
            { ShipComponent.westCannon, 0f },
        };

        //Debug.Log(this.transform.position.ToString());
        if (!isPlayer)
        {
            //Debug.Log("Starting here");
            //initEnemy(shipTiles);
            StartCoroutine(initEnemy(shipTiles));
        }
        else
        {
            this.isPlayer = isPlayer;
            this.alive = alive;
            shipCubes = new List<ShipCube>();
            nextFireTime = new Dictionary<ShipComponent, float>()
            {
                { ShipComponent.northCannon, 0f },
                { ShipComponent.eastCannon, 0f },
                { ShipComponent.southCannon, 0f },
                { ShipComponent.westCannon, 0f },
            };
            for (int r = 0; r < shipTiles.GetLength(0); ++r)
            {
                for (int c = 0; c < shipTiles.GetLength(1); ++c)
                {
                    if (shipTiles[r, c].comp != ShipComponent.empty)
                    {
                        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        //cube.transform.position = new Vector3(cubeLength*r - ShipCreation.shipMaxHeight/2*cubeLength, 0, cubeLength*c-ShipCreation.shipMaxWidth/2*cubeLength);
                        //cube.transform.localScale = new Vector3(cubeLength, cubeHeight, cubeLength);
                        //cube.GetComponent<Renderer>().material.color = Color.gray;
                        GameObject tile = null;
                        if (alive)
                        {
                            //Debug.Log("creating cube!");
                            tile = createTile(shipTiles[r, c].comp, r, c, shipTiles.GetLength(0), shipTiles.GetLength(1));
                        }
                        shipCubes.Add(new ShipCube(tile, shipTiles[r, c].comp, r, c));

                        //if(shipTiles[r, c].comp == ShipComponent.cargo)
                        //{

                        //}
                    }

                }
            }
        }





        if (isPlayer)
        {
            startinCubeCount = shipCubes.Count;
            baseSpeed = 15f;
            baseRotateSpeed = 15f;
            updateStats();

            Debug.Log("Ship inited!");
        }
    }

    private void updateWeight()
    {
        float totalWeight = 0f;
        foreach(var cube in shipCubes)
        {
            //if(cube.comp == ShipComponent.hull)
            //{
            //    totalWeight += 1;
            //}
            //else if(cube.comp == ShipComponent.sail)
            //{
            //    totalWeight -= 3;
            //}
            //else
            //{
            //    totalWeight += 2;
            //}
            totalWeight += Ship.weightPerComp[cube.comp];
        }
        weight = totalWeight;
        if (weight < 0)
            weight = 0;
        //weight = 0;
    }

    private void updateSpeed()
    {
        float minSpeedPercent = 0.10f;
        float maxSpeedPercent = 1f;
        if (!isPlayer)
        {
            minSpeedPercent = 0.4f;
            maxSpeedPercent = 0.8f;
        }
        //float maxWeight = 100f;
        float speedReductionPerCube = (maxSpeedPercent - minSpeedPercent) / Ship.maxWeight;
        speedModifier = maxSpeedPercent - (speedReductionPerCube * weight);
        if (speedModifier < minSpeedPercent)
            speedModifier = minSpeedPercent;
        
        //Debug.Log(speedModifier);
        movementSpeed = baseSpeed * speedModifier;
        
        rotateSpeed = baseRotateSpeed * speedModifier;

        if (!isPlayer)
        {
            //movementSpeed *= 1.3f;
            rotateSpeed *= 3f;
            movementSpeed *= (shipCubes.Count / startinCubeCount /5) * 4f;
            if(startinCubeCount != shipCubes.Count)
            {
                movementSpeed *= 0.8f;
            }
            if(gameController != null && gameController.levels != null && gameController.levels.ContainsKey(gameController.currentLevel))
                movementSpeed *= gameController.levels[gameController.currentLevel].enemySpeedIncrease;
        }
        else
        {
            movementSpeed *= 0.8f;
            rotateSpeed *= 2f;
        }
    }

    public void updateStats()
    {
        updateWeight();
        updateSpeed();
    }

    private GameObject createTile(ShipComponent comp, int r, int c, int maxRows, int maxCols)
    {
        GameObject prefab;
        if (comp == ShipComponent.northCannon)
        {
            prefab = northCannonPrefab;
        }
        else if (comp == ShipComponent.eastCannon)
        {
            prefab = eastCannonPrefab;
        }
        else if (comp == ShipComponent.southCannon)
        {
            prefab = southCannonPrefab;
        }
        else if (comp == ShipComponent.westCannon)
        {
            prefab = westCannonPrefab;
        }
        else if (comp == ShipComponent.cargo)
        {
            prefab = cargoPrefab;
        }
        else if (comp == ShipComponent.sail)
        {
            prefab = sailPrefab;
        }
        else
        {
            prefab = hullPrefab;
        }

        //if(comp == ShipComponent.cargo)
        //{
        //    prefab = cargoPrefab;
        //}
        //else 

        GameObject tile = GameObject.Instantiate(prefab);
        tile.transform.parent = this.transform;
        //Debug.Log(this.transform.position);
        //tile.transform.position = new Vector3(cubeLength * r - maxRows / 2 * cubeLength, 0, cubeLength * c - maxCols/ 2 * cubeLength);
        tile.transform.localPosition = new Vector3(cubeLength * c - maxCols / 2 * cubeLength, 0, cubeLength * r - maxRows / 2 * cubeLength);

        //cube.transform.localScale = new Vector3(cubeLength, cubeHeight, cubeLength);
        //cube.GetComponent<Renderer>().material.color = Color.gray;

        //if(comp == ShipComponent.northCannon)
        //{
        //    tile.transform.Rotate(new Vector3(0, 0, 0));
        //}
        //if (comp == ShipComponent.eastCannon)
        //{
        //    tile.transform.Rotate(new Vector3(0, 270, 0));
        //}
        //else if (comp == ShipComponent.southCannon)
        //{
        //    tile.transform.Rotate(new Vector3(0, 180, 0));
        //}
        //else if (comp == ShipComponent.westCannon)
        //{
        //    tile.transform.Rotate(new Vector3(0, 90, 0));
        //}

        return tile;
    }

    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        //float angle360 =  (signed_angle + 180) % 360;

        return signed_angle;
    }

    void moveAsEnemy()
    {
        // TODO: better predict player spot
        //Vector3 playerVector = controller.playerShip.transform.position - transform.position;

        //float cSize = Vector3.Distance(controller.playerShip.transform.position, this.transform.position) * this.movementSpeed;
        //float bSize = Mathf.Abs(controller.playerShip.transform.position.x - this.transform.position.x) * this.movementSpeed;

        //a = (V0.x * V0.x) + (V0.y * V0.y) - (s1 * s1)
        //b = 2 * ((P0.x * V0.x) + (P0.y * V0.y) - (P1.x * V0.x) - (P1.y * V0.y))
        //c = (P0.x * P0.x) + (P0.y * P0.y) + (P1.x * P1.x) + (P1.y * P1.y) - (2 * P1.x * P0.x) - (2 * P1.y * P0.y)

        //t1 = (-b + sqrt((b * b) - (4 * a * c))) / (2 * a)
        //t2 = (-b - sqrt((b * b) - (4 * a * c))) / (2 * a)

        //t = smallestWhichIsntNegativeOrNan(t1, t2)

        //V = (P0 - P1 + (t * s0 * V0)) / (t * s1)
        //V.x = (P0.x - P1.x + (t * s0 * V0.x)) / (t * s1)
        //V.y = (P0.y - P1.y + (t * s0 * V0.y)) / (t * s1)

        float distanceBetweenShips = Vector3.Distance(gameController.playerShip.transform.position, this.transform.position);
        Vector3 playerSpotInFront = gameController.playerShip.transform.position + gameController.playerShip.transform.forward * distanceBetweenShips/3;
        Vector3 targetVector = playerSpotInFront - transform.position;
        Vector3 currentVector = this.transform.forward;

        Vector3 cross = Vector3.Cross(targetVector, currentVector);
        Vector3 rotation = new Vector3();
        if (cross.y < 0)
        {
            rotation = new Vector3(0, rotateSpeed * Time.deltaTime, 0);
        }
        else if (cross.y > 0)
        {
            rotation = new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
        }

        //Debug.Log(rotateSpeed);

        this.transform.Rotate(rotation);
        this.transform.Translate(Vector3.forward * movementSpeed *1.5f * Time.deltaTime);

        // too far so just delete it
        if(distanceBetweenShips > 200)
        {
            Destroy(this.gameObject);
        }

    }

    void move()
    {
        if (isPlayer)
        {
            moveAsPlayer();
            fireIfApplicable();
            if(countCargo() < gameController.levels[gameController.currentLevel].numberOfCargoNeeded)
            {
                die();
            }
        }  
        else
        {
            moveAsEnemy();
            if(shipCubes.Count <startinCubeCount/2)
            {
                die();
            }
        }

        //if(isPL <= 0)
        //{
        //    die();
        //}
    }

    public int countCargo()
    {
        int total = 0;
        foreach(var cube in shipCubes)
        {
            if (cube.comp == ShipComponent.cargo)
                total += 1;
        }
        return total;
    }

    void die()
    {
        Debug.Log("explode ship!");
        //GameObject explode = GameController.Instantiate(explodeShipPrefab, this.transform.position, new Quaternion());
        //var particleSystem = explodePrefab.GetComponent<ParticleSystem>();
        //particleSystem.transform.position = explodePrefab.transform.position;
        //particleSystem.gameObject.transform.position = explodePrefab.transform.position;
        //particleSystem.Play();

        //explode.transform.position = this.transform.position;
        if(isPlayer)
        {
            gameController.die();
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public IEnumerator randomExplosionTime(Vector3 position, bool explodeSound)
    {
        float rand = Random.Range(0f, 0.3f);
        //Debug.Log("waiting " + rand.ToString() + " seconds");
        yield return new WaitForSeconds(rand);


        

        GameObject explodeCube = GameObject.Instantiate(explodeCubePrefab);
        explodeCube.transform.position = position;
        if (explodeSound)
        {
            explodeCube.AddComponent<AudioSource>();
            explodeCube.GetComponent<AudioSource>().volume = 0.05f;
            explodeCube.GetComponent<AudioSource>().clip = hitSound;
            explodeCube.GetComponent<AudioSource>().Play();
        }
        



    } 

    public void hit(GameObject objHit, bool explodeSound)
    {
        if (Vector3.Distance(this.transform.position, gameController.playerShip.transform.position) > 200)
        {
            Debug.Log("Skipping hit");
            return;
        }


        StartCoroutine(randomExplosionTime(objHit.transform.position, explodeSound));

        foreach (ShipCube cube in shipCubes)
        {
            if (cube.obj == objHit)
            {
                //Debug.Log("hit!");
                shipCubes.Remove(cube);
                
                GameObject.Destroy(objHit);
                break;
            }
        }
        //GameObject explodeCube = GameObject.Instantiate(explodeCubePrefab);
        //explodeCube.transform.position = objHit.transform.position;
        //if (explodeSound)
        //{
        //    //explodeCube.AddComponent<AudioSource>();
        //    //explodeCube.GetComponent<AudioSource>().volume = 0.05f;
        //    //explodeCube.GetComponent<AudioSource>().clip = hitSound;
        //    //explodeCube.GetComponent<AudioSource>().Play();
        //}
        //cube.obj.GetComponent<AudioSource>().Play();
        //health -= damagePerHit;
        //foreach (ShipCube cube in shipCubes)
        //{
        //    if (cube.obj == objHit)
        //    {
        //        Debug.Log("hit!");
        //        shipCubes.Remove(cube);
        //        break;
        //    }
        //}
        updateStats();
    }

    public void win()
    {
        gameController.win();
    }

    void fireIfApplicable()
    {
        //Vector3 fireDirection = new Vector3();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            fireCannons(ShipComponent.northCannon);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            fireCannons(ShipComponent.eastCannon);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            fireCannons(ShipComponent.westCannon);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            fireCannons(ShipComponent.southCannon);
        }
        else
        {
            return;
        }
    }

    void fireCannons(ShipComponent comp)
    {
        //Vector3 fireDirection = new Vector3();

        //if (comp == ShipComponent.eastCannon)
        //{
        //    fireDirection = Vector3.Cross(-this.transform.forward, Vector3.up);
        //}

        //if (comp == ShipComponent.eastCannon)
        //{
        //    fireDirection = Vector3.Cross(-this.transform.forward, Vector3.up);
        //}

        if(nextFireTime[comp] > Time.time)
        {
            // still on cooldown
            return;
        }

        nextFireTime[comp] = Time.time + cannonCooldown;

        bool fired = false;

        foreach(ShipCube cube in shipCubes)
        {
            if(cube.comp == comp)
            {
                GameObject cannonObj = GameObject.Instantiate(cannonBallPrefab);
                cannonObj.GetComponent<CannonBall>().shipFired = this;
                cannonObj.transform.forward = cube.obj.transform.forward;
                cannonObj.transform.position = new Vector3(cube.obj.transform.position.x, 
                                               cube.obj.transform.position.y + 1.5f, cube.obj.transform.position.z);
                //cube.obj.GetComponent<AudioSource>().clip = fireSound;
                cube.obj.GetComponent<AudioSource>().volume = 0.05f;
                cube.obj.GetComponent<AudioSource>().Play();
            }
        }
        
        //if(fired)
        //{
        //    this.fireSound.Play();
        //}

    }

    public void resetPlayerCubes()
    {
        if (shipCubes == null)
            return;
        foreach(ShipCube cube in shipCubes)
        {
            Debug.Log("destroy player cube");
            Destroy(cube.obj);
        }
        shipCubes = new List<ShipCube>();
    }

    void moveAsPlayer()
    { 
        
        float currentSpeed = movementSpeed;
        
        Vector3 rotation = Vector3.zero;
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed *= 1.5f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            currentSpeed /= 2;
            //currentSpeed = 0;
        }
        if(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            rotation = new Vector3(0, -rotateSpeed * Time.deltaTime, 0);
            //rotation = new Vector3(0, 0, 0.1f);
        }

        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            rotation = new Vector3(0, rotateSpeed * Time.deltaTime, 0);
            //rotation = new Vector3(0, 0, 0.1f);
        }

        this.transform.Translate(Vector3.forward * currentSpeed *Time.deltaTime);
        this.transform.Rotate(rotation);

        objectiveArrow.transform.LookAt(objective.transform);


        //foreach (ShipCube cube in shipCubes)
        //{
        //    cube.obj.transform.Rotate(new Vector3(0, 1, 0));
        //    cube.obj.transform.Translate(Vector3.forward * currentSpeed);
        //    //cube.obj.transform.Rotate(rotation);
        //}

    }

    //public GameObject createBase(int r, int c)
    //{
    //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    cube.transform.position = new Vector3(cubeLength * r - ShipCreation.shipMaxHeight / 2 * cubeLength, 0, cubeLength * c - ShipCreation.shipMaxWidth / 2 * cubeLength);
    //    cube.transform.localScale = new Vector3(cubeLength, baseHeight, cubeLength);
    //    cube.GetComponent<Renderer>().material.color = Color.gray;
    //    return cube;
    //}

    //public GameObject createTop(int r, int c)
    //{
    //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    cube.transform.position = new Vector3(cubeLength * r - ShipCreation.shipMaxHeight / 2 * cubeLength, cubeHeight, cubeLength * c - ShipCreation.shipMaxWidth / 2 * cubeLength);
    //    cube.transform.localScale = new Vector3(cubeLength, cubeHeight, cubeLength);
    //    cube.GetComponent<Renderer>().material.color = Color.gray;
    //    return cube;
    //}

    //public static Ship createShip(ShipTile[,] shipTiles)
    //{
    //    //GameObject shipObject = GameObject.Instantiate(reserve.unitPrefab, spawnPositions[reserve.lane], rotation);
    //    //UnitController unit = newUnitObj.GetComponent<UnitController>();
    //    return new Ship();
    //}
}
