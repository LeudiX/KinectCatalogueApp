using UnityEngine;
using System.Collections.Generic;

namespace Catalogue
{
	public static class AppSettingsManager
	{
		private static readonly Vector2 MinScreenSize = new Vector2(1024, 768);

		// ------------------------------------------------------------------------------------------------------------
		#region main

		/// <summary> Restaura la configuración guardada. Normalmente se llama a esto tan pronto como la ejecución de la aplicación ha comenzado </summary>
		public static void RestoreSettings()
		{
			// asegurarse de que ha sido inicializada
			InitializeVolumeTypes();

			// restore sound volume
			for (int i = 0; i < soundVolumes.Length; i++)
			{
				soundVolumes[i] = PlayerPrefs.GetFloat($"Settings.Volume.{i}", 1f);
				SetSoundVolume((SoundVolumeType)i, soundVolumes[i]);
			}
			
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		
		
		//-------------------------------------------------------------------------------------------------------------
		#region Kinect2Toggle

		public abstract class OptionBase<T, U> : MonoBehaviour
		where T : struct
		where U : UIDataType<T> {

		[Tooltip("Key for saving & loading, with other possible re-use.")]
		public string optionName;

		public U defaultSetting; //Un tipo de valor simple, envuelto en una clase Serializable para utilizar PropertyDrawers

		public abstract T Value { get; set; }

		
		
		protected bool allowPresetCallback = true;
		/// <summary>
		/// Método que envuelve al Value setter para evitar las devoluciones de llamada cuando se cambia a través de la preselección.
		/// </summary>
		public void ApplyPreset(T _value){
			allowPresetCallback = false;
			Value = _value;
			allowPresetCallback = true;
		}

		/// <summary>
		/// Anule con el código pertinente para aplicar la configuración que desea cambiar.
		/// Abstracto en lugar de virtual porque no tiene funcionalidad de base y es el núcleo de la clase.
		/// </summary>
		protected abstract void ApplySetting(T _value);

#if UNITY_EDITOR
		/// <summary>
		/// Asignación automática de optionName por defecto.
		/// NOTE:Requiere la asignación en el editor para trabajar con múltiples instancias.
		/// Los VolumeSliders, por ejemplo, necesitan nombres separados para cada deslizador o se sobreescribirán unos a otros.
		/// </summary>
		protected virtual void Reset(){
			optionName = GetType().ToString();
		}
#endif
	}
		[System.Serializable]
		public class UIDataType<T> where T : struct {
		[Tooltip("Ajuste utilizado si no existe ningún ajuste guardado. También se puede utilizar externamente para restaurar los valores predeterminados.")]
		[SerializeField] public T value;
	}


		[System.Serializable] public class BoolToggle : UIDataType<bool> {}


		public static void SaveBool(string _key, bool _value){
			PlayerPrefs.SetInt(_key, _value ? 1 : 0); //Convert bool to int (1=true, 0=false)
		}
		public static bool LoadBool(string _key, bool _defaultValue){
			if (PlayerPrefs.HasKey(_key))
				return PlayerPrefs.GetInt(_key) > 0; //Converts int to bool (1=true, 0=false)
			else
				return _defaultValue;
		}

		public static void SaveToDisk(){
			PlayerPrefs.Save();
		}

		#endregion
		// ------------------------------------------------------------------------------------------------------------
		#region sound

		// mismo número de entradas que el enum SoundVolumeType
		private static float[] soundVolumes = null;
		private static List<SoundVolumeUpdater>[] soundVolumeUpdaters = null;

		// para los gestores interesados en saber cuándo cambia alguno de los tipos de volumen
		public static event System.Action<SoundVolumeType, float> SoundVolumeChanged;

		public static void RegisterVolumeUpdater(SoundVolumeType type, SoundVolumeUpdater target)
		{
			InitializeVolumeTypes();

			int idx = (int)type;
			if (!soundVolumeUpdaters[idx].Contains(target))
			{
				soundVolumeUpdaters[idx].Add(target);
				target.UpdateVolume(soundVolumes[idx]);
			}
		}

		public static void RemoveVolumeUpdater(SoundVolumeType type, SoundVolumeUpdater target)
		{
			InitializeVolumeTypes();

			soundVolumeUpdaters[(int)type].Remove(target);
		}

		/// <summary> Establece el volumen del sonido principal. El valor es un valor flotante entre 0 (sin sonido) y 1 (completo). Así que (0.5) es la mitad del volumen del sonido.</summary>
		public static void SetMainSoundVolume(float value)
		{
			AudioListener.volume = Mathf.Clamp01(value);
		}

		/// <summary> Obtiene el volumen del sonido principal. El valor es un valor flotante entre 0 (sin sonido) y 1 (completo). Así que (0.5) es la mitad del volumen del sonido.</summary>
		public static float GetMainSoundVolume()
		{
			return AudioListener.volume;
		}

		/// <summary> Establece el volumen del sonido del tipo de sonido especificado. El valor es un valor flotante entre 0 (sin sonido) y 1 (completo). Así que (0,5) es la mitad del volumen del sonido.</summary>
		public static void SetSoundVolume(SoundVolumeType type, float value)
		{
			InitializeVolumeTypes();

			int idx = (int)type;
			value = Mathf.Clamp01(value);
			PlayerPrefs.SetFloat($"Settings.Volume.{idx}", value);
			PlayerPrefs.Save();

			if (type == SoundVolumeType.Main)
			{
				AudioListener.volume = value;
			}

			soundVolumes[idx] = value;
			for (int i = 0; i < soundVolumeUpdaters[idx].Count; i++)
			{
				soundVolumeUpdaters[idx][i].UpdateVolume(soundVolumes[idx]);
			}

			SoundVolumeChanged?.Invoke(type, value);
		}

		/// <summary> Obtiene el volumen del sonido del tipo de sonido especificado. El valor es un valor flotante entre 0 (sin sonido) y 1 (completo). Así que (0.5) es la mitad del volumen del sonido.</summary>
		public static float GetSoundVolume(SoundVolumeType type)
		{
			InitializeVolumeTypes();

			if (type == SoundVolumeType.Main)
			{
				return AudioListener.volume;
			}

			return soundVolumes[(int)type];
		}

		private static void InitializeVolumeTypes()
		{
			if (soundVolumes != null && soundVolumes.Length > 0)
			{
				return;
			}

			var count = System.Enum.GetNames(typeof(SoundVolumeType)).Length;
			soundVolumes = new float[count];
			soundVolumeUpdaters = new List<SoundVolumeUpdater>[count];

			for (int i = 0; i < count; i++)
			{
				soundVolumes[i] = 1f;
				soundVolumeUpdaters[i] = new List<SoundVolumeUpdater>(1);
			}
		}
		#endregion
		// ------------------------------------------------------------------------------------------------------------
	}
}
