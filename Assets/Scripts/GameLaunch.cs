using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : SingletonMono<GameLaunch>
{
    // Start is called before the first frame update

    public int a = 0;
    void Start()
    {
        Test.Instance.TestFunc();
        StartCoroutine(TestCoroutine());

        XLuaMgr.Instance.SafeDoString("require 'Test.TestMain'");  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TestCoroutine()
    {
        Debug.Log("TestCoroutine Start"+"wait 3 seconds");
        yield return new WaitForSeconds(3);

    }
}
