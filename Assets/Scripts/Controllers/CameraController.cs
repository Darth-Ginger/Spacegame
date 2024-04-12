using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityRandom = UnityEngine.Random;
using CustomInspector;
using NaughtyAttributes;
using Injector;
using Baracuda.Monitoring;
using Cinemachine;
using Unity;

namespace Spacegame.Controllers
{
	public class CameraController : MonoBehaviour
	{
		private InputController 		 				   cameraActions;
		private InputAction 			 				   movement;
		[SerializeField]private   Transform 			   cameraTransform;
		[SerializeField] private  CinemachineVirtualCamera VCam;
		[SerializeField] private  CinemachineInputProvider CinputProvider;

		[BoxGroup("Horizontal Translation")]
		[SerializeField][Range(0f, 10f)] private float maxSpeed = 5f;
		private float speed;

		[BoxGroup("Horizontal Translation")][SerializeField]
		[Range(0f, 10f)] private float acceleration = 5f;

		[BoxGroup("Horizontal Translation")]
		[SerializeField]
		[Range(0f, 10f)] private float damping = 5f;

		[BoxGroup("Vertical Translation")][SerializeField]
		[Min(1f)]
		private float zoomSpeed = 1f;

		[BoxGroup("Rotation")][SerializeField]
		private float maxRotationSpeed = 1f;

		[BoxGroup("Edge Movement")][SerializeField]
		[Range(0f,0.1f)]
		private float edgeTolerance = 0.05f;

		//Value set in various Functions
		//used to update the position of the camera base object
		private Vector3 targetPosition;

		

		//used to track and maintain velocity w/o rigidbody
		private Vector3 horizontalVelocity;
		private Vector3 lastPosition;

		//tracks where the dragging started
		Vector3 startDrag;

		private void Awake()
		{
			cameraActions 	= new();			
		}

		private void OnEnable()
		{
			
			Monitor.StartMonitoring(this);

			lastPosition = transform.position;

			movement = cameraActions.Camera.MoveCamera;
			cameraActions.Camera.RotateCamera.performed += RotateCamera;
			cameraActions.Camera.Enable();
		}

		private void OnDisable()
		{
			cameraActions.Camera.RotateCamera.performed -= RotateCamera;
			cameraActions.Camera.Disable();

			Monitor.StopMonitoring(this);
		}

		private void Update()
		{
			//Get Input
			GetKeyboadMovement();
			float z = CinputProvider.GetAxisValue(2);

			//Move base and camera
			UpdateVelocity();
			UpdateBasePosition();

			if (z != 0) ZoomScreen(z);
		}

		//gets horizontal forward vector of camera
		[MonitorMethod]
		private Vector3 GetCameraForward()
		{
			Vector3 forward = cameraTransform.forward;
			
			forward.z = 0;
			return forward;
		}

		//gets horizontal right vector of camera
		[MonitorMethod]
		private Vector3 GetCameraRight()
		{
			Vector3 right = cameraTransform.right;
			
			right.z = 0;
			// if (transform.rotation.z == 180 || transform.rotation.z == -180)
			// {
			// 	right = -right;
			// }
			return right;
		}

		//Updates the velocity of the camera
		private void UpdateVelocity()
		{
			horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
			horizontalVelocity.y = 0f;
			lastPosition = transform.position;
		}
		[MonitorMethod]
		private Vector3 GetKeyboadMovement()
		{
			Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
						+ movement.ReadValue<Vector2>().y * GetCameraForward();
			// float x = CinputProvider.GetAxisValue(0);
			// float y = CinputProvider.GetAxisValue(1);
			// Vector3 inputValue = new Vector3(x, y, 0);
			//Debug.Log("Before Normalization: " + inputValue);
			inputValue = inputValue.normalized;
			//Debug.Log("After Normalization: " + inputValue);
			if (inputValue.sqrMagnitude > 0.1f) { targetPosition += inputValue; }

			return inputValue;
		}

		//Updates the Base Rig position
		private void UpdateBasePosition()
		{
			if (targetPosition.sqrMagnitude > 0.1f)
			{
				//Create ramp up
				speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
				transform.position += speed * Time.deltaTime * targetPosition;
			}
			else
			{
				//Create ramp down
				horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
				transform.position += horizontalVelocity * Time.deltaTime;
			}

			//Reset for next fram
			targetPosition = Vector3.zero;
		}

		public void ZoomScreen(float z)
		{
			float fieldOfView = VCam.m_Lens.FieldOfView;
			VCam.m_Lens.FieldOfView = Mathf.Lerp(fieldOfView,
                                        Mathf.Clamp(fieldOfView - z,30,90),
                                        Time.deltaTime * zoomSpeed);
		
		}
		
		private void RotateCamera(InputAction.CallbackContext obj)
		{
			if (!Mouse.current.middleButton.isPressed) return;
			
			float inputValue = obj.ReadValue<Vector2>().x;
			transform.rotation = Quaternion.Euler(0f, 0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.z);
		}

		private Vector3 ShiftXYZtoYZX(Vector3 inputVector)
		{
			return new Vector3(inputVector.y, inputVector.z, inputVector.x);
		}

	}
}