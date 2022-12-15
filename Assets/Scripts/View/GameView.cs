using System.Collections.Generic;
using Controller;
using Data;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using View.UI;

namespace View
{
    /// <summary>
    /// TEMP CODE!!!!
    /// </summary>
    public class GameView : MonoBehaviour
    {
        public GameData GameData => gameData;
        
        [SerializeField] private GameData gameData;
        
        [SerializeField] private Transform sheep1SpawnPoint;
        [SerializeField] private Transform sheep2SpawnPoint;

        [SerializeField] private UnitData ship1;
        [SerializeField] private UnitData ship2;
        
        [SerializeField] private UISheepInfo uiShip1;
        [SerializeField] private UISheepInfo uiShip2;

        [SerializeField] private TMP_Text uiText;
        [SerializeField] private Button uiStartBattle;
        
        // todo remove this
        private UnitFabric _unitFabric;

        private List<UnitModel> _unitModels;

        private void Awake()
        {
            _unitFabric = new UnitFabric(gameData);
            _unitModels = new List<UnitModel>(2);
            uiStartBattle.onClick.AddListener(StartBattle);
            uiText.text = "";
            gameData.Init();
            SpawnShips();
        }

        private void SpawnShips()
        {
            var unit1 = _unitFabric.SpawnUnit(ship1, sheep1SpawnPoint, 1);
            var unit2 = _unitFabric.SpawnUnit(ship2, sheep2SpawnPoint, 2);

            _unitModels.Add(unit1);
            _unitModels.Add(unit2);
            
            uiShip1.Connect(unit1);
            uiShip2.Connect(unit2);
            
            unit1.OnDie += OnUnitDie;
            unit2.OnDie += OnUnitDie;

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
                    unitModel.RecoverShield();
                }
            }

            uiText.text = $"Player{winner.ToString()} win";
        }
        
        

        private void Restart()
        {
            SceneManager.LoadScene(0);
        }

        private void StartBattle()
        {
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