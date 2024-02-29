using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterState // 몬스터의 상태
{
    Idle,           // 대기
    Trace,          // 추적
    Attack,         // 공격
    Comeback,       // 복귀
    Damaged,        // 공격 당함
    Die,            // 사망
    Patrol          // 순찰
}

public class Monster : MonoBehaviour, IHitable
{
    [Range(0, 100)]
    public int Health;
    public int MaxHealth = 100;
    public Slider HealthSliderUI;
    /***********************************************************************/

    // private CharacterController _characterController;
    private NavMeshAgent _nevMeshAgent;
    private Animator _animator;

    private Transform _target;         // 플레이어
    public float FindDistance = 5f;  // 감지 거리
    public float AttackDistance = 2f;  // 공격 범위 
    public float MoveSpeed = 4f;  // 이동 상태
    public Vector3 StartPosition;     // 시작 위치
    public float MoveDistance = 40f; // 움직일 수 있는 거리
    public const float TOLERANCE = 0.1f;
    public int Damage = 10;
    public const float AttackDelay = 1f;
    private float _attackTimer = 0f;
    public float PatrolTime = 3f;
    private float _patrolTimer = 0f;
    public float PatrolRadius = 30f;
    Vector3 randomDirection;

    private Vector3 _knockbackStartPosition;
    private Vector3 _knockbackEndPosition;
    private const float KNOCKBACK_DURATION = 0.1f;
    private float _knockbackProgress = 0f;
    public float KnockbackPower = 1.2f;

    private MonsterState _currentState = MonsterState.Idle;

    private void Start()
    {
        //_characterController = GetComponent<CharacterController>();
        _nevMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();

        _target = GameObject.FindGameObjectWithTag("Player").transform;
        StartPosition = transform.position;

        Init();
    }

