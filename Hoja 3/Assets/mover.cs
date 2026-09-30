using UnityEngine;

public class mover : MonoBehaviour
{
    Transform _trans;
    [SerializeField]float speed = 10.0f;
    void Start()
    {
        _trans = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 direction = new Vector3();

        if (Input.GetKey(KeyCode.W))
            direction.z = 1;
        else if (Input.GetKey(KeyCode.S))
            direction.z = -1;
        if (Input.GetKey(KeyCode.A))
            direction.x = -1;
        else if (Input.GetKey(KeyCode.D))
            direction.x = 1;
        if (Input.GetKey(KeyCode.Space))
            direction.y = 1;
        else if (Input.GetKey(KeyCode.LeftShift))
            direction.y = -1;

        direction = direction * speed;
        _trans.position = _trans.position + direction * Time.deltaTime;

    }
}