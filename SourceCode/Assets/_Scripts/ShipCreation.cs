using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShipComponent
{
    northCannon,
    eastCannon,
    southCannon,
    westCannon,
    cargo,
    hull,
    sail,
    empty
}

public class ShipTile
{
    public Button button;
    public ShipComponent comp;

    public ShipTile(Button button, ShipComponent comp)
    {
        this.button = button;
        this.comp = comp;
    }

    public void setComp(ShipComponent newComp, Texture texture)
    {
        comp = newComp;
        //string text = "?";
        //if(newComp == ShipComponent.northCannon)
        //{
        //    text = "north";
        //}
        //button.GetComponentInChildren<Text>().text = comp.ToString();

        //Debug.Log("Set Texture!");
        button.GetComponent<RawImage>().texture = texture;
        button.GetComponent<RawImage>().transform.rotation = Quaternion.identity;
        if (newComp == ShipComponent.northCannon)
        {
            button.GetComponent<RawImage>().transform.Rotate(new Vector3(0, 0, 270));
        }
        else if (newComp == ShipComponent.eastCannon)
        {
            button.GetComponent<RawImage>().transform.Rotate(new Vector3(0, 0, 180));
        }
        if (newComp == ShipComponent.southCannon)
        {
            button.GetComponent<RawImage>().transform.Rotate(new Vector3(0, 0, 90));
        }


    }

    public static ShipTile[,] generateEmpty(int rows, int cols)
    {
        ShipTile[,] tiles = new ShipTile[rows, cols];
        for(int r =0; r < rows; r++)
        {
            for(int c =0; c < cols; c++)
            {
                tiles[r, c] = new ShipTile(null, ShipComponent.empty);
            }
        }
        return tiles;
    }
}

public class ShipCreation : MonoBehaviour {

    public GameObject prefabButton;
    public RectTransform ParentPanel;

    public static int shipMaxHeight = 20;
    public static int shipMaxWidth = 40;

    private ShipTile[,] playerBlueprint;

    private Button northButton;
    private Button eastButton;
    private Button southButton;
    private Button westButton;

    private Button sailButton;
    private Button cargoButton;
    private Button hullButton;
    private Button emptyButton;

    private Button confirmButton;
    private Button lastButtonPressed=null;

    public Texture northButtonTexture;
    //public Texture eastButtonTexture;
    //public Texture southButtonTexture;
    //public Texture westButtonTexture;
    public Texture cargButtonTexture;
    public Texture sailButtonTexture;
    public Texture hullButtonTexture;

    public Dictionary<ShipComponent, Texture> compTextures;

    private GameController gameController;

    //private ShipTile[,] playerShipBlueprint;

