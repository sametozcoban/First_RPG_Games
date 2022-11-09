using RPG.Combat;
using RPG.Movemenent;
using UnityEngine;

namespace RPG.Core
{
    /* ---Burada ki temel amaç Mover ve Combat scriptlerinin birbirlerine olan bağlılıklarına çözüm bulmaya çalışmak.
       ---Birbirlerine direkt ulaşmaktansa bir script yardımıyla hangi action alıyorsak onunla ilgili script devreye girmesini sağlıyoruz.
       ---Tabi ki en verimli yöntem değil geliştirlecek. */
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;
        public void StartAction(IAction action)
        {
            if(currentAction == action) return;
            if (currentAction != null)
            { 
                currentAction.Cancel();
            }
            currentAction = action;
            Debug.Log(currentAction);
        }

        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}