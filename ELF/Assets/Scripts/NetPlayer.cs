using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;//引入socket命名空间
using System.Threading;
using System.Text;
using UnityEngine.SceneManagement;

public class NetPlayer : MonoBehaviour
{
    //添加两个按钮用于发送数据和关闭连接
    public Button sen;
    public Button col;
    //输入文本
    public InputField inputText;
    // Start is called before the first frame update
    void Start()
    {
        sen.onClick.AddListener(send_smg);//发送
        col.onClick.AddListener(close_btnClick);//关闭
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void send_smg()
    {
        Send(inputText.text);
    }
    public void close_btnClick()
    {
        close();
    }

    /// <summary>
    /// 连接服务器
    /// </summary>
    static Socket socket_client;
    public static void ConnectServer()
    {
        try
        {
            IPAddress pAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint pEndPoint = new IPEndPoint(pAddress, 52315);
            socket_client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket_client.Connect(pEndPoint);
            Debug.Log("连接成功");
            //创建线程，执行读取服务器消息
            Thread c_thread = new Thread(Received);
            c_thread.IsBackground = true;
            c_thread.Start();
        }
        catch (System.Exception)
        {

            Debug.Log("IP端口号错误或者服务器未开启");
        }
    }

    /// <summary>
    /// 读取服务器消息
    /// </summary>
    public static void Received()
    {
        while (true)
        {
            try
            {
                byte[] buffer = new byte[1024];
                int len = socket_client.Receive(buffer);
                if (len == 0) break;
                string str = Encoding.UTF8.GetString(buffer, 0, len);
                Debug.Log("客户端打印服务器返回消息：" + socket_client.RemoteEndPoint + ":" + str);
            }
            catch (System.Exception)
            {

                throw;
            }

        }
    }
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="msg"></param>
    public static void Send(string msg)
    {
        try
        {
            byte[] buffer = new byte[1024];
            buffer = Encoding.UTF8.GetBytes(msg);
            socket_client.Send(buffer);
        }
        catch (System.Exception)
        {

            Debug.Log("未连接");
        }
    }
    /// <summary>
    /// 关闭连接
    /// </summary>
    public static void close()
    {
        try
        {
            socket_client.Close();
            Debug.Log("关闭客户端连接");
            SceneManager.LoadScene("control");
        }
        catch (System.Exception)
        {
            Debug.Log("未连接");
        }
    }
}