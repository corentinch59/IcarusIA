using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class IsEnemyStun : Action
    {
        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            if (_enemyShip.StunPenaltyCountdown <= 0)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}

