using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGen : MonoBehaviour
{
    public static MapGen instance;
    private void Awake() {
        instance = this;
    }

    //public MapLoader _mapLoader;

    public Transform[] layers;
    public Sprite[] events;
    public GameObject player;
    public GameObject nextButton;

    public GameObject mapIcon;
    public Material lineMat;
    public int nodes = 0;
    public int currNode=-1;
    public int currLayer=0;

    public List<GameObject> flatGrid = new List<GameObject>();
    public List<List<GameObject>> grid = new List<List<GameObject>>();
    public List<int>[] adjList = new List<int>[10];
    

    MapLoader _mapLoader;

    // Start is called before the first frame update
    void Start()
    {
        _mapLoader = MapLoader.instance;
        if (PlayerPrefs.GetInt("ReloadMap") == 1) GenerateMap();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMap()
    {
        currNode = -1;
        List<GameObject> l1 = new List<GameObject>();

        int i=1;
        if (PlayerPrefs.GetInt("Map") >1) i=0;
        else{
            l1.Add(Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[0]));
            grid.Add(l1);
        }
        for (; i<4; i++)
        {
            int n = Random.Range(1,4);
            List<GameObject> l = new List<GameObject>();
            while (n>0)
            {
                GameObject temp = Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[i]);
                l.Add(temp);
                n--;
            }
            grid.Add(l);
        }
        foreach (List<GameObject> x in grid) foreach (GameObject y in x) flatGrid.Add(y);
        nodes = flatGrid.Count;
        GenerateEvents();
        AddAllListeners();
        CreateGraph();
        
        currLayer=0;
        PlayerPrefs.SetInt("ReloadMap", 0);
    }

    public void GenerateEvents()
    {
        string[] types = {"Troop", "Powerup", "Battle"};

        int layer = 1;
        if (PlayerPrefs.GetInt("Map") >1) layer =0;
        else flatGrid[0].GetComponent<IconController>().LoadType(types[0]);

        for (; layer<4; layer++)
        {
            List<bool> done = new List<bool>();
            for (int i=0; i<3; i++) done.Add(false);
            switch (grid[layer].Count)
            {
                case 3:
                    foreach(GameObject node in grid[layer])
                    {
                        int indx = Random.Range(0,3);
                        while (done[indx]) indx = Random.Range(0,3);
                        done[indx]=true;

                        node.GetComponent<IconController>().LoadType(types[indx]);
                    }
                    break;
                case 2:
                    done.RemoveAt(2);
                    if (layer%2!=0)
                    {
                        int indx = Random.Range(0,2);
                        done[indx]=true;
                        grid[layer][indx].GetComponent<IconController>().LoadType(types[2]);
                        indx = Random.Range(0, 3);
                        grid[layer][done.IndexOf(false)].GetComponent<IconController>().LoadType(types[indx]);
                    } else
                    {
                        foreach(GameObject node in grid[layer])
                        {
                            int indx = Random.Range(0,2);
                            while (done[indx]) indx = Random.Range(0,2);
                            done[indx]=true;

                            node.GetComponent<IconController>().LoadType(types[indx]);
                        }
                    }
                    break;
                default:
                    if (layer%2!=0) grid[layer][0].GetComponent<IconController>().LoadType(types[2]);
                    else{
                        int indx = Random.Range(0,2);
                        grid[layer][0].GetComponent<IconController>().LoadType(types[indx]);
                    }
                    break;
            }
        }
    }

    public void GenMapFromSave(List<string> layer1, List<string> layer2, List<string> layer3, List<string> layer4)
    {
        List<GameObject> l1 = new List<GameObject>();
        List<GameObject> l2 = new List<GameObject>();
        List<GameObject> l3 = new List<GameObject>();
        List<GameObject> l4 = new List<GameObject>();
        
        foreach (string s in layer1)
        {
            GameObject temp = Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[0]);
            l1.Add(temp);
            temp.GetComponent<IconController>().LoadType(s);
        }

        foreach (string s in layer2)
        {
            GameObject temp = Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[1]);
            l2.Add(temp);
            temp.GetComponent<IconController>().LoadType(s);
        }

        foreach (string s in layer3)
        {
            GameObject temp = Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[2]);
            l3.Add(temp);
            temp.GetComponent<IconController>().LoadType(s);
        }

        foreach (string s in layer4)
        {
            GameObject temp = Instantiate(mapIcon, Vector3.zero, Quaternion.identity, layers[3]);
            l4.Add(temp);
            temp.GetComponent<IconController>().LoadType(s);
        }

        grid.Add(l1);
        grid.Add(l2);
        grid.Add(l3);
        grid.Add(l4);

        foreach (List<GameObject> x in grid) foreach (GameObject y in x) flatGrid.Add(y);
        nodes = flatGrid.Count;
        SetButton(currNode, true);
        AddAllListeners();

        if (currNode>=0) if (grid[3].Contains(flatGrid[currNode])) nextButton.SetActive(true);
    }

    public void GenConnections()
    {
        createLineFromGraph();
        /*for (int layer = 1; layer<4; layer++)
        {
            int l1 = grid[layer-1].Count;
            int l2 = grid[layer].Count;

            if (l1 == 1 || l2 == 1)
            {
                for (int i=0; i<l1; i++) for (int j=0; j<l2; j++) CreateLine(grid[layer-1][i].transform.position, grid[layer][j].transform.position);
            }
            else if (l1==3 && l2==3)
            {
                //int mx = 3;
                CreateLine(grid[layer-1][0].transform.position, grid[layer][0].transform.position);
                CreateLine(grid[layer-1][2].transform.position, grid[layer][2].transform.position);
                for (int i=0; i<3; i++)
                {
                    int x = Random.Range(0,2);
                    if (x==1)
                    {
                        CreateLine(grid[layer-1][1].transform.position, grid[layer][i].transform.position);
                        //mx--;
                    } else if (i==1)
                    {
                        x = Random.Range(0,3);
                        CreateLine(grid[layer-1][x].transform.position, grid[layer][i].transform.position);
                        //mx--;
                    }
                }
                for (int i=0; i<3; i++)
                {
                    int x = Random.Range(0,2);
                    if (x==1)
                    {
                        CreateLine(grid[layer-1][i].transform.position, grid[layer][1].transform.position);
                        //mx--;
                    }
                }
            } else  if (l1 ==2 && l2==2)
            {
                int x = Random.Range(0,2);
                int y = Random.Range(0,2);

                for (int i=0; i<l1; i++) for (int j=0; j<l2; j++) 
                {
                    if (i==x && j==y) continue;
                    CreateLine(grid[layer-1][i].transform.position, grid[layer][j].transform.position);
                }
            } else if (l1==3)
            {
                int x = Random.Range(0,3);
                CreateLine(grid[layer-1][0].transform.position, grid[layer][0].transform.position);
                CreateLine(grid[layer-1][2].transform.position, grid[layer][1].transform.position);
                if (x==0 || x==2) CreateLine(grid[layer-1][1].transform.position, grid[layer][0].transform.position);
                if (x==1 || x==2) CreateLine(grid[layer-1][1].transform.position, grid[layer][1].transform.position);
            } else 
            {
                int x = Random.Range(0,3);
                CreateLine(grid[layer-1][0].transform.position, grid[layer][0].transform.position);
                CreateLine(grid[layer-1][1].transform.position, grid[layer][2].transform.position);
                if (x==0 || x==2) CreateLine(grid[layer-1][0].transform.position, grid[layer][1].transform.position);
                if (x==1 || x==2) CreateLine(grid[layer-1][1].transform.position, grid[layer][1].transform.position);
            }
        }*/

        if (currNode < 0) foreach(GameObject g in grid[0]) g.GetComponent<Button>().enabled=true;
        //else player.transform.position = flatGrid[currNode].transform.position;
    }

    void CreateLine(Vector3 p1, Vector3 p2)
    {
        LineRenderer lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = lineMat;

        lineRenderer.SetPosition(0, p1);
        lineRenderer.SetPosition(1, p2);
    }

    void CreateGraph()
    {
        for (int i=0; i<10; i++) adjList[i] = new List<int>();
        int firstOnLayer = 0;
        for (int layer = 1; layer<4; layer++)
        {
            int l1 = grid[layer-1].Count;
            int l2 = grid[layer].Count;

            if (l1 == 1)
            {
                for (int i=0; i<l2; i++) adjList[firstOnLayer].Add(firstOnLayer+1+i);
            }
            else if (l2==1)
            {
                for (int i=0; i<l1; i++) adjList[firstOnLayer+i].Add(firstOnLayer+l1);
            }
            else if (l1==3 && l2==3)
            {
                adjList[firstOnLayer].Add(firstOnLayer+l1);
                adjList[firstOnLayer+2].Add(firstOnLayer+5);
                for (int i=0; i<3; i++)
                {
                    int x = Random.Range(0,2);
                    if (x==1) adjList[firstOnLayer+1].Add(firstOnLayer+l1+i);
                    else if (i==1)
                    {
                        x = Random.Range(0,3);
                        adjList[firstOnLayer+x].Add(firstOnLayer+l1+1);
                    }
                }
                for (int i=0; i<3; i++)
                {
                    int x = Random.Range(0,2);
                    if (x==1 && !adjList[firstOnLayer+i].Contains(firstOnLayer+l1+1)) adjList[firstOnLayer+i].Add(firstOnLayer+l1+1);
                }
                if (adjList[firstOnLayer+1].Count == 0)
                {
                    int x = Random.Range(0,3);
                    adjList[firstOnLayer+1].Add(firstOnLayer+l1+x);
                }
            } else  if (l1 ==2 && l2==2)
            {
                int x = Random.Range(0,2);
                int y = Random.Range(0,2);

                for (int i=0; i<l1; i++) for (int j=0; j<l2; j++) 
                {
                    if (i==x && j==y) continue;
                    adjList[firstOnLayer+i].Add(firstOnLayer+l1+j);
                }
            } else if (l1==3)
            {
                int x = Random.Range(0,3);
                adjList[firstOnLayer].Add(firstOnLayer+l1);
                adjList[firstOnLayer+2].Add(firstOnLayer+l1+1);
                if (x==0 || x==2) adjList[firstOnLayer+1].Add(firstOnLayer+l1);
                if (x==1 || x==2) adjList[firstOnLayer+1].Add(firstOnLayer+l1+1);
            } else 
            {
                int x = Random.Range(0,3);
                adjList[firstOnLayer].Add(firstOnLayer+l1);
                adjList[firstOnLayer+1].Add(firstOnLayer+l1+2);
                if (x==0 || x==2) adjList[firstOnLayer].Add(firstOnLayer+l1+1);
                if (x==1 || x==2) adjList[firstOnLayer+1].Add(firstOnLayer+l1+1);
            }

            firstOnLayer += l1;
        }

        /*for (int i=0; i<nodes; i++)
        {
            foreach (int x in adjList[i]) Debug.Log(i + "-->" +x);
        }*/
    }

    void createLineFromGraph()
    {
        for(int u=0; u<nodes; u++)
        {
            foreach(int v in adjList[u]) CreateLine(flatGrid[u].transform.position, flatGrid[v].transform.position);
        }
    }

    void AddAllListeners()
    {
        for (int i=0; i<nodes; i++)
        {
            int x = new int();
            x=i;
            flatGrid[x].GetComponent<Button>().onClick.AddListener(delegate {SetButton(x);});
            flatGrid[x].GetComponent<Button>().onClick.AddListener(delegate {_mapLoader.SaveData();});

            string type = flatGrid[x].GetComponent<IconController>().type;
            flatGrid[x].GetComponent<Button>().onClick.AddListener(delegate {player.GetComponent<MapPlayer>().MoveWrapper(flatGrid[x].transform, type);});
        }
    }

    public void SetButton(int u, bool loading=false)
    {
        if (u<0) return;
        if (!loading) 
        {
            currLayer++;
            _mapLoader.mapData = new MapData(adjList, grid, u, currNode, currLayer);
        }
        currNode=u;
        PlayerPrefs.SetInt("ActionComplete", 0);
        flatGrid[u].GetComponent<Button>().enabled=false;
        DeactivateIconsDfs(u);
        foreach (int x in adjList[u])
        {
            //Debug.Log(x);
            flatGrid[x].GetComponent<Button>().enabled = true;
        }
    }

    void DeactivateIconsDfs(int u)
    {
        List<bool> vis = new List<bool>();
        for (int i=0; i<nodes; i++) vis.Add(false);

        dfs(u, vis);
        for (int i=0; i<nodes; i++)
        {
            if (!vis[i]) flatGrid[i].GetComponent<IconController>().GrayOut();
        }
    }

    void dfs(int u, List<bool> vis)
    {
        vis[u] = true;
        foreach (int v in adjList[u])
        {
            if (!vis[v]) dfs(v, vis);
        }
    }
}
