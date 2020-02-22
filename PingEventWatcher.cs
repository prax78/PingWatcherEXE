using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;

namespace ConsoleApp3
{
    class Program
    {
       static int Success = 0;
        static  int Failure =0;
        static void Main(string[] args)
        {
            Console.CancelKeyPress += (s, e) =>
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"\nSuccess Packets {Success}, Failed Packets {Failure}");

            };
            if(args.Length==0)
            {
                
                Console.WriteLine($"No Arguements Passed, Run this command Like this {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.EXE WWW.GOOGLE.COM ");
            }
            else if (args != null || args.Length == 1)
            {

                EventPub ev = new EventPub();
                ev.MyEventSuccess += mySuccessHandler;
                ev.MyEventfail += mySuccessFailure;
                while (true)
                {

                    ev.PingCheck(args[0]);
                    System.Threading.Thread.Sleep(1000);
                }
            }
           


            

        }

       static void mySuccessHandler()
        {
            
               Success += 1;
            Console.ForegroundColor = ConsoleColor.Green;
            //Console.Write("Success");
            
        }
        static void mySuccessFailure()
        {
            
               Failure += 1;
            Console.ForegroundColor = ConsoleColor.Red;
           // Console.Write("Failure");
        
        }

     
    }

    class EventPub
    {
        public delegate void MyDel();
        public event MyDel MyEventSuccess;
        public event MyDel MyEventfail;
       
       public string hostname { get; set; }
        
        public void PingCheck(string _hostname)
        {
            Ping p = new Ping();
            hostname = _hostname;
            try
            {
                PingReply pr = p.Send(hostname);
                string result = pr.Status.ToString();

                if (result == "Success")
                {

                    Console.Write($"Ping Reply  {hostname} Status {pr.Status.ToString()} Address {pr.Address} Buffer {pr.Buffer.Length}  RoundTripTime(MS) {pr.RoundtripTime}  TTL {pr.Options.Ttl}\n");
                    MyEventSuccess();

                }
                else
                {
                    Console.Write($"Ping Reply  {hostname} Status {pr.Status.ToString()}\n");
                    MyEventfail();
                }
                
                
            }
            catch(PingException pEx) {
                Console.Write($"No Such Host Found {hostname} Exception {pEx.Message}\n");
                MyEventfail();
                
            }
        }

        

    }
}

