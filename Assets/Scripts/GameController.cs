using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class GameController : MonoBehaviour
{

    // the singleton of the gameController, readonly
    private static GameController _instance = null;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("GameControllerGlobal").AddComponent<GameController>();
            }
            return _instance;
        }
    }

    public DoorInfosDatas doorInfosDatas = null;

    private Cursor _cursor = null;
    public Cursor cursor
    {
        get
        {
            if (_cursor == null)
                _cursor = new GameObject("TheCursor").AddComponent<Cursor>();
            return _cursor;
        }
    }

    private int _score = 0;
    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    public class Country
    {
        string _name;
        bool[] _color = { false, false, false, false, false, false, false };

        public string Name { get { return _name; } }
        public bool[] Color { get { return _color; } }

        public Country(string n)
        {
            _name = n;
        }

        public void setColor(Entry.Color color)
        {
            _color[(int)color] = true;
        }
    }

    public Country[] countries = null;

    private const float LIMIT_TIMER = 60.0f;
    private float _gameTimer = LIMIT_TIMER;
    public float GameTimer
    {
        get { return _gameTimer; }
    }

    private int _indexCountries = 0;
    public int IndexCountries
    {
        get { return _indexCountries; }
        set { _indexCountries = value; }
    }

    private static GUIText _message = null;
    public static GUIText Message
    {
        get
        {
            if (_message == null)
            {
                _message = new GameObject("Message").AddComponent<GUIText>();
            }
            return _message;
        }
    }

    private bool[] _color;
    public void initColor()
    {
        _color = new bool[] { false, false, false, false, false, false, false };
    }

    public void setColor(Entry.Color color)
    {
        _color[(int)color] = true;
    }
    public bool isValidColor(Entry.Color color)
    {
        if (countries[_indexCountries].Color[(int)color] == true)
            return true;
        return false;
    }
    public bool completeColor()
    {
        for (int i = 0; i < 7; ++i)
        {
            if (_color[i] != countries[_indexCountries].Color[i])
                return false;
        }
        return true;
    }

    public int nbCountries = 0;

    public void regenerateIndexCountries()
    {
        _indexCountries = Random.Range(0, nbCountries);
    }

    public void displayMessage()
    {
        if (countries != null)
            Message.text = "Score: " + Score + "             Country's colors: " + countries[_indexCountries].Name + "                       Time: " + _gameTimer;
    }

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start()
    {
        ReadFile();

        Cursor curTmp = cursor;
        curTmp.enabled = true;
        curTmp.crosshair = (Texture2D)Resources.Load("crosshair");

        GUIText msg = Message;
        msg.text = "";
        msg.transform.position = new Vector3(0.0f, 1.0f, .0f);
        DontDestroyOnLoad(msg.gameObject);

        if (countries != null)
        {
            regenerateIndexCountries();
            initColor();
            displayMessage();
        }
    }

    bool _finished = false;
    public bool Finished { get { return _finished; } }

    void Update()
    {
        if (_finished == true)
            return;
        _gameTimer -= Time.deltaTime;
        if (_gameTimer <= 0.0f)
        {
            _gameTimer = 0.0f;
            _finished = true;
        }
        displayMessage();
    }

    void OnGUI()
    {
        if (_finished == true)
        {
            float height = 80.0f;
            float width = 100.0f;
            if (GUI.Button(new Rect(200.0f, Screen.height / 2.0f - height / 2.0f, width, height), "Try again"))
            {
                _finished = false;
                Score = 0;
                _gameTimer = LIMIT_TIMER;
                regenerateIndexCountries();
                initColor();
                displayMessage();
            }
            else if (GUI.Button(new Rect(Screen.width - 200.0f - width, Screen.height / 2.0f - height / 2.0f, width, height), "Quit"))
            {
                Application.Quit();
            }

        }
    }

    // avoid crash of the unity editor
    static private bool _exist = true;
    static public bool Exist { get { return _exist; } }

    void OnDestroy()
    {
        _exist = false;
        Debug.Log("exist false");
    }
    
    // File reading
    public struct ColorData
    {
        public string name;
        public Entry.Color col;
    }

    private ColorData[] _colors = {
                                  new ColorData() { name = "blue", col = Entry.Color.BLUE },
                                  new ColorData() { name = "black", col = Entry.Color.BLACK },
                                  new ColorData() { name = "green", col = Entry.Color.GREEN },
                                  new ColorData() { name = "yellow", col = Entry.Color.YELLOW },
                                  new ColorData() { name = "orange", col = Entry.Color.ORANGE },
                                  new ColorData() { name = "red", col = Entry.Color.RED },
                                  new ColorData() { name = "white", col = Entry.Color.WHITE }
                              };

    bool checkColor(string line, string s, Entry.Color c, int index)
    {
        if (line == s)
        {
            GameController.Instance.countries[index].setColor(c);
            return true;
        }
        return false;
    }

    public void ReadFile()
    {
        //  DontDestroyOnLoad(this.gameObject);
        TextAsset levelFile = (TextAsset)Resources.Load("countries", typeof(TextAsset));

        if (levelFile == null)
        {
            Debug.LogError("fail fichier");
        }

        StringReader readertmp = new StringReader(levelFile.text);
        if (readertmp == null)
        {
            Debug.LogError("map not found or not readable");
        }
        int len = 0;
        for (string line = readertmp.ReadLine(); line != null; line = readertmp.ReadLine())
            ++len;

        StringReader reader = new StringReader(levelFile.text);
        if (reader == null)
        {
            Debug.LogError("map not found or not readable");
        }

        GameController.Instance.countries = new GameController.Country[len];
        int i = 0;
        GameController.Instance.nbCountries = len;

        for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            line = line.Trim().ToLower();
            line = Regex.Replace(line, @"\s{2,}", " ");
            string[] sp = line.Split(' ');

            char[] name = sp[0].ToCharArray();
            name[0] = char.ToUpper(name[0]);

            GameController.Instance.countries[i] = new GameController.Country(new string(name));
            foreach (string s in sp)
            {
                foreach (ColorData cd in _colors)
                {
                    if (checkColor(s, cd.name, cd.col, i) == true)
                        break;
                }
            }
            ++i;
        }
    }
}
