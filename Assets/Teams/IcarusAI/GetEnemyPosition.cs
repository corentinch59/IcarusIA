using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class GetEnemyPosition : Action
    {
        public SharedVector2 position;

        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            position.Value = _enemyShip.Position;

            return TaskStatus.Success;
        }
    }

}
