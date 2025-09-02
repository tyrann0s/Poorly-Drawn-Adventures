using System;
using UnityEngine;

namespace UI.Menu
{
    public class Menu : MonoBehaviour
    {
        private Journal journal;
        
        private void Start()
        {
            journal = GetComponentInParent<Journal>();
        }

        public void Resume()
        {
            journal.HideMenu();
        }
    }
}
