using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPlayer : MonoBehaviour
{
    public Animator mapAnim;
    public CamEvents _ce;

    Vector3 mouse_pos;
    Vector3 object_pos;
    Transform target;
    float angle;

    bool moving=false;
    Vector3 startPos;
    Vector3 endPos;
    float elapsedTime=0;

    MapSceneController _msc;
    // Start is called before the first frame update
    void Start()
    {
        _msc = MapSceneController.instance;
        target = transform;
        if (MapGen.instance.currNode>=0) transform.position = MapGen.instance.flatGrid[MapGen.instance.currNode].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!moving)
        {
            mouse_pos = Input.mousePosition;
            mouse_pos.z = 93; //The distance between the camera and object
            object_pos = Camera.main.WorldToScreenPoint(target.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
        } else
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime);
        }
    }

    public void MoveWrapper(Transform icon, string type) {StartCoroutine(move(icon, type));}

    IEnumerator move(Transform icon, string scene)
    {
        startPos = transform.position;
        endPos = icon.position;
        endPos.x -= startPos.x;
        endPos.y -= startPos.y;
        endPos.z -= startPos.z; 
        float angle = Mathf.Atan2(endPos.y, endPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));

        endPos = icon.position;
        moving = true;

        yield return new WaitForSeconds(1f);

        mapAnim.SetTrigger("next");

        yield return new WaitForSeconds(.3f);

        foreach(GameObject g in MapGen.instance.flatGrid) g.SetActive(false);
        foreach (LineRenderer line in FindObjectsOfType<LineRenderer>()) Destroy(line);
        GetComponent<Image>().enabled=false;
        _ce.map = scene;
    }
}
