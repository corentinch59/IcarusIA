using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;

namespace Icarus
{
    [TaskCategory("Icarus")]
    public class CanShootHit : Action
    {
        private SpaceShipView _icarusShip;
        private SpaceShipView _enemyShip;

        public override void OnAwake()
        {
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
            _enemyShip = gameObject.GetComponent<Icarus>().EnemyShip;
        }

        public override TaskStatus OnUpdate()
        {
            if (AimingHelpers.CanHit(_icarusShip, _enemyShip.Position, _enemyShip.Velocity, 0.15f))
            {
                return TaskStatus.Success;
            }

            return TaskStatus.Failure;
        }
    }
}
