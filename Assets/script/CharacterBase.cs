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
    //상속으로 구현하느라 지워둠
    //public float ATK { get; protected set; }

    public Vector3 Position => transform.position;
    public CharacterState CurrentState { get; protected set; }

    protected virtual void InitializeStats(float maxHP, float atk)
    {
        MaxHP = maxHP;
        CurrentHP = MaxHP;
        // 상속으로 구현하느라 지워둠
        //ATK = atk;
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
    // 상속으로 구현하느라 지워둠
    //public abstract void Attack(CharacterBase target);

    protected virtual void Die()
    {
        CurrentState = CharacterState.Dead;
    }

    public virtual void Heal(int amount)
    {
        if (CurrentState == CharacterState.Dead) return;

        CurrentHP += amount;
        if (CurrentHP > MaxHP)
        {
            CurrentHP = MaxHP;
        }
    }
}