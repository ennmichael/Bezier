using Bezier;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    class ApproximateEqualityComparer : IEqualityComparer<float>
    {
        public static readonly float Epsilon = 0.0001f;

        public bool Equals(float x, float y) => Math.Abs(x - y) <= Epsilon;

        public int GetHashCode(float obj) => obj.GetHashCode();
    }

    [TestClass]
    public class EquationsTest
    {
        [DataTestMethod]
        [DataRow(1.0f, 2.0f, -2.0f)]
        [DataRow(3.0f, -10.0f, 10.0f / 3.0f)]
        [DataRow(0.0f, 2223.0f, null)]
        public void TestSolveLinear(float a, float b, float? expectedSolution)
        {
            float? solution = Equation.SolveLinear(a, b);
            if (expectedSolution.HasValue)
            {
                Assert.IsTrue(solution.HasValue);
                Assert.AreEqual(solution.Value, expectedSolution.Value, ApproximateEqualityComparer.Epsilon);
            }
            else
            {
                Assert.IsFalse(solution.HasValue);
            }
        }

        [DataTestMethod]
        [DataRow(2.0f, 3.0f, 1.125f, new[] { -0.75f })]
        [DataRow(2.0f, 3.0f, -1.0f, new[] { -1.780776064f, 0.2807764064f })]
        [DataRow(2.0f, -4.0f, -5.0f, new[] { 2.870828693f, -0.87082869f })]
        [DataRow(0.0f, 3.0f, -10.0f, new[] { 10.0f / 3.0f })]
        [DataRow(0.0f, 0.0f, -62517.0f, new float[] { })]
        [DataRow(12.0f, -9.6f, 2.0f, new float[] { })]
        public void TestSolveQuadratic(float a, float b, float c, float[] expected) =>
            CheckSolutions(Equation.SolveQuadratic(a, b, c).ToArray(), expected);

        [DataTestMethod]
        [DataRow(1.0f, 2.0f, 3.0f, 4.0f, new[] { -1.6506291f })]
        [DataRow(4.0f, 3.0f, 2.0f, 1.0f, new[] { -0.60582958f })]
        [DataRow(2.0f, -4.0f, -22.0f, 24.0f, new[] { 4.0f, -3.0f, 1.0f })]
        [DataRow(-2.0f, -4.0f, 22.0f, 24.0f, new[] { 3.0f, -4.0f, -1.0f })]
        [DataRow(-1.0f, 2.0f, -3.0f, -4.0f, new[] { -0.7760454f })]
        [DataRow(-7.0f, 14.0f, 0.0f, -4.0f, new[] { 1.8292233f, -0.480014f, 0.65079064f })]
        [DataRow(-7.0f, 14.0f, 0.0f, 0.0f, new[] { 2.0f, 0.0f })]
        [DataRow(-7.0f, 0.0f, 0.0f, 0.0f, new[] { 0.0f })]
        [DataRow(0.0f, 0.0f, 0.0f, 0.0f, new float[] { })]
        [DataRow(0.0f, 2.0f, 3.0f, 1.125f, new float[] { -0.75f })]
        [DataRow(0.0f, 0.0f, 3.0f, 1.125f, new float[] { -1.125f / 3.0f })]
        public void TestSolveCubic(float a, float b, float c, float d, float[] expected) =>
            CheckSolutions(Equation.SolveCubic(a, b, c, d).ToArray(), expected);

        private void CheckSolutions(float[] solutions, float[] expected)
        {
            Assert.AreEqual(expected.Length, solutions.Length);
            foreach (float solution in solutions)
                Assert.IsTrue(expected.Contains(solution, new ApproximateEqualityComparer()));
        }
    }
}
