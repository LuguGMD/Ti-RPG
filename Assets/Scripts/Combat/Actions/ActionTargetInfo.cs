using UnityEngine;

namespace RPG.Combat.Actions
{
    [System.Serializable]
    public class ActionTargetInfo
    {
        public EnemyController User;
        public CharacterController Target;
        public Vector2Int TargetDirection;
        public float Damage;

        public ActionTargetInfo(EnemyController user, CharacterController target, Vector2Int targetDirection, float damage)
        {
            User= user;
            Target= target;
            TargetDirection = targetDirection;

            bool hasSpotlightEffect = user.TileObject.IsOnSpotlight;
            Damage = hasSpotlightEffect ? damage : damage*2;
        }

        #region Properties

        public float TargetHealth { get { return Target.CurrentMotivation; } }
        public CombatAction Action { get { return User.Actions[User.SelectedActionIndex]; } }

        #endregion
    }
}
