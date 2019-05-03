using UnityEngine;

public class PickUpFlour : GoapAction
{
    private bool completed = false;
    private float startTime = 0;
    public float workDuration = 2; //seconds
    public Inventory windmillInventory;

    public PickUpFlour()
    {
        AddPrecondition("hasStock", true);
        AddPrecondition("hasFlour", false);
        AddEffect("doJob", true);
        Name = "PickUpFlour";
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
            if (windmillInventory.flourLevel >= 5)
            {
                GetComponent<Inventory>().flourLevel += 5;
                windmillInventory.flourLevel -= 5;
            }
            completed = true;
        }
        return true;
    }
}