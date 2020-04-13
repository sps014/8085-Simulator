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
        public string Height { get; set; }
        [Parameter]
        public string Address { get; set; }
        [Parameter]
        public string Mnemonic { get; set; }
        [Parameter]
        public ProcessorTypes ProcessorType { get; set; } = ProcessorTypes.Intel8085;

        public static int ID = 0;
        private int id { get; set; }
        private string idStr => $"cell_{id}";

        string MarkDownText { get; set; } = @"
        MVI B, 008
        MVI C, 08H
        MOV A, D
        BACK: RAR
        JNC SKIP
        INR B
        SKIP: DCR C
        JNZ BACK
        HLT ";

        private ElementReference cellElement;

        protected override Task OnInitializedAsync()
        {
            id = ID++;
            StateHasChanged();
            CreateEditor();
            return base.OnInitializedAsync();
        }
        async void CreateEditor()
        {
            await Runtime.InvokeVoidAsync("createCodeCell", idStr);
        }
        void onKeyUp()
        {
            GenerateMarkDownText();
        }

        private async Task<string> FetchRawText()
        {
            return await Runtime.InvokeAsync<string>("getInnerCodeCellText", cellElement);
        }

        private async void GenerateMarkDownText()
        {
            string raw = await FetchRawText();
            
            switch(ProcessorType)
            {
                case ProcessorTypes.Intel8085:
                    var s = Intel8085Highlighter.GenerateMarkDownText(raw);
                    MarkDownText = s;
                    StateHasChanged();
                    Console.WriteLine(s);
                    break;
            }

        }

        public enum ProcessorTypes
        {
            Intel8085,
            Intel8086
        }
    }
}
