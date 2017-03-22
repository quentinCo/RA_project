using UnityEngine;
using Vuforia;

public class ImageTargetGenerator : MonoBehaviour,
                                            ITrackableEventHandler
{
    private TrackableBehaviour mTrackableBehaviour;
    private GameManager gameManager;
    private GameObject generator;
 
    void Start()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
     
    public void OnTrackableStateChanged(
                                    TrackableBehaviour.Status previousStatus,
                                    TrackableBehaviour.Status newStatus)
    {
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
            // Play audio when target is found
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            generator = gameManager.CreateGenerator(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            generator.transform.SetParent(gameObject.transform);
            generator.transform.localPosition = new Vector3(0, 0, 0);

            Quaternion quaterRotation = Quaternion.identity;
            quaterRotation.eulerAngles = new Vector3(0, -180, 0);
            generator.transform.localRotation = quaterRotation;

            generator.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
        else
        {
            // Stop audio when target is lost
            //audio.Stop();
            gameManager.RemoveGenerator(generator);
        }
    }   
}