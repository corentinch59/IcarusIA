using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class CanHitImmobile : Conditional
    {
        public SharedVector2 immobilePosition;
        public SharedFloat angleTolerance;

        private SpaceShipView _icarusShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            return AimingHelpers.CanHit(_icarusShip, immobilePosition.Value, angleTolerance.Value) ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
