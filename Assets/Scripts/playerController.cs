using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : MonoBehaviour
{
    public bool isPlayer1 = false;
    public bool isPlayer2 = false;

    public GameObject playerModel;
    public GameObject chainsaw;

    public int bombRange = 1;
    public int bombAmount = 2;
    public float playerSpeed = 3f;
    [SerializeField] private GameObject bomb;

    private Vector3 moveDirection;
    private Rigidbody rb;

    private float lastBombTime;
    private float bombCooldown = 0.15f;

    //reset
    public Vector3 initialPos;
    private int INITbombRange;
    private int INITbombAmount;
    private float INITplayerSpeed;

    private HashSet<Vector3> myPlacedBombs = new HashSet<Vector3>();


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerModel = transform.GetChild(0).gameObject;

        initialPos = transform.position;
        INITbombRange = bombRange;
        INITbombAmount = bombAmount;
        INITplayerSpeed = playerSpeed;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void GetInput()
    {
        moveDirection = Vector3.zero;

        if (isPlayer1)
        {
            if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.forward;
            if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.back;
            if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;
            if (Input.GetKey(KeyCode.Alpha1)) PlaceBomb();
            if (Input.GetKey(KeyCode.Alpha3)) chainsaw.SetActive(true);

        }
        if (isPlayer2)
        {
            if (Input.GetKey(KeyCode.UpArrow)) moveDirection += Vector3.forward;
            if (Input.GetKey(KeyCode.DownArrow)) moveDirection += Vector3.back;
            if (Input.GetKey(KeyCode.LeftArrow)) moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.RightArrow)) moveDirection += Vector3.right;
            if (Input.GetKey(KeyCode.Space)) PlaceBomb();
            if (Input.GetKey(KeyCode.Keypad3)) chainsaw.SetActive(true);

        }

        moveDirection.Normalize();
    }

    private void MovePlayer()
    {
        Vector3 targetPos = rb.position + moveDirection * playerSpeed * Time.fixedDeltaTime;

        if (moveDirection != Vector3.zero)
        {
            RaycastHit hit;
            Vector3 direction = moveDirection.normalized;
            Vector3 startPosition = rb.position + Vector3.up * 0.5f;

            if (Physics.Raycast(startPosition, direction, out hit, playerSpeed * Time.fixedDeltaTime))
            {
                if (hit.collider != null)
                {
                    if (Mathf.Abs(hit.normal.x) > Mathf.Abs(hit.normal.z))
                    {
                        targetPos = new Vector3(rb.position.x, rb.position.y, targetPos.z);
                    }
                    else if (Mathf.Abs(hit.normal.z) > Mathf.Abs(hit.normal.x))
                    {
                        targetPos = new Vector3(targetPos.x, rb.position.y, rb.position.z);
                    }
                }
            }
            else
            {
                targetPos = rb.position + moveDirection * playerSpeed * Time.fixedDeltaTime;
            }

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, targetRotation, 0.15f);
        }

        rb.MovePosition(targetPos);
    }

    private void PlaceBomb()
    {
        if (Time.time - lastBombTime < bombCooldown) return;
        lastBombTime = Time.time;

        Vector3 gridPosition = new Vector3(
            Mathf.Round(transform.position.x),
            transform.position.y,
            Mathf.Round(transform.position.z)
        );

        if (GridInstance.Instance.placedBombs.Contains(gridPosition)) return;

        if (myPlacedBombs.Count < bombAmount)
        {
            GameObject newBomb = Instantiate(bomb, gridPosition, Quaternion.identity);
            bombScript bombScriptInstance = newBomb.GetComponent<bombScript>();
            if (bombScriptInstance != null)
            {
                bombScriptInstance.SetPlayerController(this);
            }

            myPlacedBombs.Add(gridPosition);
            GridInstance.Instance.placedBombs.Add(gridPosition);
        }
    }


    public void ResetAbilities()
    {
        bombRange = INITbombRange;
        bombAmount = INITbombAmount;
        playerSpeed = INITplayerSpeed;
    }

    public void RemoveBomb(Vector3 position)
    {
        myPlacedBombs.Remove(position);
        GridInstance.Instance.placedBombs.Remove(position);
    }

}
