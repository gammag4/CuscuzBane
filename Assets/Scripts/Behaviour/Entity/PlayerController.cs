using CuscuzBane.Base;
using CuscuzBane.Behaviour.Extras;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 1.5f;
    private float sprintMultiplier = 1.5f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    private bool sprinting;
    private bool canRecoverStamina;
    private bool moving;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    private float dashTime;
    private float maxDashTime = 0.2f;
    private float dashMultiplier = 4f;
    private float dashStamina = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<DashAnimation>().Dashing = () => Utils.Player.Dashing;
    }

    private void Update()
    {
        if (Utils.Player == null) return;
        if (Utils.Player.Dead) return;

        UpdateInputs();
        UpdateStamina();
        UpdateMovement();
        FlipSprite();
    }

    private void UpdateInputs()
    {
        if (Input.GetMouseButton(0))
        {
            Utils.Player.IsClickedLeft = true;
        }
        if (Input.GetMouseButton(1))
        {
            Utils.Player.IsClickedRight = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Utils.Player.UsingItemLeft = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            Utils.Player.UsingItemRight = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Utils.Player.Interacting = true;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Utils.Player.DroppingItem = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Utils.Player.InventoryOpen = !Utils.Player.InventoryOpen;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Utils.Player.Reloading = true;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Utils.Player.RollingInventory = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Utils.Player.Escape = !Utils.Player.Escape;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Utils.Player.Stamina > dashStamina)
            {
                Utils.Player.Stamina -= dashStamina;
                Utils.Player.Dashing = true;
            }
        }

        Utils.Player.ResizingCamera = Input.GetKey(KeyCode.LeftControl);

        sprinting = false;
        canRecoverStamina = true;
        if (Input.GetKey(KeyCode.LeftShift) && moving)
        {
            if (Utils.Player.Stamina > 0)
            {
                sprinting = true;
            }
            else
            {
                canRecoverStamina = false;
            }
        }

        movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementInput = NormalizeMovementInput(movementInput);

        UpdateInventoryKeys();
    }

    private void UpdateInventoryKeys()
    {
        if (Utils.Player?.Inventory == null) return;

        if (!Utils.Player.ResizingCamera)
        {
            Utils.Player.Inventory.RollSelection((int)-Input.mouseScrollDelta.y);
        }

        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++)
        {
            if (!Input.GetKeyDown((KeyCode)i)) continue;

            var number = i - (int)KeyCode.Alpha0;
            var inventorySlot = (number + Utils.Player.Inventory.Width - 1) % Utils.Player.Inventory.Width;
            Utils.Player.Inventory.Select(inventorySlot);
            break;
        }
    }

    private void UpdateStamina()
    {
        if (sprinting)
        {
            var newStamina = Utils.Player.Stamina - Time.deltaTime * Utils.Player.StaminaDecreaseRate;
            if (newStamina < 0)
                newStamina = 0;

            Utils.Player.Stamina = newStamina;
        }
        else if (canRecoverStamina)
        {
            var newStamina = Utils.Player.Stamina + Time.deltaTime * Utils.Player.StaminaRecoverRate;
            if (newStamina > Utils.Player.TotalStamina)
                newStamina = Utils.Player.TotalStamina;

            Utils.Player.Stamina = newStamina;
        }
    }

    private void UpdateMovement()
    {
        //if (Utils.Player.Attacking) return;

        var col = GetComponent<Collider2D>();

        var move = movementInput * moveSpeed * Time.deltaTime;

        if (Utils.Player.Dashing)
        {
            if (dashTime < maxDashTime)
            {
                dashTime += Time.deltaTime;
                move *= dashMultiplier;
            }
            else
            {
                dashTime = 0;
                Utils.Player.Dashing = false;
            }
        }
        else if (sprinting)
        {
            move *= sprintMultiplier;
        }


        bool success = Utils.Player.TryMove(move);

        moving = success;
        Utils.Player.Animator.SetBool("isMoving", success);
        if (moving)
            Utils.Player.GoingRight = move.x >= 0;
    }

    private Vector2 NormalizeMovementInput(Vector2 input)
    {
        if (input.x == 0 || input.y == 0) return input;

        var x = Mathf.Abs(input.x);
        var y = Mathf.Abs(input.y);
        var tan = x > y ? y / x : x / y;

        y = Mathf.Abs(input.y);
        var h = Mathf.Sqrt(input.x * input.x + input.y * input.y);
        var sin = y / h;
        sin = sin > 1 / Mathf.Sqrt(2) ? Mathf.Sqrt(1 - sin * sin) : sin;

        return input * sin / tan;
    }

    private void FlipSprite()
    {
        if (Utils.Player.Attacking) return;

        // Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void SwordAttack()
    {
        Utils.Player.Attacking = true;
    }

    public void EndSwordAttack()
    {
        Utils.Player.Attacking = false;
    }
}
