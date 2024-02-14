using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    // 목표: 키보드 방향키(wasd)를 누르면 캐릭터를 바라보는 방향 기준으로 이동시키고 싶다.
    // 속성:
    // - 이동속도
    public float MoveSpeed = 5; // 일반 속도
    public float RunSpeed = 10; // 뛰는 속도

    public float Stamina = 100;
    public const float MaxStamina = 100;    // 스태미나 최대량
    public float StaminaConsumeSpeed = 33f; // 초당 스태미나 소모량
    public float StaminaChargeSpeed = 50f;  // 초당 스태미나 충전량

    [Header("스태미나 슬라이더 UI")]
    public Slider StaminaSliderUI;

    private CharacterController _characterController;

    // 목표: 스페이스바를 누르면 캐릭터를 점프하고 싶다.
    // 필요 속성:
    // - 점프 파워 값
    public float JumpPower = 10f;
    private int _jumpCount = 0;
    public int MaxJumpCount = 2;
    private bool _isJumping = false;
    // 구현 순서:
    // 1. 만약에 [spacebar] 버튼을 누르면
    // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.

    // 목표: 캐릭터에게 중력을 적용하고 싶다.
    // 필요 속성:
    // - 중력 값
    private float _gravity = -20; // 중력 변수
    // - 누적할 중력 변수
    private float _yVelocity = 0f;
    // 구현 순서:
    // 1. 중력 가속도가 누적된다.
    // 2. 플레이어에게 y축에 있어 중력을 적용한다.



    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }


    private void Start()
    {
        Stamina = MaxStamina;
    }

    void Update()
    {
        // 버튼에 따라 카메라 FPS/TPS 변경 (처음에는 FPS) (9번: FPS, 0번: TPS)
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.FPS);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            CameraManager.Instance.SetCameraMode(CameraMode.TPS);
        }

        // 1. 키 입력 받기
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // 2. '캐릭터가 바라보는 방향'을 기준으로 방향 구하기
        Vector3 dir = new Vector3(h, 0, v); // 로컬 좌표계 (나만의 동서남북)
        dir.Normalize();
        // Transforms direction from local space to world space.
        dir = Camera.main.transform.TransformDirection(dir); // 글로벌 좌표계 (세상의 동서남북)

        // Shift 누르고 있으면 빨리 뛰기 (이동 속도 10)
        float speed = MoveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Stamina -= StaminaConsumeSpeed * Time.deltaTime;    // 스태미나 소모(3초)
            if (Stamina > 0)
            {
                speed = RunSpeed;
            }         
        }
        else
        {
            Stamina += StaminaChargeSpeed * Time.deltaTime; // 스태미나 충전(2초)
        }

        Stamina = Mathf.Clamp(Stamina, 0, MaxStamina);
        StaminaSliderUI.value = Stamina / MaxStamina; // 0~1

        // 점프 구현   
        if (_characterController.isGrounded) // 땅일 때
        {
            _isJumping = false;
            _yVelocity = 0;
            _jumpCount = 0;
        }
        // 1. 만약에 [spacebar] 버튼을 누르는 순간
        if (Input.GetKeyDown (KeyCode.Space) && _jumpCount < MaxJumpCount) // 누른 그 순간만 true
        {
            _isJumping = true;
            // 2. 플레이어에게 y축에 있어 점프 파워를 적용한다.
            _yVelocity = JumpPower;
            dir.y = _yVelocity;
            _jumpCount++;
        }

        // 3-1. 중력 적용
        // 1. 중력 가속도가 누적된다.
        _yVelocity += _gravity * Time.deltaTime;
            
        // 2. 플레이어에게 y축에 있어 중력을 적용한다.
        dir.y = _yVelocity;

        // 3-2. 이동하기
        //transform.position += speed * dir * Time.deltaTime;
        _characterController.Move(dir * speed * Time.deltaTime);
    }
}
