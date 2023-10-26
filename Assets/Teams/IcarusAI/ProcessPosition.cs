using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using UnityEngine;
using UnityEngine.Playables;

namespace Icarus
{
    [TaskCategory("Icarus")]
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
            orientation.Value = AimingHelpers.ComputeSteeringOrient(_icarusShip, position.Value);

            return TaskStatus.Success;
        }
    }

}
