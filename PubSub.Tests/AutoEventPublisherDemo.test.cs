using NUnit.Framework;
using PublisherConsoleClient;

namespace PubSub.Tests
{
    [TestFixture]
    public class AutoEventPublisherDemoTests
    {       

        [Test]
        public void OpenServerDoesNotThrowExceptions()
        {
            // arrange
            using (var test = new ServerEventPublisherDemo())
            {
                // act
                test.PublishEvents();

                // assert
                Assert.IsNotNull(test);
            }
        }     
        
        [Test]
        public void UnSubscribeDoesNotThrowExceptions()
        {
            // arrange
            using (var test = new ServerEventPublisherDemo())
            {

                // act
                test.Unsubscribe();
                test.PublishEvents();
                test.Unsubscribe();

                // assert
                Assert.IsNotNull(test);
            }
        }

    }    
}