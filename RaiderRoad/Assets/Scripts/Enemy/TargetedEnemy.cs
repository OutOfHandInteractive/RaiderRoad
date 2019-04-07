using UnityEngine;
using System.Linq;
using System.Collections;

/// <summary>
/// Class for states that target a particular object. Once selected it will stay targeted as long as it is a valid target.
/// </summary>
public abstract class TargetedEnemy : EnemyAIState
{
    [SerializeField] private GameObject _target;

    /// <summary>
    /// Get the target. Should be called at least once per frame
    /// </summary>
    /// <returns>Gets the current target</returns>
    public GameObject GetTarget()
    {
        if (!IsValidTarget(_target))
        {
            foreach (string tag in TargetedTags())
            {
                _target = Closest(from obj in GameObject.FindGameObjectsWithTag(tag) where IsValidTarget(obj) && SearchFilter(obj) select obj);
                if(_target != null)
                {
                    break;
                }
            }
        }
        return _target;
    }

    /// <summary>
    /// Manually set the target
    /// </summary>
    /// <param name="target">The target to set</param>
    protected void SetTarget(GameObject target)
    {
        _target = target;
    }

    /// <summary>
    /// Callback for filtering search results when looking for targets. Returns true by default
    /// </summary>
    /// <param name="obj">The object to test. Guaranteed to be a valid target</param>
    /// <returns>true if the object should be included in the search</returns>
    protected virtual bool SearchFilter(GameObject obj)
    {
        return true;
    }

    /// <summary>
    /// Return the tags of objects you're looking for. Should be in descending order of priority
    /// </summary>
    /// <returns>The tags to look for</returns>
    protected abstract string[] TargetedTags();

    /// <summary>
    /// Callback for testing if an object is a valid target
    /// </summary>
    /// <param name="obj">The object to test</param>
    /// <returns>True if the target is valid</returns>
    protected abstract bool IsValidTarget(GameObject obj);
}
