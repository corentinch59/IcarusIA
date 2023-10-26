using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using UnityEngine.Playables;

namespace Icarus
{
    [TaskCategory("Icarus/Tasks")]
    public class ProcessPosition : Action
    {
        public SharedVector2 position;

        [BehaviorDesigner.Runtime.Tasks.Tooltip("The orientation of the ship")]
        public SharedFloat orientation;

        private SpaceShipView _icarusShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            Vector2 direction = position.Value - _icarusShip.Position;
            float angle = Vector2.SignedAngle(_icarusShip.LookAt, direction);

            orientation.Value = angle;

            return TaskStatus.Success;
        }
    }

}
