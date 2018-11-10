using System;
using System.Collections.Generic;
using System.Linq;

namespace Bezier
{
    class WeightsController
    {
        private static readonly int noIndex = -1;

        private readonly ICurve curve;
        private int selectedWeightIndex = noIndex;

        public WeightsController(ICurve curve) => this.curve = curve;

        public void MouseUp() => selectedWeightIndex = noIndex;

        public bool MouseDown(Vector2 position)
        {
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
            float minimumDistance = Drawing.WeightIndicatorDiameter * 2;
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
            from item in Utils.ValuesAndIndices(curve.Weights)
            let sqrDistance = fromPosition.SqrDistance(item.Value)
            let sqrMinimumDistance = minimumDistance * minimumDistance
            where sqrDistance <= sqrMinimumDistance
            select (item.Index, sqrDistance);

        private void MoveSelectedWeight(Vector2 position) => curve.Weights[selectedWeightIndex] = position;
    }
}
