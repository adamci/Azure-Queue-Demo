using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

// These tests are essentially identical to the ones written for the LocalQueueUnitTest class
namespace QueueDemoTest
{
    [TestClass]
    public class CloudQueueUnitTest
    {
        [TestMethod]
        public void Test_LocalQueueInitialization()
        {
            QueueDemo.MockMyQueue q = new QueueDemo.MockMyQueue(true, 100);

            string ExpectedQueueType = "Cloud";
            string ActualQueueType = q.QueueType();
            Assert.AreEqual(ExpectedQueueType, ActualQueueType);
        }

        [TestMethod]
        public void Test_EnqueueDequeueForOneElement()
        {
            QueueDemo.MockMyQueue q = new QueueDemo.MockMyQueue(true, 100);

            string ExpectedToBeDequeued = "foo";
            string ActuallyDequeued;

            q.Enqueue(ExpectedToBeDequeued);
            ActuallyDequeued = q.Dequeue();

            Assert.AreEqual(ExpectedToBeDequeued, ActuallyDequeued);
        }

        [TestMethod]
        public void Test_EnqueueWhenFullDequeueWhenEmpty()
        {
            QueueDemo.MockMyQueue q = new QueueDemo.MockMyQueue(true, 10);

            // Enqueue when full
            int ExpectedReturnValue;
            int ActualReturnValue;

            for (int i = 0; i < 10; i++)
            {
                ExpectedReturnValue = 0;
                ActualReturnValue = q.Enqueue(i.ToString());

                Assert.AreEqual(ExpectedReturnValue, ActualReturnValue);
            }


            ExpectedReturnValue = -1;
            ActualReturnValue = q.Enqueue("foo");

            Assert.AreEqual(ExpectedReturnValue, ActualReturnValue);

            
            // Dequeue when empty
            string ExpectedStringReturnValue;
            string ActualStringReturnValue;

            for (int i = 0; i < 10; i++)
            {
                ExpectedStringReturnValue = i.ToString();
                ActualStringReturnValue = q.Dequeue();

                Debug.WriteLine("Expected: {0}", ExpectedStringReturnValue);
                Debug.WriteLine("Actual  : {0}", ActualReturnValue);

                Assert.AreEqual(ExpectedStringReturnValue, ActualStringReturnValue);
            }
            return;

            ExpectedStringReturnValue = null;
            ActualStringReturnValue = q.Dequeue();

            Assert.AreEqual(ExpectedStringReturnValue, ActualStringReturnValue);
        }
    }
}