    public void Init()
    {
        Health = MaxHealth;
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Go)
        {
            HealthSliderUI.value = (float)Health / (float)MaxHealth;  // 0 ~ 1

            // 상태 패턴: 상태에 따라 행동을 다르게 하는 패턴 
            // 1. 몬스터가 가질 수 있는 행동에 따라 상태를 나눈다.
            // 2. 상태들이 조건에 따라 자연스럽게 전환(Transition)되게 설계한다.
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                Debug.Log(gameObject.name + "," + _currentState);
            }
            switch (_currentState)
            {
                case MonsterState.Idle:
                    Idle();
                    break;

                case MonsterState.Trace:
                    Trace();
                    break;

                case MonsterState.Comeback:
                    Comeback();
                    break;

                case MonsterState.Attack:
                    Attack();
                    break;

                case MonsterState.Damaged:
                    Damaged();
                    break;

                case MonsterState.Patrol:
                    Patrol();
                    break;

                case MonsterState.Die:
                    Die(); 
                    break;
            }
        }      
    }

    private void Idle()
    {
        // todo: 몬스터의 Idle 애니메이션 재생
        _patrolTimer += Time.deltaTime;
        if (_patrolTimer > PatrolTime)
        {
            Debug.Log("상태 전환: Idle -> Patrol");
            _animator.SetTrigger("IdleToPatrol");
            _currentState = MonsterState.Patrol;
            RandomPosition();
        }
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Idle -> Trace");
            _animator.SetTrigger("IdleToTrace");
            _currentState = MonsterState.Trace;
        }       
    }
    private void Trace()
    {
        // Trace 상태일때의 행동 코드를 작성

        // 플레이어게 다가간다.
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = _target.transform.position - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 네비게이션이 접근하는 최소 거리를 공격 가능 거리로 설정
        _nevMeshAgent.stoppingDistance = AttackDistance;
        // 네비게이션의 목적지를 플레이어의 위치로 한다.
        _nevMeshAgent.destination = _target.position;
        // 3. 쳐다본다.
        //transform.forward = dir; //(_target);

        if (Vector3.Distance(transform.position, StartPosition) >= MoveDistance)
        {
            Debug.Log("상태 전환: Trace -> Comeback");
            _animator.SetTrigger("TraceToComeback");
            _currentState = MonsterState.Comeback;
        }

        if (Vector3.Distance(_target.position, transform.position) <= AttackDistance)
        {
            Debug.Log("상태 전환: Trace -> Attack");
            _animator.SetTrigger("TraceToAttack");
            _currentState = MonsterState.Attack;
        }
    }
    private void Comeback()
    {
        // 실습 과제 34. 복귀 상태의 행동 구현하기:
        // 시작 지점 쳐다보면서 시작지점으로 이동하기 (이동 완료하면 다시 Idle 상태로 전환)
        // 1. 방향을 구한다. (target - me)
        Vector3 dir = StartPosition - this.transform.position;
        dir.y = 0;
        dir.Normalize();
        // 2. 이동한다.
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // 네비게이션이 접근하는 최소 거리를 오차범위로 설정
        _nevMeshAgent.stoppingDistance = TOLERANCE;
        // 네비게이션의 목적지를 시작지점으로 한다.
        _nevMeshAgent.destination = StartPosition;
        // 3. 쳐다본다.
        //transform.forward = dir; //(_target);

        if (Vector3.Distance(StartPosition, transform.position) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Comeback -> idle");
            _animator.SetTrigger("ComebackToIdle");
            _currentState = MonsterState.Idle;
            _patrolTimer = 0f;
        }
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Comeback -> Trace");
            _animator.SetTrigger("ComebackToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    private void Attack()
    {
        // 전이 사건: 플레이어와 거리가 공격 범위보다 멀어지면 다시 Trace
        if (Vector3.Distance(_target.position, transform.position) > AttackDistance)
        {
            _attackTimer = 0f;
            Debug.Log("상태 전환: Attack -> Trace");
            _animator.SetTrigger("AttackToTrace");
            _currentState = MonsterState.Trace;
            return;
        }

        // 실습 과제 35. Attack 상태일 때 N초에 한 번 때리게 딜레이 주기
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackDelay)
        {
            _animator.SetTrigger("Attack");
        }
    }
    public void PlayerAttack()
    {
        IHitable playerHitable = _target.GetComponent<IHitable>();
        if (playerHitable != null)
        {
            Debug.Log("때렸다!");
            DamageInfo damageInfo = new DamageInfo(DamageType.Normal, Damage);
            playerHitable.Hit(damageInfo);
            _attackTimer = 0f;
        }
    }
    private void Damaged()
    {
        // 1. Damage 애니메이션 실행(0.5초)
        _animator.SetTrigger("Damaged");
        // todo: 애니메이션 실행

        // 2. 넉백 구현
        // 2-1. 넉백 시작/최종 위치를 구한다.
        if (_knockbackProgress == 0)
        {
            _knockbackStartPosition = transform.position;

            Vector3 dir = transform.position - _target.position;
            dir.y = 0;
            dir.Normalize();

            _knockbackEndPosition = transform.position + dir * KnockbackPower;
        }

        _knockbackProgress += Time.deltaTime / KNOCKBACK_DURATION;

        // 2-2. Lerp를 이용해 넉백하기
        transform.position = Vector3.Lerp(_knockbackStartPosition, _knockbackEndPosition, _knockbackProgress);

        if (_knockbackProgress > 1)
        {
            _knockbackProgress = 0f;

            Debug.Log("상태 전환: Damaged -> Trace");
            _animator.SetTrigger("DamagedToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    private void Patrol()
    {
        // 특정 지점으로 순찰을 간다.
        _nevMeshAgent.stoppingDistance = TOLERANCE;
        _nevMeshAgent.destination = randomDirection;
          
        if (Vector3.Distance(transform.position, randomDirection) <= TOLERANCE)
        {
            Debug.Log("상태 전환: Patrol -> Comeback");
            _animator.SetTrigger("PatrolToComeback");
            _currentState = MonsterState.Comeback;
            /*RandomPosition();
            _nevMeshAgent.destination = randomDirection;*/
        }
        if (Vector3.Distance(_target.position, transform.position) <= FindDistance)
        {
            Debug.Log("상태 전환: Patrol -> Trace");
            _animator.SetTrigger("PatrolToTrace");
            _currentState = MonsterState.Trace;
        }
    }
    public void RandomPosition()
    {
        randomDirection = Random.insideUnitSphere * PatrolRadius;
        randomDirection += StartPosition;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, PatrolRadius, NavMesh.AllAreas))
        {
            randomDirection.y = 1;
            Debug.Log($"Random position : {randomDirection}");
        }
    }

    public void Hit(DamageInfo damage)
    {
        if (_currentState == MonsterState.Die)
        {
            return;
        }

        // Todo. 데미지 타입이 크리티컬이면 피흘리기
        if (damage.DamageType == DamageType.Critical)
        {
            // 실습 과제 47: 블러드를 팩토리패턴으로 구현하기 (파일 및 클래스명:BloodFactory)
            BloodFactory.Instance.Make(damage.Position, damage.Normal);
        }

        Health -= damage.Amount;
        if (Health <= 0)
        {
            Debug.Log("상태 전환: Any -> Die");

            // Die 애니메이션 확률
            int num = Random.Range(0, 2);
            _animator.SetTrigger(num == 0 ? "Die1" : "Die2");

            _currentState = MonsterState.Die;
        }
        else
        {
            Debug.Log("상태 전환: Any -> Damaged");
            _currentState = MonsterState.Damaged;
        }
    }

    private Coroutine _dieCoroutine;
    private void Die()
    {
        // 매 프레임마다 해야 할 행동을 추가

        if (_dieCoroutine == null)
        {
            _dieCoroutine = StartCoroutine(Die_Coroutine());
        }      
    }
    private IEnumerator Die_Coroutine()
    {
        _nevMeshAgent.isStopped = true;
        _nevMeshAgent.ResetPath();
        HealthSliderUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        // 죽을때 아이템 생성
        ItemObjectFactory.Instance.MakePercent(transform.position);
        Destroy(gameObject);
    }
}