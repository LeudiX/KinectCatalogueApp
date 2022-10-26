using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class Test2Managers {

    AppManager manager = new AppManager ();
    // A Test behaves as an ordinary method
    [Test]
    public void Test2AppManagerSetTimeScale () {

        manager.SetTimeScale (0.75f);

    }

    [Test]
    public void Test2AppManagerResetTimeScale () {
        manager.ResetTimeScale ();

    }
    [Test]
    public void Test2AppManagerPause () {
        manager.Pause ();

    }

    [Test]
    public void Test2AppManagerUnPause () {
        manager.UnPause ();
    }

}