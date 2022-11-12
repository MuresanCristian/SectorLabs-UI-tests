using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightTests.PageObjects
{
    public class CommonPageObjects
    {
        private readonly IPage _page;

        public CommonPageObjects(IPage page)
        {
            _page = page;
        }

        public async Task NavigateAsync(string websiteUrl)
        {
            await _page.GotoAsync(websiteUrl);
        }
    }
}
