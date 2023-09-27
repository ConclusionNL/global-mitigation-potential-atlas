using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models.Umbraco;

namespace Marketing.Core.Grid.Controls
{
    public class EmbedGridControl : GridControl
    {
        public override string Alias => "componentEmbed";
        public override string ViewPath => "~/Views/Partials/Embed.cshtml";

        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (ComponentEmbed)gridControl.Component;

            var viewModel = new EmbedViewModel();

            var regex = new Regex("\\<iframe(.*)\\>\\<\\/iframe\\>");
            var regexMatch = regex.Match(component.Code);
            if (regexMatch.Groups.Count > 1)
            {
                viewModel.Code = component.Code;
            }
            else
            {
                viewModel.Source = component.Code;
                viewModel.Height = component.Height;
            }

            return ViewModel(ViewPath, viewModel);
        }

        public class EmbedViewModel
        {
            public string Source { get; set; }
            public string Code { get; set; }
            public int Height { get; set; }
        }
    }
}
