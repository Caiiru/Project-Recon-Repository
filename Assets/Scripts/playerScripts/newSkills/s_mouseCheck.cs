using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_mouseCheck : MonoBehaviour
{
    #region Singleton
    private static s_mouseCheck _instance;
    public static s_mouseCheck Instance { get { return _instance; } }
    private void Awake()
    {
        if(_instance !=null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        
    }
}
