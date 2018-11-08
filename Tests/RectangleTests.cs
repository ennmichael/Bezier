using Bezier;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class RectangleTests
    {
        private Rectangle pivot;

        [TestInitialize]
        public void SetUp() => pivot = new Rectangle(new Vector2(0, 6), new Vector2(8, 0));

        [TestMethod]
        public void TestOverlapping()
        {
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(-4, 0), new Vector2(0, -3))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(3, 1), new Vector2(5, -2))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(6, 1), new Vector2(13, -2))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(7, 3), new Vector2(9, 1))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(7, 10), new Vector2(9, 4))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(3, 7), new Vector2(4, 4))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(-2, 9), new Vector2(1, 5))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(-1, 4), new Vector2(1, 1))));
            Assert.IsTrue(pivot.Overlaps(new Rectangle(new Vector2(-4, 0), new Vector2(0, -3))));
        }

        [TestMethod]
        public void TestNonOverlapping()
        {
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(3, -4), new Vector2(5, -7))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(9, -3), new Vector2(10, -5))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(12, 5), new Vector2(15, 3))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(11, 12), new Vector2(15, 9))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(3, 13), new Vector2(5, 11))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(-4, 12), new Vector2(-3, 11))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(-7, 7), new Vector2(-4, 5))));
            Assert.IsFalse(pivot.Overlaps(new Rectangle(new Vector2(-9, 1), new Vector2(-6, 1))));
        }
    }
}
