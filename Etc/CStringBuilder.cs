using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class CStringBuilder : MonoBehaviour {

    public static StringBuilder _sb = new StringBuilder("");

    public static void StringBuilderRefresh()
    {
        _sb = new StringBuilder("");
    }
}
