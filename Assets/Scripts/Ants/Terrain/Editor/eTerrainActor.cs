namespace Ants.Terrain
{
    /// <summary>
    /// Editor for the terrain actor
    /// </summary>
    public class eTerrainActor : Ants.Editor.SimpleEditorBase<TerrainActor>
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
            explicitTarget.Generate();
        }
        #endregion
    }
}