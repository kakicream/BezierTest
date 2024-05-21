using Unity.VisualScripting;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollowerSCL : MonoBehaviour
    {
        public PathCreator[] pathCreatorSCL;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        [Range(1, 9)] public int accidentNumber = 1;
        [Range(0, 3)] public int pathNumber = 0;

        [SerializeField] private bool canDrive = false;

        [SerializeField] private Vector3 initPos;
        [SerializeField] private Quaternion initRot;

        void Start()
        {
            accidentNumber = 1;
            initPos = transform.position;
            initRot = transform.rotation;
            canDrive = false;
            if (pathCreatorSCL != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreatorSCL[pathNumber].pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                canDrive = !canDrive;
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                distanceTravelled = 0f;
                transform.position = initPos;
                transform.rotation = initRot;
                canDrive = false;
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (accidentNumber < 9)
                {
                    accidentNumber += 1;
                }
                else
                {
                    accidentNumber = 1;
                }
            }

            if (pathCreatorSCL != null)
            {
                PathNumberSelection_SCL();
                if (canDrive == true)
                {
                    distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreatorSCL[pathNumber].path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    // transform.rotation = pathCreatorSCL[accidentNumber].path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
                }
            }
        }

        private void PathNumberSelection_SCL()
        {
            switch (accidentNumber)
            {
                case 1:
                    pathNumber = 1;
                    break;
                case 2:
                    pathNumber = 2;
                    break;
                case 3:
                    pathNumber = 3;
                    break;
                case 4:
                    pathNumber = 0;
                    break;
                case 5:
                    pathNumber = 1;
                    break;
                case 6:
                    pathNumber = 2;
                    break;
                case 7:
                    pathNumber = 3;
                    break;
                case 8:
                    pathNumber = 0;
                    break;
                case 9:
                    pathNumber = 1;
                    break;
                default:
                    break;
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path

        void OnPathChanged()
        {
            distanceTravelled = pathCreatorSCL[pathNumber].path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}