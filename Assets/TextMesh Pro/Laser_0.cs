using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_0 : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Script")]

    [SerializeField] private Waves Waves_Script;

    [Header("Laser")]   
    [SerializeField] public LineRenderer Laser;
    [SerializeField] private LayerMask Layer_Laser;
    [SerializeField] private float Laser_Distance;
    [SerializeField] private Ray Ray;
    [SerializeField] private RaycastHit RayCast;

    
    // Update is called once per frame
    void Update()
    {

        Ray = new(transform.position, transform.forward);
        if(Physics.Raycast(Ray,out RayCast,Laser_Distance,Layer_Laser))
        {
            Laser.SetPosition(0, transform.position);
            Laser.SetPosition(1, RayCast.point);

            Waves_Script.Panel[2].SetActive(false);
            Waves_Script.Panel[1].SetActive(true);
          Waves_Script.Pause();

            Debug.Log("Player");
        }


        else
        {
            Laser.SetPosition(0, transform.position);
            Laser.SetPosition(1, transform.position+transform.forward*Laser_Distance);
        }
    }


    private void OnDrawGizmos()
    {
     Gizmos.color=Color.red;

        Gizmos.DrawRay(Ray.origin, Ray.direction * Laser_Distance);
     Gizmos.color=Color.blue;
        Gizmos.DrawWireSphere(RayCast.point, 0.22f);

    }

    
}
