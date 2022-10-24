using UnityEngine;

namespace Catalogue
{
	[AddComponentMenu("GameSettings/Sound Volume Updater")]
	public class SoundVolumeUpdater : MonoBehaviour
	{
		[Tooltip("La fuente de audio para la que este actualizador controlará el volumen")]
		public AudioSource target;

		[Tooltip("El tipo de volumen de sonido al que debe vincularse este actualizador. Establecerá el AudioSource de destino a cualquier cambio que tenga lugar en el tipo de volumen especificado solamente.")]
		public SoundVolumeType volumeType = SoundVolumeType.GUI;

		protected void Reset()
		{
			target = GetComponent<AudioSource>();
		}

		protected void Awake()
		{
			if (target == null)
			{
				target = GetComponent<AudioSource>();
				if (target == null)
				{
					Debug.LogError("[SoundVolumeUpdater] No se ha podido encontrar ningún AudioSource en el objeto.", gameObject);
					return;
				}
			}

			AppSettingsManager.RegisterVolumeUpdater(volumeType, this);
		}

		private void OnDestroy()
		{
			AppSettingsManager.RemoveVolumeUpdater(volumeType, this);
		}

		public void UpdateVolume(float v)
		{
			if (target == null) return;
			target.volume = v;
		}

		// ------------------------------------------------------------------------------------------------------------
	}
}
