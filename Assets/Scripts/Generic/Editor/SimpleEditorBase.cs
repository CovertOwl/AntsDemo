namespace Ants.Editor
{
    /// <summary>
    /// Derived by all editor scripts, provides simple initialise, draw and apply methods.
    /// </summary>
    public abstract class SimpleEditorBase<T> : UnityEditor.Editor where T : class
    {
        //True when the editor script has been initialised
        public bool IsInitialised { get; private set; }

        //Target of the script
        protected T explicitTarget { get; private set; }
        private UnityEngine.Object objectTarget { get; set; }

        /// <summary>
        /// Raises the inspector GUI event.
        /// </summary>
        public sealed override void OnInspectorGUI()
        {
            //If not initialised, initialise, draw and return;
            if (!this.IsInitialised)
            {
                this.IsInitialised = true;

                this.objectTarget = this.target as UnityEngine.Object;
                if (this.objectTarget == null)
                {
                    throw new System.Exception("Editor target is invalid");
                }

                this.explicitTarget = this.target as T;

                this.Init();

                this.DrawInspector();

                return;
            }

            UnityEngine.GUI.changed = false;

            this.DrawInspector();

            //Required for keeping variables set when starting simulation
            if (UnityEngine.GUI.changed)
            {
                this.ApplyInspectorValues();
                UnityEditor.EditorUtility.SetDirty(this.objectTarget);
            }
        }

        /// <summary>
        /// Init this instance.
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// Draws the inspector elements.
        /// </summary>
        protected abstract void DrawInspector();

        /// <summary>
        /// Applies the inspector values.
        /// </summary>
        protected abstract void ApplyInspectorValues();
    }
}