using UnityEngine;

public enum CharacterState
{
    Idle,
    Move,
    Attack,
    Dead
}

public abstract class CharacterBase : MonoBehaviour
{
    public float MaxHP { get; protected set; }
    public float CurrentHP { get; protected set; }
    public float ATK { get; protected set; }

    public Vector3 Position => transform.position;
    public CharacterState CurrentState { get; protected set; }

    protected virtual void InitializeStats(float maxHP, float atk)
    {
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        ATK = atk;
        CurrentState = CharacterState.Idle;
    }

    public virtual void TakeDamage(float damage)
    {
        if (CurrentState == CharacterState.Dead) return;

        CurrentHP -= damage;

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            Die();
        }
    }

    public abstract void Attack(CharacterBase target);

    public virtual void Die()
    {
        CurrentState = CharacterState.Dead;
    }
}