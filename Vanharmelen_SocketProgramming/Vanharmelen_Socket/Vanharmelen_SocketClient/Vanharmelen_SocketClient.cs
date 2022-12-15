using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace Vanharmelen_SocketClient
{
    class Vanharmelen_SocketClient
    {
        public static string eof = "<EOF>";

        public static void StartClient()
        {

            byte[] bytes = new byte[1024];
            string input = "";


            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket sender = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    sender.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        sender.RemoteEndPoint.ToString());

                    while (true)
                    {
                        Console.WriteLine(" ");
                        Console.WriteLine("Enter a message");
                        input = Console.ReadLine();

                        byte[] msg = Encoding.ASCII.GetBytes(input + eof);

                        int bytesSent = sender.Send(msg);

                        int bytesRec = sender.Receive(bytes);

                        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRec));


                        Console.WriteLine(" ");
                        Console.WriteLine("Exit or Continue");
                        input = Console.ReadLine();

                        if (input == "exit")
                        {
                            msg = Encoding.ASCII.GetBytes("End of Stream" + eof);
                            bytesSent = sender.Send(msg);
                            break;
                        }
                        else
                        {
                            msg = Encoding.ASCII.GetBytes(input + eof);
                            bytesSent = sender.Send(msg);
                            bytesRec = sender.Receive(bytes);

                        }

                    }

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            StartClient();
            return 0;
        }
    }
}
