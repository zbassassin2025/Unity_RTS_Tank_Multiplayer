using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 
using UnityEngine.SceneManagement;

public class ZB_RTS_NetworkManager : NetworkManager 
{
    [SerializeField] private GameObject unitBasePrefab = null; // changed from unitSpawnPrefab 
    [SerializeField] private ZB_GameOverHandler gameOverHandlerPrefab = null;

    public static event Action ClientOnConnected;
    public static event Action ClientOnDisconnected;

    private bool isGameStarted; 
    public List<ZB_RTS_Player> Players { get; } = new List<ZB_RTS_Player>();

    #region Server 

    public override void OnServerConnect(NetworkConnection conn)
    {
       if(!isGameStarted)
        {
            return; 
        }

        conn.Disconnect(); // kick player 
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
       ZB_RTS_Player player = conn.identity.GetComponent<ZB_RTS_Player>();

        Players.Remove(player); // remove players 

        base.OnServerDisconnect(conn);
    }

    public override void OnStopServer()
    {
        Players.Clear(); // clear players list 

        isGameStarted = false; 
    }

    public void StartGame()
    {
        if(Players.Count < 2)
        {
            return; 
        }

        isGameStarted = true;

        ServerChangeScene("ZB_Scene_Map_01");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        ZB_RTS_Player player = conn.identity.GetComponent<ZB_RTS_Player>();

        Players.Add(player); //add players 

        player.SetDisplayName($"Player {Players.Count}"); 

        player.SetTeamColor(new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f)));

        player.SetPartyOwner(Players.Count == 1); 
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        if (SceneManager.GetActiveScene().name.StartsWith("ZB_Scene_Map"))
        {
            ZB_GameOverHandler gameOverHandlerInstance = Instantiate(gameOverHandlerPrefab);
            NetworkServer.Spawn(gameOverHandlerInstance.gameObject);

            foreach(ZB_RTS_Player player in Players)
            {
                GameObject baseInstance = Instantiate(unitBasePrefab, GetStartPosition().position, Quaternion.identity);

                NetworkServer.Spawn(baseInstance, player.connectionToClient); 
            }
        }
    }
    #endregion

    #region Client 

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientOnConnected?.Invoke();
    }

    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        ClientOnDisconnected?.Invoke();
    }

    public override void OnStopClient()
    {
        Players.Clear(); 
    }

    #endregion
}