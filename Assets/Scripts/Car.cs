using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private List<GameObject> _backWheels = new();
    private List<GameObject> _frontWheels = new();
    private float _wheelSpeed;
    public bool turnLeft;
    // Start is called before the first frame update
    void Start()
    {
        _wheelSpeed = 100;
        _backWheels.Add(transform.GetChild(0).gameObject);
        _backWheels.Add(transform.GetChild(2).gameObject);
        _frontWheels.Add(transform.GetChild(1).gameObject);
        _frontWheels.Add(transform.GetChild(3).gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        TurnWheels();
    }

    private void TurnWheels()
    {
        foreach (var wheel in _backWheels) 
        {
            wheel.transform.Rotate(_wheelSpeed * Time.deltaTime * Vector3.back);
        }
        foreach (var wheel in _frontWheels)
        {
            if (turnLeft)
            {
                wheel.transform.Rotate(_wheelSpeed * Time.deltaTime * new Vector3(0, 1, 0));
                if (wheel.transform.rotation.y >= 45)
                {
                    turnLeft = false;
                }
            }
            else
            {
                wheel.transform.Rotate(_wheelSpeed * Time.deltaTime * Vector3.back);
            }
        }
    }
}
