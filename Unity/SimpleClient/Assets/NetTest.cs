using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using L.Network;

public class NetTest : MonoBehaviour {

    private ServerTest test;
    // Use this for initialization
    void Start () {
        test = new ServerTest();
        test.Awake(NetworkProtocol.TCP, "127.0.0.1", 2000);
    }
	
	// Update is called once per frame
	void Update () {
        test.Update();
    }
}
