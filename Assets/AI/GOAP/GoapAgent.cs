using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class GoapAgent : MonoBehaviour
{
    private HashSet<GoapAction> availableActions;
    private Queue<GoapAction> currentActions;
    private IGoap dataProvider;
    private FSM.FSMState idleState;
    private FSM.FSMState moveToState;
    private FSM.FSMState performActionState;
    private GoapPlanner planner;
    private FSM stateMachine;

    public void AddAction(GoapAction a)
    {
        availableActions.Add(a);
    }

    public GoapAction GetAction(Type action)
    {
        foreach (GoapAction g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }

    public void RemoveAction(GoapAction action)
    {
        availableActions.Remove(action);
    }

    private void CreateIdleState()
    {
        idleState = (fsm, gameObj) =>
        {
            HashSet<KeyValuePair<string, object>> worldState = dataProvider.GetWorldState();
            HashSet<KeyValuePair<string, object>> goal = dataProvider.CreateGoalState();

            Queue<GoapAction> plan = planner.Plan(gameObject, availableActions, worldState, goal);
            if (plan != null)
            {
                currentActions = plan;
                dataProvider.PlanFound(goal, plan);

                fsm.popState();
                fsm.pushState(performActionState);
            }
            else
            {
                Debug.Log("Failed Plan: " + goal);
                dataProvider.PlanFailed(goal);
                fsm.popState();
                fsm.pushState(idleState);
            }
        };
    }

    private void CreateMoveToState()
    {
        moveToState = (fsm, gameObj) =>
        {
            GoapAction action = currentActions.Peek();
            if (action.RequiresInRange() && action.target == null)
            {
                Debug.Log("Fatal error: Action requires a target but has none. Planning failed. You did not assign the target in your Action.checkProceduralPrecondition()");
                fsm.popState();
                fsm.popState();
                fsm.pushState(idleState);
                return;
            }

            Debug.Log("Move to do: " + action.Name);
            if (dataProvider.MoveAgent(action))
            {
                fsm.popState();
            }
        };
    }

    private void CreatePerformActionState()
    {
        performActionState = (fsm, gameObj) =>
        {
            if (!HasActionPlan())
            {
                Debug.Log("<color=red>Done actions</color>");
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.ActionsFinished();
                return;
            }

            GoapAction action = currentActions.Peek();
            if (action.isDone())
            {
                currentActions.Dequeue();
            }

            if (HasActionPlan())
            {
                action = currentActions.Peek();
                bool inRange = action.RequiresInRange() ? action.IsInRange() : true;

                if (inRange)
                {
                    bool success = action.Perfom(gameObj);

                    if (!success)
                    {
                        fsm.popState();
                        fsm.pushState(idleState);
                        dataProvider.PlanAborted(action);
                    }
                }
                else
                {
                    fsm.pushState(moveToState);
                }
            }
            else
            {
                fsm.popState();
                fsm.pushState(idleState);
                dataProvider.ActionsFinished();
            }
        };
    }

    private void FindDataProvider()
    {
        foreach (Component comp in gameObject.GetComponents(typeof(Component)))
        {
            if (typeof(IGoap).IsAssignableFrom(comp.GetType()))
            {
                dataProvider = (IGoap)comp;
                return;
            }
        }
    }

    private bool HasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void LoadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        foreach (GoapAction a in actions)
        {
            availableActions.Add(a);
        }
        Debug.Log("Found actions: " + actions);
    }

    private void Start()
    {
        stateMachine = new FSM();
        availableActions = new HashSet<GoapAction>();
        currentActions = new Queue<GoapAction>();
        planner = new GoapPlanner();
        FindDataProvider();
        CreateIdleState();
        CreateMoveToState();
        CreatePerformActionState();
        stateMachine.pushState(idleState);
        LoadActions();
    }

    private void Update()
    {
        stateMachine.Update(this.gameObject);
    }
}