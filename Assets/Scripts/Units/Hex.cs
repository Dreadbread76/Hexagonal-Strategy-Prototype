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
    private Renderer rend;
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

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    private void Update()
    {
        if (highlighted || movable)
            rend.material = highlight;
        
        else if (selected)
            rend.material = selectedColour;

        else if (attachedRed)
            rend.material = attachedColour;
        
        else
            rend.material = teamColour;

        
    }
    #endregion
    #region On Mouse Exit
    private void OnMouseExit()
    {
        highlighted = false;

        
    }


    #endregion



}
