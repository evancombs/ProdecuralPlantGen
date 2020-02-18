using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] lines = { new Vector3(0, 0, 0), new Vector3(1,0,2) };
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();
        //lineRenderer.SetPosition(new Vector3(0, 0, 0));
        lineRenderer.SetPositions(lines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
