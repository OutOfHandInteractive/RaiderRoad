using UnityEngine;
using System.Collections.Generic;

public class RandomChoice<E>
{
    private Dictionary<E, float> _probmap = new Dictionary<E, float>();
    private E _default;

    public RandomChoice(E def)
    {
        this._default = def;
    }

    public void SetChance(E obj, float prob)
    {
        Dictionary<E, float> tmp = new Dictionary<E, float>(_probmap);
        tmp.Remove(obj);
        float sum = 0f;
        foreach(float p in tmp.Values)
        {
            sum += p;
        }
        if(sum + prob > 1f)
        {
            throw new System.Exception("Probabilities cannot add up to more than 1!");
        }
        _probmap[obj] = prob;
    }

    public E Choose(E def)
    {
        float f = Random.value;
        foreach(E e in _probmap.Keys)
        {
            float p = _probmap[e];
            if(f <= p)
            {
                return e;
            }
            f -= p;
        }
        return def;
    }

    public E Choose()
    {
        return Choose(_default);
    }
}
