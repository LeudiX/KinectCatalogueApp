using UnityEngine;
using UnityEngine.UI;

namespace Catalogue
{
	[AddComponentMenu("GameSettings/UI/Sound Volume Slider")]
	public class SoundVolumeSlider : MonoBehaviour
	{
		[SerializeField] private Slider targetElement;
		[SerializeField] private SoundVolumeType volumeType = SoundVolumeType.Main;

		// ------------------------------------------------------------------------------------------------------------

		private void Reset()
		{
			targetElement = GetComponentInChildren<Slider>();
		}

		private void Start()
		{
			if (targetElement == null)
			{
				targetElement = GetComponentInChildren<Slider>();
				if (targetElement == null)
				{
					Debug.Log("[SoundVolumeSlider] No se ha podido encontrar ningun componente de tipo Slider en este GameObject", gameObject);
					return;
				}
			}

			targetElement.value = AppSettingsManager.GetSoundVolume(volumeType);
			targetElement.onValueChanged.AddListener(OnValueChange);
		}

		private void OnValueChange(float value)
		{
			AppSettingsManager.SetSoundVolume(volumeType, value);
		}

		// ------------------------------------------------------------------------------------------------------------
	}

}
