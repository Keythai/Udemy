namespace CRUDTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange: variables declaration and collection of inputs
            MyMath mm = new MyMath();
            int input1 = 10, input2 = 5;
            int expected = 15;

            //Act: calling methods
            int actual = mm.Add(input1, input2);

            //Assert: comparing expected value with actual value
            Assert.Equal(expected, actual);
        }
    }
}
