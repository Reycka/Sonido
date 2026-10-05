using UnityEngine;
using UnityEngine.UIElements;


public class mover : MonoBehaviour
{
    Transform _trans;
    void Start(){
        _trans = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("W"))
            _trans.position.Set(_trans.position.x +1.0f,transform.position.y,transform.position.z );
        if (Input.GetKeyDown("S"))
            _trans.position.Set(_trans.position.x -1.0f,transform.position.y,transform.position.z );
        if (Input.GetKeyDown("A"))
            _trans.position.Set(_trans.position.x,transform.position.y,transform.position.z+1.0f );
        if (Input.GetKeyDown("D"))
            _trans.position.Set(_trans.position.x,transform.position.y,transform.position.z-1.0f );
        if (Input.GetKeyDown("space"))
            _trans.position.Set(_trans.position.x,transform.position.y+1.0f,transform.position.z );
        if (Input.GetKeyDown(KeyCode.LeftShift))
            _trans.position.Set(_trans.position.x ,transform.position.y-1.0f,transform.position.z );
            
    }
}
