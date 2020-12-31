using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OrderManager : MonoBehaviour
{
    //public List<Text> loadList;
    //public InputField locationCode;
    //public InputField quantity;
    //public Dropdown priority;
    //public InputField serial;
    //public Image image;

    //public Text PrevoiusJson;
    public GameObject prefabItem;
    public GameObject itemTable;

    public string oldJson = "";
    public string newJson = "";
    public List<GameObject> itemList;
        
    [SerializeField]
    public Orders orders;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<GameObject>();
        orders = new Orders();
        orders.ordersList = new List<Order>();
    }

    // Update is called once per frame
    void Update()
    {
        ReadData();
    }

    #region Methods
    public void SaveDataButton(Sprite newImage)
    {
        string dataJson = "";
        Order newOrder = NewOrder(newImage);

        orders.ordersList.Add(newOrder);

        dataJson = JsonUtility.ToJson(orders);
        Debug.Log(dataJson);

        File.WriteAllText(Application.persistentDataPath + "/jsonDataBase.js", dataJson);
    }


    public void ReadData()
    {
        if (File.Exists(Application.persistentDataPath + "/jsonDataBase.js"))
        {
            string dataList = File.ReadAllText(Application.persistentDataPath + "/jsonDataBase.js");

            newJson = dataList;

            if(newJson != oldJson)
            {
                //PrevoiusJson.text = dataList;
                orders = JsonUtility.FromJson<Orders>(dataList);

                if (itemList.Count > 0)
                {
                    foreach (GameObject o in itemList)
                    {
                        GameObject.Destroy(o);
                    }

                    itemList.Clear();
                }

                foreach (Order o in orders.ordersList)
                {
                    ObjectItem(o);
                }

                oldJson = newJson;
            }
        }
        else
            Debug.Log("NO DATA");
    }

    public void ObjectItem(Order itemOrder)
    {
        GameObject newItem = GameObject.Instantiate(prefabItem, itemTable.transform);

        newItem.transform.GetChild(1).transform.GetChild(0).GetComponent<Text>().text = itemOrder.locationCode.ToString();
        newItem.transform.GetChild(1).transform.GetChild(1).GetComponent<Text>().text = itemOrder.quantity.ToString();
        newItem.transform.GetChild(1).transform.GetChild(2).GetComponent<Text>().text = itemOrder.priority.ToString();
        newItem.transform.GetChild(1).transform.GetChild(3).GetComponent<Text>().text = itemOrder.serial.ToString();
        newItem.transform.GetChild(1).transform.GetChild(4).GetComponent<Text>().text = itemOrder.urlImage;

        itemList.Add(newItem);
    }

    public Sprite InstanceSprite(string path, Rect rectImage, Vector2 vector2Image)
    {
        byte[] data = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        texture.LoadImage(data);
        texture.name = Path.GetFileNameWithoutExtension(path);

        Sprite newSprite = Sprite.Create(texture, rectImage, vector2Image);

        return newSprite;
    }

    public Order NewOrder(Sprite _image)
    {
        Order newOrder = new Order();

        newOrder.locationCode = UnityEngine.Random.Range(426000, 526001);
        newOrder.quantity = UnityEngine.Random.Range(1, 1001);
        newOrder.priority = (Order.PriorityLevel)UnityEngine.Random.Range(0, 4);
        newOrder.serial = UnityEngine.Random.Range(10000, 50001);
        if (_image)
        {
            newOrder.image = _image;
            newOrder.urlImage = Application.dataPath + "/" + _image.name + ".png";
        }
        else
        {
            newOrder.image = null;
            newOrder.urlImage = "";
        }


        return newOrder;
    }
    #endregion

    [Serializable]
    public class Order
    {
        public enum PriorityLevel { low, normal, medium, high }

        public int locationCode;
        public int quantity;
        public PriorityLevel priority;
        public int serial;
        [SerializeField]
        public Sprite image;
        public string urlImage;
    }

    [Serializable]
    public class Orders
    {
        public List<Order> ordersList;
    }
}


