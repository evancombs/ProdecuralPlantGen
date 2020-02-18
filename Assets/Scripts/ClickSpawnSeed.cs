using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSpawnSeed : MonoBehaviour
{
    public int forceSeed = 0;

    public GameObject Prefab;
    [SerializeField]
    public Plane groundPlane;
    //public int RayDistance;
    //private Vector3 Point;
    //public LayerMask layerMask;
    //public Seed seedInstance;
    Vector3 spawnPosition;




    void Update()
    {
        /*
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Debug.Log("Update is running!");
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Click Detected!");
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.white, 100f);
                //
                spawnPosition = hit.point;
                Debug.Log("( " + spawnPosition.x + ", " + spawnPosition.y + ", " + spawnPosition.z + " )");
                
                

                //Debug.DrawRay(transform.position, spawnPosition, Color.white, 100f);
                //Debug.DrawRay(transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction, Color.white, 100f);
                Debug.DrawRay(transform.position, spawnPosition, Color.white, 100f);
                Debug.Log("Plane Intersection!");
                // spawnPosition = ray.GetPoint(enter);
                GameObject lSystemSeed = Instantiate(Prefab, spawnPosition, Quaternion.identity);
            }
        }
        */
        // Creates a ray going from the camera, to the screen position with ~magic~
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Structure that stores information about a raycast collision
        RaycastHit hit;

        // Detects left clicks
        if (Input.GetMouseButton(0))
        {
            // Sends the ray out, and returns information about collisions to hit. Only returns true if there was a collision
            if (Physics.Raycast(raycast, out hit))
            {
                spawnPosition = hit.point;
                Debug.Log("Hit detected at : ( " + spawnPosition.x + ", " + spawnPosition.y + ", " + spawnPosition.z + " )");
                GameObject lSystemSeed = Instantiate(Prefab, spawnPosition , Quaternion.identity);
                //lSystemSeed.transform.position = spawnPosition;
                lSystemSeed.transform.localPosition = spawnPosition;
            }
            else
            {
                Debug.Log("No hit detected");
            }
        }
    }

    void SpawnSeed()
    {/*
        Vector3 spawnPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPosition.z = 0.0f;

        GameObject seedObject = Instantiate(Prefab, spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
        //seedInstance
        // Now access the seedInstance seed class and add the forced seed, or generate a random one. Now, this particular seed instance has a numerical seed
        seedObject.GetComponent<SeedBehavior>().seedInstance = new Seed(forceSeed); ;
       */
        
        groundPlane = new Plane();
        //groundPlane.Set3Points()
        groundPlane.SetNormalAndPosition(Vector3.zero, Vector3.up);

        //float enter = 0.0f;


        //Debug.DrawRay(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Color.white, 100f);
        Debug.DrawRay(transform.position, Camera.main.ScreenPointToRay(Input.mousePosition).direction);
        /*
        if (groundPlane.Raycast(ray, out enter))
        {
            Debug.Log("Plane Intersection!");
            spawnPosition = ray.GetPoint(enter);
            GameObject lSystemSeed = Instantiate(Prefab, spawnPosition, Quaternion.identity);
        }
        */
        //RaycastHit hit;

        
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(spawnPosition,100f);
    }
}
