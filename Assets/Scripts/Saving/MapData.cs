using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData
{
    public int currNode;
    public int prevNode;
    public int currLayer;

    public List<string> layer1 = new List<string>();
    public List<string> layer2 = new List<string>();
    public List<string> layer3 = new List<string>();
    public List<string> layer4 = new List<string>();

    public List<int> node1 = new List<int>();
    public List<int> node2 = new List<int>();
    public List<int> node3 = new List<int>();
    public List<int> node4 = new List<int>();
    public List<int> node5 = new List<int>();
    public List<int> node6 = new List<int>();
    public List<int> node7 = new List<int>();
    public List<int> node8 = new List<int>();
    public List<int> node9 = new List<int>();
    public List<int> node10 = new List<int>();

    public MapData(List<int>[] al, List<List<GameObject>> g, int curr, int layer)
    {
        currNode = curr;
        prevNode = curr;

        currLayer = layer;

        foreach (GameObject x in g[0]) layer1.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[1]) layer2.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[2]) layer3.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[3]) layer4.Add(x.GetComponent<IconController>().type);

        node1 = al[0];
        node2 = al[1];
        node3 = al[2];
        node4 = al[3];
        node5 = al[4];
        node6 = al[5];
        node7 = al[6];
        node8 = al[7];
        node9 = al[8];
        node10 = al[9];
    }

    public MapData(List<int>[] al, List<List<GameObject>> g, int curr, int prev, int layer)
    {
        currNode = curr;
        prevNode = prev;
        
        currLayer = layer;

        foreach (GameObject x in g[0]) layer1.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[1]) layer2.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[2]) layer3.Add(x.GetComponent<IconController>().type);
        foreach (GameObject x in g[3]) layer4.Add(x.GetComponent<IconController>().type);

        node1 = al[0];
        node2 = al[1];
        node3 = al[2];
        node4 = al[3];
        node5 = al[4];
        node6 = al[5];
        node7 = al[6];
        node8 = al[7];
        node9 = al[8];
        node10 = al[9];
    }
}
