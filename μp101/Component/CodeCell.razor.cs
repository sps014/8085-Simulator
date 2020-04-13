using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using μp101.Highlighters;

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
        public string Memonics { get; set; }
        [Parameter]
        public ProcessorTypes ProcessorType { get; set; } = ProcessorTypes.Intel8085;

        string MarkDownText { get; set; } = @"<span class='l slector'>.code {</span>
    < span class='l property'>background</span>: <span class='value'>$bg-color</span>";

        private ElementReference cellElement;

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
                    MarkDownText = Intel8085Highlighter.GenerateMarkDownText(raw);
            }

        }

        public enum ProcessorTypes
        {
            Intel8085,
            Intel8086
        }
    }
}
