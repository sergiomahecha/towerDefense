using UnityEngine;

public class CameraController : MonoBehaviour {

    //Bool que cambia de valor cuando pulsas escape.
    //private bool doMovement=true;

    //Velocidad con la que se movera la camara.
    public float panSpeed = 30f;

    //Margen por el cual se pasara el ratón y movera la camara.
    public float panBorderThickness = 10f;

    //Velocidad con la que se hara zoom al girar la rueda.
    public float scrollSpeed = 5f;

    //Representará lo maximo y lo minimo que puedes hacer zoom.
    public float minY = 10f;
    public float maxY = 10f;

    // Update is called once per frame
	void Update () {

        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        //Si pulsas escape el valor de doMovement cambia.
        //if (Input.GetKeyDown(KeyCode.Escape)) doMovement = !doMovement;

        //Si doMovement equivale a false no se puede mover la camara.
        //if (!doMovement) return;

        //Si se pulsa la w o se mueve el raton en la parte de arriba de la escena.
        if (Input.GetKey("w")||Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            transform.Translate(Vector3.forward*panSpeed*Time.deltaTime, Space.World);
        }

		//Si se pulsa la s o se mueve el raton en la parte de abajo de la escena.
		if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
		{
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
		}

		//Si se pulsa la s o se mueve el raton en la parte de derecha de la escena.
		if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
		{
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
		}

		//Si se pulsa la a o se mueve el raton en la parte de izquierda de la escena.
		if (Input.GetKey("a") || Input.mousePosition.x <=  panBorderThickness)
		{
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
		}

        //Al mover la rueda del raton en la escena esto nos devolvera un float positivo o negativo dependiendo hacia donde se gire.
        float scroll=Input.GetAxis("Mouse ScrollWheel");

        //Campo que representara la posicion actual de la camara.
        Vector3 pos = transform.position;

        //Con esto se modificara la posición de y, hara que se acerque o se aleje la camara.
        pos.y -= scroll * 100 * scrollSpeed * Time.deltaTime;

        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        //Haras que la posición de la camara cambie con pos, que es la posición con el movimiento que le ha dado la rueda.
        transform.position = pos;
    }
}
