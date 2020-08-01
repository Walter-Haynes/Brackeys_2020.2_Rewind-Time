using UnityEngine;
using System.Collections;

using Random = UnityEngine.Random;

using CommonGames.Utilities.CustomTypes;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName="Common-Games/Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	[MinMaxSlider(minValue: 0, maxValue: 1, showFields: true)]
	public Vector2 volume = new Vector2(0.9f, 1.0f);
	//public RangedFloat volume = new RangedFloat(MinValue: 0.9f, MaxValue: 1.0f);

	//[MinMaxRange(0, 2)]
	[MinMaxSlider(minValue: 0, maxValue: 2, showFields: true)]
	public Vector2 pitch = new Vector2(0.9f, 1.1f);
	//public RangedFloat pitch = new RangedFloat(MinValue: 0.9f, MaxValue: 1.1f);

	public override void Play(in AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.x, volume.y);
		source.pitch = Random.Range(pitch.x, pitch.y);
		source.Play();
	}
}