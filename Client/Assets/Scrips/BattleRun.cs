using CommonClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleRun : MonoBehaviour {
    private const int port = 8088;
    private static string IpStr = "127.0.0.1";
    // Use this for initialization
    void Start () {
        ClientManager.Instance.Init(IpStr, port);
    }
	
	// Update is called once per frame
	void Update () {
        ClientManager.Instance.Update();
    }
}
