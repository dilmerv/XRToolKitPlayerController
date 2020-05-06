using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.XR.Interaction.Toolkit;

namespace Tests
{
    public class XRPlayerControllerTests
    {
        private const string SCENE_TO_LOAD = "XRPlayerController";
        private const int SCENE_WAIT_TIME = 1;

        private XRRig XRRig { get; set; }

        [UnitySetUp]
        public IEnumerator XRPlayerController_XRRig_Setup()
        {
            SceneManager.LoadScene(SCENE_TO_LOAD);

            yield return new WaitForSeconds(SCENE_WAIT_TIME);

            XRRig = GameObject.FindObjectOfType<XRRig>();
        }

        [UnityTest]
        public IEnumerator XRPlayerController_XRRig_With_WaitTime_Exist()
        {  
            Assert.IsNotNull(XRRig);
            yield return null;
        }

        [Test]
        public void XRPlayerController_XRRig_XRPlayerController_Exist()
        {
            Assert.IsNotNull(XRRig.GetComponent<XRPlayerController>());
        }
    }
}
