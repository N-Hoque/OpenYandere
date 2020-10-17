using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters.Player
{
    public class Player : Character
    {
        [FormerlySerializedAs("Reputation")] [Range(-100, 100)]
        public int reputation;
    }
}