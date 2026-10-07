using UnityEngine;

[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Scriptable Objects/AudioDatabase")]
public class AudioDatabase : ScriptableObject
{
    public Sound[] sounds;
}
