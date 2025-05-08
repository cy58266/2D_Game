using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tips : MonoBehaviour
{

    public GameObject introduce;
    public GameObject closeBtn;
    public GameObject information;

    public int flags = 0;

    public void Open()
    {
        introduce.SetActive(true);
    }

    public void Close()
    {
        introduce.SetActive(false);
    }

    public void Next()
    {
        if (flags >= 1)
        {
            information.transform.GetChild(flags-1).gameObject.SetActive(false);
        }
        information.transform.GetChild(flags).gameObject.SetActive(true);
        flags++;
    }

}
