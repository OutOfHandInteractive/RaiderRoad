using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using Rewired;

namespace Tests
{
    [TestFixture(0, 1)]
    [TestFixture(5, 7)]
    [TestFixture(-1, 1)]
    public class HelloTest
    {
        private int _foo, _baz;
        public GameObject obj;

        public HelloTest(int foo, int baz)
        {
            _foo = foo;
            _baz = baz;
        }

        // A Test behaves as an ordinary method
        [Test]
        public void HelloTestSimplePasses()
        {
            // Use the Assert class to test conditions
            List<int> l = new List<int>();
            l.Add(_foo);
            l.Add(_baz);
            l.RemoveAll(i => i < 0);
            Assert.That(l, Has.None.Negative);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator HelloTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
