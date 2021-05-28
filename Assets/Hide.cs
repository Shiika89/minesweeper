using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    [SerializeField] GameObject m_button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ButtanOpen();
    }

    void ButtanOpen()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Destroy(m_button);
        }
    }
}
