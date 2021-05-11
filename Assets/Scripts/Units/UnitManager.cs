using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Layout;

namespace Units
{
    public class UnitManager : MonoBehaviour
    {
        #region Variables
        public Camera cam;
        public GameObject[] units = new GameObject[7];
        public GameObject dummyBot;
        public bool human;
        public Material highlight;
        public Material defaultTeam;
        public Hex currentHex;
        public bool selectMode;
        public bool spawnMode;
        [SerializeField]
        private GameObject selectedUnit;
        private Hex selectedHex;
        #endregion
        #region Start
        void Start()
        {
            spawnMode = true;
            StartCoroutine(SelectMode());
#if UNITY_EDITOR

            Hex enemyHex = LayoutManager.hexPoints[4, 4].GetComponent<Hex>();
            Instantiate(dummyBot, enemyHex.attachPoint.position, enemyHex.attachPoint.rotation);
            enemyHex.attachedObject = dummyBot;

            BoardUnit dummyScript = dummyBot.GetComponent<BoardUnit>();
            dummyScript.attachedHex = enemyHex;
            dummyScript.dummy = true;

#endif
        }
        #endregion
        #region Unit Spawn Mode
        IEnumerator SpawnMode(int unitIndex)
        {
            //when in spawnmode
            spawnMode = true;

            while (spawnMode)
            {

                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    //Get the Hex script of the object hit raycast
                    GameObject currentObj = hit.transform.gameObject;
                    currentHex = currentObj.GetComponent<Hex>();

                    //Only highlight spawnable blue hexes to spawn
                    if (currentHex.blue && currentHex.spawnable && currentHex.attachedObject == null) 
                    {
                        currentHex.highlighted = true;

                        //Spawn unit when mouse is clicked
                        if (Input.GetMouseButtonDown(0))
                        {

                            GameObject newUnit = Instantiate(units[unitIndex], currentHex.attachPoint.position, currentHex.attachPoint.rotation);

                            currentHex.attachedObject = newUnit;
                            BoardUnit unitFunctions = newUnit.GetComponent<BoardUnit>();
                            unitFunctions.attachedHex = currentHex;

                            spawnMode = false;
                            StartCoroutine(SelectMode());
                            selectMode = true;
                            yield return null;

                           
                        }
                        
                    }



                }
                //Cancel Spawnmode
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    spawnMode = false;
                    StartCoroutine(SelectMode());
                    selectMode = true;
                    yield return null;


                }

                yield return null;
            }


        }
        #endregion
        #region Unit Select Mode
        IEnumerator SelectMode()
        {

            
            selectMode = true;
            //When select mode is active
            while (selectMode)
            {
                //Shoot Ray
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                //Get the hexes you can move to
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject currentObj = hit.transform.gameObject;
                    currentHex = currentObj.GetComponent<Hex>();

                    currentHex.highlighted = true;

                    // If the clicked hex has a unit attached to it, select the unit
                    if (Input.GetMouseButtonDown(0) && currentHex.attachedObject != null)
                    {
                        
                        
                        selectedUnit = currentHex.gameObject;
                        Debug.Log(selectedUnit);
                        SelectUnit();
                    }
                    
                    if(Input.GetMouseButtonDown(0) && selectedUnit != null)
                    {
                        // Selecting movable hexes
                        if (currentHex.movable)
                        {
                            Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                            BoardUnit unit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                            currentHex.attachedObject = unit.gameObject;
                            unit.ChangeToMove(currentHex);

                            DeselectUnit();
                        }
                        // Selecting hexes with an enemy on it
                        if (currentHex.attachedRed)
                        {
                            Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                            BoardUnit yourUnit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                            BoardUnit enemyUnit = currentHex.attachedObject.GetComponent<BoardUnit>();

                            enemyUnit.currentHealth -= yourUnit.unitDamage;
                            Debug.Log(enemyUnit.currentHealth);
                            yourUnit.ChangeToAttack();

                            DeselectUnit();
                        }
                       
                    }
                   /* if (Input.GetMouseButtonDown(0) && selectedUnit != null)
                    {
                        Hex newCurrentHex = selectedUnit.GetComponent<Hex>();
                        BoardUnit unit = newCurrentHex.attachedObject.GetComponent<BoardUnit>();
                        currentHex.attachedObject = unit.gameObject;
                        unit.ChangeToMove(currentHex);
                        DeselectUnit();
                    }
                    */
                }
                //Deselection
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    DeselectUnit();
                    yield return null;


                }

                yield return null;
            }
        }
        #endregion
        #region Select
        //Select Hex
        public void SelectUnit()
        {
            
            currentHex.selected = true;
            selectedHex = currentHex;

        }
        #endregion
        #region Deselect
        //Deselect Hex
        public void DeselectUnit()
        {
            selectedHex.selected = false;
            selectedHex = null;
        }
        #endregion
        // Update is called once per frame
        void Update()
        {
            /* if (currentHex != null && currentHex.attachedObject != null)
             {

                 if (currentHex.gameObject == selectedUnit)
                 {

                     currentHex.selected = true;
                 }
                 else if (currentHex.gameObject != selectedUnit)
                 {
                     currentHex.selected = false;
                 }

             }
             */

            #region Dev Tools
#if UNITY_EDITOR
            //Testing Spawnmode
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                selectMode = false;
                StopCoroutine(SelectMode());
                StartCoroutine(SpawnMode(0));
            }
          
#endif
            #endregion
        }


    }
}

