using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mini_Player : MonoBehaviour {


    [Header("Componant")]
    [SerializeField]private Transform batsmanleft;
    [SerializeField]private Transform batsmanright;


    [Header("Player Data")]
   
    [SerializeField] private float flt_PaddleRoationSpeed; // paddle Movement Speed
    [SerializeField] private float flt_PaddleMovementSpeed; // Roatation Speed

    // InputData
    private float flt_HorizontalInput;   //Input Value
    private bool isRotate;

    float flt_TargetPostion;
    float flt_CurrentPostion;
    private Vector2 startTouchPosition;
    private Vector2 moveDirection;
    private float flt_Delta = 4;
    private float flt_SenstyVity = 50;

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

        
        PaddleMotion();
    }

    private void PaddleMotion() {

#if UNITY_EDITOR
        flt_HorizontalInput = Input.GetAxis("Horizontal");


        if (Input.GetKey(KeyCode.Space)) {

            isRotate = true;
        }
        else {
            isRotate = false;
        }

        transform.Translate(Vector3.right * flt_HorizontalInput * flt_PaddleMovementSpeed * Time.deltaTime, Space.World);
        float x_Postion = transform.position.x;

        x_Postion = Mathf.Clamp(x_Postion, flt_MinCalmpValue, flt_MaxClampValue);

        transform.position = new Vector3(x_Postion, transform.position.y, transform.position.z);

#else
        SwipeControl();
#endif

        RotateClockWisePaddle();


    }


    private void SwipeControl() {

        if (Input.GetMouseButtonDown(0)) {
            startTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        }
        else if (Input.GetMouseButton(0)) {

            Vector2 currentSwipe = new Vector2(Input.mousePosition.x - startTouchPosition.x, 0).normalized;
            float flt_Distance = Mathf.Abs(Vector2.Distance(startTouchPosition, Input.mousePosition));
            startTouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (flt_Distance > flt_Delta) {
                startTouchPosition = Input.mousePosition;
                moveDirection = currentSwipe * flt_SenstyVity;

                flt_TargetPostion = moveDirection.x;
                flt_TargetPostion = Mathf.Clamp(flt_TargetPostion, flt_MinCalmpValue, flt_MaxClampValue);
                flt_CurrentPostion = transform.position.x;
                flt_CurrentPostion = Mathf.Lerp(flt_CurrentPostion, flt_TargetPostion, Time.deltaTime * flt_PaddleMovementSpeed);
                transform.position = new Vector2(flt_CurrentPostion, transform.position.y);

            }

        }


    }


    public void BtnClick(bool value) {
        isRotate = value;
    }

    private void RotateClockWisePaddle() {

        if (isRotate) {
            transform.Rotate(Vector3.forward * flt_PaddleRoationSpeed * Time.deltaTime);
        }

    }

}
