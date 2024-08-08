using System.Collections;
using System.Collections.Generic;
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
    public Inventory inventory;

    public World world;


    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        world = FindObjectOfType<World>();

        inventory = FindObjectOfType<Inventory>();
        mineParticle = FindObjectOfType<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Mine();
        }

        if (Input.GetMouseButtonDown(1))
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
        if(world.currentBlockMine == BlockType.Bedrock)
        {
            world.SetBlock(hit, BlockType.Bedrock);
        }
        else 
        {
            mineParticle.transform.position = hit.point;
            mineParticle.Play();
        }
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
