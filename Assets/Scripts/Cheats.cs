using Lugu.Singleton;
using RPG.Combat;
using RPG.Level;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = RPG.Combat.CharacterController;

namespace RPG
{
    public class Cheats : SingletonMonoPersistent<Cheats>
    {
        // Update is called once per frame
        void Update()
        {
            if(UnityEngine.Input.GetKeyDown(KeyCode.F1))
            {
                foreach (CharacterScriptable character in GameManager.AvailableCharacters)
                {
                    GameManager.Instance.CharacterMotivated(character);
                    GameManager.Instance.Save();
                }

                if (CombatManager.Instance == null) return;

                List<CharacterController> characters = CombatManager.RemainingCharacters;
                foreach (CharacterController character in characters)
                {
                    character.Heal(10);
                }

                CombatManager.Apresentador.Heal(10);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.F2))
            {
                foreach (CharacterScriptable character in GameManager.AvailableCharacters)
                {
                    GameManager.Instance.CharacterDemotivated(character);
                    GameManager.Instance.Save();
                }

                if (CombatManager.Instance == null) return;

                List<CharacterController> characters = CombatManager.RemainingCharacters;
                foreach (CharacterController character in characters)
                {
                    character.TakeDamage(10);
                }
                
                CombatManager.Apresentador.TakeDamage(10);
            }
            if(UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                WorldNavigationManager levelManager = GameObject.FindAnyObjectByType<WorldNavigationManager>();
                if (levelManager != null)
                {
                    levelManager.UnlockAllLevels();
                }
            }

            if(UnityEngine.Input.GetKeyDown(KeyCode.F4))
            {
                GameManager.Instance.AddCoins(50);
            }
        }
    }
}
