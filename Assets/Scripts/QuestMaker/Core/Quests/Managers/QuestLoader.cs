using QuestMaker.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace QuestMaker.Core
{
    public class QuestLoader : MonoBehaviour
    {
 
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            var x = Resources.LoadAll<QuestSO>("Quests");
            if (x == null || !x.Any())
            {
                Debug.Log("Failed to load quests from path Assets/Resources/Quests");
                return;
            }
            

            foreach(var item in x)
            {
                Quest q = new(item);
                
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
