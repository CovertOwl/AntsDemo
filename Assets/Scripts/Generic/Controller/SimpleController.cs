namespace Ants.Controller
{
    /// <summary>
    /// Simple controller class hacked together for quick movement
    /// </summary>
    public class SimpleController : Ants.Actor.Actor2D
    {
        #region Properties
        //Static movement directions initialized up front
        private readonly static UnityEngine.Vector3 DirectionUp = new UnityEngine.Vector3(0.0f, 1.0f);
        private readonly static UnityEngine.Vector3 DirectionDown = new UnityEngine.Vector3(0.0f, -1.0f);
        private readonly static UnityEngine.Vector3 DirectionLeft = new UnityEngine.Vector3(-1.0f, 0.0f);
        private readonly static UnityEngine.Vector3 DirectionRight = new UnityEngine.Vector3(1.0f, 0.0f);
        private readonly static UnityEngine.Vector3 DirectionUpLeft = (DirectionUp + DirectionLeft).normalized;
        private readonly static UnityEngine.Vector3 DirectionUpRight = (DirectionUp + DirectionRight).normalized;
        private readonly static UnityEngine.Vector3 DirectionBottomLeft = (DirectionDown + DirectionLeft).normalized;
        private readonly static UnityEngine.Vector3 DirectionBottomRight = (DirectionDown + DirectionRight).normalized;

        [UnityEngine.Tooltip("If true, the controller will respond to character input.")]
        public bool PlayerInputEnabled = true;

        [UnityEngine.Tooltip("Axis (up,down,left,right) movement speed, in Unity units / second")]
        public float AxisMovementSpeed = 1.0f;
        #endregion

        #region PublicMethods
        /// <summary>
        /// Player input is captured here to control this actor
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public virtual void ApplyPlayerInput(float deltaSeconds)
        {
            //Check movement axes bond in input manager and move accordingly
            var horizontalAxis = UnityEngine.Input.GetAxis("Horizontal");
            var verticalAxis = UnityEngine.Input.GetAxis("Vertical");
            bool left = horizontalAxis < 0.0f;
            bool right = horizontalAxis > 0.0f;
            bool up = verticalAxis > 0.0f;
            bool down = verticalAxis < 0.0f;

            if (up)
            {
                if (left)
                {
                    this.MoveUpLeft(deltaSeconds);
                }
                else if (right)
                {
                    this.MoveUpRight(deltaSeconds);
                }
                else
                {
                    this.MoveUp(deltaSeconds);
                }
            }
            else if (down)
            {
                if (left)
                {
                    this.MoveDownLeft(deltaSeconds);
                }
                else if (right)
                {
                    this.MoveDownRight(deltaSeconds);
                }
                else
                {
                    this.MoveDown(deltaSeconds);
                }
            }
            else if (left)
            {
                this.MoveLeft(deltaSeconds);
            }
            else if (right)
            {
                this.MoveRight(deltaSeconds);
            }
        }

        /// <summary>
        /// Move the controller up
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveUp(float deltaSeconds)
        {
            this.Move(DirectionUp, deltaSeconds);
        }

        /// <summary>
        /// Move the controller down
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveDown(float deltaSeconds)
        {
            this.Move(DirectionDown, deltaSeconds);
        }

        /// <summary>
        /// Move the controller left
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveLeft(float deltaSeconds)
        {
            this.Move(DirectionLeft, deltaSeconds);
        }

        /// <summary>
        /// Move the controller right
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveRight(float deltaSeconds)
        {
            this.Move(DirectionRight, deltaSeconds);
        }

        /// <summary>
        /// Move the controller diagonally up/left
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveUpLeft(float deltaSeconds)
        {
            this.Move(DirectionUpLeft, deltaSeconds);
        }

        /// <summary>
        /// Move the controller diagonally up/right
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveUpRight(float deltaSeconds)
        {
            this.Move(DirectionUpRight, deltaSeconds);
        }

        /// <summary>
        /// Move the controller diagonally down/left
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveDownLeft(float deltaSeconds)
        {
            this.Move(DirectionBottomLeft, deltaSeconds);
        }

        /// <summary>
        /// Move the controller diagonally down/right
        /// </summary>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void MoveDownRight(float deltaSeconds)
        {
            this.Move(DirectionBottomRight, deltaSeconds);
        }

        /// <summary>
        /// Move the controller in a direction
        /// </summary>
        /// <param name="direction">The direction to move the controller</param>
        /// <param name="deltaSeconds">Time since last frame, used to scale controller transformation</param>
        public void Move(UnityEngine.Vector3 direction, float deltaSeconds)
        {
            this.transform.localPosition = this.transform.localPosition + direction * deltaSeconds * this.AxisMovementSpeed;
        }
        #endregion

        #region UnityInterface
        public void Update()
        {
            //If this controller accepts player input
            if (this.PlayerInputEnabled)
            {
                this.ApplyPlayerInput(UnityEngine.Time.deltaTime);
            }
        }
        #endregion
    }
}