using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExactFramework
{
    public class ExactRFIDBase : MonoBehaviour
    {
        // Start is called before the first frame update
        bool mouseIsDown = false;
        Scene scene;
        Vector2 startPosition;
        TwinObject selectedTile = null;
        TwinObject previousSelectedTile = null;
        SpriteRenderer mySprite = null;
        public string RFIDInHex = "";

        void Start()
        {
            scene = SceneManager.GetActiveScene();
        }

        // Update is called once per frame
        void Update()
        {
            previousSelectedTile = selectedTile;
            selectedTile = null;
            if (mouseIsDown)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = (pos);

                Vector2 touchPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D[] hits = Physics2D.RaycastAll(pos, new Vector2(0, 0), 0.01f);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider != null)
                    {
                        SpriteRenderer hit = hits[i].collider.GetComponent<SpriteRenderer>();
                        TwinObject t2 = hit.GetComponent<TwinObject>();
                        if (t2 != null)
                        {
                            //print(hit);
                            //t2.SendMessage("SetColor",Color.red);
                            selectedTile = t2;
                        }

                    }
                }
            }
            if (previousSelectedTile == null)
            {
                if (selectedTile != null)
                {
                    // selectedTile.SendMessage("SetColor", Color.red);
                    selectedTile.SendMessage("OnRFIDEnter", RFIDInHex);
                }
            }
            else
            {
                if (selectedTile != previousSelectedTile)
                {
                    // previousSelectedTile.SendMessage("SetColor", Color.green);
                    previousSelectedTile.SendMessage("OnRFIDLeave", RFIDInHex);
                    if (selectedTile != null)
                    {
                        // selectedTile.SendMessage("SetColor", Color.red);
                        selectedTile.SendMessage("OnRFIDEnter", RFIDInHex);
                    }
                }
            }


        }

        void OnMouseDown()
        {
            //  var itsRenderer = gameObject.GetComponent<SpriteRenderer>();
            //  itsRenderer.color = green;
            mouseIsDown = true;
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        void OnMouseUp()
        {
            //  var itsRenderer = gameObject.GetComponent<SpriteRenderer>();
            //  itsRenderer.color = green;
            mouseIsDown = false;
            transform.position = (startPosition);
        }

        public Color GetColor()
        {
            mySprite = GetComponent<SpriteRenderer>();
            if (mySprite != null)
            {
                return mySprite.color;
            }
            else
            {
                return Color.black;
            }
        }
    }
}