    // Use this for initialization
    void Start ()
    {
        //var parentRect = ParentPanel.GetComponent<RectTransform>();
        //float buttonHeigth = parentRect.
        //Debug.Log(ParentPanel.GetComponent<RectTransform>().rect.height.ToString());
        //Debug.Log(ParentPanel.sizeDelta.x.ToString());
        //float buttonHeight = ParentPanel.GetComponent<RectTransform>().rect.height / shipMaxHeight;
        //float buttonWidth = ParentPanel.GetComponent<RectTransform>().rect.width / shipMaxWidth;

        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        confirmButton = GameObject.Find("ConfirmButton").GetComponent<Button>();
        confirmButton.onClick.AddListener(() => ConfirmClicked());

        northButton = GameObject.Find("UpButton").GetComponent<Button>();
        northButton.onClick.AddListener(() => sideButtonsClicked(northButton));

        eastButton = GameObject.Find("RightButton").GetComponent<Button>();
        eastButton.onClick.AddListener(() => sideButtonsClicked(eastButton));

        southButton = GameObject.Find("DownButton").GetComponent<Button>();
        southButton.onClick.AddListener(() => sideButtonsClicked(southButton));

        westButton = GameObject.Find("LeftButton").GetComponent<Button>();
        westButton.onClick.AddListener(() => sideButtonsClicked(westButton));

        sailButton = GameObject.Find("SailButton").GetComponent<Button>();
        sailButton.onClick.AddListener(() => sideButtonsClicked(sailButton));

        cargoButton = GameObject.Find("CargoButton").GetComponent<Button>();
        cargoButton.onClick.AddListener(() => sideButtonsClicked(cargoButton));

        hullButton = GameObject.Find("HullButton").GetComponent<Button>();
        hullButton.onClick.AddListener(() => sideButtonsClicked(hullButton));

        emptyButton = GameObject.Find("ClearButton").GetComponent<Button>();
        emptyButton.onClick.AddListener(() => sideButtonsClicked(emptyButton));

        playerBlueprint = new ShipTile[shipMaxHeight, shipMaxWidth];

        compTextures = new Dictionary<ShipComponent, Texture>()
        {
            {ShipComponent.northCannon, northButtonTexture },
            {ShipComponent.eastCannon, northButtonTexture},
            {ShipComponent.southCannon, northButtonTexture },
            {ShipComponent.westCannon, northButtonTexture},
            {ShipComponent.sail, sailButtonTexture},
            {ShipComponent.cargo, cargButtonTexture},
            {ShipComponent.hull, hullButtonTexture},
            {ShipComponent.empty, null}
        };


        float buttonHeight = 14;
        float buttonWidth = 14;
        float buttonWidthBuffer = 18;
        float buttonHeightBuffer = 18;
        float xOffset = 90;
        float yOffset = 50;
        for (int row = 0; row < shipMaxHeight; row++)
        {
            for (int col = 0; col < shipMaxWidth; ++col)
            {
                GameObject goButton = (GameObject)Instantiate(prefabButton);
                goButton.transform.SetParent(ParentPanel, false);
                //goButton.transform.localScale = new Vector3(1, 1, 1);
                //goButton.transform.position = new Vector3(50 * row, 50 * row, 0);
                goButton.transform.position = new Vector3(col * (buttonWidth+buttonWidthBuffer) + xOffset, row * (buttonHeight+buttonHeightBuffer) + yOffset);
                goButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, buttonWidth);
                goButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, buttonHeight);

                Button tempButton = goButton.GetComponent<Button>();

                ShipTile tile = new ShipTile(tempButton, ShipComponent.empty);

                playerBlueprint[row, col] = tile;
                //tempButton.OnPointerDown().
                //tempButton.onClick.AddListener(() => shipButtonClicked(tile));

                UnityEngine.EventSystems.EventTrigger trigger = tempButton.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
                var pointerDown = new UnityEngine.EventSystems.EventTrigger.Entry();
                pointerDown.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
                pointerDown.callback.AddListener((e) => shipButtonClicked(tile));
                trigger.triggers.Add(pointerDown);
            }
        }

        initDefaultShip();

    }

    void initDefaultShip()
    {

        //this.setComp(shipTiles[18, 8], ShipComponent.westCannon);
        //this.setComp(shipTiles[18, 9], ShipComponent.sail);
        //this.setComp(shipTiles[18, 10], ShipComponent.sail);
        //this.setComp(shipTiles[18, 11], ShipComponent.sail);
        //this.setComp(shipTiles[18, 12], ShipComponent.eastCannon);

        //this.setComp(shipTiles[19, 8], ShipComponent.westCannon);
        //this.setComp(shipTiles[19, 9], ShipComponent.hull);
        //this.setComp(shipTiles[19, 10], ShipComponent.hull);
        //this.setComp(shipTiles[19, 11], ShipComponent.hull);
        //this.setComp(shipTiles[19, 12], ShipComponent.eastCannon);

        //this.setComp(shipTiles[20, 8], ShipComponent.westCannon);
        //this.setComp(shipTiles[20, 9], ShipComponent.cargo);
        //this.setComp(shipTiles[20, 10], ShipComponent.cargo);
        //this.setComp(shipTiles[20, 11], ShipComponent.cargo);
        //this.setComp(shipTiles[20, 12], ShipComponent.eastCannon);

        //this.setComp(shipTiles[21, 8], ShipComponent.westCannon);
        //this.setComp(shipTiles[21, 9], ShipComponent.cargo);
        //this.setComp(shipTiles[21, 10], ShipComponent.cargo);
        //this.setComp(shipTiles[21, 11], ShipComponent.cargo);
        //this.setComp(shipTiles[21, 12], ShipComponent.eastCannon);

        //this.setComp(shipTiles[22, 8], ShipComponent.westCannon);
        //this.setComp(shipTiles[22, 9], ShipComponent.hull);
        //this.setComp(shipTiles[22, 10], ShipComponent.hull);
        //this.setComp(shipTiles[22, 11], ShipComponent.hull);
        //this.setComp(shipTiles[22, 12], ShipComponent.eastCannon);

        //this.setComp(shipTiles[23, 9], ShipComponent.northCannon);
        //this.setComp(shipTiles[23, 10], ShipComponent.northCannon);
        //this.setComp(shipTiles[23, 11], ShipComponent.northCannon);

        //this.setComp(playerBlueprint[7, 19], ShipComponent.southCannon);
        //this.setComp(playerBlueprint[7, 20], ShipComponent.southCannon);
        //this.setComp(playerBlueprint[7, 21], ShipComponent.southCannon);

        //this.setComp(playerBlueprint[8, 18], ShipComponent.westCannon);
        //this.setComp(playerBlueprint[8, 19], ShipComponent.sail);
        //this.setComp(playerBlueprint[8, 20], ShipComponent.sail);
        //this.setComp(playerBlueprint[8, 21], ShipComponent.sail);
        //this.setComp(playerBlueprint[8, 22], ShipComponent.eastCannon);

        //this.setComp(playerBlueprint[9, 18], ShipComponent.westCannon);
        //this.setComp(playerBlueprint[9, 19], ShipComponent.hull);
        //this.setComp(playerBlueprint[9, 20], ShipComponent.hull);
        //this.setComp(playerBlueprint[9, 21], ShipComponent.hull);
        //this.setComp(playerBlueprint[9, 22], ShipComponent.eastCannon);

        //this.setComp(playerBlueprint[10, 18], ShipComponent.westCannon);
        //this.setComp(playerBlueprint[10, 19], ShipComponent.cargo);
        //this.setComp(playerBlueprint[10, 20], ShipComponent.cargo);
        //this.setComp(playerBlueprint[10, 21], ShipComponent.cargo);
        //this.setComp(playerBlueprint[10, 22], ShipComponent.eastCannon);

        //this.setComp(playerBlueprint[11, 18], ShipComponent.westCannon);
        //this.setComp(playerBlueprint[11, 19], ShipComponent.cargo);
        //this.setComp(playerBlueprint[11, 20], ShipComponent.cargo);
        //this.setComp(playerBlueprint[11, 21], ShipComponent.cargo);
        //this.setComp(playerBlueprint[11, 22], ShipComponent.eastCannon);

        //this.setComp(playerBlueprint[12, 18], ShipComponent.westCannon);
        //this.setComp(playerBlueprint[12, 19], ShipComponent.hull);
        //this.setComp(playerBlueprint[12, 20], ShipComponent.hull);
        //this.setComp(playerBlueprint[12, 21], ShipComponent.hull);
        //this.setComp(playerBlueprint[12, 22], ShipComponent.eastCannon);

        //this.setComp(playerBlueprint[13, 19], ShipComponent.northCannon);
        //this.setComp(playerBlueprint[13, 20], ShipComponent.northCannon);
        //this.setComp(playerBlueprint[13, 21], ShipComponent.northCannon);

        //this.setComp(playerBlueprint[14, 20], ShipComponent.northCannon);

        //for (int r = 0; r < shipMaxHeight; r++)
        //{
        //    for (int c = 0; c < shipMaxWidth; c++)
        //    {
        //        this.setComp(shipTiles[r, c], ShipComponent.hull);
        //    }
        //}

        //this.setComp(playerBlueprint[14, 20], ShipComponent.northCannon);

        GameController.playerGold = 7777;

        this.setComp(playerBlueprint[8, 19], ShipComponent.sail);
        this.setComp(playerBlueprint[8, 20], ShipComponent.southCannon);
        this.setComp(playerBlueprint[8, 21], ShipComponent.sail);

        this.setComp(playerBlueprint[9, 18], ShipComponent.westCannon);
        this.setComp(playerBlueprint[9, 19], ShipComponent.sail);
        this.setComp(playerBlueprint[9, 20], ShipComponent.sail);
        this.setComp(playerBlueprint[9, 21], ShipComponent.sail);
        this.setComp(playerBlueprint[9, 22], ShipComponent.eastCannon);

        this.setComp(playerBlueprint[10, 18], ShipComponent.westCannon);
        this.setComp(playerBlueprint[10, 19], ShipComponent.cargo);
        this.setComp(playerBlueprint[10, 20], ShipComponent.cargo);
        this.setComp(playerBlueprint[10, 21], ShipComponent.cargo);
        this.setComp(playerBlueprint[10, 22], ShipComponent.eastCannon);

        this.setComp(playerBlueprint[11, 19], ShipComponent.northCannon);
        this.setComp(playerBlueprint[11, 20], ShipComponent.northCannon);
        this.setComp(playerBlueprint[11, 21], ShipComponent.northCannon);

        this.setComp(playerBlueprint[12, 18], ShipComponent.hull);
        this.setComp(playerBlueprint[12, 19], ShipComponent.hull);
        this.setComp(playerBlueprint[12, 20], ShipComponent.hull);
        this.setComp(playerBlueprint[12, 21], ShipComponent.hull);
        this.setComp(playerBlueprint[12, 22], ShipComponent.hull);

        GameController.playerGold = 300;
        gameController.updatePlayerStats();
    }

    void setComp(ShipTile tile, ShipComponent comp)
    {
        if(tile.comp != ShipComponent.empty)
        {
            GameController.playerGold += Ship.pricePerComp[tile.comp];
        }
        if (comp != ShipComponent.empty)
        {
            GameController.playerGold -= Ship.pricePerComp[comp];
        }


        tile.setComp(comp, compTextures[comp]);
        gameController.playerShip.init(true, playerBlueprint, false);
        gameController.playerShip.updateStats();
        gameController.updatePlayerStats();
        //gameController.updatePlayerStats(shipTiles);

    }

    void sideButtonsClicked(Button button)
    {
        Debug.Log(button.GetComponentInChildren<Text>().text);
        //Debug.Log(button.GetComponent<RawImage>().color);
        
        if (button.name == "ClearButton")
        {
            button.GetComponentInChildren<Text>().color = Color.green;
        }
        else
        {
            button.GetComponent<RawImage>().color = Color.green;
        }

        if (lastButtonPressed != null && lastButtonPressed != button)
        {
            if(lastButtonPressed.name == "ClearButton")
            {
                lastButtonPressed.GetComponentInChildren<Text>().color = Color.white;
            }
            else
            {
                lastButtonPressed.GetComponent<RawImage>().color = new Color(1000f, 1000f, 1000f, 1000f);
            }
            
        }
        lastButtonPressed = button;
    }

    void shipButtonClicked(ShipTile tile)
    {
        ShipComponent comp = ShipComponent.empty;
        if (lastButtonPressed == null)
            return;
        else if (lastButtonPressed == northButton)
            comp = ShipComponent.northCannon;
        else if (lastButtonPressed == eastButton)
            comp = ShipComponent.eastCannon;
        else if (lastButtonPressed == southButton)
            comp = ShipComponent.southCannon;
        else if (lastButtonPressed == westButton)
            comp = ShipComponent.westCannon;
        else if (lastButtonPressed == cargoButton)
            comp = ShipComponent.cargo;
        else if (lastButtonPressed == sailButton)
            comp = ShipComponent.sail;
        else if (lastButtonPressed == hullButton)
            comp = ShipComponent.hull;
        else if (lastButtonPressed == emptyButton)
            comp = ShipComponent.empty;

        this.setComp(tile, comp);

        checkPriceAvailable(ShipComponent.northCannon, northButton);
        checkPriceAvailable(ShipComponent.eastCannon, eastButton);
        checkPriceAvailable(ShipComponent.southCannon, southButton);
        checkPriceAvailable(ShipComponent.westCannon, westButton);
        checkPriceAvailable(ShipComponent.sail, sailButton);
        checkPriceAvailable(ShipComponent.cargo, cargoButton);
        checkPriceAvailable(ShipComponent.hull, hullButton);

    }

    public void beginMenu()
    {
        checkPriceAvailable(ShipComponent.northCannon, northButton);
        checkPriceAvailable(ShipComponent.eastCannon, eastButton);
        checkPriceAvailable(ShipComponent.southCannon, southButton);
        checkPriceAvailable(ShipComponent.westCannon, westButton);
        checkPriceAvailable(ShipComponent.sail, sailButton);
        checkPriceAvailable(ShipComponent.cargo, cargoButton);
        checkPriceAvailable(ShipComponent.hull, hullButton);
    }

    void checkPriceAvailable(ShipComponent comp, Button button)
    {
        if (button == null)
            return;
        if (GameController.playerGold < Ship.pricePerComp[comp])
        {
            Debug.Log("disabling!");
            button.interactable = false;
            if(button == lastButtonPressed)
            {
                if (button.name == "ClearButton")
                {
                    button.GetComponentInChildren<Text>().color = Color.white;
                }
                else
                {
                    button.GetComponent<RawImage>().color = new Color(1000f, 1000f, 1000f, 1000f);
                }
                lastButtonPressed = null;
            }
            

        }
        else
        {
            button.interactable = true;
        }
    }

    void ConfirmClicked()
    {
        Debug.Log("Confirm Clicked!");
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().beginLevelPlay(playerBlueprint);
    }

    // Update is called once per frame
    void Update ()
    {
    }
}
