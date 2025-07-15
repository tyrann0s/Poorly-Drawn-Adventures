using System.Collections.Generic;
using Cards;
using Mobs;
using UnityEngine;

namespace Managers
{
    public class ResourceManager : MonoBehaviour
    {
        private static ResourceManager instance;
        public static ResourceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<ResourceManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("ResourceManager");
                        instance = go.AddComponent<ResourceManager>();
                    }
                }
                return instance;
            }
        }

        public IconData Icons {get; private set;}
        private Dictionary<string, MobData> mobLookup;
        private Dictionary<string, ElementCombo> comboLookup;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
            
            Icons = Resources.Load<IconData>("Settings/Icons Data");
            CreateMobDB();
            CreateComboDB();
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
