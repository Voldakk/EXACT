using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExactFramework
{
    public class Drag : MonoBehaviour
    {
        // Start is called before the first frame update
        bool mouseIsDown = false;
        Scene scene;
        Vector2 startPosition;


        void Start()
        {
            scene = SceneManager.GetActiveScene();
        }

        // Update is called once per frame
        void Update()
        {
            if (mouseIsDown)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = (pos);
                foreach (GameObject g in scene.GetRootGameObjects())
                {
                    // print(g.name);
                    if (g.name == "RedTile")
                    {
                        Vector2 touchPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D[] hits = Physics2D.RaycastAll(pos, new Vector2(0, 0), 0.01f);

                        for (int i = 0; i < hits.Length; i++)
                        {
                            if (hits[i].collider != null)
                            {
                                SpriteRenderer hit = hits[i].collider.GetComponent<SpriteRenderer>();
                                if (hit.name == "RedTile")
                                {
                                    print(hit);
                                    g.SendMessage("Toggle");
                                }

                            }
                        }
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
    }
}
