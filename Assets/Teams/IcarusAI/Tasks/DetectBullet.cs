using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class DetectBullet : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The distance at which the enemy is considered close")]
        public SharedFloat radius;

        public SharedBool takeShipRadius;

        private SpaceShipView _icarusShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            float computeRadius = takeShipRadius.Value ? radius.Value + _icarusShip.Radius : radius.Value;

            Collider2D hit = Physics2D.OverlapCircle(_icarusShip.Position, computeRadius);

            if (!hit)
            {
                return TaskStatus.Failure;
            }

            return hit.CompareTag("Bullet") ? TaskStatus.Success : TaskStatus.Failure;
        }

    }

}
