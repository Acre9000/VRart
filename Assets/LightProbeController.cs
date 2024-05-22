using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class LightProbeController : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int number;
    public bool perform;

    public float rot=14.966f;
    public GameObject target;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (perform == true) 
        {
            GameObject t = Instantiate (target,new Vector3(0,0,0),new Quaternion(0,0,0,0));
            t.transform.Rotate(0f,rot,0f, Space.World);
            number += -1;
            rot += 14.966f;
            if(number == 0)
            {
                perform = false;
            }
        }
    }
}
