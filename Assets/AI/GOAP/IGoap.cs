using System.Collections.Generic;

public interface IGoap
{
    void ActionsFinished();

    HashSet<KeyValuePair<string, object>> CreateGoalState();

    HashSet<KeyValuePair<string, object>> GetWorldState();

    bool MoveAgent(GoapAction nextAction);

    void PlanAborted(GoapAction aborter);

    void PlanFailed(HashSet<KeyValuePair<string, object>> failedGoal);

    void PlanFound(HashSet<KeyValuePair<string, object>> goal, Queue<GoapAction> actions);
}