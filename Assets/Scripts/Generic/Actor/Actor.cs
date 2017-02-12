namespace Ants.Actor
{
    /// <summary>
    /// Anything that has a transform to represent it spatially within the simulation should derive from this, including scripts attached to objects
    /// </summary>
    public class Actor : UnityEngine.MonoBehaviour
    {
        #region UtilityMethods
        /// <summary>
        /// Calls GameObject.DestroyImmediate for all children of this actor
        /// </summary>
        protected void DestroyImmediateAllChildren()
        {
            if (this.transform != null)
            {
                var children = new System.Collections.Generic.List<UnityEngine.GameObject>();
                foreach (UnityEngine.Transform child in this.transform)
                {
                    children.Add(child.gameObject);
                }
                foreach (var gameObject in children)
                {
                    UnityEngine.GameObject.DestroyImmediate(gameObject);
                }
            }
        }
        #endregion
    }
}
