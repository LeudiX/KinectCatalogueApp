using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using static Catalogue.AppSettingsManager;

public class Test1SettingsManager {

    // A Test behaves as an ordinary method
    [Test]
    public void Test1RestoreSettings () {
        // Use the Assert class to test conditions
        Catalogue.AppSettingsManager.RestoreSettings ();
    }

    [Test]
    public void Test1AppSettingsManager () {
        Catalogue.AppSettingsManager.SaveBool ("asd23czx", true);
        Catalogue.AppSettingsManager.SaveBool ("asd231223czx", false);
        Catalogue.AppSettingsManager.LoadBool ("asd23czx", true);

    }

    [Test]
    public void Test1AppSettingsManagerSaveToDisk () {
        Catalogue.AppSettingsManager.SaveToDisk ();

    }

    [Test]
    public void Test1AppSettingsManagerSetMainVolume () {

        Catalogue.AppSettingsManager.SetMainSoundVolume (0.9f);
    }

    [Test]
    public void Test1AppSettingsManagerGetMainSoundVolume () {
        Catalogue.AppSettingsManager.GetMainSoundVolume ();
    }

    [Test]
    public void Test1AppSettingsManagerSetSoundVolume () {

        Catalogue.AppSettingsManager.SetSoundVolume (Catalogue.SoundVolumeType.Music, 0.3f);
    }

    [Test]
    public void Test1AppSettingsManagerGetSoundVolume(){
        Catalogue.AppSettingsManager.GetSoundVolume(Catalogue.SoundVolumeType.Main);
    }

}