using UnityEngine;

public class PlayerMineBuild : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public float buildRayLength = 3.5f;
    public float mineRayLength = 1.5f;
    public LayerMask groundMask;
    public BlockType currentBlockMine;
    public ParticleSystem mineParticle;
    public HotBarManager inventory;
    public InventoryManager inventoryManager;
    public bool isInventoryOpen;
    public World world;
    public BlockType curentBlockBuild;
    public GameManager gameManager;
    public Vector3 blockposition;

    public GameObject uttilityPanel;
    public ChunkRenderer currentChunk;




    void Start()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        world = FindObjectOfType<World>();

        gameManager = FindObjectOfType<GameManager>();


        inventory = FindObjectOfType<HotBarManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        mineParticle = FindObjectOfType<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        isInventoryOpen = inventoryManager.isInventoryOpen;

        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, groundMask))
        {
            blockposition = hit.point;
        }

        VisualizeRaycast();
        DebugPlayerChunk();
        curentBlockBuild = world.GetBlockFromChunkCoordinates(currentChunk.ChunkData, Mathf.RoundToInt(blockposition.x), Mathf.RoundToInt(blockposition.y), Mathf.RoundToInt(blockposition.z));


        if (Input.GetMouseButtonDown(0) && !isInventoryOpen)
        {
            Mine();
        }

        if (Input.GetMouseButtonDown(1) && !isInventoryOpen)
        {
            Build();
        }


        if(Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Escape)) 
        {
            CloseAllWindows();
        }

      

    }

    private void VisualizeRaycast()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        // Dessine une ligne rouge pour le raycast
        if (Physics.Raycast(playerRay, out hit, mineRayLength, groundMask))
        {
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 0.1f);
        }
        else
        {
            Debug.DrawLine(mainCamera.transform.position, mainCamera.transform.position + mainCamera.transform.forward * mineRayLength, Color.red, 0.1f);
        }
    }


    private void Mine()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, mineRayLength, groundMask))
        {
            PerformMine(hit);
        }



    }

    private void PerformMine(RaycastHit hit)
    {
        world.SetBlock(hit, BlockType.Air);
        if (world.currentBlockMine == BlockType.Bedrock)
        {
            world.SetBlock(hit, BlockType.Bedrock);
        }
        else
        {
            GameObject mineParticleGameObject = Instantiate(mineParticle.gameObject, hit.point, Quaternion.identity);

            // Récupère le système de particules
            ParticleSystem particleSystem = mineParticleGameObject.GetComponent<ParticleSystem>();

            // Récupère le module Renderer du système de particules
            ParticleSystemRenderer particleRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();

            // Trouve l'ItemData correspondant au type de bloc miné
            ItemData minedBlockItem = FindItemDataForBlockType(world.currentBlockMine);

            // Si un matériau de particule est défini pour ce bloc, l'appliquer
            if (minedBlockItem != null && minedBlockItem.visualOfParticle != null)
            {
                particleRenderer.material.mainTexture = minedBlockItem.visualOfParticle;
            }
            // Joue les particules
            particleSystem.Play();

            Destroy(mineParticleGameObject, 2f);
        }
    }


    private ItemData FindItemDataForBlockType(BlockType blockType)
    {
        foreach (var item in inventory.items) // Assuming you have a list of items in your inventory
        {
            if (item.isABlock && item.blocs == blockType)
            {
                return item;
            }
        }
        return null;
    }

    private void Build()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, mineRayLength, groundMask) && inventory.currentItem != null)
        {
            hit.point = hit.point + hit.normal * 0.5f;
        }

        PerformBuild(hit);

        curentBlockBuild = world.GetBlockFromChunkCoordinates(currentChunk.ChunkData, Mathf.RoundToInt(blockposition.x), Mathf.RoundToInt(blockposition.y), Mathf.RoundToInt(blockposition.z));

    }

    private void PerformBuild(RaycastHit hit)
    {
        Debug.Log("Build");
        if (inventory.currentItem.isABlock && inventory.currentItem != null)
        {
            world.SetBlock(hit, inventory.currentItem.blocs);
            Debug.Log(FindItemDataForBlockType(curentBlockBuild).isAnUttilityBlock);
        }

        Debug.Log(FindItemDataForBlockType(curentBlockBuild).isAnUttilityBlock);

        if (FindItemDataForBlockType(curentBlockBuild).isAnUttilityBlock)
        {
            world.SetBlock(hit, BlockType.Air);

            foreach (UttilityBlock utility in BlockUtilityManager.instance.uttilityBlock)
            {
                if (curentBlockBuild == utility.block)
                {
                    // Trouvé, on ouvre le panel associé
                    uttilityPanel = utility.panel; // Active le panel de l'utility block
                    utility.panel.SetActive(true);
                    Debug.Log($"Panel pour {utility.blockUtility} ouvert.");
                    return; // Sort de la fonction une fois le bloc trouvé
                }

                curentBlockBuild = utility.block;

            }

            Debug.LogWarning("Bloc utilitaire non trouvé dans la liste.");


        }
        else
        {
            Debug.Log("Ce n'est pas un block, vous ne pouvez pas le poser.");
        }


    }

    public void CloseAllWindows() 
    {
        foreach (UttilityBlock utility in BlockUtilityManager.instance.uttilityBlock)
        {
           
            utility.panel.SetActive(false);
            
        }
    }

    public void DebugPlayerChunk()
    {
        foreach (GameObject chunk in GameObject.FindGameObjectsWithTag("Ground"))
        {
            // Vérifie si les coordonnées du chunk correspondent à currentPlayerChunkPosition
            if (chunk.transform.position == gameManager.currentPlayerChunkPosition)
            {
                currentChunk = chunk.GetComponent<ChunkRenderer>();
                return; // On sort de la boucle une fois qu'on a trouvé le chunk
            }
        }

    }
}
