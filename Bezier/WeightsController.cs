using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bezier
{
    class WeightsController
    {
        private static readonly int noIndex = -1;

        private readonly IBezierCurve bezierCurve;
        private int selectedWeightIndex = noIndex;

        public WeightsController(IBezierCurve bezierCurve) => this.bezierCurve = bezierCurve;

        public void MouseUp()
        {
            Debug.WriteLine($"MouseUp");
            selectedWeightIndex = noIndex;
        }

        public bool MouseDown(Vector2 position)
        {
            Debug.WriteLine($"MouseMove {selectedWeightIndex} {position}");
            if (selectedWeightIndex == noIndex)
            {
                selectedWeightIndex = SelectWeight(position);
                return false;
            }
            else
            {
                MoveSelectedWeight(position);
                return true;
            }
        }

        private int SelectWeight(Vector2 position)
        {
            float minimumDistance = BezierDrawing.WeightIndicatorDiameter * 2;
            var sqrDistances = SqrDistances(position, minimumDistance);
            try
            {
                return sqrDistances.Min().Index;
            }
            catch (InvalidOperationException)
            {
                return noIndex;
            }
        }

        private IEnumerable<(int Index, float Distance)> SqrDistances(Vector2 fromPosition, float minimumDistance) =>
            from item in Utils.ValuesAndIndices(bezierCurve.Weights)
            let sqrDistance = Vector2.SqrDistance(item.Value, fromPosition)
            let sqrMinimumDistance = minimumDistance * minimumDistance
            where sqrDistance <= sqrMinimumDistance
            select (item.Index, sqrDistance);

        private void MoveSelectedWeight(Vector2 position) => bezierCurve.Weights[selectedWeightIndex] = position;
    }
}
