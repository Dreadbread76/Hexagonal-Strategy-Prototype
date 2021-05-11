using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units;

public class Hex : MonoBehaviour
{
    #region Variables
    [Header("Stats")]
    [SerializeField]
    public bool blue;
    [SerializeField]
    public bool spawnable;
    public bool highlighted;
    public bool movable;
    public bool selected;
    public bool attachedRed;
    public bool oddLane;

    [Header("Materials")]
    public Material highlight;
    public Material teamColour;
    public Material selectedColour;
    public Material attachedColour;

    [Header("Vectors and Positions")]
    public Transform attachPoint;
    public GameObject attachedObject;
    public int horiPoint;
    public int vertPoint;
    #endregion
    #region Update
    private void Update()
    {
        if (highlighted || movable)
        {
            this.GetComponent<Renderer>().material = highlight;
        }
        else if (selected)
        {
            this.GetComponent<Renderer>().material = selectedColour;
        }
        else if (attachedRed)
        {
            this.GetComponent<Renderer>().material = attachedColour;
        }
        else
        {
            this.GetComponent<Renderer>().material = teamColour;
        }
        
    }
    #endregion
    #region On Mouse Exit
    private void OnMouseExit()
    {
        highlighted = false;

        
    }


    #endregion



}
