using UnityEngine;

namespace Project.Scripts.GameLogic.Entity
{
    public abstract class ObjectMovement
    {
        protected readonly Rigidbody2D _rb;
        
        protected ObjectMovement(Rigidbody2D rb) =>
            _rb = rb;

        public virtual void Move(Vector2 dir, float speed) =>
            _rb.MovePosition(_rb.position + dir * (Time.deltaTime * speed));
    }
}