using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 orbitPoint = transform.parent.position;
        //transform.RotateAround(orbitPoint, 20 * Time.deltaTime);
        transform.Rotate(0, 20 * Time.deltaTime, 0);
        //transform.Rotate(0, 3f);
    }
}
