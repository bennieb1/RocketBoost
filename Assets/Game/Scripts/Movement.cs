using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // Parameters - for tuning, typically set in the editor
    [SerializeField] private float mainThrust = 100f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float fuelConsumptionRate = .2f;
    [SerializeField] private AudioClip mainEngine;
    [FormerlySerializedAs("sideThrust")] [SerializeField] private AudioClip SecondaryEngine;
    public string SceneName;

    //CACHE - E.G references for readability or speed
    public Image fuelBar;
     [FormerlySerializedAs("rightThrust")] public ParticleSystem rightBooster;
     [FormerlySerializedAs("leftThrust")] public ParticleSystem leftBooster;
     [FormerlySerializedAs("p_mainThrust")] public ParticleSystem mainBooster;

    // STATE - private instance (member) variables  
    private Rigidbody _rb;
    private HandleCollision _handleCollision;
    [FormerlySerializedAs("audioSource")] [SerializeField] private AudioSource audioSourceMain;
    [SerializeField] private AudioSource audioSourceThrust;
    
    
   private void Start()
    {
       
        _rb = GetComponent<Rigidbody>();
        _handleCollision = GetComponent<HandleCollision>();

        // Initialize the fuel bar at the start
        ResetFuelBar();
        UpdateFuelBar();
    }
   
    private void Update()
    {
        ProcessThrust();
      
        ProcessRotation();

        UpdateFuelBar();
        
       
    }

    

    void ProcessThrust()
    {
        if (_handleCollision.fuelAmount > 0)
        {
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                StartThrust();
            }
            else
            {
                ThrustStop();
            }
        }
        else
        {
            audioSourceMain.Stop();
        }
    }
    
    private void ThrustStop()
    {
        audioSourceMain.Stop();
        mainBooster.Stop();
    }
    
    private void StartThrust()
    {
        _rb.AddRelativeForce(Vector3.up * (mainThrust * Time.deltaTime));
        _handleCollision.fuelAmount -= fuelConsumptionRate * Time.deltaTime;

        if (!audioSourceMain.isPlaying)
        {
            audioSourceMain.PlayOneShot(mainEngine);
        }

        if (!mainBooster.isPlaying)
        {
            mainBooster.Play();
        }
    }

    void ProcessRotation()
    {
        if (_handleCollision.fuelAmount > 0)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {

                
                leftBooster.Play();
                BoosterBlast();
                ApplyRotation(rotationSpeed);

            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
             
                rightBooster.Play();
                BoosterBlast();
                ApplyRotation(-rotationSpeed);
            }
            else
            {
                SideThrust();
            }
        }
    }

    private void BoosterBlast()
    {
        if (!audioSourceThrust.isPlaying)
        {
            audioSourceThrust.PlayOneShot(SecondaryEngine);

        }
        else
        {
            audioSourceThrust.Stop();
        }
    }

    private void SideThrust()
    {
        leftBooster.Stop();
        rightBooster.Stop();
    }

   


    void ApplyRotation(float rotatesThisFrame)
    {
        _rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * (rotatesThisFrame * Time.deltaTime));
        _rb.freezeRotation = false;
    }

    void UpdateFuelBar()
    {
        if (fuelBar != null)
        {
            fuelBar.fillAmount = _handleCollision.fuelAmount / _handleCollision.maxFuel;
        }
    }

    void ResetFuelBar()
    {
        if (fuelBar != null)
        {
            fuelBar.fillAmount = _handleCollision.fuelAmount / _handleCollision.maxFuel;
        }
    }
}