using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleCollision : MonoBehaviour
{
    public string nextLevel;
    [SerializeField] private float reloadAfterDeath = 1.5f;
    [SerializeField] private float menuLoad = 1.5f;
    public float fuelAmount;
    public float maxFuel = 100f;

    public AudioClip crash;
    public AudioClip success;
    public ParticleSystem crashParticles;
    public ParticleSystem successParticles;

    private AudioSource audiosource;
    private bool transitioning = false;
    private bool isColliding = false;

    private void Start()
    {
        // Initialize fuel amount
        fuelAmount = maxFuel;
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        ProcessInteract();
    }

    void ProcessInteract()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            isColliding = !isColliding;
        }

        

    }
    private void OnCollisionEnter(Collision other)
    {
        if (transitioning || isColliding)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Start":
                Debug.Log("This is the Starting Platform");
                break;
            case "Landing":
                SuccessSequence();
                break;
            default:
                CrashSequence();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Fuel":
                CollectFuel(other.gameObject);
                break;
        }
    }

    private void CollectFuel(GameObject fuel)
    {
        float fuelValue = 15f;
        fuelAmount = Mathf.Clamp(fuelAmount + fuelValue, 0, maxFuel);

        Debug.Log($"Fuel collected! Current fuel: {fuelAmount}/{maxFuel}");

        Destroy(fuel);
    }

    public void SuccessSequence()
    {
        transitioning = true;
        audiosource.Stop();
        audiosource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        
        // Start the coroutine for loading the next scene
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        // Optional: Add a delay before loading the next scene, for example 1.5 seconds
        yield return new WaitForSeconds(menuLoad);

        // Call the loading screen's LoadScene method
        LoadingScreen.Instance.LoadScene(nextLevel);
    }

    public void CrashSequence()
    {
        transitioning = true;
        audiosource.Stop();
        audiosource.PlayOneShot(crash);
        crashParticles.Play();
        GetComponent<Movement>().enabled = false;
        StartCoroutine(ReloadLevel());
    }


    private IEnumerator ReloadLevel()
    {
        yield return new WaitForSeconds(reloadAfterDeath);

        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(current);

    }


  
}