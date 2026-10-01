using RPG.Combat.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace RPG.Combat
{
    public class SpectatorManager : MonoBehaviour
    {
        private Dictionary<int, List<SpectatorHandler>> _spectatorsDictionary = new Dictionary<int, List<SpectatorHandler>>();
        private List<SpectatorHandler> _spectatorList = new List<SpectatorHandler>();

        private const float MIN_DISTANCE_TO_SECTION = 5;

        private void Start()
        {
            PopulateList();
            CalculateSpectatorSectionInCircle();
        }

        private void OnEnable()
        {
            ActionsManager.Instance.OnApresentadorDamageTaken += HandleApresentadorDamage;
            ActionsManager.Instance.OnCombatLost += HandleApresentadorDeath;
            ActionsManager.Instance.OnEnemyDamageTaken += HandleEnemyDamage;
            ActionsManager.Instance.OnEnemyDefeated += HandleEnemyDeath;
            ActionsManager.Instance.OnCharacterDamageTaken += HandleCharacterDamage;
            ActionsManager.Instance.OnCharacterDefeated += HandleCharacterDeath;
            ActionsManager.Instance.OnPreviewTileSelected += Debug;
        }

        private void OnDisable()
        {
            ActionsManager.Instance.OnApresentadorDamageTaken -= HandleApresentadorDamage;
            ActionsManager.Instance.OnCombatLost -= HandleApresentadorDeath;
            ActionsManager.Instance.OnEnemyDamageTaken -= HandleEnemyDamage;
            ActionsManager.Instance.OnEnemyDefeated -= HandleEnemyDeath;
            ActionsManager.Instance.OnCharacterDamageTaken -= HandleCharacterDamage;
            ActionsManager.Instance.OnCharacterDefeated -= HandleCharacterDeath;
            ActionsManager.Instance.OnPreviewTileSelected -= Debug;
        }

        private void Debug(Vector2Int tilePosition)
        {
            //ChangeSectionPoses(tilePosition.x, SpectatorHandler.PosesEnum.Cheer);
            //StopAllCoroutines();
            //StartCoroutine(WaveCoroutine());
        }

        private IEnumerator WaveCoroutine()
        {
            for (int i = 0; i < Map.Columns; i++)
            {
                for (int j = 0; j < _spectatorsDictionary[i].Count; j++)
                {
                    SpectatorHandler spectator = _spectatorsDictionary[i][j];
                    spectator.ChangePose(SpectatorHandler.PosesEnum.Cheer);
                    //yield return new WaitForSeconds(0.0001f);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }

        #region Handlers

        private void HandleApresentadorDamage()
        {
            ChangeAllSectionsPoses(SpectatorHandler.PosesEnum.Boo);
        }

        private void HandleApresentadorDeath()
        {
            ChangeAllSectionsPoses(SpectatorHandler.PosesEnum.Boo);
        }

        private void HandleEnemyDamage(EnemyController enemy)
        {
            ChangeSectionPoses(enemy.Position.x, SpectatorHandler.PosesEnum.Cheer);
        }

        private void HandleEnemyDeath(EnemyController enemy)
        {
            ChangeAllSectionsPoses(SpectatorHandler.PosesEnum.Cheer);
        }

        private void HandleCharacterDamage(CharacterController character)
        {
            ChangeSectionPoses(character.Position.x, SpectatorHandler.PosesEnum.Boo);
        }

        private void HandleCharacterDeath(CharacterController character)
        {
            ChangeAllSectionsPoses(SpectatorHandler.PosesEnum.Boo);
        }

        #endregion

        private void ChangeAllSectionsPoses(SpectatorHandler.PosesEnum newPose)
        {
            for(int i = 0; i < Map.Columns; i++)
            {
                ChangeSectionPoses(i, newPose);
            }
        }

        private void ChangeSectionPoses(int sectionIndex, SpectatorHandler.PosesEnum newPose)
        {
            foreach (SpectatorHandler spectator in _spectatorsDictionary[sectionIndex])
            {
                spectator.ChangePose(newPose);
            }
        }

        private void PopulateList()
        {
            SpectatorHandler[] spectators = GetComponentsInChildren<SpectatorHandler>();
            for (int i = 0; i < spectators.Length; i++)
            {
                _spectatorList.Add(spectators[i]);
            }
        }

        private void CalculateSpectatorSectionInCircle()
        {
            int totalSections = Map.Columns;
            int spectatorsCount = _spectatorList.Count;

            for(int i = 0; i < totalSections; i++)
            {
                if (!_spectatorsDictionary.ContainsKey(i))
                {
                    _spectatorsDictionary[i] = new List<SpectatorHandler>();
                }

                Vector3 tilePosition = MapManager.Instance.GetWorldPosition(new Vector2Int(i, Map.Rows - 1));
                tilePosition.y = 0;

                foreach(SpectatorHandler spectator in _spectatorList)
                {
                    Vector3 spectatorPosition = spectator.transform.position;
                    spectatorPosition.y = 0;
                    spectatorPosition = spectatorPosition.normalized * tilePosition.magnitude;

                    if(Vector3.Distance(tilePosition, spectatorPosition) < MIN_DISTANCE_TO_SECTION)
                    {
                        _spectatorsDictionary[i].Add(spectator);
                    }
                }
            }
        }
    }
}
