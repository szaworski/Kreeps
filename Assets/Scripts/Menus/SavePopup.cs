using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePopup : MonoBehaviour
{
    [SerializeField] private GameObject popUpText;
    public bool triggerPopupText;

    public void StartPopup()
    {
        triggerPopupText = true;
        popUpText.gameObject.SetActive(true);

        if (SceneManager.GetActiveScene().name == "Scene1")
        {
            StartCoroutine(SpawnPopup(0.3f, 0.5f, 2));
        }
        else
        {
            StartCoroutine(SpawnPopup(0.3f, 0.5f, 8));
        }
    }

    void Update()
    {
        if(triggerPopupText)
        {
            StartPopup();
        }
    }

    IEnumerator SpawnPopup(float delayTime, float distance, float speed)
    {
        float startingXpos = this.transform.position.x;
        float startingYpos = this.transform.position.y;

        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(this.transform.position.x, this.transform.position.y + distance), speed * Time.unscaledDeltaTime);
        yield return new WaitForSecondsRealtime(delayTime);

        this.transform.position = new Vector2(startingXpos, startingYpos);
        popUpText.gameObject.SetActive(false);
        triggerPopupText = false;
    }
}
