using Assets.Scripts.AI.Utilities;
using Assets.Scripts.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Car
{
    /// <summary>
    /// Class responsible for controlling car.
    /// It is applying values calculated by Neural Network to the car in order to steer and accelerate.
    /// It is also feeding Neural Network with Input values from rays that are being shot in 5 different directions.
    /// Furthermore this class also contains methods that calculates travelled distance of the car,
    /// as well as detecting collision with track walls and changing car material regarding to the current car status (being Leader or not)
    /// </summary>
    public class CarController : MonoBehaviour
    {
        [Space]
        [Header("Wheel Colliders")]
        [Space]
        public WheelCollider FrontLeftWheelCollider;
        public WheelCollider FrontRightWheelCollider;
        public WheelCollider RearLeftWheelCollider;
        public WheelCollider RearRightWheelCollider;

        [Space]
        [Header("Wheel Transforms")]
        [Space]
        public Transform FrontLeftWheelTransform;
        public Transform FrontRightWheelTransform;
        public Transform RearLeftWheelTransform;
        public Transform RearRightWheelTransform;

        [Space]
        [Header("Rays Origin Transforms")]
        [Space]
        public Transform LeftRayOrigin;
        public Transform LeftForwardRayOrigin;
        public Transform ForwardRayOrigin;
        public Transform RightForwardRayOrigin;
        public Transform RightRayOrigin;

        [Space]
        [Header("Driving Variables")]
        [Space]
        public float MaxWheelTurnAngle = 30;
        public float DriveForce = 50;
        public float CurrentVelocity;
        public float CurrentSteeringValue;

        [Space]
        [Header("Other")]
        [Space]
        public Camera CarCamera;
        public List<Renderer> CarMeshRenderers;
        public Material LeaderMaterial;
        public Material DefaultMaterial;
        public bool IsCarInOperation = true;
        public float DistanceTravelled;
        public Rigidbody CarRigidbody;
        public CarBrain CarBrain { get; set; }
        public Vector3 LastCarPosition;
        public int FinishedLaps = 0;

        private float horizontalInput;
        private float verticalInput;
        private float currentSteerAngle;

        /// <summary>
        /// Method that sets LastCarPosition to the current starting position
        /// </summary>
        private void Start()
        {
            LastCarPosition = transform.position;
        }

        /// <summary>
        /// Method used to calculate and update different values each fixed time tick
        /// It calculates and controls the car movement
        /// </summary>
        private void FixedUpdate()
        {
            if (IsCarInOperation)
            {
                var rayOutput = ShootRaysAndUpdateUI();

                var controlInputs = CarBrain.ProcessInputs(rayOutput);

                horizontalInput = controlInputs[0];
                CurrentSteeringValue = horizontalInput;
                verticalInput = controlInputs[1];

                if (verticalInput <= 0)
                {
                    DisableCar();
                }
                Steer();
                Accelerate();
                CalculateAndUpdatePositionAndRotationOfAllWheels();
            }

            CalculateAndUpdateDistanceTravelled();
            CalculateAndUpdateVelocity();
        }

        /// <summary>
        /// Method called in exact moment of car collision with track collider
        /// It allows us to stop the car and feed Genotype with calculated travelled distance of the car
        /// </summary>
        /// <param name="collision">The object the car collided with</param>
        private void OnCollisionEnter(Collision collision)
        {
            DisableCar();
        }

        /// <summary>
        /// Method that changes car material on all available mesh renderers.
        /// </summary>
        /// <param name="material">Material that is going to be assigned on mesh renderers</param>
        public void ChangeCarMaterials(Material material)
        {
            foreach (var carMeshRenderer in CarMeshRenderers)
            {
                carMeshRenderer.material = material;
            }
        }

        /// <summary>
        /// Method that disables the car, prevents it from moving and sets travelled distance on this car genotype
        /// </summary>
        private void DisableCar()
        {
            IsCarInOperation = false;
            CarRigidbody.velocity = new Vector3(0, 0, 0);
            CarRigidbody.constraints = RigidbodyConstraints.FreezeAll;
            CarBrain.Genotype.Distance = DistanceTravelled;
        }

        /// <summary>
        /// Method that handles calling method that shoots all 5 rays on car and updates their values on UI if this car happens to be Leader
        /// </summary>
        /// <returns>List of floats representing distance from ray origin to the collider rays hit (or 0 if hit nothing)</returns>
        private List<float> ShootRaysAndUpdateUI()
        {
            var rayOutput = new List<float>();

            ShootRays(rayOutput);
            if (SceneManager.Instance.Leader == this)
            {
                UpdateRaysDataOnUI(rayOutput);
            }

            return rayOutput;
        }

        /// <summary>
        /// Method that actually handles shooting 5 rays and adding their output to list passed in as parameter
        /// </summary>
        /// <param name="rayOutput">List of 5 floats representing distance from ray origin to collider hit by rays (or 0 if hit nothing)</param>
        private void ShootRays(List<float> rayOutput)
        {
            rayOutput.Add(ShootRay(LeftRayOrigin, Vector3.left));
            rayOutput.Add(ShootRay(LeftForwardRayOrigin, Quaternion.Euler(0, -Constants.RayAngle, 0) * Vector3.forward));
            rayOutput.Add(ShootRay(ForwardRayOrigin, Vector3.forward));
            rayOutput.Add(ShootRay(RightForwardRayOrigin, Quaternion.Euler(0, Constants.RayAngle, 0) * Vector3.forward));
            rayOutput.Add(ShootRay(RightRayOrigin, Vector3.right));
        }

        /// <summary>
        /// Method that actually handles updating rays text on UI by calling UIManager Instance and passing in the text
        /// </summary>
        /// <param name="rayOutput">List of 5 floats representing distance from ray origin to collider hit by rays (or 0 if hit nothing)</param>
        private static void UpdateRaysDataOnUI(List<float> rayOutput)
        {
            UIManager.Instance.UpdateLeftRayText(string.Format("Left: {0}", rayOutput[0]));
            UIManager.Instance.UpdateLeftForwardRayText(string.Format("Left-Forward: {0}", rayOutput[1]));
            UIManager.Instance.UpdateForwardRayText(string.Format("Forward: {0}", rayOutput[2]));
            UIManager.Instance.UpdateRightForwardRayText(string.Format("Right-Forward: {0}", rayOutput[3]));
            UIManager.Instance.UpdateRightRayText(string.Format("Right: {0}", rayOutput[4]));
        }
        
        /// <summary>
        /// Method used to calculate new steering angle for wheels in order to turn the car
        /// </summary>
        private void Steer()
        {
            currentSteerAngle = MaxWheelTurnAngle * horizontalInput;
            FrontLeftWheelCollider.steerAngle = currentSteerAngle;
            FrontRightWheelCollider.steerAngle = currentSteerAngle;
        }

        /// <summary>
        /// Method used to calculate acceleration values for car to move forward
        /// </summary>
        private void Accelerate()
        {
            FrontLeftWheelCollider.motorTorque = verticalInput * DriveForce;
            FrontRightWheelCollider.motorTorque = verticalInput * DriveForce;
        }

        /// <summary>
        /// Method used to update positions and rotations of all wheels to create visual representation of data is being processed
        /// </summary>
        private void CalculateAndUpdatePositionAndRotationOfAllWheels()
        {
            CalculateNewWheelPositionAndRotation(FrontLeftWheelCollider, FrontLeftWheelTransform);
            CalculateNewWheelPositionAndRotation(FrontRightWheelCollider, FrontRightWheelTransform);
            CalculateNewWheelPositionAndRotation(RearLeftWheelCollider, RearLeftWheelTransform);
            CalculateNewWheelPositionAndRotation(RearRightWheelCollider, RearRightWheelTransform);
        }

        /// <summary>
        /// Method that calculates new position and rotation of given wheel
        /// </summary>
        /// <param name="wheelCollider">Wheel collider of the wheel</param>
        /// <param name="wheelTransform">Wheel transform of the wheel</param>
        private void CalculateNewWheelPositionAndRotation(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 position;
            Quaternion rotation;

            wheelCollider.GetWorldPose(out position, out rotation);

            wheelTransform.position = position;
            wheelTransform.rotation = rotation;
        }

        /// <summary>
        /// Method that calculates distance travelled by the car, by adding distance between current and last car positions
        /// </summary>
        private void CalculateAndUpdateDistanceTravelled()
        {
            DistanceTravelled += Vector3.Distance(transform.position, LastCarPosition);
            LastCarPosition = transform.position;
        }

        /// <summary>
        /// Method that calculates velocity of the car's rigidbody
        /// Velocity is in KM/H
        /// </summary>
        private void CalculateAndUpdateVelocity()
        {
            CurrentVelocity = CarRigidbody.velocity.magnitude * 3.6f;
        }

        /// <summary>
        /// Method that shoots ray from rayOrigin transform parameter in direction Vector3 parameter.
        /// </summary>
        /// <param name="rayOrigin">Transform of the ray origin point</param>
        /// <param name="direction">Vector3 of the direction in which ray should be shot</param>
        /// <returns>Distance in floating point value from the hit collider and ray origin (car)</returns>
        private float ShootRay(Transform rayOrigin, Vector3 direction)
        {
            RaycastHit hit;

            Physics.Raycast(rayOrigin.position, transform.TransformDirection(direction), out hit, Constants.MaxRayDistance, ~Constants.RayCastIgnore);

            if (hit.collider != null)
            {
                Debug.DrawRay(rayOrigin.position, transform.TransformDirection(direction) * hit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(rayOrigin.position, transform.TransformDirection(direction) * Constants.MaxRayDistance, Color.green);
            }

            return hit.distance;
        }
    }
}
