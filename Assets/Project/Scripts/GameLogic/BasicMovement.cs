using Project.Scripts.GameLogic.Entity;
using UnityEngine;

namespace Project.Scripts.GameLogic
{
    public class BasicMovement: ObjectMovement
    {
        public BasicMovement(Rigidbody2D rb) : base(rb) { }
    }
}