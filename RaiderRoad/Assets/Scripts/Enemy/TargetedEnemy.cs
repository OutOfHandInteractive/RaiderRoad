using UnityEngine;
using System.Linq;
using System.Collections;

public abstract class TargetedEnemy : EnemyAIState
{
    private GameObject _target;
    public GameObject GetTarget()
    {
        if (!IsValidTarget(_target))
        {
            foreach (string tag in TargetedTags())
            {
                _target = Closest(from obj in GameObject.FindGameObjectsWithTag(tag) where IsValidTarget(obj) select obj);
                if(_target != null)
                {
                    break;
                }
            }
        }
        return _target;
    }

    protected abstract string[] TargetedTags();
    protected abstract bool IsValidTarget(GameObject obj);
}
