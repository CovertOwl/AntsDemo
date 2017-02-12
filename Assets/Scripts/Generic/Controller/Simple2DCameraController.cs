namespace Ants.Controller
{
    /// <summary>
    /// Simple controller class hacked together for quick camera movement
    /// </summary>
    public class Simple2DCameraController : SimpleController
    {
        #region Properties
        [UnityEngine.Tooltip("Minimum zoom level for the camera (orthographic size).")]
        public int MinZoom = 15;
        [UnityEngine.Tooltip("Maximum zoom level for the camera (orthographic size).")]
        public int MaxZoom = 50;

        [UnityEngine.Tooltip("Zoom speed for the camera.")]
        public float ZoomDeltaSpeed = 15.0f;
        
        [UnityEngine.Tooltip("Minimum axis for the camera.")]
        public float MinAxisMovementSpeed = 20.0f;
        [UnityEngine.Tooltip("Maximum axis for the camera.")]
        public float MaxAxisMovementSpeed = 100.0f;

        /// <summary>
        /// The camera targeted by this script
        /// </summary>
        private UnityEngine.Camera thisCamera = null;

        /// <summary>
        /// The zoom level for the camera (orthographic size).
        /// </summary>
        public float Zoom
        {
            get
            {
                return thisCamera.orthographicSize;
            }

            set
            {
                thisCamera.orthographicSize = UnityEngine.Mathf.Clamp(value, MinZoom, MaxZoom);
            }
        }
        #endregion

        #region SimpleController
        public override void ApplyPlayerInput(float deltaSeconds)
        {
            //Discover mouse wheel delta and use it to scale zoom speed
            var scrollDelta = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
            Zoom = Zoom + scrollDelta * ZoomDeltaSpeed * -1.0f;

            //Scale movement based on how far we are zoomed in
            AxisMovementSpeed = UnityEngine.Mathf.Lerp(MinAxisMovementSpeed, MaxAxisMovementSpeed, (Zoom - MinZoom) / (MaxZoom - MinZoom));
            
            base.ApplyPlayerInput(deltaSeconds);
        }
        #endregion

        #region UnityInterface
        public void Start()
        {
            //Discover camera targeted by this script
            thisCamera = gameObject.GetComponent<UnityEngine.Camera>();
            if (thisCamera == null)
            {
                throw new System.Exception("Could not find camera.");
            }
        }
        #endregion
    }
}