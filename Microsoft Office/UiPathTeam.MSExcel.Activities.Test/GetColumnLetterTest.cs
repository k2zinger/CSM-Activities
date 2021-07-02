using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPathTeam.MSExcel.Activities.Test
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

        [TestMethod]
        public void RetrieveColumnLetterByIndexTest()
        {
            var columns = new[] { new object[] { 1, "A" }, new object[] { 27, "AA" }, new object[] { 703, "AAA" }, new object[] { 4083, "FAA" }, new object[] { 16384, "XFD" } };

            foreach (var col in columns)
            {
                Assert.AreEqual(GetColumnLetter. CalculateColumnLetter((int)col[0]), col[1]);
            }
        }
    }
}
