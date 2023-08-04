using System.Collections.Generic;
using Data;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class GameController : MonoBehaviour
    {
        public GameData GameData => gameData;
        
        [SerializeField] private GameData gameData;
        [SerializeField] private List<SpawnUnitData> spawnUnitsData;
        [SerializeField] private TMP_Text uiText;
        [SerializeField] private Button uiStartBattle;

        private List<UnitModel> _unitModels;

        private void Awake()
        {
            _unitModels = new List<UnitModel>(2);
            uiStartBattle.onClick.AddListener(StartBattle);
            uiText.text = "Set modules and weapons by clicking on the slot on the ship";
            gameData.Init();
            SpawnShips();
        }

        private void SpawnShips()
        {
            for (var i = 0; i < spawnUnitsData.Count; i++)
            {
                var spawnUnitData = spawnUnitsData[i];
                var newUnit = new UnitModel(spawnUnitData.unitData, spawnUnitData.spawnPoint, i + 1, gameData);
                spawnUnitData.ui.Connect(newUnit);
                newUnit.OnDie += OnUnitDie;
                _unitModels.Add(newUnit);
            }

            SetShieldColliders(false);
        }

        private void SetShieldColliders(bool value)
        {
            foreach (UnitModel unitModel in _unitModels)
            {
                unitModel.SetShieldCollider(value);
            }
        }

        private void OnUnitDie(UnitModel unit)
        {
            int winner = 0;
            foreach (var unitModel in _unitModels)
            {
                unitModel.StopFire();

                if (unitModel.IsAlive)
                {
                    winner = unitModel.Player;
                }
            }

            uiText.text = winner != 0 ? 
                $"Player{winner.ToString()} win" : 
                "War.... \nWar never changes...";
        }

        private void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private void StartBattle()
        {
            uiText.text = "";
            
            foreach (UnitModel unitModel in _unitModels)
            {
                SetShieldColliders(true);
                unitModel.Fire();
            }
            
            uiStartBattle.onClick.RemoveListener(StartBattle);
            uiStartBattle.onClick.AddListener(Restart);
            uiStartBattle.gameObject.GetComponentInChildren<TMP_Text>().text = "Restart";
        }
    }
}