using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using μp101.Core;

namespace μp101.Component
{
    public partial class CodeCell
    {

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public string Width { get; set; }
        [Parameter]
        public string Mnemonics { get; set; }
        [Parameter]
        public string Message { get; set; }
        [Parameter]
        public MessageTypes MessageType { get; set; } = MessageTypes.Success;
        [Parameter]
        public ProcessorTypes ProcessorType { get; set; } = ProcessorTypes.Intel8085;

        public static int ID = 0;
        private int id { get; set; }
        private string idStr => $"cell_{id}";



        protected override Task OnInitializedAsync()
        {
            id = ID++;
            StateHasChanged();
            return base.OnInitializedAsync();
        }
        protected override void OnAfterRender(bool firstRender)
        {
            if(firstRender)
            {
                CreateEditor();
            }
            base.OnAfterRender(firstRender);
        }
        async void CreateEditor()
        {
            await Runtime.InvokeVoidAsync("createCodeCell", idStr);
        }

        public enum ProcessorTypes
        {
            Intel8085,
            Intel8086
        }
        public enum MessageTypes
        {
            Error,
            Success
        }
    }
}
