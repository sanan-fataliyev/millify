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

        // lı
        [InlineData("lı","savad","savadlı")]
        [InlineData("lı","bilik","bilikli")]
        [InlineData("lı","uğur","uğurlu")]
        [InlineData("lı","güc","güclü")]

        public void TestAll(string suffix, string root, string expected)
        {
            Assert.Equal(expected, root.AddSuffix(suffix));
        }
    }
}