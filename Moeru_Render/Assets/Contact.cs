using UnityEngine;
public class Contact : MonoBehaviour
{
    private float speed = 2f;
    Renderer rend;
    int colorPicker = 0;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 90, 0);

        //Create two GameObjects to act as walls
        GameObject leftWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject rightWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Move the walls to the correct positions
        leftWall.transform.position = new Vector3(-10, 0, 0);
        rightWall.transform.position = new Vector3(10, 0, 0);
        //Scale the walls
        leftWall.transform.localScale = new Vector3(1, 2, 1);
        rightWall.transform.localScale = new Vector3(1, 2, 1);
    }

    //moves the Primitive 2 units a second in the forward direction
    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    //When the Primitive collides with the walls, it will reverse direction
    private void OnTriggerEnter(Collider other)
    {
        speed = speed * -1;
        colorPicker = Random.Range(0, 10);
    }

    //When the Primitive exits the collision, it will change Color
    private void OnTriggerExit(Collider other)
    {
        switch (colorPicker)
        {
            case 0: rend.material.color = Color.white; break;
            case 1: rend.material.color = Color.cyan; break;
            case 2: rend.material.color = Color.blue; break;
            case 3: rend.material.color = Color.black; break;
            case 4: rend.material.color = Color.red; break;
            case 5: rend.material.color = Color.green; break;
            case 6: rend.material.color = Color.grey; break;
            case 7: rend.material.color = Color.magenta; break;
            case 8: rend.material.color = Color.yellow; break;
            case 9: rend.material.color = Color.gray; break;
        }
    }
}