using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.JSInterop;

namespace MultipleBlazorApps.Client.Helpers
{
    public static class IJSRuntimeExtensionsMethods
    {

        public static async ValueTask IDScrollToTop(this IJSRuntime js, string elementId)
        {
            await js.InvokeVoidAsync("IDScrollIntoView");
        }

        public static ValueTask<object> SetInLocalStorage(this IJSRuntime js, string key, string content)
            => js.InvokeAsync<object>(
                "localStorage.setItem",
                key, content
                );

        public static ValueTask<string> GetFromLocalStorage(this IJSRuntime js, string key)
            => js.InvokeAsync<string>(
                "localStorage.getItem",
                key
                );

        public static ValueTask<object> RemoveItem(this IJSRuntime js, string key)
            => js.InvokeAsync<object>(
                "localStorage.removeItem",
                key);


        // to open a link in a new window tab
        public static async ValueTask OpenLinkInNewTab(this IJSRuntime js, string url)
        {
            await js.InvokeVoidAsync("BlazorOpenNew", url);
        }


    }
}
