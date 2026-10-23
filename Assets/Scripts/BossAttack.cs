using System.Collections;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    public bool IsRunning { get; private set; }

    public IEnumerator Run()
    {
        IsRunning = true;

        yield return ExecuteAttack();

        IsRunning = false;
    }

    protected abstract IEnumerator ExecuteAttack();

    public virtual void CancelAttack()
    {
        StopAllCoroutines();
        IsRunning = false;
    }
}