﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DoNotModify;
using BehaviorDesigner.Runtime;

namespace Icarus
{

	public class Icarus : BaseSpaceShipController
    {
        [SerializeField] private BehaviorTree _b;

        private SpaceShipView _otherSpaceship;

		public override void Initialize(SpaceShipView spaceship, GameData data)
		{
            base.Initialize(spaceship, data);
            _otherSpaceship = data.GetSpaceShipForOwner(1 - spaceship.Owner);
            _b.SetVariableValue("orientation", spaceship.Orientation);
        }

        public override InputData UpdateInput(SpaceShipView spaceship, GameData data)
		{
            //float targetOrient = spaceship.Orientation + 90.0f;
            //bool needShoot = AimingHelpers.CanHit(spaceship, otherSpaceship.Position, otherSpaceship.Velocity, 0.15f);
            float thrust = (float)_b.GetVariable("thrust").GetValue();
            float orientation = (float) _b.GetVariable("orientation").GetValue();
            bool shoot = (bool)_b.GetVariable("shoot").GetValue();
            bool dropMine = (bool) _b.GetVariable("dropMine").GetValue();
            bool fireShockwave = (bool) _b.GetVariable("fireShockwave").GetValue();
            return new InputData(thrust, orientation, shoot, dropMine, fireShockwave);
        }
	}

}
