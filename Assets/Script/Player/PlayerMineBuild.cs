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


    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        world = FindObjectOfType<World>();

        inventory = FindObjectOfType<HotBarManager>();
        inventoryManager = FindObjectOfType<InventoryManager>();

        mineParticle = FindObjectOfType<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        isInventoryOpen = inventoryManager.isInventoryOpen;

        if (Input.GetMouseButtonDown(0) && !isInventoryOpen)
        {
            Mine();
        }

        if (Input.GetMouseButtonDown(1) && !isInventoryOpen)
        {
            Build();
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
            PerformBuild(hit);
        }

    }

    private void PerformBuild(RaycastHit hit)
    {
        if (inventory.currentItem.isABlock && inventory.currentItem != null)
        {
            world.SetBlock(hit, inventory.currentItem.blocs);
        }
        else
        {
            Debug.Log("Ce n'est pas un block vous ne pouvez pas le poser");
        }

    }

}
