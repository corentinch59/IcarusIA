using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus/Tasks")]
    public class IsEnemyHit : Action
    {
        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            if (_enemyShip.HitPenaltyCountdown <= 0)
            {
                return TaskStatus.Failure;
            }

            return TaskStatus.Success;
        }
    }
}

