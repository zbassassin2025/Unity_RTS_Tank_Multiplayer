using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ZB_RTS_NetworkManager : NetworkManager 
{
    [SerializeField] private GameObject unitSpawnPrefab = null;
    [SerializeField] private ZB_GameOverHandler gameOverHandlerPrefab = null; 

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        ZB_RTS_Player player = conn.identity.GetComponent<ZB_RTS_Player>();
        player.SetTeamColor(new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)));

       GameObject unitSpawnInstance = Instantiate(unitSpawnPrefab, conn.identity.transform.position, conn.identity.transform.rotation);
       NetworkServer.Spawn(unitSpawnInstance, conn); 
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if(SceneManager.GetActiveScene().name.StartsWith("ZB_Scene_Map"))
        {
            ZB_GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance.gameObject); 
        }
    }
}