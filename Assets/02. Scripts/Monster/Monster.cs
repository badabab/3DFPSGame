using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public enum MonsterState // 몬스터의 상태
{
    Idle,       // 대기
    Trace,      // 추적
    Attack,     // 공격
    Comeback,     // 복귀
    Damaged,    // 공격 당함
    Die         // 사망
}

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health = 100;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;

    private CharacterController _characterController;
    private Transform _target;          // 플레이어
    public float MoveSpeed = 3;      // 이동 속도
    public float FindDistance = 5;      // 감지 범위
    public float AttackDistance = 2;    // 공격 범위
    public Vector3 StartPosition;       // 시작 위치
    public float MoveDistance = 40f;    // 움직일 수 있는 범위
    public const float TOLERANCE = 0.01f;

    private MonsterState _currentState = MonsterState.Idle;

    void Start()
    {
        Init();
        _characterController = GetComponent<CharacterController>();
        _target = GameObject.Find("Player").transform;
        StartPosition = transform.position;
    }

    private void Update()
    {
        HealthSliderUI.value = (float)Health / (float)MaxHealth;

        // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴
        // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
        // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.

        switch (_currentState)
        {
            case MonsterState.Idle:
                Idle();
                break;
            case MonsterState.Trace:
                Trace();
                break;
            case MonsterState.Attack:
                Attack(); 
                break;
            case MonsterState.Comeback:
                Comeback(); 
                break;
            case MonsterState.Damaged:
                //Damaged();
                break;
            case MonsterState.Die:
                //Die(); 
                break;

        }
    }

    private void Idle()
    {
        // todo: 몬스터의 Idle 애니메이션 재생

        if(Vector3.Distance(_target.position, transform.position) <= FindDistance) //플레이어와의 거리가 일정 범위 안이면
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _currentState = MonsterState.Trace;      
        }
    }
    private void Trace()
    {
        // 플레이어에게 다가간다.

        Vector3 dir = _target.position - this.transform.position;
        //dir.y = 0;
        dir.Normalize();
        //transform.forward = dir;            
        transform.LookAt(_target);
        // transform.position += dir * MoveSpeed * Time.deltaTime;
        
        //Quaternion targetRotation = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance) // 거리가 공격범위 안이면
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _currentState = MonsterState.Attack;
        }
        if (Vector3.Distance(_target.position, StartPosition) >= MoveDistance) // 원점과의 거리가 멀어지면
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _currentState = MonsterState.Comeback;
        }
    }
    private void Attack()
    {
        // 공격 코드


        if (Vector3.Distance(_target.position,transform.position) >= AttackDistance)
        {
            Debug.Log("상태 전환: Attack -> Trace");
            _currentState = MonsterState.Trace;
            return;
        }
    }
    private void Comeback()
    {
        Vector3 dir = StartPosition - transform.position;
        dir.Normalize();
        _characterController.Move(dir* MoveSpeed * Time.deltaTime);
        //transform.forward = StartPosition;
        transform.LookAt(StartPosition);

        if (Vector3.Distance(transform.position, StartPosition) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Return -> Idle");
            _currentState = MonsterState.Idle;
        }
    }

    

    public void Init()
    {
        Health = MaxHealth;
    }

    public void Hit(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    // 몬스터 죽을 때 아이템 드랍
    public void Die()
    {
        ItemObjectFactory.Instance.MakePercent(transform.position);

        Destroy(gameObject);
    }
}
