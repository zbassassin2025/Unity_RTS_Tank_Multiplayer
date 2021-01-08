using Mirror;
using UnityEngine.AI;
using UnityEngine;

public class ZB_UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private ZB_Targeting targeting = null;
    [SerializeField] private float chaseRange = 10;

    #region Server

    public override void OnStartServer()
    {
        ZB_GameOverHandler.ServerOnGameOver += ServerHandleGameOver;
    }

    public override void OnStopServer()
    {
        ZB_GameOverHandler.ServerOnGameOver -= ServerHandleGameOver;
    }

    [ServerCallback]
    private void Update()
    {
        ZB_Target target = targeting.GetTarget(); 

        if(target != null)
        {
            if((target.transform.position - transform.position).sqrMagnitude > chaseRange * chaseRange) // more efficient than calculating Vector3 sqroots 
            {
                // chase 
                agent.SetDestination(target.transform.position);
            }
            else if(agent.hasPath) 
            {
                // stop chasing 
                agent.ResetPath(); 
            }

            return;
        }

        if(agent.hasPath)
        {
            return; 
        }

        if(agent.remainingDistance > agent.stoppingDistance)
        {
            return; 
        }

        agent.ResetPath(); 
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        ServerMove(position); 
    }

    [Server]
    public void ServerMove(Vector3 position)
    {
        targeting.ClearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) // if not valid position
        {
            return;
        }

        agent.SetDestination(hit.position);
    }

    [Server]
    private void ServerHandleGameOver()
    {
        agent.ResetPath(); 
    }

    #endregion
}