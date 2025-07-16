using Managers.Base;
using UnityEngine;

namespace Base.UI
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public void ShowScreen()
        {
            gameObject.SetActive(true);
            UpdateScreen();
        }

        public void HideScreen()
        {
            gameObject.SetActive(false);       
        }

        protected virtual void OnReturnButtonClick()
        {
            ServiceLocator.Get<BaseManager>().ShowMainScreen();
        }
        
        protected abstract void UpdateScreen();
    }
}
