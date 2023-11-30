using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini_Player : MonoBehaviour {


    [Header("Componant")]
    [SerializeField]private Transform batsmanleft;
    [SerializeField]private Transform batsmanright;


    [Header("Player Data")]
   
    [SerializeField] private float flt_PaddleRoationSpeed; // paddle Movement Speed
    [SerializeField] private float flt_PaddleMovementSpeed; // Roatation Speed

    // InputData
    private float flt_HorizontalInput;   //Input Value

    // Clamp data
    private float flt_MinCalmpValue = -3;  //Left Clamp Value
    private float flt_MaxClampValue = 3;  // Right Clamp Value

    // force CalculationData
    private float flt_DistanceBetweenCenterToEdgeOfPaddle = 0.5f;   // Distance between centre of the paddle to edge of the paddle


    public void SetbatsManPostion() {


        flt_MinCalmpValue = batsmanleft.position.x + transform.localScale.x / 2;
        flt_MaxClampValue = batsmanright.position.x - transform.localScale.x / 2;


        transform.position = new Vector3(0, Camera.main.orthographicSize - 7.5f, 0);
    }


    private void Update() {

        UserInput();
        PaddleMotion();
    }



    private void PaddleMotion() {

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_PaddleMovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);

        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);
    }

    private void UserInput() {

        flt_HorizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space)) {

            RotateClockWisePaddle();
        }


       



    }


    private void RotateClockWisePaddle() {

        transform.Rotate(Vector3.forward * flt_PaddleRoationSpeed * Time.deltaTime);
    }

}
