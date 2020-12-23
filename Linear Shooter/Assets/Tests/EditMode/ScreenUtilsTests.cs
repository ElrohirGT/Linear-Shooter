using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Utilities;

namespace Tests
{
    public class ScreenUtilsTests
    {
        [Test]
        public void Correct_World_Width()
        {
            ScreenUtils.ForceInitialize();
            float expected = ScreenUtils.WorldRight - ScreenUtils.WorldLeft;

            Assert.AreEqual(expected, ScreenUtils.WorldWidth);
        }

        [Test]
        public void Correct_World_Height()
        {
            ScreenUtils.ForceInitialize();
            float expected = ScreenUtils.WorldTop - ScreenUtils.WorldBottom;

            Assert.AreEqual(expected, ScreenUtils.WorldHeight);
        }

        [Test]
        public void Get_MainCamera_Reference()
        {
            ScreenUtils.ForceInitialize();
            Assert.AreSame(Camera.main, ScreenUtils.MainCamera);
        }
    }
}
