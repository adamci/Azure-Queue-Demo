using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;


namespace QueueDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            QuickDemo();
        }

        // Just a quick demonstration of the Azure Table-based queue
        static void QuickDemo()
        {
            try
            {
                // Setting the first parameter of the constructor to true makes the queue
                // cloud-based (false would make it a local queue). 100 signifies the maximum
                // allowed size of the queue.
                MyQueue q = new MyQueue(true, 100);

                for (int i = 0; i < 1000; i++)
                {
                    q.Enqueue(i.ToString());
                    String output = String.Format("Enqueueing value {0}", i.ToString());
                    System.Console.WriteLine(output);
                }

                string ret;
                for (int i = 0; i < 1000; i++)
                {
                    ret = q.Dequeue();
                    if (ret != null)
                    {
                        String output = String.Format("Dequeueing value {0}", ret);
                        System.Console.WriteLine(output);
                    }

                }
            }
            catch (StorageException ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
