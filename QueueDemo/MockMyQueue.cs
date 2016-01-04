using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueDemo
{
    // This is the final mock queue (built on top of MockAzureQueuesInTableService) that is used
    // exclusively in the testing of my queue class. MockMyQueue is identical to MyQueue in every way
    // except for the Mock wrapper around the Azure Storage SDK. Notice that the difference between
    // MockCloudQueue and CloudQueue is minimal.
    public class MockMyQueue
    {
        private bool CloudQueue;

        private LocalQueue LQueue;
        private MockCloudQueue CQueue;

        private int MaxQueueSize;

        public MockMyQueue(bool UseCloudQueue, int MaxQueueSize)
        {
            CloudQueue = UseCloudQueue;
            this.MaxQueueSize = MaxQueueSize;

            if (CloudQueue)
            {
                CQueue = new MockCloudQueue(MaxQueueSize);
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


    class MockCloudQueue
    {
        private int CloudHead;
        private int CloudTail;

        private MockAzureQueuesInTableService MAQITS;

        private int QueueSize;
        private int MAXQUEUESIZE;

        public MockCloudQueue(int MaxQueueSize)
        {
            MAQITS = new MockAzureQueuesInTableService(null);

            MAXQUEUESIZE = MaxQueueSize;
        }

        public int Enqueue(string Data)
        {
            // Check if new data will fit
            if (QueueSize == MAXQUEUESIZE)
                return -1;

            // New queue
            if (QueueSize == 0)
            {
                MAQITS.Insert("Q1", "0", Data);

                CloudHead = 0;
                CloudTail = 0;
            }
            // Continue old queue
            else
            {
                ++CloudTail;
                MAQITS.Insert("Q1", CloudTail.ToString(), Data);
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

            ReturnValue = MAQITS.Remove("Q1", CloudHead.ToString());

            if (ReturnValue != null)
            {
                --QueueSize;
                ++CloudHead;
            }

            return ReturnValue;
        }
    }
}