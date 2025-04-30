using System;
using UnityEngine;

public class SoapboxController : MonoBehaviour
{
    [SerializeField] private float m_motorForce;
    [SerializeField] private float m_breakForce;
    [SerializeField] private float m_maxSteeringAngle;

    [SerializeField] private Transform m_frontLeftWheel;
    [SerializeField] private Transform m_frontRightWheel;
    [SerializeField] private Transform m_rearLeftWheel;
    [SerializeField] private Transform m_rearRightWheel;

    [SerializeField] private WheelCollider m_frontLeftCollider;
    [SerializeField] private WheelCollider m_frontRightCollider;
    [SerializeField] private WheelCollider m_rearLeftCollider;
    [SerializeField] private WheelCollider m_rearRightCollider;

    private float m_horizonalInput;
    private float m_verticalInput;
    private bool m_isBraking;

    private void FixedUpdate()
    {
        GetInput();
        ApplyForces();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        m_horizonalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        m_isBraking = Input.GetKey(KeyCode.Space);
    }

    private void ApplyForces()
    {
        m_frontLeftCollider.motorTorque = m_verticalInput * m_motorForce;
        m_frontRightCollider.motorTorque = m_verticalInput * m_motorForce;

        float currentBreakForce = m_isBraking ? m_breakForce : 0.0f;
        m_frontLeftCollider.brakeTorque = currentBreakForce;
        m_frontRightCollider.brakeTorque = currentBreakForce;
        m_rearLeftCollider.brakeTorque = currentBreakForce;
        m_rearRightCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        m_frontLeftCollider.steerAngle = m_horizonalInput * m_maxSteeringAngle;
        m_frontRightCollider.steerAngle = m_horizonalInput * m_maxSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateWheel(m_frontLeftCollider, m_frontLeftWheel);
        UpdateWheel(m_frontRightCollider, m_frontRightWheel);
        UpdateWheel(m_rearLeftCollider, m_rearLeftWheel);
        UpdateWheel(m_rearRightCollider, m_rearRightWheel);
    }

    private void UpdateWheel(WheelCollider collider, Transform wheel)
    {
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        wheel.rotation = rot;

        wheel.Rotate(Vector3.forward, 90, Space.Self);
    }
}
