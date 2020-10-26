using UnityEngine;

public class NotesController : MonoBehaviour
{
    GameController gameController;
    public GameObject[] MyNotes;
    void Start()
    {
        gameController = GameObject.Find("Master").GetComponent<GameController>();
        Invoke("GetChild", 1f);
    }

    void GetChild()
    {
        int i = 0;
        MyNotes = new GameObject[gameController.TotalNotesNumber];
        foreach (Transform child in transform)
        {
            MyNotes[i] = child.gameObject;
            i++;
        }
    }

    public void NotesActive(GameObject obj)
    {
        obj.SetActive(false);
    }
}
