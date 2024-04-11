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

namespace Spacegame.Controllers
{
	public class CameraController : MonoBehaviour
	{
		private InputController cameraActions;
		private InputAction 	movement;
		private Transform 		cameraTransform;

		[BoxGroup("Horizontal Translation")]
		[SerializeField]
		[Range(0f, 10f)] private float maxSpeed = 5f;
		private float speed;
		[BoxGroup("Horizontal Translation")]
		[SerializeField]
		[Range(0f, 10f)] private float acceleration = 5f;
		[BoxGroup("Horizontal Translation")]
		[SerializeField]
		[Range(0f, 10f)] private float damping = 5f;

		[BoxGroup("Vertical Translation")]
		[SerializeField]
		private float zoomDampening = 5f;
		[BoxGroup("Vertical Translation")]
		[SerializeField]
		private float maxHeight = 6f;
		[BoxGroup("Vertical Translation")]
		[SerializeField]
		private float minHeight = 1f;
		[BoxGroup("Vertical Translation")]
		[SerializeField]
		private float zoomSpeed = 2f;

		[BoxGroup("Rotation")]
		[SerializeField]
		private float maxRotationSpeed = 1f;

		[BoxGroup("Edge Movement")]
		[SerializeField]
		[Range(0f,0.1f)]
		private float edgeTolerance = 0.05f;

		//Value set in various Functions
		//used to update the position of the camera base object
		private Vector3 targetPosition;

		private Vector3 zoomHeight;

		//used to track and maintain velocity w/o rigidbody
		private Vector3 horizontalVelocity;
		private Vector3 lastPosition;

		//tracks where the dragging started
		Vector3 startDrag;

		[Monitor]
		public float inMag;

		private void Awake()
		{
			cameraActions = new();
			cameraTransform = GetComponentInChildren<Camera>().transform;
			
		}

		private void OnEnable()
		{
			Monitor.StartMonitoring(this);
			cameraTransform.LookAt(transform);

			lastPosition = transform.position;

			movement = cameraActions.Camera.MoveCamera;
			cameraActions.Camera.RotateCamera.performed += RotateCamera;
			cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
			cameraActions.Camera.Enable();
		}

		private void OnDisable()
		{
			cameraActions.Camera.RotateCamera.performed -= RotateCamera;
			cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
			cameraActions.Camera.Disable();

			Monitor.StopMonitoring(this);
		}

		private void Update()
		{
			//Get Input
			GetKeyboadMovement();

			//Move base and camera
			UpdateVelocity();
			UpdateBasePosition();
			UpdateCameraPosition();

		}

		//gets horizontal forward vector of camera
		private Vector3 GetCameraForward()
		{
			Vector3 forward = cameraTransform.forward;
			forward.z = 0;
			return forward;
		}

		//gets horizontal right vector of camera
		private Vector3 GetCameraRight()
		{
			Vector3 right = cameraTransform.right;
			right.z = 0;
			return right;
		}

		//Updates the velocity of the camera
		private void UpdateVelocity()
		{
			horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
			horizontalVelocity.y = 0f;
			lastPosition = transform.position;
		}

		private void GetKeyboadMovement()
		{
			Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
						+ movement.ReadValue<Vector2>().y * GetCameraForward();

			inputValue = inputValue.normalized;

			if (inputValue.sqrMagnitude > 0.1f) targetPosition += inputValue;
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

		private void UpdateCameraPosition()
		{
			//@todo This weirdly moves the camera until a weird place
			
			//Set zoom target
			Vector3 zoomTarget = new(cameraTransform.localPosition.x, cameraTransform.localPosition.y, zoomHeight.z);

			cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
			cameraTransform.LookAt(transform);

		}

		private void RotateCamera(InputAction.CallbackContext obj)
		{
			if (!Mouse.current.middleButton.isPressed) return;
			
			float inputValue = obj.ReadValue<Vector2>().x;
			// Debug.Log($"x: {inputValue} \ny: {obj.ReadValue<Vector2>().y}");
			transform.rotation = Quaternion.Euler(0f, 0f, inputValue * maxRotationSpeed + transform.rotation.eulerAngles.z);
			// Debug.Log(transform.rotation);
		}

		private void ZoomCamera(InputAction.CallbackContext obj)
		{
			
			Vector3 inputValue = -obj.ReadValue<Vector2>() / 100f;
			inMag = Mathf.Abs(inputValue.magnitude);
			// if(inMag > 0.1f)
			// {
			// 	zoomHeight = cameraTransform.localPosition + ShiftXYZtoYZX(inputValue);

			// 	if (zoomHeight.z < minHeight)
			// 	{
			// 		zoomHeight.z = minHeight;
			// 		zoomHeight.y = minHeight;
			// 	}
			// 	else if (zoomHeight.z > maxHeight)
			// 	{
			// 		zoomHeight.z = maxHeight;
			// 		zoomHeight.y = maxHeight;
			// 	}
			// }
		}

		private Vector3 ShiftXYZtoYZX(Vector3 inputVector)
		{
			return new Vector3(inputVector.y, inputVector.z, inputVector.x);
		}

	}
}