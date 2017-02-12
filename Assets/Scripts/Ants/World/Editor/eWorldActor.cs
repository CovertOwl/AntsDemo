namespace Ants.World
{
    /// <summary>
    /// Editor for the world actor
    /// </summary>
    [UnityEditor.CustomEditor(typeof(WorldActor))]
    public class eWorldActor : Ants.Editor.SimpleEditorBase<WorldActor>
    {
        #region SimpleEditorBase
        protected sealed override void Init()
        {

        }

        protected sealed override void DrawInspector()
        {
            DrawDefaultInspector();
        }

        protected sealed override void ApplyInspectorValues()
        {
            explicitTarget.GenerateWorld();
        }
        #endregion
    }
}