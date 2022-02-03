using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { MoveToCursor(); }

        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        bool hasHit = Physics.Raycast(ray, out hitInfo);

        if (hasHit) { GetComponent<NavMeshAgent>().destination = hitInfo.point; }
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = GetComponent<NavMeshAgent>().velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity); //convert global velocity to local velocity
        float speed = localVelocity.z; //how fast should I be moving in a forward direction
        GetComponentInChildren<Animator>().SetFloat("forwardSpeed", speed);
    }
}
