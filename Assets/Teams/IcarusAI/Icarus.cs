using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;

namespace Icarus
{

	public class Icarus : BaseSpaceShipController
    {
        [SerializeField] private BehaviorTree _b;
        public GameData GameData { get; private set; }
        public SpaceShipView IcarusShip { get; private set; }
        public SpaceShipView EnemyShip { get; private set; }

        public override void Initialize(SpaceShipView spaceship, GameData data)
		{
            base.Initialize(spaceship, data);
            GameData = data;
            IcarusShip = spaceship;
            EnemyShip = data.GetSpaceShipForOwner(1 - spaceship.Owner);
            _b.SetVariableValue("orientation", spaceship.Orientation);
        }

        public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
            //bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
            // Set Variables of the blackboard
            _b.SetVariableValue("energy", spaceship.Energy);

            // Get Variables from blackboard
            float thrust = (float)_b.GetVariable("thrust").GetValue();
            float orientation = (float) _b.GetVariable("orientation").GetValue();
            bool shoot = (bool)_b.GetVariable("shoot").GetValue();
            if(shoot)
                _b.SetVariableValue("shoot", false);
            bool dropMine = (bool) _b.GetVariable("dropMine").GetValue();
            if(dropMine)
                _b.SetVariableValue("dropMine", false);
            bool fireShockwave = (bool) _b.GetVariable("fireShockwave").GetValue();
            if(fireShockwave)
                _b.SetVariableValue("fireShockwave", false);
            return new InputData(thrust, orientation, shoot, dropMine, fireShockwave);
        }
        
	}

}
