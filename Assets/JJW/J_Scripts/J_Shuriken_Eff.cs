using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 돌아라
public class J_Shuriken_Eff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TurnAround();
    }
    void TurnAround()
    {
        transform.Rotate(Vector3.forward*50);
    }
}
