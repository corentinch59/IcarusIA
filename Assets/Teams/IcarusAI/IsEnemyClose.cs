using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class IsEnemyClose : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance at which the enemy is considered close")]
        public SharedFloat distance;

        public SharedBool takeShipRadius;

        private SpaceShipView _icarusShip;
        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            float distanceSqr = distance.Value * distance.Value;
            if (takeShipRadius.Value)
                distanceSqr += _icarusShip.Radius * _icarusShip.Radius;

            float distanceFrom = (_enemyShip.Position - _icarusShip.Position).sqrMagnitude;


            if (distanceFrom <= distanceSqr)
            {
                Debug.Log("Success");
                return TaskStatus.Success;
            }

            Debug.Log("Fail");
            return TaskStatus.Failure;
        }

    }
}
