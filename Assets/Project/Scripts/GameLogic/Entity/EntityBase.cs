using System;
using UnityEngine;

namespace Project.Scripts.GameLogic.Entity
{
    public abstract class EntityBase<T>: MonoBehaviour, IHealth<T> where T: EntityBase<T>
    {
        private int _maxHealth;
        private float _currentHealth;

        public int MaxHealth
        {
            get => _maxHealth;
            private set => _maxHealth = Mathf.Max(1, value); 
        }
        public float CurrentHealth
        {
            get => _currentHealth;
            protected set
            {
                var baseHealth = _currentHealth;
                _currentHealth = Mathf.Clamp(value, 0, MaxHealth); 
                OnHealthChange?.Invoke(CreateHealthChangeArgs(baseHealth,CurrentHealth,transform.position));
            }
        }
        public T Object => (T)this;
        
        public event Action<OnHealthChangeArgs<T>> OnHealthChange;

        private OnHealthChangeArgs<T> CreateHealthChangeArgs(float previousHealth, float currentHealth, Vector3 position)
        {
            var type = HeathChangeType.Damage;
            if (currentHealth > previousHealth)
                type = HeathChangeType.Heal;
            else if (currentHealth == 0)
                type = HeathChangeType.Death;
            return new OnHealthChangeArgs<T>
            {
                Object = (T)this,
                Value = currentHealth,
                Position = position,
                Type = type
            };
        }

        public virtual void TakeDamage(float dmg)
        {
            CurrentHealth -= dmg;
            Debug.Log($"{gameObject.name} takes <color=red>{dmg}</color> damage | {CurrentHealth}/{MaxHealth}");
        }
        
        public virtual void Heal(float heal)
        {
            CurrentHealth += heal;
            //Debug.Log($"{gameObject.name} heals <color=green>{heal}</color> | {CurrentHealth}/{MaxHealth}");
        }
        
        public virtual void IncreaseMaxHealth(int value)
        {
            var relative = CurrentHealth / MaxHealth;
            var health = MaxHealth + value;
            if (health < 0)
                MaxHealth = 10;
            else
                MaxHealth += value;
            CurrentHealth = MaxHealth * relative;
            Debug.Log($"{gameObject.name} max health increased by <color=blue>{value}</color> | {CurrentHealth}/{MaxHealth}");
        }
        
        protected void SetInitialHealth(int maxHealth)
        {
            if (maxHealth <= 0) throw new ArgumentException("MaxHealth must be positive.");
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}