using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class IsEnemyBehind : Action
    {
        private SpaceShipView _icarusShip;
        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 enemyDirection = _enemyShip.Position - _icarusShip.Position;
            float dot = Vector2.Dot(_icarusShip.LookAt, enemyDirection);

            if (dot <= 0)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }

}
