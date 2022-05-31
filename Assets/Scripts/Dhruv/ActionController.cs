using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    public GameObject possessBtn;
    public GameObject unPossessBtn;
    public GameObject action1Btn;
    public GameObject action2Btn;

    private PlayerController player;


    [HideInInspector]
    public float timestamp;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            TogglePossessBtn();
            ToggleAction1Btn();
            ToggleAction2Btn();
        }
    }

    public void TogglePossessBtn()
    {
        possessBtn.SetActive(!possessBtn.activeSelf);
    }

    public void ToggleUnPossessBtn()
    {
        unPossessBtn.SetActive(!unPossessBtn.activeSelf);
    }

    public void ToggleAction1Btn()
    {
        action1Btn.SetActive(!action1Btn.activeSelf);
    }
     
   public void ToggleAction2Btn()
    {
        action2Btn.SetActive(!action2Btn.activeSelf);
    }
    
    public void PossessObjectBtn()
    {
        player.PossessObject();
    }
    

    public void UnPossessObjectBtn()
    {
        GetCurrentItem().UnPossessObject();
    }

    public void UseAttackBtn()
    {
        // GetCurrentItem().UnPossessObject();
        if (timestamp <= Time.time)
        {
            timestamp = Time.time + 0.7f;
            GetCurrentItem().DoKill();
        }
    }

    public void UseSoundBtn()
    {
        if (timestamp <= Time.time)
        {
            timestamp = Time.time + 0.7f;
            GetCurrentItem().DoAction();
        }
    }

    private PossessionController GetCurrentItem()
    {
        PossessionController[] myItems = FindObjectsOfType(typeof(PossessionController)) as PossessionController[];
        // Debug.Log ("Found " + myItems.Length + " instances with this script attached");
        foreach(PossessionController item in myItems)
        {
            if (item.isActiveAndEnabled)
            {
                return item;
            }
        }
        return null;
    }

}
