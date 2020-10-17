using UnityEngine;
using UnityEngine.Serialization;

namespace OpenYandere.Characters
{
    [RequireComponent(typeof(CharacterAnimator))]
    public abstract class Character : MonoBehaviour
    {
        public enum ClubType
        {
            None
        }

        public enum GenderType
        {
            Male,
            Female
        }

        [FormerlySerializedAs("Id")]      public int        id;
        [FormerlySerializedAs("Name")]    public string     characterName;
        [FormerlySerializedAs("Gender")]  public GenderType gender;
        [FormerlySerializedAs("Club")]    public ClubType   club;
        [FormerlySerializedAs("IsAlive")] public bool       isAlive = true;
    }
}