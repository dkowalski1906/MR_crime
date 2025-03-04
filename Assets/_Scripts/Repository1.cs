using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class Repository1 : MonoBehaviour
{
    //test lever
    public event Action OnLeverActivate;

    //display on the block list items
    private string objectInRepo;
    public TMP_Text textMeshPro;
    private string currentItem;
    private string finalData = "";

    //check the step
    private List<string> detectedObjects = new List<string>();
    private List<string> colors = new List<string> { "red", "blue", "green", "yellow", "orange", "purple" };
    private List<string> heads = new List<string> { "head1", "head2" };

    //instantiate prefab
    public GameObject botHead1;
    public GameObject botHead2;

    //move robot
    private float moveDuration = 5f;
    private float moveSpeed = 1f;

    //lever sound
    public AudioSource leverSound;

    // void Start()
    // {
    //     Debug.Log("test");
    //     StartCoroutine(CallCheckValidationAfterDelay(2f));
    // }

    private void OnTriggerEnter(Collider other)
    {
        //display on the block list items
        objectInRepo = other.tag;
        Debug.Log("objectInRepo : " + objectInRepo);
        switch (objectInRepo)
        {
            case "head1":
                currentItem = "Tête";
                break;
            case "head2":
                currentItem = "Tête";
                break;
            case "body1":
                currentItem = "Corps";
                break;
            case "body2":
                currentItem = "Corps";
                break;
            case "arm":
                currentItem = "Bras";
                break;
            case "leg":
                currentItem = "Jambe";
                break;
            case "red":
                currentItem = "Rouge";
                break;
            case "blue":
                currentItem = "Bleu";
                break;
            case "green":
                currentItem = "Vert";
                break;
            case "yellow":
                currentItem = "Jaune";
                break;
            case "orange":
                currentItem = "Orange";
                break;
            case "purple":
                currentItem = "Violet";
                break;
            default:
                currentItem = "Objet inconnu";
                break;
        }

        finalData = finalData + "\n" + currentItem;
        textMeshPro.text = finalData;

        detectedObjects.Add(objectInRepo);

        Destroy(other.gameObject); 
    }

    // private IEnumerator CallCheckValidationAfterDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     CheckValidation();
    // }

    public void CheckValidation()
    {
        //lever sound
        leverSound.Play();

        //retrieve GameManager
        int currentStep = GameManager.Instance.currentStep;

        //check if it is the right step
        if(currentStep == 1)
        {
            int headCount = detectedObjects.FindAll(obj => heads.Contains(obj)).Count;
            int colorCount = detectedObjects.FindAll(obj => colors.Contains(obj)).Count;
            bool validated = headCount == 1 && colorCount == 1;
            Debug.Log("Validation step : " + validated);

            if (validated)
            {
                GameManager.Instance.currentStep = 2;
                currentStep = GameManager.Instance.currentStep;
                textMeshPro.text = "Étape validée ! : " + currentStep;
                Customization();
                StartCoroutine(MoveRobot());
                detectedObjects.Clear();
                finalData = $"{currentStep}";
            }
            else
            {
                textMeshPro.text = "Étape non validée, vous devez mettre une tête et une couleur" + currentStep;
                detectedObjects.Clear();
                finalData = "";
                Debug.Log("Step not validated");
            }
        }
        else
        {
            textMeshPro.text = "Vous devez faire l'étape " + currentStep;
        }
    }

    public void unCheckValidation()
    {
        //lever sound
        leverSound.Play();
    }

    public IEnumerator MoveRobot()
    {
        GameObject robot = GameObject.FindWithTag("robot");
        if (robot == null)
        {
            Debug.LogError("Le robot n'a pas été trouvé dans la scène !");
            yield break;
        }
        float timeElapsed = 0f;
        while (timeElapsed < moveDuration)
        {
            robot.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void Customization()
    {
        string headName = detectedObjects.Find(obj => heads.Contains(obj));
        string colorName = detectedObjects.Find(obj => colors.Contains(obj));

        //head customization
        GameObject robot = GameObject.FindWithTag("robot");
        if (robot == null)
        {
            Debug.LogError("Le robot n'a pas été trouvé dans la scène !");
            return;
        }
        Debug.Log("botHead1 : "+ botHead1);
        GameObject headPrefab = headName == "head1" ? botHead1 : botHead2;
        GameObject instantiatedHead = Instantiate(headPrefab, robot.transform);

        instantiatedHead.transform.localPosition = new Vector3(0, 0.5f, 0);
        instantiatedHead.transform.localRotation = headName == "head1" ? Quaternion.Euler(0,-90f,0) : Quaternion.Euler(0,0,0); 
        Rigidbody rigidbody = instantiatedHead.GetComponent<Rigidbody>();
        Collider collider = instantiatedHead.GetComponent<Collider>();
        Destroy(rigidbody);
        Destroy(collider);

        Debug.Log("Tête instanciée et attachée au robot.");
        
        //color customization
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("ColoredObjectHead");
        foreach (GameObject targetObject in targetObjects)
        {
            Renderer renderer = targetObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                switch(colorName)
                {
                    case "red":
                    renderer.material.color = Color.red;
                    break;
                    case "blue":
                    renderer.material.color = Color.blue;
                    break;
                    case "yellow":
                    renderer.material.color = Color.yellow;
                    break;
                    case "green":
                    renderer.material.color = Color.green;
                    break;
                    case "purple":
                    renderer.material.color = new Color(0.5f, 0f, 0.5f);
                    break;
                    case "orange":
                    renderer.material.color = new Color(1f, 0.647f, 0f);
                    break;
                }
                Debug.Log("Color changed");
            }
            else
            {
                Debug.LogError("Le GameObject " + targetObject.name + " n'a pas de Renderer attaché.");
            }
        }
    }
}
