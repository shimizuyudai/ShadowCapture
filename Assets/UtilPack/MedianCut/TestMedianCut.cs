using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Median;
using UnityEngine.UI;
using System.Linq;

public class TestMedianCut : MonoBehaviour {
    MedianCut mediancut;
    [SerializeField]
    Texture2D texture;
    [SerializeField]
    int num;
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform parent;
    [SerializeField]
    float areaSize;
    [SerializeField]
    Vector3 offset;
    List<GameObject> goList = new List<GameObject>();
    [SerializeField]
    Text text;
    [SerializeField]
    bool isDebug;
    [SerializeField]
    MedianCut.Mode mode;

    // Use this for initialization
    void Start()
    {
        Test();
        //Init(texture);
        if (isDebug)
        {
            Init(texture);
        }
    }

    public async void Init(Texture2D texture)
    {
        mediancut = new MedianCut();
        mediancut.mode = mode;
        mediancut.OnError += Mediancut_OnError;
        mediancut.OnCut += Mediancut_OnCut;
        var pixels = texture.GetPixels();
        var testColors = new List<Color>();
        var colors = await mediancut.Cut(pixels, num);

#if !UNITY_EDITOR
        Destroy(texture);
#endif
        print("complete");
    }


    void Test()
    {
        //print(5/2);
        //var temp = new int[] {1,2,3,4};
        //var m = 0f;
        //if (temp.Length % 2 == 0)
        //{
        //    var i = temp.Length / 2 - 1;
        //    var j = temp.Length / 2 ;
        //    var a = (float)temp[i];
        //    var b = (float)temp[j];
        //    m = (a+b) / 2f;
        //    print("even" + a+b);
        //}
        //else
        //{
        //    var i = temp.Length / 2;
        //    m = temp[i];
        //    print("odd");
        //}
        //print(m);
    }
    

    public void Generate(Median.Cube cube)
    {
        if (!this.gameObject.activeInHierarchy) return;

        var go = GameObject.Instantiate(prefab) as GameObject;
        go.transform.SetParent(parent, false);
        var centerColor = cube.CenterColor;
        go.GetComponent<Renderer>().material.color = centerColor;
        go.transform.position = offset + new Vector3(
            EMath.Map(centerColor.r, 0f, 1f, -areaSize / 2f, areaSize / 2f),
            EMath.Map(centerColor.g, 0f, 1f, -areaSize / 2f, areaSize / 2f),
            EMath.Map(centerColor.b, 0f, 1f, -areaSize / 2f, areaSize / 2f)
            );
        go.transform.localScale = new Vector3(cube.RLength, cube.GLength, cube.BLength)*areaSize;
        var rigidBody = go.GetComponent<Rigidbody>();
        rigidBody.useGravity = true;
        goList.Add(go);
    }

    private void Mediancut_OnCut(List<Cube> obj)
    {
        mediancut.IsCutting = false;
        text.text = obj.Count.ToString();
        Clear();
        //print(obj.Max(e => e.MaxLength));
        foreach (var cube in obj)
        {
            Generate(cube);
        }
        if (isDebug)
        {
            mediancut.IsCutting = true;
        }

    }

    public void Dispose()
    {
        if (mediancut != null)
        {
            mediancut.OnError -= Mediancut_OnError;
            mediancut.OnCut -= Mediancut_OnCut;
            mediancut = null;
        }
        foreach (var go in goList)
        {
            Destroy(go);
        }
    }

    public void Clean()
    {
        if (text == null) return;
        text.text = string.Empty;
    }

    void Clear()
    {
        foreach (var go in goList)
        {
            Destroy(go);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Resume();
        }
    }

    public void Resume()
    {
        if (mediancut != null)
        {
            mediancut.IsCutting = true;
        }
    }

    private void Mediancut_OnError(string obj)
    {
        print(obj);
    }
    

    private void OnDestroy()
    {
        Dispose();
    }
}
