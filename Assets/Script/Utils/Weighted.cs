
using System;
using System.Collections.Generic;
using System.Linq;

public static class Weighted
{
    public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, Func<T, float> weightSelector)
    {
        float totalWeight = sequence.Sum(weightSelector);
        float itemWeightIndex = UnityEngine.Random.value * totalWeight;
        float currentWeightIndex = 0;

        foreach (var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) })
        {
            currentWeightIndex += item.Weight;

            if (currentWeightIndex >= itemWeightIndex)
                return item.Value;

        }

        return default(T);
    }
}
