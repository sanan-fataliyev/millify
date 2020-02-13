using Xunit;
namespace Millify.Tests
{
    public class TestAddSuffix
    {

        [Theory]
        // da-də
        [InlineData("da","yol","yolda")]
        [InlineData("da","bar","barda")]
        [InlineData("da","sinif","sinifdə")]
        [InlineData("da","təyyarə","təyyarədə")]
        
        // la-lə
        [InlineData("la","qayçı","qayçıla")]
        [InlineData("la","yel","yellə")]
        [InlineData("la","balta","baltala")]
        
        // ma-mə
        [InlineData("ma","yaz","yazma")]
        [InlineData("ma","gəz","gəzmə")]
        [InlineData("ma","basdala","basdalama")]
        public void TestInline(string suffix, string root, string expected)
        {
            Assert.Equal(expected, root.AddSuffix(suffix));
        }
    }
}