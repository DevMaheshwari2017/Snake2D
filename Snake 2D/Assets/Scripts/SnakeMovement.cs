using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5;

    private void Movement(float horizontal, float vertical)
    {
        Vector2 pos = transform.position;
        pos.x += horizontal * speed * Time.deltaTime;
        pos.y += vertical * speed * Time.deltaTime;
        transform.position = pos;

        Vector3 Rot = transform.localEulerAngles;
        if (vertical > 0 || vertical < 0)// S - W
        {
            Rot.z = vertical * 90;
        }

        //Vector2 scale = transform.localScale;
        if (horizontal < 0 ) // A
        {
            Rot.z = horizontal * 180;

        }
        else if (horizontal > 0) // D
        {
            Rot.z = horizontal * 0;
        }
        transform.localEulerAngles = Rot;
    }
    private void Update()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        Movement(Horizontal,Vertical);   
    }
}
