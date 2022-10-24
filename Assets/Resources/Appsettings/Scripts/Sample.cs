using UnityEngine;

namespace Catalogue
{
	public class Sample : MonoBehaviour
	{
		[SerializeField] private AudioSource openMenuSound=null;
		[SerializeField] private AudioSource backMenuSound=null;
		[SerializeField] private AudioSource openItemSound=null;
		
		

		private void Start()
		{
			// initialize and restore game settings as soon as game started
			//GameSettingsManager.RestoreSettings();
		}

		public void PlayOpenMenuSound()
		{
			openMenuSound.Play();
		}

		public void PlayOpenItemSound()
		{
			openItemSound.Play();
		}
		

		public void PlayBackMenuSound()
		{
			backMenuSound.Play();
		}

		


		// ------------------------------------------------------------------------------------------------------------
	}
}
