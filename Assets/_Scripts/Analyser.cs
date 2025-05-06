using UnityEngine;

public class Analyser : MonoBehaviour
{
    [Header("Laser settings")]
    public float speed = 1f;
    public float moveDistance = 2f;

    private bool isLaunched = false;
    private Vector3 topPosition;
    private Vector3 bottomPosition;
    private Vector3 targetPosition;

    [Header("Sound Settings")]
    public GameObject boxLaserGO;
    public AudioClip leverSound;
    public AudioClip laserSound;

    private AudioSource leverSoundAudioSource;
    private AudioSource laserSoundAudioSource;

    [Header("Lighting Settings")]
    public GameObject laboratoryLightGO;
    public float minLightIntensity;
    private float maxLightIntensity;

    private Light laboratoryLight;

    void Start()
    {
        leverSoundAudioSource = SetupAudioSource(leverSound, leverSoundAudioSource, boxLaserGO);
        laserSoundAudioSource = SetupAudioSource(laserSound, laserSoundAudioSource, boxLaserGO, true);

        //laboratoryLight = laboratoryLightGO.GetComponent<Light>();
        //if (laboratoryLight == null)
        //    laboratoryLight = laboratoryLightGO.AddComponent<Light>();
        //maxLightIntensity = laboratoryLight.intensity;
    }

    private AudioSource SetupAudioSource(AudioClip clip, AudioSource existingSource, GameObject targetGO, bool loop = false)
    {
        existingSource = targetGO.GetComponent<AudioSource>();
        if (existingSource == null)
            existingSource = targetGO.AddComponent<AudioSource>();

        existingSource.clip = clip;
        existingSource.loop = loop;
        return existingSource;
    }

    public void MoveLaser()
    {
        if (isLaunched) return;

        isLaunched = true;

        bottomPosition = transform.position;
        topPosition = bottomPosition + Vector3.up * moveDistance;
        targetPosition = topPosition;

        leverSoundAudioSource.Play();
        laserSoundAudioSource.Play();

        //laboratoryLight.intensity = minLightIntensity;
    }

    public void StopLaser()
    {
        if (!isLaunched) return;

        isLaunched = false;
        transform.position = bottomPosition;

        leverSoundAudioSource.Play();
        laserSoundAudioSource.Stop();

        //laboratoryLight.intensity = maxLightIntensity;
    }

    void Update()
    {
        if (!isLaunched) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = (targetPosition == topPosition) ? bottomPosition : topPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if(other.tag == "Saliva")
        {
            GameManager.Instance.AddSalivaAnalysis();
            other.tag = "Untagged";
        }
    }
}