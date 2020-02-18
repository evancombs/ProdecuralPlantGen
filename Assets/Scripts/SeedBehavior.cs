using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : MonoBehaviour
{
    public Seed seedInstance;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("stemThickness: " + seedInstance.stemThickness);
        Debug.Log("numLeaves: " + seedInstance.numLeaves);
        Debug.Log("plantHeight: " + seedInstance.plantHeight);
        //StartCoroutine(LateStart(1));
    }

    IEnumerator LateStart(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
  {
        Debug.Log("Collision!");
        Destroy(gameObject);
    }
}
