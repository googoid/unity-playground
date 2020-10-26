using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool jumpKeyPressed = false;
    private bool aimKeyPressed = false;
    private bool shootKeyPressed = false;
    private Rigidbody rigidbodyCmp = null;
    private float horizontalInput = 0;

    [SerializeField] private GameObject levelReference = null;
    [SerializeField] private GameObject groundReference = null;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private GameObject bulletTemplate;

    private float mouseX;
    private float mouseY;
    private float startMouseX;
    private float startMouseY;

    private LineRenderer line;

    private GameObject lastBullet = null;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyCmp = GetComponent<Rigidbody>();
        line = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Physics.OverlapSphere(groundReference.transform.position, 0.1f, playerMask).Length == 1)
        {
            jumpKeyPressed = true;
        }

        shootKeyPressed = Input.GetKeyDown(KeyCode.F);
        horizontalInput = Input.GetAxis("Horizontal") * 1.2f;

        //mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime;
        //mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime;


        if (Input.GetMouseButton(0))
        {
            if (!aimKeyPressed)
            {
                startMouseX = Input.mousePosition.x;
                startMouseY = Input.mousePosition.y;
            }

            aimKeyPressed = true;

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
        }
        else
        {
            aimKeyPressed = false;
        }
    }

    void FixedUpdate()
    {
        if (jumpKeyPressed)
        {
            jumpKeyPressed = false;
            rigidbodyCmp.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
        }

        if (shootKeyPressed/* && lastBullet == null*/)
        {
            Vector3 bulletPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            bulletPos.x += 0.1f;
            lastBullet = Instantiate(bulletTemplate, bulletPos, transform.rotation);
            lastBullet.GetComponentInChildren<Rigidbody>().AddForce(Vector3.right * 1.5f, ForceMode.VelocityChange);

            shootKeyPressed = false;
        }

        line.SetPosition(0, transform.position);

        if (aimKeyPressed)
        {

            line.SetPosition(line.positionCount - 1, new Vector3((mouseX - startMouseX) / Screen.width, (- mouseY + startMouseY) / Screen.height, transform.position.z));
        }

        rigidbodyCmp.velocity = new Vector3(horizontalInput, rigidbodyCmp.velocity.y, 0);
    }
}
