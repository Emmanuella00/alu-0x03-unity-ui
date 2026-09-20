using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Editable in the Inspector to easily tweak movement speed
    public float speed = 8f;
    public int health = 5;

    private Rigidbody rb;
    private int score = 0;

    // Start is called once, before the first frame update
    void Start()
    {
        // Cache the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Check every frame whether the Player has run out of health
        if (health <= 0)
        {
            Debug.Log("Game Over!");

            // Reloading the current scene destroys and recreates every
            // GameObject in it, including the Player -- this automatically
            // resets health and score back to their declared starting values,
            // since a fresh PlayerController instance is created from scratch.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // FixedUpdate is called on a fixed timestep, in sync with the physics engine
    // Since we're moving a Rigidbody, movement logic belongs here, not in Update()
    void FixedUpdate()
    {
        // GetAxis("Horizontal") reads A/D and Left/Right arrow keys by default
        // GetAxis("Vertical") reads W/S and Up/Down arrow keys by default
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Build a movement vector on the X/Z plane only -- Y stays at 0,
        // so gravity (not this script) is the only thing affecting vertical movement
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * speed * Time.fixedDeltaTime;

        // Move the Rigidbody by this offset, respecting physics collisions
        // (won't let the Player clip through maze walls)
        rb.MovePosition(rb.position + movement);
    }

    // Called automatically whenever this GameObject's collider overlaps
    // a Trigger collider -- in this case, a Coin
    void OnTriggerEnter(Collider other)
    {
        // Only react to objects specifically tagged "Pickup" -- ignores
        // any other trigger colliders that might exist in the maze
        if (other.CompareTag("Pickup"))
        {
            score++;
            Debug.Log("Score: " + score);

            // Remove the coin from the scene now that it's been collected
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            health--;
            Debug.Log("Health: " + health);

            // Traps stay in the maze as a repeatable hazard rather than
            // disappearing on contact -- remove this line if you'd rather
            // they behave like one-time hits, similar to Coins.
        }
        else if (other.CompareTag("Goal"))
        {
            Debug.Log("You win!");
        }
    }
}