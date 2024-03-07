using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private enum State
    {
        Roaming,
        ChaseTarget,
        BackToStartState,
        
    }
    
    private GameObject _player;
    private Rigidbody _enemyRigidbody;
    
    private Pathfinding _pathfinding;
    private Vector3 _startingPosition;
    private Vector3 _roamPosition;
    private State _state;

    private void Awake()
    {
        _pathfinding = GetComponent<Pathfinding>();
        _enemyRigidbody = GetComponent<Rigidbody>();

        _player = GameObject.FindGameObjectWithTag("Player");
        _state = State.Roaming;
    }

    private void Start(){
        _startingPosition = transform.position;
        _roamPosition = GetRoamingPosition();
    }

    private void Update()
    {
        switch (_state)
        {
            default:
            case State.Roaming:
                _pathfinding.MoveTo(_roamPosition);
                const float reachedPositionDistance = 1f;
                if (Vector3.Distance(transform.position, _roamPosition) < reachedPositionDistance)
                {
                    _roamPosition = GetRoamingPosition();

                }

                FindTarget();
                break;
            case State.ChaseTarget:
                _pathfinding.MoveTo(_player.transform.position);

                var attackRange = 5f;
                Debug.Log(Vector3.Distance(transform.position, _player.transform.position) < attackRange);
                if (Vector3.Distance(transform.position, _player.transform.position) < attackRange)
                {
                    _enemyRigidbody.velocity = Vector3.zero;
                    var direction = (_player.transform.position - transform.position).normalized;
                    _enemyRigidbody.AddForce(direction * 20f, ForceMode.Impulse);
                    _state = State.BackToStartState;
                }

                break;
            case State.BackToStartState:
                _pathfinding.MoveTo(_startingPosition);
                if (Vector3.Distance(transform.position, _startingPosition) < reachedPositionDistance)
                {
                    _state = State.Roaming;
                }
                break;
        }
       
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Wall")) return;

        Debug.Log("Wall, new target");
        _roamPosition = GetRoamingPosition();
    }

    private Vector3 GetRoamingPosition()
    {
        return _startingPosition + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * Random.Range(10f, 70f);

    }

    private void FindTarget()
    {
        var targetRange = 10f;
        if(Vector3.Distance(transform.position, _player.transform.position) < targetRange)
        {
            _state = State.ChaseTarget;
        }
    }
}
