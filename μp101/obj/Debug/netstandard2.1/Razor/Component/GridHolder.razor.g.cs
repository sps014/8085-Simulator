#pragma checksum "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\Component\GridHolder.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d194b385851f1c0ac9a7ad95610454e4e689fd71"
// <auto-generated/>
#pragma warning disable 1591
namespace μp101.Component
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using μp101;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\_Imports.razor"
using μp101.Shared;

#line default
#line hidden
#nullable disable
    public partial class GridHolder : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
            __builder.OpenElement(0, "div");
            __builder.AddAttribute(1, "class", "container my-5");
            __builder.AddMarkupContent(2, "\r\n\r\n\r\n    \r\n    ");
            __builder.OpenElement(3, "section");
            __builder.AddAttribute(4, "class", "dark-grey-text");
            __builder.AddMarkupContent(5, "\r\n\r\n        \r\n        ");
            __builder.AddMarkupContent(6, "<p class=\"lead text-center white-text w-responsive mx-auto text-muted mt-4 pt-2 mb-5\">Online Simulator for Microprocessors</p>\r\n\r\n        \r\n        ");
            __builder.OpenElement(7, "div");
            __builder.AddAttribute(8, "class", "row");
            __builder.AddMarkupContent(9, "\r\n\r\n            ");
            __builder.AddContent(10, 
#nullable restore
#line 13 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\Component\GridHolder.razor"
             ChildContent

#line default
#line hidden
#nullable disable
            );
            __builder.AddMarkupContent(11, "\r\n           \r\n        ");
            __builder.CloseElement();
            __builder.AddMarkupContent(12, "\r\n        \r\n\r\n    ");
            __builder.CloseElement();
            __builder.AddMarkupContent(13, "\r\n\r\n\r\n");
            __builder.CloseElement();
        }
        #pragma warning restore 1998
#nullable restore
#line 24 "C:\Users\shive\Desktop\Test\MicroProcessor101\μp101\Component\GridHolder.razor"
 
    [Parameter]
    public RenderFragment ChildContent { get; set; }

#line default
#line hidden
#nullable disable
    }
}
#pragma warning restore 1591
