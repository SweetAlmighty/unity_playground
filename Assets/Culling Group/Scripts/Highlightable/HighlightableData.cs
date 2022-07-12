using UnityEngine;

namespace Virbela.CodingTest.Highlightables
{
    /// <summary>
    /// ScriptableObject representing customized data used by <see cref="Highlightable"/> objects.
    /// </summary>
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/HighlightableData", order = 1)]
    public class HighlightableData : ScriptableObject
    {
        /// <summary>
        /// The value to change the <see cref="Material.color"/> of the <see cref="Highlightable"/> object back to if the
        /// aforementioned object is not the closest to the <see cref="Player"/>.
        /// </summary>
        public Color baseColor;

        /// <summary>
        /// The value to change the <see cref="Material.color"/> of the <see cref="Highlightable"/> object to if the
        /// <see cref="Highlightable"/> object is the closest to the <see cref="Player"/>.
        /// </summary>
        public Color highlightColor;
    }
}
