using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class IsOnWaypoint : Conditional
    {
        public SharedVector2 targetPosition;
        public SharedFloat minimalDistance;

        private SpaceShipView _icarusShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        public override TaskStatus OnUpdate()
        {
            float distance = (targetPosition.Value - _icarusShip.Position).sqrMagnitude;

            return distance <= minimalDistance.Value ? TaskStatus.Success : TaskStatus.Failure;
        }
    }
}
