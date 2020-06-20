using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class J_CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        cam2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        camChange();
    }
    public GameObject cam1;
    public GameObject cam2;
    void camChange()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            cam2.SetActive(true);
            cam1.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            cam2.SetActive(false);
            cam1.SetActive(true);
        }

    }
}
