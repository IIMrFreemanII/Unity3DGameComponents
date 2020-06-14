using UnityEngine;

namespace GameComponents.AI.Scripts
{
    public enum Team
    {
        Red,
        Blue
    }
    public class Teammate : MonoBehaviour
    {
        public Team currentTeam;
    }
}
