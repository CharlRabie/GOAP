using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour, IGoap
{
    public Inventory windmillInventory;
    private NavMeshAgent agent;
    private Inventory inventory;
    private Vector3 previousDestination;

    public void ActionsFinished()
    {
    }

    public HashSet<KeyValuePair<string, object>> CreateGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("doJob", true));

        return goal;
    }

    public HashSet<KeyValuePair<string, object>> GetWorldState()
    {
        HashSet<KeyValuePair<string, object>> worldData = new HashSet<KeyValuePair<string, object>>();
        worldData.Add(new KeyValuePair<string, object>("hasStock", (windmillInventory.flourLevel > 4)));
        worldData.Add(new KeyValuePair<string, object>("hasDelivery", (inventory.breadLevel > 4)));
        worldData.Add(new KeyValuePair<string, object>("hasFlour", (inventory.flourLevel > 1)));
        return worldData;
    }

    public bool MoveAgent(GoapAction nextAction)
    {
        //if we don't need to move anywhere
        if (previousDestination == nextAction.target.transform.position)
        {
            nextAction.SetInRange(true);
            return true;
        }

        agent.SetDestination(nextAction.target.transform.position);

        if (agent.hasPath && agent.remainingDistance < 2)
        {
            nextAction.SetInRange(true);
            previousDestination = nextAction.target.transform.position;
            return true;
        }
        else
            return false;
    }

    public void PlanAborted(GoapAction aborter)
    {
    }

    public void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal)
    {
    }

    public void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions)
    {
    }

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        inventory = this.GetComponent<Inventory>();
    }

    private void Update()
    {
        if (agent.hasPath)
        {
            Vector3 toTarget = agent.steeringTarget - this.transform.position;
            float turnAngle = Vector3.Angle(this.transform.forward, toTarget);
            agent.acceleration = turnAngle * agent.speed;
        }
    }
}