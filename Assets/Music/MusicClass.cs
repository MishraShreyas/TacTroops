 using UnityEngine;
 
 public class MusicClass : MonoBehaviour
 {
     private static MusicClass instance;
     private void Awake()
     {
        if (instance!=null && instance != this) Destroy(gameObject);
        else {
            DontDestroyOnLoad(transform.gameObject);
            instance=this;
        }
     }
 }