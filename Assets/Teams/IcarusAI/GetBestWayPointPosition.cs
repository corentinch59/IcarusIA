using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DoNotModify;
using System.Collections.Generic;
using System.Linq;

namespace Icarus
{
    [TaskCategory("Icarus/Tasks")]
    public class GetBestWayPointPosition : Action
    {
        [BehaviorDesigner.Runtime.Tasks.Tooltip("The position that will get updated")]
        public SharedVector2 position;

        private GameData _gameData;
        private SpaceShipView _icarusShip;

        // OnAwake is called once when the behavior tree is enabled. Think of it as a constructor.
        public override void OnAwake()
        {
            _gameData = gameObject.GetComponent<Icarus>().GameData;
            _icarusShip = gameObject.GetComponent<Icarus>().IcarusShip;
        }

        // OnUpdate runs the actual task.
        public override TaskStatus OnUpdate()
        {
            Dictionary<WayPointView, float> scores = new Dictionary<WayPointView, float>();
            foreach (var waypoint in _gameData.WayPoints)
            {
                Vector2 distanceShipFromWaypoint = waypoint.Position - _icarusShip.Position;

                float distance = distanceShipFromWaypoint.sqrMagnitude;
                int mask = LayerMask.NameToLayer("Asteroid");

                RaycastHit2D hit = Physics2D.Raycast(_icarusShip.Position, distanceShipFromWaypoint, distanceShipFromWaypoint.sqrMagnitude, mask);

                if (hit)
                {
                    Vector2 ShipToCircle = hit.point - _icarusShip.Position;

                    Vector2 CtoA = hit.point -
                                   new Vector2(hit.transform.position.x, hit.transform.position.y);
                    Vector2 CtoB = waypoint.Position - new Vector2(hit.transform.position.x, hit.transform.position.y);

                    float AsteroidRadius = hit.transform.gameObject.GetComponent<AsteroidView>().Radius;
                    float circleDistancePerimeter = Vector2.Angle(CtoA, CtoB) * Mathf.Deg2Rad * AsteroidRadius;

                    distance = ShipToCircle.sqrMagnitude + CtoB.sqrMagnitude - AsteroidRadius + circleDistancePerimeter;
                }

                float value = 1;
                switch (waypoint.Owner)
                {
                    case -1:
                        value = 2;
                        break;
                    case 0:
                        value = waypoint.Owner == _icarusShip.Owner ? 0 : 1;
                        break;
                    case 1:
                        value = waypoint.Owner == _icarusShip.Owner ? 0 : 1;
                        break;
                }
                float score = distance * value;
                scores.Add(waypoint, score);
            }

            WayPointView bestWayPoint = scores
                .Where(x => x.Value > 0)
                .OrderBy(x => x.Value)
                .FirstOrDefault().Key;


#if UNITY_EDITOR
            foreach (var waypoint in _gameData.WayPoints)
            {
                Color lineColor = (waypoint == bestWayPoint) ? Color.green : Color.red;
                Debug.DrawLine(_icarusShip.Position, waypoint.Position, lineColor);
            }
#endif

            position = bestWayPoint.Position;
            return TaskStatus.Success;
        }
    }

}
