using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueDemo
{
    // For readabilty, I decided to split MyQueue into the LocalQueue and CloudQueue classes.
    // The CloudQueue bool keeps track of whether or not the Queue MyQueue is a
    // cloud-based queue.
    public class MyQueue
    {
        private bool CloudQueue;

        private LocalQueue LQueue;
        private CloudQueue CQueue;

        private int MaxQueueSize;

        public MyQueue(bool UseCloudQueue, int MaxQueueSize)
        {
            CloudQueue = UseCloudQueue;
            this.MaxQueueSize = MaxQueueSize;

            if (CloudQueue)
            {
                CQueue = new CloudQueue(MaxQueueSize);
            }
            else
            {
                LQueue = new LocalQueue(MaxQueueSize);
            }
        }

        public int Enqueue(string Data)
        {
            if (CloudQueue)
            {
                return CQueue.Enqueue(Data);
            }
            else
            {
                return LQueue.Enqueue(Data);
            }
        }

        public string Dequeue()
        {
            if (CloudQueue)
            {
                return CQueue.Dequeue();
            }
            else
            {
                return LQueue.Dequeue();
            }
        }

        public string QueueType()
        {
            if (CloudQueue)
            {
                return "Cloud";
            }
            else
            {
                return "Local";
            }
        }
    }

    // This is a simple linked-list queue
    class LocalQueue
    {
        private Node Head;
        private Node Tail;

        private int QueueSize;
        private int MAXQUEUESIZE;

        public LocalQueue(int MaxQueueSize)
        {
            MAXQUEUESIZE = MaxQueueSize;
        }

        public int Enqueue(string Data)
        {
            // Check if new data will fit
            if (QueueSize == MAXQUEUESIZE)
                return -1;

            Node NewNode = new Node(Data);

            // New queue
            if (Head == null)
            {
                Head = NewNode;
                Tail = NewNode;
            }
            // Continue old queue
            else
            {
                Tail.Next = NewNode;
                Tail = NewNode;
            }

            ++QueueSize;

            return 0;
        }

        public string Dequeue()
        {
            string ReturnValue;

            // If queue is empty
            if (QueueSize == 0)
            {
                return null;
            }

            ReturnValue = Head.Data;
            Head = Head.Next;
            --QueueSize;

            return ReturnValue;
        }
    }


    // Note that the logic of CloudQueue is identical except for its interface with
    // AzureQueuesInTableService.
    class CloudQueue
    {
        private int CloudHead;
        private int CloudTail;

        private AzureQueuesInTableService AQITS;

        private int QueueSize;
        private int MAXQUEUESIZE;

        public CloudQueue(int MaxQueueSize)
        {
            string CustomTableName = string.Format("Queue{0}", DateTime.UtcNow.Ticks.ToString());
            AQITS = new AzureQueuesInTableService(CustomTableName);

            MAXQUEUESIZE = MaxQueueSize;
        }

        ~CloudQueue()
        {
            AQITS.DeleteTable();
        }

        public int Enqueue(string Data)
        {
            // Check if new data will fit
            if (QueueSize == MAXQUEUESIZE)
                return -1;

            // New queue
            if (QueueSize == 0)
            {
                AQITS.Insert("Q1", "0", Data);

                CloudHead = 0;
                CloudTail = 0;
            }
            // Continue old queue
            else
            {
                ++CloudTail;
                AQITS.Insert("Q1", CloudTail.ToString(), Data);
            }

            ++QueueSize;

            return 0;
        }

        public string Dequeue()
        {
            string ReturnValue;

            // If queue is empty
            if (QueueSize == 0)
            {
                return null;
            }

            ReturnValue = AQITS.Remove("Q1", CloudHead.ToString());

            if (ReturnValue != null)
            {
                --QueueSize;
                ++CloudHead;
            }

            return ReturnValue;
        }
    }


    class Node
    {
        public string Data { get; set; }
        public Node Next { get; set; }

        public Node(string Data)
        {
            this.Data = Data;
        }
    }
}
