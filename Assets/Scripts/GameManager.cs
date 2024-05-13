using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {

	// make game manager public static so can access this from other scripts
	public static GameManager gm;

	// public variables
	public int score = 0;
	public int health = 3;
	public TMP_Text mainScoreDisplay;
	public TMP_Text healthDisplay;
	public TMP_Text gameOverDisplay;
	public TMP_Text rankUpText;
	public GameObject weakSpawn;
	public GameObject medSpawn;
	public GameObject strongSpawn;
	public GameObject finalSpawn;

	public AudioSource musicAudioSource;

	public bool gameIsOver = false;

	public GameObject playAgainButtons;
	public string playAgainLevelToLoad;

	

	void Start () {


		// get a reference to the GameManager component for use by other scripts
		if (gm == null) 
			gm = this.gameObject.GetComponent<GameManager>();

		// init scoreboard to 0
		mainScoreDisplay.text = "Score: " + score.ToString();
		healthDisplay.text = "Health: " + health.ToString();

		Instantiate(weakSpawn, transform.position, transform.rotation);


		// inactivate the playAgainButtons gameObject, if it is set
		if (playAgainButtons)
			playAgainButtons.SetActive (false);

	}

	void Update ()
	{
		if(score == 20 || score == 60 || score == 150)
		{
			rankUpText.text = "Rank Up!!";
		}
		else rankUpText.text = "";
	}

	public void EndGame() {
		// game is over
		gameIsOver = true;
		Destroy (GameObject.FindWithTag("Spawner"));
		GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
		foreach(GameObject enemyProjectile in projectiles)
		{
			enemyProjectile.gameObject.SetActive(false);
		}
		// repurpose the timer to display a message to the player
		gameOverDisplay.text = "GAME OVER";

	
		// activate the playAgainButtons gameObject, if it is set 
		if (playAgainButtons)
			playAgainButtons.SetActive (true);

		// reduce the pitch of the background music, if it is set 
		if (musicAudioSource)
			musicAudioSource.pitch = 0.5f; // slow down the music
	}
	

	// public function that can be called to update the score or time
	public void enemyHit (int scoreAmount)
	{
		// increase the score by the scoreAmount and update the text UI
		score += scoreAmount;
		mainScoreDisplay.text = "Score: " + score.ToString();
		if (score == 20)
		{
			Destroy (GameObject.FindWithTag("Spawner"));
			Instantiate(medSpawn, transform.position, transform.rotation);
			rankUpText.text = "Rank Up!!";
		}
		else if (score == 60)
		{
			Destroy (GameObject.FindWithTag("Spawner"));
			Instantiate(strongSpawn, transform.position, transform.rotation);
			rankUpText.text = "Rank Up!!";
		}
		else if (score == 150)
		{
			Destroy (GameObject.FindWithTag("Spawner"));
			Instantiate(finalSpawn, transform.position, transform.rotation);
			rankUpText.text = "Rank Up!!";
		}
		else 
		{
			rankUpText.text = "";
		}
	}

	public void playerHit (int healthAmount)
	{
		health = healthAmount;
		healthDisplay.text =  "Health: " + health.ToString();
	}

	// public function that can be called to restart the game
	public void RestartGame ()
	{
		// we are just loading a scene (or reloading this scene)
		// which is an easy way to restart the level
        SceneManager.LoadScene(playAgainLevelToLoad);
	}
	

}
