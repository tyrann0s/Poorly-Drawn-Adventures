using System.Collections.Generic;
using System.Threading.Tasks;
using Cards;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour, IService
    {
        public static ResourceManager Instance { get; private set; }

        public IconData Icons {get; private set;}
        private Dictionary<string, MobData> mobLookup;
        private Dictionary<string, ElementCombo> comboLookup;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            
            } 
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        public Task InitializeAsync()
        {
            Icons = Resources.Load<IconData>("Settings/Icons Data");
            CreateMobDB();
            CreateComboDB();
            return Task.CompletedTask;
        }
        
        private void CreateMobDB()
        {
            MobData[] allMobs = Resources.LoadAll<MobData>("Data/Mobs Data");
        
            mobLookup = new Dictionary<string, MobData>();
        
            foreach (var mob in allMobs)
            {
                mobLookup[mob.GetId()] = mob;
            }
        }
    
        private void CreateComboDB()
        {
            ElementCombo[] allCombos = Resources.LoadAll<ElementCombo>("Data/Card Combinations");
        
            comboLookup = new Dictionary<string, ElementCombo>();
        
            foreach (var combo in allCombos)
            {
                comboLookup[combo.GetId()] = combo;
            }
        }

        public MobData GetMobData(string id)
        {
            mobLookup.TryGetValue(id, out MobData mobSO);
            return mobSO;
        }

        public ElementCombo GetComboData(string id)
        {
            comboLookup.TryGetValue(id, out ElementCombo comboSO);
            return comboSO;
        }
    }
}
