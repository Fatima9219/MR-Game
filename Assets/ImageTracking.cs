/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]

public class ImageTracking : MonoBehaviour
{

   
    public GameObject[] placeablePrefabs;

    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    public void Awake()

    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        foreach(GameObject prefab in placeablePrefabs)
       
        {
            GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newPrefab.name = prefab.name;
           // newPrefab.SetActive(false);
            spawnedPrefabs.Add(prefab.name, newPrefab);

        }

    }

    private void OnEnable()

    {

        trackedImageManager.trackedImagesChanged += ImageChanged;

    }

    private void OnDisable()

    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
        

    }
      
    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)

    {

        foreach(ARTrackedImage trackImage in eventArgs.added)
        {
            UpdateImage(trackImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)

        {

            UpdateImage(trackedImage);

        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)

        {

            spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)

    {

        string name = trackedImage.name;
        Vector3 position = trackedImage.transform.position;
        Quaternion rotation = trackedImage.transform.rotation;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

       foreach(GameObject go in spawnedPrefabs.Values)
        {
            if(go.name != name)
            {
                go.SetActive(false);
            }
       }

    }

 }
    // Start is called before the first frame update
    
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;


[RequireComponent(typeof(ARTrackedImageManager))]
[RequireComponent(typeof(ARTrackedImageManager))]


public class ImageTracking : MonoBehaviour

{
    [SerializeField]
    [Header("The length of this list must match the number of images in Reference Image Library")]
    public List<GameObject> ObjectsToPlace;

    private int refImageCount;
    private Dictionary<string, GameObject> allObjects;
    private ARTrackedImageManager arTrackedImageManager;
    private IReferenceImageLibrary refLibrary;

    public Text debugText;


    void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    private void Start()
    {
        refLibrary = arTrackedImageManager.referenceLibrary;
        refImageCount = refLibrary.count;
        LoadObjectDictionary();
    }

    void LoadObjectDictionary()
    {
        allObjects = new Dictionary<string, GameObject>();
        for (int i = 0; i < refImageCount; i++)
        {
       
       
            allObjects.Add(refLibrary[i].name, ObjectsToPlace[i]);
           
        }
    }

    void ActivateTrackedObject(string _imageName)
    {


        foreach (GameObject child in allObjects.Values)
        {

            child.SetActive(false);


        }

        allObjects[_imageName].SetActive(true);
    }

    public void OnImageChanged(ARTrackedImagesChangedEventArgs _args)
    {
        foreach (var addedImage in _args.added)
        {
            debugText.text = "added";
            ActivateTrackedObject(addedImage.referenceImage.name);

        }

        foreach (var removedImage in _args.removed)
        {
            debugText.text = "removed";
            allObjects[removedImage.referenceImage.name].SetActive(false);
        }

        foreach (var updatedImage in _args.updated)
        {
         

            if (updatedImage.trackingState == TrackingState.Tracking) {

         

                allObjects[updatedImage.referenceImage.name].transform.position = updatedImage.transform.position;
            //s    allObjects[updatedImage.referenceImage.name].transform.rotation = updatedImage.transform.rotation;
                ActivateTrackedObject(updatedImage.referenceImage.name);

            }
            else
            {
                allObjects[updatedImage.referenceImage.name].SetActive(false);
                debugText.text = "removed";
            }


        }
    }
}
