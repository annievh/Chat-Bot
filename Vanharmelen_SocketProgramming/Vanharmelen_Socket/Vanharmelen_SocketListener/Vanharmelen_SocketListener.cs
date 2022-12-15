using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Vanharmelen_SocketListener
{
    class Vanharmelen_SocketListener
    {
        public static string data = null;
        public static string eof = "<EOF>";

        public static void StartListening()
        {

            byte[] bytes = new Byte[1024];
            string message = "";
            int num = 0;
            string[] colors = { "red", "orange", "yellow", "green", "blue", "purple" };


            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);


                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    Socket handler = listener.Accept();

                    while (true)
                    {
                        data = null;


                        while (true)
                        {
                            int bytesRec = handler.Receive(bytes);
                            data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                            if (data.IndexOf(eof) > -1)
                            {
                                break;
                            }
                        }

                        Console.WriteLine("Text received : {0}", data);

                        message = data.Substring(0, data.IndexOf("<EOF>"));


                        //colour
                        if (data.ToLower().IndexOf("red") > -1)
                        {
                            message = "red";
                        }
                        else if (data.ToLower().IndexOf("orange") > -1)
                        {
                            message = "orange";
                        }
                        else if (data.ToLower().IndexOf("yellow") > -1)
                        {
                            message = "yellow";
                        }
                        else if (data.ToLower().IndexOf("green") > -1)
                        {
                            message = "green";
                        }
                        else if (data.ToLower().IndexOf("blue") > -1)
                        {
                            message = "blue";
                        }
                        else if (data.ToLower().IndexOf("purple") > -1)
                        {
                            message = "purple";
                        }



                        switch (message)
                        {
                            case "red":
                                data = "Strawberries are the colour red." + eof;
                                break;
                            case "orange":
                                data = "Oranges happen to be the colour orange." + eof;
                                break;
                            case "yellow":
                                data = "Bananas are the colour yellow." + eof;
                                break;
                            case "green":
                                data = "Apples are sometimes green." + eof;
                                break;
                            case "blue":
                                data = "A blueberry is a berry that is blue." + eof;
                                break;
                            case "purple":
                                data = "The vegetable eggplant is purple." + eof;
                                break;


                        }

                        //respond with square root
                        int.TryParse(message, out num);
                        if (num > 0)
                        {
                            double sqrt = num * num;
                            data = "The square root of " + num + " is " + sqrt;
                        }

                        //respond buzz
                        else if (data.ToLower().IndexOf("fizz") > -1)
                        {
                            data = "buzz";
                        }

                        //replace silver
                        if (data.ToLower().IndexOf("silver") > 1)
                        {
                            data = data.Replace("silver", "gold");
                        }
                        //filter
                        if (data.ToLower().IndexOf("hard coded") > 1)
                        {
                            data = data.Replace("hard coded", "$%^# %$^@#") +
                                "\n" +
                                " hard coding is bad development.";
                        }

                        if (data == "end of stream" + eof)
                        {
                            break;
                        }



                        // Echo the data back to the client.  
                        data = data.Replace("<EOF>", " ");
                        byte[] msg = Encoding.ASCII.GetBytes(data);

                        handler.Send(msg);
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
        public static int Main(string[] args)
        {
            StartListening();
            return 0;
        }
    }
}
