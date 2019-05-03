using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GoapAgent))]
public abstract class GoapAction : MonoBehaviour
{
    /// <summary>
    /// A cost to perform the action.
    /// </summary>
    public float Cost = 1f;

    public string Name = "No Name";

    /// <summary>
    /// A target that the action needs to be performed on.
    /// </summary>
    public GameObject target;

    private HashSet<KeyValuePair<string, object>> effects;
    private bool inRange = false;
    private HashSet<KeyValuePair<string, object>> preconditions;

    public GoapAction()
    {
        preconditions = new HashSet<KeyValuePair<string, object>>();
        effects = new HashSet<KeyValuePair<string, object>>();
    }

    public HashSet<KeyValuePair<string, object>> Effects
    {
        get
        {
            return effects;
        }
    }

    public HashSet<KeyValuePair<string, object>> Preconditions
    {
        get
        {
            return preconditions;
        }
    }

    public void AddEffect(string key, object value)
    {
        effects.Add(new KeyValuePair<string, object>(key, value));
    }

    public void AddPrecondition(string key, object value)
    {
        preconditions.Add(new KeyValuePair<string, object>(key, value));
    }

    /// <summary>
    /// Procedurally check if action can be run. Not all action may need this.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public abstract bool CheckProceduralPrecondition(GameObject agent);

    public void DoReset()
    {
        inRange = false;
        reset();
    }

    /// <summary>
    /// Indicates whether or not the action has been completed.
    /// </summary>
    /// <returns></returns>
    public abstract bool isDone();

    /// <summary>
    /// Is the target in range? This is managed by the MoveTo state.
    /// </summary>
    /// <returns></returns>
    public bool IsInRange()
    {
        return inRange;
    }

    /// <summary>
    ///Performs the action.
    ///Returns True if the action performed successfully, if the method returns false the goal cannot be reached and the action queue should clear out.
    /// </summary>
    /// <param name="agent"></param>
    /// <returns></returns>
    public abstract bool Perfom(GameObject agent);

    public void RemoveEffect(string key)
    {
        KeyValuePair<string, object> remove = default;
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            effects.Remove(remove);
    }

    public void RemovePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in preconditions)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            preconditions.Remove(remove);
    }

    public abstract bool RequiresInRange();

    /// <summary>
    /// Reset variables before new plan.
    /// </summary>
    public abstract void reset();

    /// <summary>
    /// Indicates wheter or not a target need to be in range of a a target in order to be executed. If enabled the MoveTo state will execute if not in range.
    /// </summary>
    /// <returns></returns>
    public void SetInRange(bool inRange)
    {
        this.inRange = inRange;
    }
}