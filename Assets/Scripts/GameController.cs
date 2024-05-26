using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public List<GameObject> aisles;
    public bool SpawnMode;
    public bool BuildMode;
    public int managerWriteUps = 0;
    public TextMeshProUGUI managerWriteUpText;
    // Start is called before the first frame update
    void Start()
    {
        managerWriteUps = 0;
        managerWriteUpText.text = "Manager Write-Ups " + managerWriteUps + " / 5";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void addWriteUp()
    {
        managerWriteUps++;
        if(managerWriteUps >= 5)
        {
            managerWriteUpText.text = "Game Over!";
        }
        else
        {
            managerWriteUpText.text = "Manager Write-Ups " + managerWriteUps + " / 5";
        }
    }
}
