using RPG.Combat.Preview;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat.Actions
{
    public abstract class SuperHandler : CombatAction
    {
        protected int _upgradeTier = 0;
        protected int _chargeAmount = 1 /*10*/;

        #region Properties

        public int UpgradeTier { get { return _upgradeTier; }  }
        public int ChargeAmount { get { return _chargeAmount; } }

        #endregion

        public void SetUpgradeTier(int stage)
        {
            _upgradeTier = stage;
        }
    }
}
