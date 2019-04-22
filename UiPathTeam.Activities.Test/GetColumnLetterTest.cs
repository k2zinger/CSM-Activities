using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPathTeam.Activities.Test
{
    [TestClass]
    public class GetColumnLetterTest
    {
        [TestMethod]
        public void CalculateColumnLetterTest()
        {
            var columns = new [] { new object[] { 0, "A" }, new object[] { 26, "AA" }, new object[] { 702, "AAA" }, new object[] { 4082, "FAA" }, new object[] { 16383, "XFD" } };

            foreach(var col in columns)
            {
                Assert.AreEqual(GetColumnLetter.CalculateColumnLetter((int)col[0] + 1), col[1]);
            }
        }
    }
}
