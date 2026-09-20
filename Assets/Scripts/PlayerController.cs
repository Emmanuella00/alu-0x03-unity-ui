using System.Collections; // needed for IEnumerator
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // needed for Text / Image

public class PlayerController : MonoBehaviour
{
    // Editable in the Inspector to easily tweak movement speed
    public float speed = 8f;
    public int health = 5;

    // ---- UI references (drag these in the Inspector) ----
    public Text scoreText;      // ScoreText GameObject
    public Text healthText;     // HealthText GameObject
    public Text winLoseText;    // WinLoseText GameObject
    public Image winLoseBG;     // WinLoseBG GameObject's Image component
                                 // (using Image instead of GameObject lets us
                                 // both SetActive() via .gameObject AND set .color
                                 // from a single Inspector slot)

    private Rigidbody rb;
    private int score = 0;
    private bool isGameOver = false; // guards against starting the reload
                                      // coroutine every single frame while
                                      // health stays <= 0

    // Start is called once, before the first frame update
    void Start()
    {
        // Cache the Rigidbody component attached to this GameObject
        rb = GetComponent<Rigidbody>();

        // Initialize UI text so it matches starting values immediately
        SetScoreText();
        SetHealthText();

        // WinLoseBG should start hidden (Task 4 sets it inactive in the
        // Editor already, but this keeps behavior correct if the scene
        // reloads and the object was left active by a previous state)
        if (winLoseBG != null)
        {
            winLoseBG.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Check every frame whether the Player has run out of health
        if (health <= 0 && !isGameOver)
        {
            isGameOver = true; // prevents this block re-firing every frame

            // Task 6: show Game Over! in white text on a red background
            ShowWinLoseUI("Game Over!", Color.white, Color.red);

            // Task 7: wait 3 seconds before reloading instead of an
            // instant, jarring scene reload
            StartCoroutine(LoadScene(3f));
        }

        // Task 9: pressing Esc returns to the menu scene at any time
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("menu");
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
            SetScoreText(); // Task 1: push the new score to the UI

            // Remove the coin from the scene now that it's been collected
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Trap"))
        {
            health--;
            SetHealthText(); // Task 3: push the new health to the UI

            // Traps stay in the maze as a repeatable hazard rather than
            // disappearing on contact -- remove this line if you'd rather
            // they behave like one-time hits, similar to Coins.
        }
        else if (other.CompareTag("Goal") && !isGameOver)
        {
            isGameOver = true; // stop the Player triggering this twice

            // Task 5: show You Win! in black text on a green background
            ShowWinLoseUI("You Win!", Color.black, Color.green);

            // Task 7: same delayed reload used for Game Over
            StartCoroutine(LoadScene(3f));
        }
    }

    // Task 1: updates the on-screen score display
    void SetScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    // Task 3: updates the on-screen health display
    void SetHealthText()
    {
        healthText.text = "Health: " + health;
    }

    // Shared by both the Win (Task 5) and Game Over (Task 6) cases --
    // they're identical except for the message and colors, so this avoids
    // duplicating the same three lines twice
    void ShowWinLoseUI(string message, Color textColor, Color bgColor)
    {
        winLoseText.text = message;
        winLoseText.color = textColor;
        winLoseBG.color = bgColor;
        winLoseBG.gameObject.SetActive(true);
    }

    // Task 7: waits `seconds`, then reloads the current scene.
    // Called as StartCoroutine(LoadScene(3f)) rather than LoadScene(3f)
    // directly -- calling it directly would run it as a normal method and
    // WaitForSeconds would have no effect; StartCoroutine is what lets
    // Unity pause this method at the yield and resume it later.
    IEnumerator LoadScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
