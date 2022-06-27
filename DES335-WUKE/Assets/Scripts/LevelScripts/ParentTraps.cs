using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentTraps : MonoBehaviour
{
    [SerializeField]
    protected bool disappearOnTrigger;
    [SerializeField]
    protected float retriggerDuration;
    [SerializeField]
    protected float trapEffectDuration;
    
    //All damage values will be in percentage
    [SerializeField]
    protected float trapPlayerDamage;
    [SerializeField]
    protected float trapEnemyDamage;
}
