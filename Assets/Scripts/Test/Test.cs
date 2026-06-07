using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : SingletonMono<Test>
{
    // Start is called before the first frame update


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TestFunc()
    {
        Debug.Log("TestFunc");
    }
}
