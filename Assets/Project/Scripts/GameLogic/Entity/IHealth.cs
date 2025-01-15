using System;
using UnityEngine;

namespace Project.Scripts.GameLogic.Entity
{
    public interface IHealth<T>
    {
        public int MaxHealth { get; }
        public float CurrentHealth { get; }
        public T Object { get; }
        
        public event Action<OnHealthChangeArgs<T>> OnHealthChange; 

        public void TakeDamage(float dmg);
        public void Heal(float heal);
        public void IncreaseMaxHealth(int value);
    }

    public struct OnHealthChangeArgs<T>
    {
        public T Object;
        public float Value;
        public Vector3 Position;
        public HeathChangeType Type;
    }

    public enum HeathChangeType
    {
        Damage,
        Death,
        Heal
    }
}