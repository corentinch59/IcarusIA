using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine.Playables;

namespace Icarus
{
    [TaskCategory("Icarus/Tasks")]
    public class IsEnemyClose : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance at which the enemy is considered close")]
        public SharedFloat distance;

        private SpaceShipView _icarusShip;
        private SpaceShipView _enemyShip;
        private GameData _gameData;

        public override void OnAwake()
        {
            _gameData = gameObject.GetComponent<Icarus>().GameData;
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            float distanceSqr = distance.Value * distance.Value;
            float distanceFrom = (_enemyShip.Position - _icarusShip.Position).sqrMagnitude;

            if (distanceSqr <= distanceFrom)
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }

    }
}
