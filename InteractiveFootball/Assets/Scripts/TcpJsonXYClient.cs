using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class XYMessage
{
    public float x;
    public float y;
}

public class TcpJsonXYClient : MonoBehaviour
{
    [SerializeField] private string host = "127.0.0.1";
    [SerializeField] private int port = 28123;

    public event Action<XYMessage> PositionReceived;

    private readonly ConcurrentQueue<XYMessage> pending = new ConcurrentQueue<XYMessage>();
    private Thread thread;
    private volatile bool running;
    private TcpClient client;

    private void OnEnable()
    {
        running = true;
        thread = new Thread(ReadLoop) { IsBackground = true };
        thread.Start();
    }

    private void Update()
    {
        while (pending.TryDequeue(out var message))
        {
            PositionReceived?.Invoke(message);
        }
    }

    private void OnDisable()
    {
        running = false;
        client?.Close();
        client = null;
    }

    private void ReadLoop()
    {
        while (running)
        {
            try
            {
                client = new TcpClient();
                client.NoDelay = true;
                client.Connect(host, port);

                using var stream = client.GetStream();
                using var reader = new StreamReader(stream, Encoding.UTF8);

                while (running)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    var message = JsonUtility.FromJson<XYMessage>(line);
                    if (message != null)
                    {
                        pending.Enqueue(message);
                    }
                }
            }
            catch
            {
                Thread.Sleep(500);
            }
            finally
            {
                client?.Close();
                client = null;
            }
        }
    }
}