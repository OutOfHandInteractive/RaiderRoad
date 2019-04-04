using UnityEngine;
using System.Collections;

public abstract class EnemyType : MonoBehaviour
{
    public abstract RandomChoice<StatefulEnemyAI.State> BoardingChooser();
}
