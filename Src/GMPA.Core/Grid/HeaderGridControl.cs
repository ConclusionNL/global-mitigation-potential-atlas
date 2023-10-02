using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Models.Umbraco;
using GMPA.Core.Models.ViewModels.GridViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMPA.Core.Grid
{
    public class HeaderGridControl : GridControl
    {
        public override GridControlViewModel Render(BlockListGridControl gridControl, bool preview = false)
        {
            var component = (Header)gridControl.Component;

            Console.WriteLine(component.Title);

            return ViewModel(ViewPath, new HeaderViewModel
            {
                Title = component.Title
            });
        }
    }
}
