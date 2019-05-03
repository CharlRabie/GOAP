using UnityEngine;

public class Harvest : GoapAction
{
    private bool completed = false;
    private float startTime = 0;
    public float workDuration = 2; //seconds

    public Harvest()
    {
        AddEffect("hasWheat", true);
        Name = "Harvest";
    }

    public override void reset()
    {
        completed = false;
        startTime = 0;
    }

    public override bool isDone()
    {
        return completed;
    }

    public override bool RequiresInRange()
    {
        return true;
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override bool Perfom(GameObject agent)
    {
        if (startTime == 0)
        {
            Debug.Log($"Starting: {Name}");
            startTime = Time.time;
        }

        if (Time.time - startTime > workDuration)
        {
            Debug.Log($"Finished: { Name}");
            completed = true;
        }
        return true;
    }
}