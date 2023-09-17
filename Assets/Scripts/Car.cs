using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private List<GameObject> backWheels = new();
    [SerializeField] private List<GameObject> frontWheels = new();
    [SerializeField] private float wheelSpeed;
    public bool turnLeft;
    
    void Update()
    {
        TurnWheels();
    }

    private void TurnWheels()
    {
        foreach (var wheel in backWheels) 
        {
            wheel.transform.Rotate(wheelSpeed * Time.deltaTime * Vector3.back);
        }
        foreach (var wheel in frontWheels)
        {
            if (turnLeft)
            {
                wheel.transform.Rotate(wheelSpeed * Time.deltaTime * new Vector3(0, 1, 0));
                if (wheel.transform.rotation.y >= 45)
                {
                    turnLeft = false;
                }
            }
            else
            {
                wheel.transform.Rotate(wheelSpeed * Time.deltaTime * Vector3.back);
            }
        }
    }
}
